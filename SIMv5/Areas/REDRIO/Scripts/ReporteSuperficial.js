$(document).ready(function () {
    var ListaReportes = [];
    $('#btnNuevoMuestra').click(function () {
        $('#vistaPrincipal').hide(); 
        $('#vistaAgregar').show(); 
        CargarEstaciones();
    });
    $('#btnVolverAgregar').click(function () {
        $('#vistaAgregar').hide();   
        $('#vistaPrincipal').show(); 
        obtenerReportesLaboratorio(); 

    });
    $('#btnVolver').click(function () {
    });
    $('#btnCancelar').click(function () {
        $('#formNuevoMuestra').hide();
        
        $('.vistaPrincipal').show();

    });
    $('#modalNuevoDatoLabel').on('shown.bs.modal', function () {
        cargarExcelPorCampañas();
    });
    $('#estacion').change(function() {
        var selectedEstacion = $(this).val();
        var selectedEstacionText = $(this).find("option:selected").text();
        $('.nombreEstacion').text(selectedEstacionText);
    });

    $(document).on('focus', 'input.input-normal', function() {
        console.log('Input enfocado');

        $('.overlay').show();

        $(this).addClass('input-expandido input-activado').removeClass('input-normal');

        $(this).css({
            top: '50%',
            left: '50%',
        });
    }).on('blur', 'input.input-activado', function() {
        console.log('Input desenfocado');

        $('.overlay').hide();

        $(this).removeClass('input-expandido input-activado').addClass('input-normal');
        $(this).css({
            top: '', 
            left: '' 
        });
    });

    $('.overlay').click(function() {
        $('input.input-expandido.input-activado').blur();
    });


    function getParameterByName(name) {
        const url = window.location.href; 
        const nameEscaped = name.replace(/[\[\]]/g, "\\$&"); 
        const regex = new RegExp("[?&]" + nameEscaped + "(=([^&#]*)|&|#|$)"); 
        const results = regex.exec(url); 
        if (!results) return null; 
        if (!results[2]) return ''; 
        return decodeURIComponent(results[2].replace(/\+/g, " ")); 
    }
    function actualizarNombreEstacion(nombreEstacion) {
        var elementos = document.querySelectorAll('#nombreEstacion');
        elementos.forEach(function(elemento) {
            elemento.textContent = nombreEstacion;
        });
    }
    function CargarEstaciones(selectedEstacionId, isEditing = false){
        $.getJSON('http://localhost:5078/api/Estacion/ObtenerEstaciones', function(data){
            console.log('Estaciones:',data);
    
            var selectEstacion =isEditing ? $ ('#editEstacion'): $('#estacion');
            selectEstacion.empty();
            selectEstacion.append($('<option>', {value: '', text: '', text: 'Seleccione una Estacion'}));
    
            if(data.result && data.result.length > 0){
                $.each(data.result, function(index, estacion){
                    selectEstacion.append($('<option>', {
                        value: estacion.idEstacion,
                        text: estacion.nombreEstacion
                    }));
                });
            }else{
                console.log("No se encontraron Estaciones.")
            }
            if(selectedEstacionId){
                selectEstacion.val(selectedEstacionId);
                var nombreEstacion = selectedEstacionId.nombreEstacion;
                actualizarNombreEstacion(nombreEstacion);
            }
        }).fail(function(jqXHR, textStatus, errorThrown){
            console.log("Error al cargar las Estaciones:", textStatus, errorThrown)
        })
    }
    var idEstacion;
    $('#estacion').change(function() {
        idEstacion = $(this).val();
    });

    // Variable global para almacenar el último tipo de fuente seleccionado
let ultimoTipoFuenteSeleccionado = null;

function obtenerReportesLaboratorio() {
    var idCampaña = getParameterByName('idCampa'); 
    console.log("ID Campaña obtenido:", idCampaña);
    
    if (!idCampaña) {
        console.error("El parámetro idCampa no está presente en la URL.");
        alert("Error: el parámetro idCampa no está especificado.");
        return; 
    }

    // Si ya hay un tipo seleccionado, se usa ese sin preguntar
    if (ultimoTipoFuenteSeleccionado) {
        cargarReportes(idCampaña, ultimoTipoFuenteSeleccionado);
    } else {
        // Si no, muestra el Swal para seleccionar el tipo de fuente
        Swal.fire({
            title: 'Selecciona el tipo de fuente',
            input: 'select',
            inputOptions: {
                'Quebrada': 'Quebrada',
                'Rio': 'Rio',
                'Listar todo': 'Listar todo' 
            },
            inputPlaceholder: 'Selecciona una opción',
            showCancelButton: false,
            allowOutsideClick: () => false,
            allowEscapeKey: false,  
            inputValidator: (value) => {
                return new Promise((resolve) => {
                    if (value) {
                        resolve();
                    } else {
                        resolve('¡Debes seleccionar una opción!');
                    }
                });
            }
        }).then((result) => {
            if (result.isConfirmed) {
                // Guardamos el tipo seleccionado en la variable global
                ultimoTipoFuenteSeleccionado = result.value;
                cargarReportes(idCampaña, ultimoTipoFuenteSeleccionado);
            }
        });
    }
}

// Función para cargar y filtrar reportes basados en el tipo de fuente
function cargarReportes(idCampaña, tipoFuenteSeleccionado) {
    var urlBase = $("#SIM").data("url");
    var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ObtenerReporteLaboratorioPorCampaña/" + idCampaña;

    $.getJSON(urlCompleta, function (data) {
        let ListaReportes = data.result;

        // Filtrar las estaciones según el tipo de fuente seleccionado
        if (tipoFuenteSeleccionado !== 'Listar todo') {
            ListaReportes = ListaReportes.filter(reporte => 
                reporte.estacion?.tipoFuente?.NombreTipoFuente === tipoFuenteSeleccionado
            );
        }

        if (ListaReportes && ListaReportes.length > 0) {
            const nombreCampaña = ListaReportes[0].campaña.nombreCampaña; 
            $('#nombreCampaña').text(nombreCampaña); 
        } else {
            $('#nombreCampaña').text('No hay reportes disponibles');
        }
        
        console.log("Reportes filtrados:", ListaReportes); 
        mostrarReportesLaboratorio(ListaReportes); 
    }).fail(function () {
        Swal.fire({
            icon: 'warning',
            text: 'No hay reportes asociado a esta Campaña'
        });
    });
}


    function groupBy(arr, key) {
        return arr.reduce((acc, current) => {
          const group = current[key];
          if (!acc[group]) {
            acc[group] = [];
          }
          acc[group].push(current);
          return acc;
        }, {});
      }
    
      function mostrarReportesLaboratorio(data) {
        const groupedData = data.reduce((acc, current) => {
            const estacion = current.IdEstacion;
            if (!acc[estacion]) {
                const municipioNombre = current.estacion.municipio ? current.estacion.municipio.nombre : '';
                acc[estacion] = {
                    idEstacion: estacion,
                    nombreEstacionCompleto: `${current.estacion.nombreEstacion} ${current.estacion.codigo} (${municipioNombre})`,
                    reportes: []
                };
            }
            acc[estacion].reportes.push(current);
            return acc;
        }, {});
    
        const dataSource = Object.values(groupedData).map(group => {
            const muestraCompuesta = group.reportes.find(reporte => reporte.muestraCompuesta);
            const tiposFuente = [...new Set(group.reportes.map(reporte => 
                reporte.estacion?.tipoFuente?.NombreTipoFuente || 'vacio'
            ))];
            return {
                idEstacion: group.idEstacion,
                nombreEstacionCompleto: group.nombreEstacionCompleto,
                idMuestraCompuesta: muestraCompuesta ? muestraCompuesta.muestraCompuesta.idMuestraCompuesta : null,
                reportes: group.reportes,
                tipoFuente: tiposFuente
            };
        });
        console.log(dataSource);
    
        $("#gridContainer").dxDataGrid({
            dataSource: dataSource,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
    
            columns: [
                {
                    caption: "ID",
                    cellTemplate: function(container, options) {
                        container.text(options.rowIndex + 1);
                    }
                },
                { 
                    dataField: "nombreEstacionCompleto", 
                    caption: "Nombre Estación" 
                },{ 
                    dataField: "tipoFuente", 
                    caption: "Tipo" 
                },
                {
                    caption: "Ver Detalles",
                    width: 350,
                    cellTemplate: function (container, options) {
                        var verDetallesIcon = $("<i>")
                            .addClass("fas fa-eye")
                            .attr("title", "Ver Detalles")
                            .css({ "cursor": "pointer", "align-items": "center", "margin-left": "150px"})
                            .click(function () {
                                mostrarDetalles({
                                    idEstacion: options.data.idEstacion,
                                    nombreEstacion: options.data.nombreEstacionCompleto,
                                    reportes: options.data.reportes
                                });
                            });
                        container.append(verDetallesIcon);
                    }
                },
                {
                    caption: "Acciones",
                    width: 200,
                    cellTemplate: function (container, options) {
                        var editButton = $("<i>")
                            .addClass("fas fa-edit")
                            .attr("title", "Editar")
                            .css({ "cursor": "pointer", "margin-right": "10px" })
                            .click(function () {
                                mostrarDetalles({
                                    idEstacion: options.data.idEstacion,
                                    nombreEstacion: options.data.nombreEstacionCompleto,
                                    reportes: options.data.reportes
                                }, true);
                            });
    
                        var deleteButton = $("<i>")
                            .addClass("fas fa-trash")
                            .attr("title", "Eliminar")
                            .css({ "cursor": "pointer", "margin-right": "10px" })
                            .click(function () {
                                eliminarReporteLaboratorio(options.data.idEstacion);
                            });
    
                        container.append(editButton).append(deleteButton);
                    }
                }
            ],
            paging: {
                pageSize: 10
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10, 20],
                showInfo: true
            },
            filterRow: {
                visible: true
            },
            searchPanel: {
                visible: true,
                placeholder: "Buscar..."
            },
            export: {
                enabled: true,
                fileName: "ReportesLaboratorio"
            }
        });
    }
    
    
    function mostrarDetalles(data, isEditing = false) {
        console.log(data);
        if (!data || !Array.isArray(data.reportes) || data.reportes.length === 0) {
            console.error("Data no válida o no contiene reportes.");
            Swal.fire({
                icon: 'info',
                title: 'Sin Resultados',
                text: 'No hay resultados disponibles para mostrar.'
            });
            return; 
        }
        function ordenarPorFecha(array, tipo) {
            return array.sort((a, b) => {
                const fechaA = new Date(a.muestraCompuesta?.[tipo]?.Fecha_Muestra || 0);
                const fechaB = new Date(b.muestraCompuesta?.[tipo]?.Fecha_Muestra || 0);
                return fechaA - fechaB; 
            });
        }
        
        const reportesUnicos = data.reportes
            .filter(reporte => reporte.muestraCompuesta && reporte.muestraCompuesta.idMuestraCompuesta) 
            .filter((reporte, index, self) => 
                index === self.findIndex((r) => 
                    r.muestraCompuesta.idMuestraCompuesta === reporte.muestraCompuesta.idMuestraCompuesta
                )
            );
    
        const reportesOrdenados = data.reportes.sort((a, b) => {
            const fechaA = new Date(a.resultadoCampo?.Fecha_Muestra);
            const fechaB = new Date(b.resultadoCampo?.Fecha_Muestra);
            if (fechaA - fechaB !== 0) {
                return fechaA - fechaB; 
            }
            const horaA = a.resultadoCampo?.Hora || '';
            const horaB = b.resultadoCampo?.Hora || '';
            return horaA.localeCompare(horaB); 
        });
    
    
        console.log(reportesOrdenados);
        console.log(reportesUnicos);
        let nombreEstacion = data.nombreEstacion;
        let modalHtml = `
            <div class="modal-header">
                <h3>Detalles de la Estación: ${nombreEstacion}</h3>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="overlay"></div>
            <div class="modal-body ">
                <div class="resultados-container">
                    <h3>Resultados de Campo</h3>
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th style="width: 30px">ID Campo</th>
                                <th style="width: 80px">Fecha Visita</th>
                                <th style="width: 30px">Hora</th>
                                <th style="width: 30px">Altura</th>
                                <th style="width: 30px">Apariencia</th>
                                <th style="width: 30px">Color</th>
                                <th style="width: 30px">Conductividad eléctrica (µS/cm)</th>
                                <th style="width: 30px">Observación</th>
                                <th style="width: 30px">OD</th>
                                <th style="width: 30px">Olor</th>
                                <th style="width: 30px">Potencial Redox (mV)</th>
                                <th style="width: 30px">pH (U de pH)</th>
                                <th style="width: 30px">Temperatura Agua C°</th>
                                <th style="width: 30px">Temperatura Ambiente C°</th>
                                <th style="width: 30px">Tiempo</th>
                                <th style="width: 30px">Turbiedad (NTU)</th>
                                <th style="width: 30px">H1 (m)</th>
                                <th style="width: 30px">H2 (m)</th>
                                ${isEditing ? '<th style="width: 30px">Acciones</th>' : ''}

                       
                            </tr>
                        </thead>
                        <tbody>
                        ${reportesOrdenados.length > 0 ? reportesOrdenados.map(reporte => {
                            const resultadoCampo = reporte.resultadoCampo; 
                            if (resultadoCampo) {
                                return `
                                    <tr>
                                        <td id="editid" >${resultadoCampo.idCampo || ''}</td>
                                    <td>
                                    ${isEditing ? 
                                        `<input type="date" id="editFechaMuestra" class="input-normal" style="width: 100px" value="${(resultadoCampo.Fecha_Muestra || '').split('T')[0]}" />` 
                                        : (resultadoCampo.Fecha_Muestra || '').split('T')[0]}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editHora" class="input-normal" value="${resultadoCampo.Hora || ''}" />` 
                                        : resultadoCampo.Hora || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editAltura" class="input-normal" value="${resultadoCampo.Altura || ''}" />` 
                                        : resultadoCampo.Altura || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editApariencia" class="input-normal" value="${resultadoCampo.Apariencia || ''}" />` 
                                        : resultadoCampo.Apariencia || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editColor" class="input-normal" value="${resultadoCampo.Color || ''}" />` 
                                        : resultadoCampo.Color || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editCond" class="input-normal" value="${resultadoCampo.Cond || ''}" />` 
                                        : resultadoCampo.Cond || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editObservacion" class="input-normal" value="${resultadoCampo.Observacion || ''}" />` 
                                        : resultadoCampo.Observacion || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editOd" class="input-normal" value="${resultadoCampo.Od || ''}" />` 
                                        : resultadoCampo.Od || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editOlor" class="input-normal" style="width: 60px" value="${resultadoCampo.Olor || ''}" />` 
                                        : resultadoCampo.Olor || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editOrp" class="input-normal" value="${resultadoCampo.Orp || ''}" />` 
                                        : resultadoCampo.Orp || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editPh" class="input-normal" value="${resultadoCampo.Ph || ''}" />` 
                                        : resultadoCampo.Ph || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editTempAgua" class="input-normal" value="${resultadoCampo.TempAgua || ''}" />` 
                                        : resultadoCampo.TempAgua || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editTempAmbiente" class="input-normal" value="${resultadoCampo.TempAmbiente || ''}" />` 
                                        : resultadoCampo.TempAmbiente || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editTiempo" class="input-normal" value="${resultadoCampo.Tiempo || ''}" />` 
                                        : resultadoCampo.Tiempo || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="number" id="editTurb" class="input-normal" value="${resultadoCampo.Turb || ''}" />` 
                                        : resultadoCampo.Turb || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editH1" class="input-normal" value="${resultadoCampo.H1 || ''}" />` 
                                        : resultadoCampo.H1 || ''}
                                </td>
                                <td>
                                    ${isEditing ? 
                                        `<input type="text" id="editH2" class="input-normal" value="${resultadoCampo.H2 || ''}" />` 
                                        : resultadoCampo.H2 || ''}
                                </td>
                                ${isEditing ? 
                                        `<td>
                                            <button class="btn btn-success btn-sm" id="btnActualizarDato" )">
                                                <i class="fas fa-check"></i> 
                                           
                                        </td>` 
                                        : ''}
                                </tr>
                                `;
                            }
                            return ''; 
                        }).join('') : '<tr><td colspan="14">No hay resultados de campo disponibles.</td></tr>'}
                    </tbody>
                </table>
                
                            
            </div>
               <div class="resultados-compuestos">
               <div class="card">
               <h3>Resultados de Laboratorio</h3>   
    
   <div class="modal-body d-flex">
    <!-- Columna de botones en vertical -->
    <div  class="btn-group-vertical">
        <button class="btn btn-primary" onclick="mostrarSeccion('insitu')">In Situ</button>
        <button class="btn btn-primary" onclick="mostrarSeccion('quimico')">Químico</button>
        <button class="btn btn-primary" onclick="mostrarSeccion('fisico')">Físico</button>
        <button class="btn btn-primary" onclick="mostrarSeccion('biologico')">Biológico</button>
        <button class="btn btn-primary" onclick="mostrarSeccion('metalAgua')">Metal Agua</button>
        <button class="btn btn-primary" onclick="mostrarSeccion('metalSedimental')">Metal Sedimental</button>
        <button class="btn btn-primary" onclick="mostrarSeccion('nutrientes')">Nutrientes</button>
    </div>

    <!-- Contenido de cada sección a la derecha de los botones -->
    <div id="muestrasContent" class="content-section flex-grow-1">
        <!-- Aquí se mostrará el contenido de la sección seleccionada -->
        <p>Seleccione una sección para ver los detalles.</p>
    </div>
</div>

</div>

       
        `;
    
        const modalContainer = document.getElementById('modalContainer');
        modalContainer.querySelector('.modal-content').innerHTML = modalHtml;
    
        window.mostrarSeccion = function(tipo) {
            const muestrasContent = document.getElementById('muestrasContent');
            muestrasContent.innerHTML = ""; 
    
            let contentHtml = "";
            const reportes = ordenarPorFecha(reportesUnicos, tipo);
    
            switch (tipo) {
                case 'insitu':
                    
                contentHtml += `
                <table class="table table-striped">
                    <tbody>
                    <tr>
                        <tr>
                            <th>ID Insitu</th>
                            ${reportes.map(reporte => `<td class="editidInsitu">${reporte.muestraCompuesta?.insitu?.idInsitu || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>Fecha Visita</th>
                            ${reportes.map(reporte => `<td>
                                ${isEditing ? 
                                    `<input type="date" class="editFecha_Muestra input-normal" style="width: 100px" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${(reporte.muestraCompuesta?.insitu?.Fecha_Muestra || '').split('T')[0]}" />` 
                                    : (reporte.muestraCompuesta?.insitu?.Fecha_Muestra || '').split('T')[0]}
                            </td>`).join('')}
                        </tr>
                        <tr>
                            <th>Conductividad Eléctrica</th>
                            ${reportes.map(reporte => `<td> ${isEditing ? `<input type="number" class="editConductiviidad_electrica input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.Conductiviidad_electrica || ''}" />` : reporte.muestraCompuesta?.insitu?.Conductiviidad_electrica || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>OD</th>
                            ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editOxigeno_disuelto input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.Oxigeno_disuelto || ''}" />` : reporte.muestraCompuesta?.insitu?.Oxigeno_disuelto || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>pH</th>
                            ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editPh input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.PhInsitu || ''}" />` : reporte.muestraCompuesta?.insitu?.PhInsitu || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>Temp Agua</th>
                            ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editTemp_agua input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.Tem_agua || ''}" />` : reporte.muestraCompuesta?.insitu?.Tem_agua || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>Temp Ambiente</th>
                            ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editTemp_ambiente input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.Temp_ambiente || ''}" />` : reporte.muestraCompuesta?.insitu?.Temp_ambiente || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>Turbiedad</th>
                            ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editTurbiedad input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.Turbiedad || ''}" />` : reporte.muestraCompuesta?.insitu?.Turbiedad || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>Potencial Redox (mV)</th>
                            ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editOrpInsitu input-normal" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}" value="${reporte.muestraCompuesta?.insitu?.OrpInsitu || ''}" />` : reporte.muestraCompuesta?.insitu?.OrpInsitu || ''}</td>`).join('')}
                        </tr>
                        <tr>
                            <th>Acciones</th>
                            ${reportes.map(reporte => `
                                <td>
                                    ${isEditing ? `
                                         <button class="btn btn-success btn-sm btnActualizarDatoInsitu" data-id-insitu="${reporte.muestraCompuesta?.insitu?.idInsitu}">
                                            <i class="fas fa-check"></i>
                                        </button>
                                        
                                    ` : ''}
                                </td>`).join('')}
                        </tr>
                    </tbody>
                </table>
            `;
                break;
                case 'quimico':
                    contentHtml += `
                    <table class="table table-striped">
                        <tbody>
                            <tr>
                                <th>ID Químico</th>
                                ${reportes.map(reporte => `<td class="editidQuimico">${reporte.muestraCompuesta?.quimico?.idQuimico || ''}</td>`).join('')}
                            </tr>
                            <tr>
                               <th>Fecha Visita</th>
                                ${reportes.map(reporte => `<td>
                                    ${isEditing ? 
                                        `<input type="date" class="editFecha_Muestra input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" style="width: 100px" value="${(reporte.muestraCompuesta?.quimico?.Fecha_Muestra || '').split('T')[0]}" />` 
                                        : (reporte.muestraCompuesta?.quimico?.Fecha_Muestra || '').split('T')[0]}
                                </td>`).join('')}
                            </tr>
                            <tr>
                                <th>Cloruros</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCloruros input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.Cloruros || ''}" />` : reporte.muestraCompuesta?.quimico?.Cloruros || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>DB05</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editDb05 input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.Db05 || ''}" />` : reporte.muestraCompuesta?.quimico?.Db05 || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>DQ0</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editDq0 input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.Dq0 || ''}" />` : reporte.muestraCompuesta?.quimico?.Dq0 || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>Grasa/Aceite</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editGasa_Aceite input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.Grasa_Aceite || ''}" />` : reporte.muestraCompuesta?.quimico?.Grasa_Aceite || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>Hierro Total</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editHierroTotal input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.HierroTotal || ''}" />` : reporte.muestraCompuesta?.quimico?.HierroTotal || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>Sulfatos</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSulfatos input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.Sulfatos || ''}" />` : reporte.muestraCompuesta?.quimico?.Sulfatos || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>Sulfuros</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSulfuros input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.Sulfuros || ''}" />` : reporte.muestraCompuesta?.quimico?.Sulfuros || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>Sustancia Activa Azul Metileno</th>
                                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSustanciaActivaAzulMetileno input-normal" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}" value="${reporte.muestraCompuesta?.quimico?.SustanciaActivaAzulMetileno || ''}" />` : reporte.muestraCompuesta?.quimico?.SustanciaActivaAzulMetileno || ''}</td>`).join('')}
                            </tr>
                            <tr>
                                <th>Acciones</th>
                                ${reportes.map(reporte => `
                                    <td>
                                        ${isEditing ? `
                                            <button class="btn btn-success btn-sm btnActualizarDatoQuimico" data-id-quimico="${reporte.muestraCompuesta?.quimico?.idQuimico}">
                                                <i class="fas fa-check"></i>
                                            </button>
                                            
                                        ` : ''}
                                    </td>`).join('')}
                            </tr>
                        </tbody>
                    </table>
                `; 
                break;
               
                case 'fisico':
                    contentHtml += `
    <table class="table table-striped">
        <tbody>
            <tr>
                <th>ID Físico</th>
                ${reportes.map(reporte => `<td id="editidFisico">${reporte.muestraCompuesta?.fisico?.idFisico || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Fecha Visita</th>
                ${reportes.map(reporte => `<td>
                    ${isEditing ? 
                        `<input type="date" class="editFecha_Muestra input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" style="width: 100px" value="${(reporte.muestraCompuesta?.fisico?.Fecha_Muestra || '').split('T')[0]}" />` 
                        : (reporte.muestraCompuesta?.fisico?.Fecha_Muestra || '').split('T')[0]}
                </td>`).join('')}
            </tr>
            <tr>
                <th>Caudal</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCaudal input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.Caudal || ''}" />` : reporte.muestraCompuesta?.fisico?.Caudal || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Color Triestimular 436 nm</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editColorTriestimular436nm input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.ColorTriestimular436nm || ''}" />` : reporte.muestraCompuesta?.fisico?.ColorTriestimular436nm || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Color Verdadero</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editColorVerdaderoUPC input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.ColorVerdaderoUPC || ''}" />` : reporte.muestraCompuesta?.fisico?.ColorVerdaderoUPC || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Color Triestimular 525 nm</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editColorTriestimular525nm input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.ColorTriestimular525nm || ''}" />` : reporte.muestraCompuesta?.fisico?.ColorTriestimular525nm || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Clasificación Caudal</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editClasificacionCaudal input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.ClasificacionCaudal || ''}" />` : reporte.muestraCompuesta?.fisico?.ClasificacionCaudal || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Color Triestimular 650 nm</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editColorTriestimular620nm input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.ColorTriestimular620nm || ''}" />` : reporte.muestraCompuesta?.fisico?.ColorTriestimular620nm || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Número de Verticales</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editNumeroDeVerticales input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.NumeroDeVerticales || ''}" />` : reporte.muestraCompuesta?.fisico?.NumeroDeVerticales || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Sólidos Disueltos Totales</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSolidosDisueltosTotales input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.SolidosDisueltosTotales || ''}" />` : reporte.muestraCompuesta?.fisico?.SolidosDisueltosTotales || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Sólidos Fijos Totales</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSolidosFijosTotales input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.SolidosFijosTotales || ''}" />` : reporte.muestraCompuesta?.fisico?.SolidosFijosTotales || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Sólidos Sedimentables</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSolidosSedimentables input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.SolidosSedimentables || ''}" />` : reporte.muestraCompuesta?.fisico?.SolidosSedimentables || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Sólidos Suspendidos Totales</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSolidosSuspendidosTotales input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.SolidosSuspendidosTotales || ''}" />` : reporte.muestraCompuesta?.fisico?.SolidosSuspendidosTotales || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Sólidos Totales</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSolidosTotales input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.SolidosTotales || ''}" />` : reporte.muestraCompuesta?.fisico?.SolidosTotales || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Sólidos Volátiles Totales</th>
                ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editSolidosVolatilesTotales input-normal" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}" value="${reporte.muestraCompuesta?.fisico?.SolidosVolatilesTotales || ''}" />` : reporte.muestraCompuesta?.fisico?.SolidosVolatilesTotales || ''}</td>`).join('')}
            </tr>
            <tr>
                <th>Acciones</th>
                ${reportes.map(reporte => `
                    <td>
                        ${isEditing ? `
                            <button class="btn btn-success btn-sm btnActualizarDatoFisico" data-id-fisico="${reporte.muestraCompuesta?.fisico?.idFisico}">
                                <i class="fas fa-check"></i>
                            </button>
                            <button class="btn btn-danger btn-sm">
                                <i class="fas fa-trash"></i>
                            </button>
                        ` : ''}
                    </td>`).join('')}
            </tr>
        </tbody>
    </table>
`;
 
                    break;                
                    case 'biologico':
                        contentHtml += `
                        <table class="table table-striped">
                            <tbody>
                                <tr>
                                    <th>ID Biológico</th>
                                    ${reportes.map(reporte => `<td id="editidBiologico">${reporte.muestraCompuesta?.biologico?.idBiologico || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Fecha Visita</th>
                                    ${reportes.map(reporte => `<td>
                                        ${isEditing ? 
                                            `<input type="date" class="editFecha_Muestra input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" style="width: 100px" value="${(reporte.muestraCompuesta?.biologico?.Fecha_Muestra || '').split('T')[0]}" />` 
                                            : (reporte.muestraCompuesta?.biologico?.Fecha_Muestra || '').split('T')[0]}
                                    </td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Escherichia Coli</th>
                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editEscherichia_coli_ufc input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" value="${reporte.muestraCompuesta?.biologico?.Escherichia_coli_ufc || ''}" />` : reporte.muestraCompuesta?.biologico?.Escherichia_coli_ufc || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Coliformes Totales (ufc)</th>
                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editColiformes_totales_ufc input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" value="${reporte.muestraCompuesta?.biologico?.Coliformes_totales_ufc || ''}" />` : reporte.muestraCompuesta?.biologico?.Coliformes_totales_ufc || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Coliformes Totales</th>
                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editEscherichia_coli_npm input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" value="${reporte.muestraCompuesta?.biologico?.Escherichia_coli_npm || ''}" />` : reporte.muestraCompuesta?.biologico?.Escherichia_coli_npm || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Indice Biológico</th>
                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editIndice_biologico input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" value="${reporte.muestraCompuesta?.biologico?.Indice_biologico || ''}" />` : reporte.muestraCompuesta?.biologico?.Indice_biologico || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Clasificación</th>
                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editClasificacionIBiologico input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" value="${reporte.muestraCompuesta?.biologico?.ClasificacionIBiologico || ''}" />` : reporte.muestraCompuesta?.biologico?.ClasificacionIBiologico || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Observaciones</th>
                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="text" class="editObservaciones input-normal" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}" value="${reporte.muestraCompuesta?.biologico?.Observaciones || ''}" />` : reporte.muestraCompuesta?.biologico?.Observaciones || ''}</td>`).join('')}
                                </tr>
                                <tr>
                                    <th>Acciones</th>
                                    ${reportes.map(reporte => `
                                        <td>
                                            ${isEditing ? `
                                                <button class="btn btn-success btn-sm btnActualizarDatoBiologico" data-id-biologico="${reporte.muestraCompuesta?.biologico?.idBiologico}">
                                                    <i class="fas fa-check"></i>
                                                </button>
                                                
                                            ` : ''}
                                        </td>`).join('')}
                                </tr>
                            </tbody>
                        </table>
                    `;
                    
                    break;
                    case 'metalAgua':
                        contentHtml += `
                            <table class="table table-striped">
                                <tbody>
                                    <tr>
                                        <th>ID Metal Agua</th>
                                        ${reportes.map(reporte => `<td id="editidMetalAgua">${reporte.muestraCompuesta?.metalAgua?.idMetalAgua || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                               <th>Fecha Visita</th>
                                ${reportes.map(reporte => `<td>
                                    ${isEditing ? 
                                        `<input type="date" class="editFecha_Muestra input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" style="width: 100px" value="${(reporte.muestraCompuesta?.metalAgua?.Fecha_Muestra || '').split('T')[0]}" />` 
                                        : (reporte.muestraCompuesta?.metalAgua?.Fecha_Muestra || '').split('T')[0]}
                                </td>`).join('')}
                            </tr>
                                    <tr>
                                        <th>Niquel</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editNiquel input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Niquel || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Niquel || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                        <th>Cromo hexavalente</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCromo_hexavalente input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Cromo_hexavalente || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Cromo_hexavalente || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                        <th>Cromo</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCromo input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Cromo || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Cromo || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                        <th>Cadmio</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCadmio input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Cadmio || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Cadmio || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                        <th>Cobre</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCobre input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Cobre || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Cobre || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                        <th>Plomo</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editPlomo input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Plomo || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Plomo || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                        <th>Mercurio</th>
                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editMercurio input-normal" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}" value="${reporte.muestraCompuesta?.metalAgua?.Mercurio || ''}" />` :reporte.muestraCompuesta?.metalAgua?.Mercurio || ''}</td>`).join('')}
                                    </tr>
                                    <tr>
                                    <th>Acciones</th>
                                    ${reportes.map(reporte => `
                                        <td>
                                            ${isEditing ? `
                                                <button class="btn btn-success btn-sm btnActualizarDatoMetalAgua" data-id-metalAgua="${reporte.muestraCompuesta?.metalAgua?.idMetalAgua}">
                                                    <i class="fas fa-check"></i>
                                                </button>
                                                
                                            ` : ''}
                                        </td>`).join('')}
                                </tr>
                            </tbody>
                            </table>
                          
                        `;
                        break;
                                case 'metalSedimental':
                                    contentHtml += `
                                        <table class="table table-striped">
                                            <tbody>
                                                <tr>
                                                    <th>ID Metal Sedimento</th>
                                                    ${reportes.map(reporte => `<td id="editidMetalSedimental">${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental || ''}</td>`).join('')}
                                                </tr>
                                                <tr>
                                                <th>Fecha Visita</th>
                                                    ${reportes.map(reporte => `<td>
                                                        ${isEditing ? 
                                                            `<input type="date" class="editFecha_Muestra input-normal" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}" style="width: 100px" value="${(reporte.muestraCompuesta?.MetalSedimental?.Fecha_Muestra || '').split('T')[0]}" />` 
                                                            : (reporte.muestraCompuesta?.MetalSedimental?.Fecha_Muestra || '').split('T')[0]}
                                                    </td>`).join('')}
                                                </tr>
                                                <tr>
                                                    <th>Cadmio sedimentable</th>
                                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCadmio_sedimentable input-normal" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}" value="${reporte.muestraCompuesta?.MetalSedimental?.Cadmio_sedimentable || ''}" />` :reporte.muestraCompuesta?.MetalSedimental?.Cadmio_sedimentable || ''}</td>`).join('')}
                                                </tr>
                                                <tr>
                                                    <th>Cobre sedimentable</th>
                                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCobre_sedimentable input-normal" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}" value="${reporte.muestraCompuesta?.MetalSedimental?.Cobre_sedimentable || ''}" />` :reporte.muestraCompuesta?.MetalSedimental?.Cobre_sedimentable || ''}</td>`).join('')}
                                                </tr>
                                                <tr>
                                                    <th>Cromo sedimentable</th>
                                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editCromo_sedimentable input-normal" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}" value="${reporte.muestraCompuesta?.MetalSedimental?.Cromo_sedimentable || ''}" />` :reporte.muestraCompuesta?.MetalSedimental?.Cromo_sedimentable || ''}</td>`).join('')}
                                                </tr>
                                                <tr>
                                                    <th>Mercurio sedimentable</th>
                                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editMercurio_sedimentable input-normal" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}" value="${reporte.muestraCompuesta?.MetalSedimental?.Mercurio_sedimentable || ''}" />` :reporte.muestraCompuesta?.MetalSedimental?.Mercurio_sedimentable || ''}</td>`).join('')}
                                                </tr>
                                                <tr>
                                                    <th>Plomo_sedimentable</th>
                                                    ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editPlomo_sedimentable input-normal" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}" value="${reporte.muestraCompuesta?.MetalSedimental?.Plomo_sedimentable || ''}" />` :reporte.muestraCompuesta?.MetalSedimental?.Plomo_sedimentable || ''}</td>`).join('')}
                                                </tr>
                                                <tr>
                                                <th>Acciones</th>
                                                ${reportes.map(reporte => `
                                                    <td>
                                                        ${isEditing ? `
                                                            <button class="btn btn-success btn-sm btnActualizarDatoMetalSedimental" data-id-metalSedimental="${reporte.muestraCompuesta?.MetalSedimental?.idMetalSedimental}">
                                                                <i class="fas fa-check"></i>
                                                            </button>
                                                            
                                                        ` : ''}
                                        </td>`).join('')}
                                </tr>
                                                
                                            </tbody>
                                        </table>
                                       
                                    `;
                                    break;
                                    case 'nutrientes':
                                        contentHtml += `
                                            <table class="table table-striped">
                                                <tbody>
                                                    <tr>
                                                        <th>ID Nutrientes</th>
                                                        ${reportes.map(reporte => `<td id="editidNutriente">${reporte.muestraCompuesta?.nutriente?.idNutriente || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                    <th>Fecha Visita</th>
                                                        ${reportes.map(reporte => `<td>
                                                            ${isEditing ? 
                                                                `<input type="date" class="editFecha_Muestra input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" style="width: 100px" value="${(reporte.muestraCompuesta?.nutriente?.Fecha_Muestra || '').split('T')[0]}" />` 
                                                                : (reporte.muestraCompuesta?.nutriente?.Fecha_Muestra || '').split('T')[0]}
                                                        </td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Nitratos</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editNitratos input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" value="${reporte.muestraCompuesta?.nutriente?.Nitratos || ''}" />` :reporte.muestraCompuesta?.nutriente?.Nitratos || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Fosfatos</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editFosfatos input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" value="${reporte.muestraCompuesta?.nutriente?.Fosfatos || ''}" />` :reporte.muestraCompuesta?.nutriente?.Fosfatos || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Fosforo organico</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editFosforo_organico input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" value="${reporte.muestraCompuesta?.nutriente?.Fosforo_organico || ''}" />` :reporte.muestraCompuesta?.nutriente?.Fosforo_organico || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Fosforo total</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editFosforo_total input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" value="${reporte.muestraCompuesta?.nutriente?.Fosforo_total || ''}" />` :reporte.muestraCompuesta?.nutriente?.Fosforo_total || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Nitritos</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class=editNitratos input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" value="${reporte.muestraCompuesta?.nutriente?.Nitritos || ''}" />` :reporte.muestraCompuesta?.nutriente?.Nitritos || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Nitrogeno Organico</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editNitrogeno_organico input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" value="${reporte.muestraCompuesta?.nutriente?.Nitrogeno_organico || ''}" />` :reporte.muestraCompuesta?.nutriente?.Nitrogeno_organico || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                        <th>Nitrogeno total kjeldahl</th>
                                                        ${reportes.map(reporte => `<td>${isEditing ? `<input type="number" class="editNitrogeno_total input-normal" data-id-insitu="${reporte.muestraCompuesta?.nutriente?.idNutriente}" style="width: 20px" value="${reporte.muestraCompuesta?.nutriente?.Nitrogeno_total || ''}" />` :reporte.muestraCompuesta?.nutriente?.Nitrogeno_total_kjeldahl || ''}</td>`).join('')}
                                                    </tr>
                                                    <tr>
                                                    <th>Acciones</th>
                                                    ${reportes.map(reporte => `
                                                        <td>
                                                            ${isEditing ? `
                                                                <button class="btn btn-success btn-sm btnActualizarDatoNutriente" data-id-nutriente="${reporte.muestraCompuesta?.nutriente?.idNutriente}">
                                                                    <i class="fas fa-check"></i>
                                                                </button>
                                                                
                                                            ` : ''}
                                            </td>`).join('')}
                                    </tr>
                                                </tbody>
                                            </table>
                                            
                                        `;
                                        break;
                                        
                            default:
                                contentHtml = '<p>Seleccione una sección para ver los detalles.</p>';
                                break;
                        }
                
    
            muestrasContent.innerHTML = contentHtml; 
        }
    
        $('#modalContainer').modal('show');
    
        mostrarSeccion('insitu');
    }
    $('#btnVolverAcampañas').click(function() {
        window.location.href = '/SIM/REDRIO/REDRIO/Campañas'; 
    });


    var currentStep = 0;
            var steps = $('.step');

            steps.eq(currentStep).addClass('active');

            $('.nextBtn').click(function() {
                var estacionSeleccionada = $('#estacion').val();
                
                if (!estacionSeleccionada) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Advertencia',
                        text: 'Por favor, selecciona una estación antes de continuar.'
                    });
                    return; 
                }
            
                steps.eq(currentStep).removeClass('active');
                currentStep++;
                steps.eq(currentStep).addClass('active');
                updateCircles();
                
            });;

            $('.prevBtn').click(function() {
                steps.eq(currentStep).removeClass('active');
                currentStep--;
                steps.eq(currentStep).addClass('active');
                updateCircles();
            });

            function updateCircles() {
                $('.step-circle').removeClass('completed current');
                $('.step-circle').each(function(index) {
                    if (index < currentStep) {
                        $(this).addClass('completed');
                    } else if (index === currentStep) {
                        $(this).addClass('current');
                    }
                });
            }
    var idCampo;
    $('#btnGuardarCampo').click(function () {
        $('#btnVolver').hide();
        var Hora =$('#Hora').val() || null;
        var TempAmbiente = $('#TempAmbiente').val() || null;
        var TempAgua = $('#TempAgua').val() || null;
        var Ph = $('#Ph').val() || null;
        var Od = $('#Od').val() || null;
        var Cond = $('#Cond').val() || null;
        var Orp = $('#Orp').val() || null;
        var Turb = $('#Turb').val() || null;
        var Tiempo = $('#Tiempo').val() || null;
        var Apariencia = $('#Apariencia').val() || null;
        var Olor = $('#Olor').val() || null;
        var Color = $('#Color').val() || null;
        var Altura = $('#Altura').val() || null;
        var H1 = $('#H1').val() || null;
        var H2 = $('#H2').val() || null;
        var Observacion = $('#Observacion').val() || null;
        var Fecha_Muestra =$('#fechaMuestra').val()|| null

        var idCampaña = getParameterByName('idCampa');

        // if(!Hora || !TempAmbiente || !TempAgua || !Ph || !Od || !Cond || !Orp || !Turb || !Tiempo || !Apariencia || !Olor || !Color || !Altura || !Observacion){
        //     Swal.fire({
        //         icon: 'warning',
        //         title: 'Advertencia',
        //         text: 'No se guardara ningún resultado campo'
        //     })
        //     return;
        // }
        var NuevoCampo = {Hora ,TempAmbiente ,TempAgua ,Ph ,Od ,Cond ,Orp ,Turb ,Tiempo ,Apariencia ,Olor ,Color ,Altura ,H1 ,H2 ,Observacion, Fecha_Muestra};
        console.log('Payload a enviar:', JSON.stringify(NuevoCampo));
        $.ajax({
            url: 'http://localhost:5078/api/resultadocampo/AgregarResultadoLaboratorio',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(NuevoCampo),
            success: function(result){
                Swal.fire({
                    icon: 'success',
                    title: 'Campo Agregado',
                    text: 'se creo  exitosamente'
                });
                console.log(result);
                idCampo = result.result.idCampo; 
                console.log('idCampo:', idCampo)
                console.log(NuevoCampo);
                ;
    
                Swal.fire({
                    title: '¿Qué deseas hacer?',
                    text: 'Puedes continuar con los resultados de laboratorio o finalizar el guardado.',
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonText: 'Continuar con resultados de laboratorio',
                    cancelButtonText: 'Finalizar guardado'
                }).then((result) => {
                    if (result.isConfirmed) {
                        } else {
                        agregarReporteLaboratorio(idEstacion, idCampo, null, idCampaña);
                        $('#vistaPrincipal').show(); 
                    }
                });
            },
            error: function(xhr){
                Swal.fire({
                    icon:'error',
                    title: 'Error',
                    text: 'Ocurrieno un error al internar guardar, intente nuevamente'
                })
            }     
        })
    });


var idMuestraCompuesta;

function agregarMuestraCompuesta(datos) {
    console.log(datos);
    
    if (Array.isArray(datos)) {
        var promises = datos.map(function(dato) {
            return $.ajax({
                url: 'http://localhost:5078/api/MuestraCompuesta/AgregarMuestraCompuesta',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(dato)
            });
        });
        
        return Promise.all(promises).then(function(responses) {
            console.log('Muestras compuestas agregadas exitosamente');
            console.log(responses);
            idMuestraCompuesta = responses[0].result.idMuestraCompuesta;
            return responses.map(function(response) {
                return response.result.idMuestraCompuesta;
            });
        }).catch(function(xhr) {
            console.error('Error al agregar muestras compuestas: ', xhr.status);
            console.log(datos);
        });
    } else {
        console.error('datos no es un arreglo');
    }
}

$('#nextBtn').last().click(function() {
            var idCampaña = getParameterByName('idCampa');
            var estacion = $('#estacion').val();
            var Fecha_Muestra = $('#fechaMuestra').val();

            var OrpInsitu = $('#OrpInsitu').val() || null;
            var Oxigeno_disueltoInsitu = $('#Oxigeno_disuelto').val() || null;
            var TurbiedadInsitu = $('#Turbiedad').val() || null;
            var Tem_ambienteInsitu = $('#Tem_ambiente').val() || null;
            var Tem_aguaInsitu = $('#Tem_agua').val() || null;
            var Conductiviidad_electricaInsitu = $('#Conductiviidad_electrica').val() || null;
            var PhInsitu = $('#PhInsitu').val() || null;

            var CaudalFisico = $('#Caudal').val() || null;
            var ClasificacionCaudalFisico = $('#ClasificacionCaudal').val() || null;
            var NumeroDeVerticalesFisico = $('#NumeroDeVerticales').val() || null;
            var ColorVerdaderoUPCFisico = $('#ColorVerdaderoUPC').val() || null;
            var ColorTriestimular436nmFisico = $('#ColorTriestimular436nm').val() || null;
            var ColorTriestimular525nmFisico = $('#ColorTriestimular525nm').val() || null;
            var ColorTriestimular620nmFisico = $('#ColorTriestimular620nm').val() || null;
            var SolidosSuspendidosTotalesFisico = $('#SolidosSuspendidosTotales').val() || null;
            var SolidosTotalesFisico = $('#SolidosTotales').val() || null;
            var SolidosVolatilesTotalesFisico = $('#SolidosVolatilesTotales').val() || null;
            var SolidosDisueltosTotalesFisico = $('#SolidosDisueltosTotales').val() || null;
            var SolidosFijosTotalesFisico = $('#SolidosFijosTotales').val() || null;
            var SolidosSedimentablesFisico = $('#SolidosSedimentables').val() || null;

            var sustanciaActivaAzulMetilenoQuimico = $('#sustanciaActivaAzulMetileno').val() || null;
            var Grasa_AceiteQuimico = $('#Grasa_Aceite').val() || null;
            var Db05Quimico = $('#Db05').val() || null;
            var Dq0Quimico = $('#Dq0').val() || null;
            var HierroTotalQuimico = $('#HierroTotal').val() || null;
            var SulfatosQuimico = $('#Sulfatos').val() || null;
            var SulfurosQuimico = $('#Sulfuros').val() || null;
            var ClorurosQuimico = $('#Cloruros').val() || null;

            var Nitrogeno_total_kjeldahlNutriente = $('#Nitrogeno_total_kjeldahl').val() || null;
            var Fosforo_organicoNutriente = $('#Fosforo_organico').val() || null;
            var NitratosNutriente = $('#Nitratos').val() || null;
            var Fosforo_totalNutriente = $('#Fosforo_total').val() || null;
            var Nitrogeno_organicoNutriente = $('#Nitrogeno_organico').val() || null;
            var NitritosNutriente = $('#Nitritos').val() || null;
            var FosfatoNutriente = $('#Fosfato').val() || null;

            var CadmioMetalAgua = $('#Cadmio').val() || null;
            var NiquelMetalAgua = $('#Niquel').val() || null;
            var CobreMetalAgua = $('#Cobre').val() || null;
            var MercurioMetalAgua = $('#Mercurio').val() || null;
            var CromoMetalAgua = $('#Cromo').val() || null;
            var PlomoMetalAgua = $('#Plomo').val() || null;
            var Cromo_hexavalenteMetalAgua = $('#Cromo_hexavalente').val() || null;

            var Cadmio_sedimentableMetalSedimento = $('#Cadmio_sedimentable').val() || null;
            var Cobre_sedimentableMetalSedimento = $('#Cobre_sedimentable').val() || null;
            var Cromo_sedimentableMetalSedimento = $('#Cromo_sedimentable').val() || null;
            var Mercurio_sedimentableMetalSedimento = $('#Mercurio_sedimentable').val() || null;
            var Plomo_sedimentableMetalSedimento = $('#Plomo_sedimentable').val() || null;

            var Escherichia_coli_npmBiologico = $('#Escherichia_coli_npm').val() || null;
            var Escherichia_coli_ufcBiologico = $('#Escherichia_coli_ufc').val() || null;
            var Indice_biologicoBiologico = $('#Indice_biologico').val() || null;
            var Coliformes_totales_ufcBiologico = $('#Coliformes_totales_ufc').val() || null;
            var Coliformes_totales_npmBiologico = $('#Coliformes_totales_npm').val() || null;
            var Riquezas_algasBiologico = $('#Riquezas_algas').val() || null;
            var ClasificacionIBiologicoBiologico = $('#ClasificacionIBiologico').val() || null;
            var ObservacionesBiologico = $('#Observaciones').val() || null;

            var allNull = [OrpInsitu, Oxigeno_disueltoInsitu, TurbiedadInsitu, Tem_ambienteInsitu, Tem_aguaInsitu, Conductiviidad_electricaInsitu, PhInsitu, 
                CaudalFisico, ClasificacionCaudalFisico, NumeroDeVerticalesFisico, ColorVerdaderoUPCFisico, ColorTriestimular436nmFisico, 
                ColorTriestimular525nmFisico, ColorTriestimular620nmFisico, SolidosSuspendidosTotalesFisico, SolidosTotalesFisico, 
                SolidosVolatilesTotalesFisico, SolidosDisueltosTotalesFisico, SolidosFijosTotalesFisico, SolidosSedimentablesFisico, 
                sustanciaActivaAzulMetilenoQuimico, Grasa_AceiteQuimico, Db05Quimico, Dq0Quimico, HierroTotalQuimico, SulfatosQuimico, 
                SulfurosQuimico, ClorurosQuimico, Nitrogeno_total_kjeldahlNutriente, Fosforo_organicoNutriente, NitratosNutriente, 
                Fosforo_totalNutriente, Nitrogeno_organicoNutriente, NitritosNutriente, FosfatoNutriente, CadmioMetalAgua, NiquelMetalAgua, 
                CobreMetalAgua, MercurioMetalAgua, CromoMetalAgua, PlomoMetalAgua, Cromo_hexavalenteMetalAgua, Cadmio_sedimentableMetalSedimento, 
                Cobre_sedimentableMetalSedimento, Cromo_sedimentableMetalSedimento, Mercurio_sedimentableMetalSedimento, Plomo_sedimentableMetalSedimento, 
                Escherichia_coli_npmBiologico, Escherichia_coli_ufcBiologico, Indice_biologicoBiologico, Coliformes_totales_ufcBiologico, 
                Coliformes_totales_npmBiologico, Riquezas_algasBiologico, ClasificacionIBiologicoBiologico, ObservacionesBiologico].every(function(element) {
                    return element === null;
                });
                var promises = [];
                var ids = {};
            if (allNull) {
                Swal.fire({
                    title: 'Error',
                    text: 'No se pueden guardar campos vacíos',
                    icon: 'error'
                });
            } else {
                Swal.fire({
                    title: '¿Estás seguro de guardar los cambios?',
                    text: 'Se guardarán todos los cambios realizados en el formulario.',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, guardar cambios',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Solicitud para Insitu
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/Insitu/AgregarInsitu',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                Orp: OrpInsitu,
                                Oxigeno_disuelto: Oxigeno_disueltoInsitu,
                                Turbiedad: TurbiedadInsitu,
                                Tem_ambiente: Tem_ambienteInsitu,
                                Tem_agua: Tem_aguaInsitu,
                                Conductiviidad_electrica: Conductiviidad_electricaInsitu,
                                Ph: PhInsitu
                            })
                        }).then(function(result) {
                            ids[0] = result.result.idInsitu; 
                            // console.log(result);
                            
                        }));
            
                        // Repite para las demás solicitudes
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/Fisico/AgregarFisico',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                Caudal: CaudalFisico,
                                ClasificacionCaudal: ClasificacionCaudalFisico,
                                NumeroDeVerticales: NumeroDeVerticalesFisico,
                                ColorVerdaderoUPC: ColorVerdaderoUPCFisico,
                                ColorTriestimular436nm: ColorTriestimular436nmFisico,
                                ColorTriestimular525nm: ColorTriestimular525nmFisico,
                                ColorTriestimular620nm: ColorTriestimular620nmFisico,
                                SolidosSuspendidosTotales: SolidosSuspendidosTotalesFisico,
                                SolidosTotales: SolidosTotalesFisico,
                                SolidosVolatilesTotales: SolidosVolatilesTotalesFisico,
                                SolidosDisueltosTotales: SolidosDisueltosTotalesFisico,
                                SolidosFijosTotales: SolidosFijosTotalesFisico,
                                SolidosSedimentables: SolidosSedimentablesFisico
                            })
                        }).then(function(result) {
                            ids[1] = result.result.idFisico;
                        }));
            
                        // Quimico
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/Quimico/AgregarQuimico',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                sustanciaActivaAzulMetileno: sustanciaActivaAzulMetilenoQuimico,
                                Grasa_Aceite: Grasa_AceiteQuimico,
                                Db05: Db05Quimico,
                                Dq0: Dq0Quimico,
                                HierroTotal: HierroTotalQuimico,
                                Sulfatos: SulfatosQuimico,
                                Sulfuros: SulfurosQuimico,
                                Cloruros: ClorurosQuimico
                            })
                        }).then(function(result) {
                            ids[2] = result.result.idQuimico;
                        }));
            
                        // Nutriente
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/Nutriente/AgregarNutriente',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                Nitrogeno_total_kjeldahl: Nitrogeno_total_kjeldahlNutriente,
                                Fosforo_organico: Fosforo_organicoNutriente,
                                Nitratos: NitratosNutriente,
                                Fosforo_total: Fosforo_totalNutriente,
                                Nitrogeno_organico: Nitrogeno_organicoNutriente,
                                Nitritos: NitritosNutriente,
                                Fosfato: FosfatoNutriente
                            })
                        }).then(function(result) {
                            ids[3] = result.result.idNutriente;
                        }));
            
                        // MetalAgua
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/MetalAgua/AgregarMetal',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                Cadmio: CadmioMetalAgua,
                                Niquel: NiquelMetalAgua,
                                Cobre: CobreMetalAgua,
                                Mercurio: MercurioMetalAgua,
                                Cromo: CromoMetalAgua,
                                Plomo: PlomoMetalAgua,
                                Cromo_hexavalente: Cromo_hexavalenteMetalAgua
                            })
                        }).then(function(result) {
                            ids[4] = result.result.idMetalAgua;
                        }));
            
                        // MetalSedimental
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/MetalSedimental/AgregarMetalSedimental',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                Cadmio_sedimentable: Cadmio_sedimentableMetalSedimento,
                                Cobre_sedimentable: Cobre_sedimentableMetalSedimento,
                                Cromo_sedimentable: Cromo_sedimentableMetalSedimento,
                                Mercurio_sedimentable: Mercurio_sedimentableMetalSedimento,
                                Plomo_sedimentable: Plomo_sedimentableMetalSedimento
                            })
                        }).then(function(result) {
                            ids[5] = result.result.idMetalSedimental;
                            console.log(result);
                            
                        }));
            
            
                       
            // Biologico
                        promises.push($.ajax({
                            url: 'http://localhost:5078/api/Biologico/AgregarBiologico',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify({
                                Escherichia_coli_npm: Escherichia_coli_npmBiologico,
                                Escherichia_coli_ufc: Escherichia_coli_ufcBiologico,
                                Indice_biologico: Indice_biologicoBiologico,
                                Coliformes_totales_ufc: Coliformes_totales_ufcBiologico,
                                Coliformes_totales_npm: Coliformes_totales_npmBiologico,
                                Riquezas_algas: Riquezas_algasBiologico,
                                ClasificacionIBiologico: ClasificacionIBiologicoBiologico,
                                Observaciones: ObservacionesBiologico
                            })
                        }).then(function(result) {
                            ids[6] = result.result.idBiologico;
                        }));
            
                        Promise.all(promises).then(function() {
                            console.log('Todos los datos guardados. IDs:', ids);
                            return agregarMuestraCompuesta([{
                                idInsitu: ids[0],
                                idFisico: ids[1],
                                idQuimico: ids[2],
                                idNutriente: ids[3],
                                idMetalAgua: ids[4],
                                idMetalSedimental: ids[5],
                                idBiologico: ids[6],
                                Fecha_Muestra: Fecha_Muestra
                            }]);
                        }).then(function(muestraCompuestaIds) {
                            console.log('IDs de muestras compuestas:', muestraCompuestaIds);
                            Swal.fire('Éxito', 'Todos los datos han sido guardados correctamente', 'success');
                            agregarReporteLaboratorio(idEstacion, idCampo, idMuestraCompuesta, idCampaña);
                        }).catch(function(error) {
                            console.error('Error al guardar los datos:', error);
                            Swal.fire('Error', 'Hubo un problema al guardar los datos', 'error');
                        });
                       

                    }
                    
            });
            
            }});
        



function agregarReporteLaboratorio(idEstacion, idCampo, idMuestraCompuesta, idCampaña) {
    $.ajax({
        url: 'http://localhost:5078/api/ReportesLaboratorio/AgregarReporteLaboratorio',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            idEstacion: idEstacion,
            idResultadoCampo: idCampo,
            idMuestraCompuesta: idMuestraCompuesta,
            idCampaña: idCampaña
        }),
        success: function(result){
            console.log(result);
            location.reload()

            $('#vistaPrincipal').show(); 
            $('#vistaAgregar').hide();
            obtenerReportesLaboratorio();
   

        },
        error: function(xhr){
        }
    });
}

function cargarExcelPorCampañas() {
    var idCampaña = getParameterByName('idCampa'); 
        
    console.log("ID Campaña obtenido:", idCampaña);
    
    if (!idCampaña) {
        console.error("El parámetro idCampa no está presente en la URL.");
        alert("Error: el parámetro idCampa no está especificado.");
        return; 
    }

    var apiUrl = 'http://localhost:5078/api/HistorialExcel/ObtenerHistorialExcelPorCampaña/' + idCampaña;

    $.getJSON(apiUrl, function (data) {
        var listHistorialExcelPorCampaña = data.result; 
        console.log('Excel', data);

        if ($("#gridHistorial").data('dxDataGrid')) {
            $("#gridHistorial").dxDataGrid("instance").option("dataSource", listHistorialExcelPorCampaña);
        } else {
            $("#gridHistorial").dxDataGrid({
                dataSource: listHistorialExcelPorCampaña,
                columns: [
                    { dataField: "nombreUsuario", caption: "Usuario", width: 350, },
                    { dataField: "nombre", caption: "Archivo", width: 350, },
                    { dataField: "fecha_cargue", caption: "Fecha",  dataType: "date", width: 350, },
                    {
                        caption: "Acciones",
                        width: 150,
                        cellTemplate: function (container, options) {
                            var downloadButton = $("<i>")
                                .addClass("fas fa-download")
                                .attr("title", "Descargar")
                                .css({ "cursor": "pointer", "margin-right": "10px" })
                                .click(function () {
                                    var idHistorialExcel = options.data.idHistorialExcel; 
                                    
                                    var downloadUrl = 'http://localhost:5078/api/HistorialExcel/DescargarHistorialExcel/' + idHistorialExcel;
                                    
                                    window.location.href = downloadUrl;
                                });

                            container.append(downloadButton);
                        }
                    }
                ],
                paging: {
                    pageSize: 5
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                searchPanel: {
                    visible: true,
                    placeholder: "Buscar..."
                }
            });
        }
    }).fail(function () {
        // alert('Error al cargar campañas.');
    });
}


$(document).on('click', '#btnActualizarDato', function () {
    var $row = $(this).closest('tr');

    var idCampo = $row.find('td[id="editid"]').text();

    var Fecha_Muestra = $row.find('#editFechaMuestra').val() || null;
    var Hora = $row.find('#editHora').val() || null;
    var Altura = $row.find('#editAltura').val() || null;
    var Apariencia = $row.find('#editApariencia').val() || null;
    var Color = $row.find('#editColor').val() || null;
    var Cond = $row.find('#editCond').val() || null;
    var Observacion = $row.find('#editObservacion').val() || null;
    var Od = $row.find('#editOd').val() || null;
    var Olor = $row.find('#editOlor').val() || null;
    var Orp = $row.find('#editOrp').val() || null;
    var Ph = $row.find('#editPh').val() || null;
    var TempAgua = $row.find('#editTempAgua').val() || null;
    var TempAmbiente = $row.find('#editTempAmbiente').val() || null;
    var Tiempo = $row.find('#editTiempo').val() || null;
    var Turb = $row.find('#editTurb').val() || null;
    var H1 = $row.find('#editH1').val() || null;
    var H2 = $row.find('#editH2').val() || null;

    if (!idCampo) {
        alert('ID de Campo no encontrado.');
        return;
    }

    var CampoActualizada = { 
        Fecha_Muestra,
        Hora,
        Altura,
        Apariencia,
        Color,
        Cond,
        Observacion,
        Od,
        Olor,
        Orp,
        Ph,
        TempAgua,
        TempAmbiente,
        Tiempo,
        Turb,
        H1,
        H2
    };

    $.ajax({
        url: "http://localhost:5078/api/ResultadoCampo/ActualizarResultadoLaboratorio/" + idCampo,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(CampoActualizada),
        success: function (response) {
            if (response.isSuccess) {
                Swal.fire({
                    icon: 'success',
                    title: 'Actualización exitosa',
                }).then(() => {
                    $('#vistaEditar').hide();
                    obtenerReportesLaboratorio();

                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al actualizar',
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
            });
        }
    });
});  
$(document).on('click', '.btnActualizarDatoInsitu', function () {
    var idInsitu = $(this).data('id-insitu');
    var parentContainer = $(this).closest('tr').parent();

    var Fecha_Muestra = parentContainer.find(`.editFecha_Muestra[data-id-insitu="${idInsitu}"]`).val() || null;
    var Conductiviidad_electrica = parentContainer.find(`.editConductiviidad_electrica[data-id-insitu="${idInsitu}"]`).val() || null;
    var Oxigeno_disuelto = parentContainer.find(`.editOxigeno_disuelto[data-id-insitu="${idInsitu}"]`).val() || null;
    var PhInsitu = parentContainer.find(`.editPh[data-id-insitu="${idInsitu}"]`).val() || null;
    var Tem_agua = parentContainer.find(`.editTemp_agua[data-id-insitu="${idInsitu}"]`).val() || null;
    var Temp_ambiente = parentContainer.find(`.editTemp_ambiente[data-id-insitu="${idInsitu}"]`).val() || null;
    var Turbiedad = parentContainer.find(`.editTurbiedad[data-id-insitu="${idInsitu}"]`).val() || null;
    var OrpInsitu = parentContainer.find(`.editOrpInsitu[data-id-insitu="${idInsitu}"]`).val() || null;

if (!idInsitu) {
    alert('ID de Campo no encontrado.');
    return;
}

var InsituActualizado = { 
    Fecha_Muestra,
    Conductiviidad_electrica,
    Oxigeno_disuelto,
    PhInsitu,
    Tem_agua,
    Temp_ambiente,
    Turbiedad,
    OrpInsitu
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/Insitu/ActualizarInsitu/" + idInsitu,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(InsituActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                 
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

                
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});
$(document).on('click', '.btnActualizarDatoQuimico', function () {
    var idQuimico = $(this).data('id-quimico');
    var parentContainer = $(this).closest('table');
    var Fecha_Muestra = parentContainer.find(`.editFecha_Muestra[data-id-quimico="${idQuimico}"]`).val() || null;
    var Cloruros = parentContainer.find(`.editCloruros[data-id-quimico="${idQuimico}"]`).val() || null;
    var Db05 = parentContainer.find(`.editDb05[data-id-quimico="${idQuimico}"]`).val() || null;
    var Dq0 = parentContainer.find(`.editDq0[data-id-quimico="${idQuimico}"]`).val() || null;
    var SustanciaActivaAzulMetileno = parentContainer.find(`.editSustanciaActivaAzulMetileno[data-id-quimico="${idQuimico}"]`).val() || null;
    var Gasa_Aceite = parentContainer.find(`.editGasa_Aceite[data-id-quimico="${idQuimico}"]`).val() || null;
    var HierroTotal = parentContainer.find(`.editHierroTotal[data-id-quimico="${idQuimico}"]`).val() || null;
    var Sulfatos = parentContainer.find(`.editSulfatos[data-id-quimico="${idQuimico}"]`).val() || null;
    var Sulfuros = parentContainer.find(`.editSulfuros[data-id-quimico="${idQuimico}"]`).val() || null;

if (!idQuimico) {
    alert('ID de Campo no encontrado.');
    return;
}

var QuimicoActualizado = { 
    Fecha_Muestra,
    Cloruros,
    Db05,
    Dq0,
    SustanciaActivaAzulMetileno,
    Gasa_Aceite,
    HierroTotal,
    Sulfatos,
    Sulfuros
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/Quimico/ActualizarQuimico/" + idQuimico,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(QuimicoActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                 
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});
$(document).on('click', '.btnActualizarDatoFisico', function () {
    var idFisico = $(this).data('id-fisico');
    var parentContainer = $(this).closest('table');
    var Fecha_Muestra = parentContainer.find(`.editFecha_Muestra[data-id-fisico="${idFisico}"]`).val() || null;
    var Caudal = parentContainer.find(`.editCaudal[data-id-fisico="${idFisico}"]`).val() || null;
    var ColorTriestimular436nm = parentContainer.find(`.editColorTriestimular436nm[data-id-fisico="${idFisico}"]`).val() || null;
    var ColorVerdaderoUPC = parentContainer.find(`.editColorVerdaderoUPC[data-id-fisico="${idFisico}"]`).val() || null;
    var ColorTriestimular525nm = parentContainer.find(`.editColorTriestimular525nm[data-id-fisico="${idFisico}"]`).val() || null;
    var ClasificacionCaudal = parentContainer.find(`.editClasificacionCaudal[data-id-fisico="${idFisico}"]`).val() || null;
    var ColorTriestimular620nm = parentContainer.find(`.editColorTriestimular620nm[data-id-fisico="${idFisico}"]`).val() || null;
    var NumeroDeVerticales = parentContainer.find(`.editNumeroDeVerticales[data-id-fisico="${idFisico}"]`).val() || null;
    var SolidosDisueltosTotales = parentContainer.find(`.editSolidosDisueltosTotales[data-id-fisico="${idFisico}"]`).val() || null;
    var SolidosFijosTotales = parentContainer.find(`.editSolidosFijosTotales[data-id-fisico="${idFisico}"]`).val() || null;
    var SolidosSedimentables = parentContainer.find(`.editSolidosSedimentables[data-id-fisico="${idFisico}"]`).val() || null;
    var SolidosSuspendidosTotales = parentContainer.find(`.editSolidosSuspendidosTotales[data-id-fisico="${idFisico}"]`).val() || null;
    var SolidosTotales = parentContainer.find(`.editSolidosTotales[data-id-fisico="${idFisico}"]`).val() || null;
    var SolidosVolatilesTotales = parentContainer.find(`.editSolidosVolatilesTotales[data-id-fisico="${idFisico}"]`).val() || null;

    
if (!idFisico) {
    alert('ID de Campo no encontrado.');
    return;
}

var FisicoActualizado = { 
    Fecha_Muestra,
    Caudal,
    ColorTriestimular436nm,
    ColorVerdaderoUPC,
    Ph,
    ColorTriestimular525nm,
    ClasificacionCaudal,
    ColorTriestimular620nm,
    NumeroDeVerticales,
    SolidosDisueltosTotales,
    SolidosFijosTotales,
    SolidosSedimentables,
    SolidosSuspendidosTotales,
    SolidosTotales,
    SolidosVolatilesTotales
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/Fisico/ActualizarFisico/" + idFisico,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(FisicoActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                 
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});
$(document).on('click', '.btnActualizarDatoBiologico', function () {
    var idBiologico = $(this).data('id-biologico')
    var parentContainer = $(this).closest('table');
    var Fecha_Muestra = parentContainer.find(`.editFecha_Muestra[data-id-biologico="${idBiologico}"]`).val() || null;
    var Escherichia_coli_ufc = parentContainer.find(`.editEscherichia_coli_ufc[data-id-biologico="${idBiologico}"]`).val() || null;
    var Coliformes_totales_ufc = parentContainer.find(`.editColiformes_totales_ufc[data-id-biologico="${idBiologico}"]`).val() || null;
    var Escherichia_coli_npm = parentContainer.find(`.editEscherichia_coli_npm[data-id-biologico="${idBiologico}"]`).val() || null;
    var Indice_biologico = parentContainer.find(`.editIndice_biologico[data-id-biologico="${idBiologico}"]`).val() || null;
    var ClasificacionIBiologico = parentContainer.find(`.editClasificacionIBiologico[data-id-biologico="${idBiologico}"]`).val() || null;
    var Observaciones = parentContainer.find(`.editObservaciones[data-id-biologico="${idBiologico}"]`).val() || null;

    
if (!idBiologico) {
    alert('ID de Biologico no encontrado.');
    return;
}

var BiologicoActualizado = { 
    Fecha_Muestra,
    Escherichia_coli_ufc,
    Coliformes_totales_ufc,
    Escherichia_coli_npm,
    Indice_biologico,
    ClasificacionIBiologico,
    Observaciones
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/Biologico/ActualizarBiologico/" + idBiologico,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(BiologicoActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                 
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});

$(document).on('click', '.btnActualizarDatoMetalAgua', function () {
    var idMetalAgua = $(this).attr('data-id-metalAgua');
    console.log('ID capturado:', idMetalAgua);
    var parentContainer = $(this).closest('table');

    var Fecha_Muestra = parentContainer.find(`.editFecha_Muestra[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Niquel = parentContainer.find(`.editNiquel[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Cromo_hexavalente = parentContainer.find(`.editCromo_hexavalente[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Cromo = parentContainer.find(`.editCromo[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Cadmio = parentContainer.find(`.editCadmio[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Cobre = parentContainer.find(`.editCobre[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Plomo = parentContainer.find(`.editPlomo[data-id-metalAgua="${idMetalAgua}"]`).val() || null;
    var Mercurio = parentContainer.find(`.editMercurio[data-id-metalAgua="${idMetalAgua}"]`).val() || null;


    
if (!idMetalAgua) {
    alert('ID de metal agua  no encontrado.');
    console.log(idMetalAgua);
    
    return;
}

var MetalAguaActualizado = { 
    Fecha_Muestra,
    Niquel,
    Cromo_hexavalente,
    Cromo,
    Cadmio,
    Cobre,
    Plomo,
    Mercurio
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/MetalAgua/ActualizarMetalAgua/" + idMetalAgua,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(MetalAguaActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                 
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

            });
        } else {    
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});
$(document).on('click', '.btnActualizarDatoMetalSedimental', function () {
    var idMetalSedimental = $(this).attr('data-id-metalSedimental');
    var Fecha_Muestra = $(`.editFecha_Muestra[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Cadmio_sedimentable = $(`.editCadmio_sedimentable[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Cobre_sedimentable = $(`.editCobre_sedimentable[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Cromo_sedimentable = $(`.editCromo_sedimentable[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Cadmio = $(`.editCadmio[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Cobre = $(`.editCobre[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Plomo_sedimentable = $(`.editPlomo_sedimentable[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;
var Mercurio_sedimentable = $(`.editMercurio_sedimentable[data-id-metalSedimental="${idMetalSedimental}"]`).val() || null;

    
if (!idMetalSedimental) {
    alert('ID de Campo no encontrado.');
    return;
}

var MetalSedimentalActualizado = { 
    Fecha_Muestra,
    Cadmio_sedimentable,
    Cobre_sedimentable,
    Cromo_sedimentable,
    Cadmio,
    Cobre,
    Plomo_sedimentable,
    Mercurio_sedimentable
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/MetalSedimental/ActualizarMetalSedimental/" + idMetalSedimental,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(MetalSedimentalActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                 
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});
$(document).on('click', '.btnActualizarDatoNutriente', function () {
    var idNutriente = $(this).data('id-nutriente');
    var Fecha_Muestra = $(`#editFecha_Muestra`).val()|| null;
    var Nitratos = $(`#editNitratos`).val()|| null;
    var Fosfatos = $(`#editFosfatos`).val()|| null;
    var Fosforo_organico = $(`#editFosforo_organico`).val()|| null;
    var Fosforo_total = $(`#editFosforo_total`).val()|| null;
    var Nitratos = $(`#editNitratos`).val()|| null;
    var Nitrogeno_organico = $(`#editNitrogeno_organico`).val()|| null;
    var Nitrogeno_total = $(`#editNitrogeno_total`).val()|| null;

    
if (!idNutriente) {
    alert('ID de Campo no encontrado.');
    return;
}

var NutrienteActualizado = { 
    Fecha_Muestra,
    Nitratos,
    Fosfatos,
    Fosforo_organico,
    Fosforo_total,
    Nitratos,
    Nitrogeno_organico,
    Nitrogeno_total
    
};

// var urlBase = $("#SIM").data("url");
// var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ResultadoCampo/ActualizarResultadoLaboratorio" + idCampaña; 

$.ajax({
    url: "http://localhost:5078/api/Nutriente/ActualizarNutriente/" + idNutriente,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(NutrienteActualizado),
    success: function (response) {
        if (response.isSuccess) {
            Swal.fire({
                icon: 'success',
                title: 'Actualización exitosa',
            }).then(() => {
                $('#vistaEditar').hide();
                        obtenerReportesLaboratorio();

            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Error al actualizar',
            });
            console.log(urlCompleta);
        }
    },
    error: function () {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Error al actualizar la Campo. Inténtelo nuevamente.'
        });
    }
});

});


$(document).on('click', '#btnDescargarExcel', function () {
    Swal.fire({
        title: '¿Qué tipo de resultados deseas descargar?',
        showCancelButton: true,
        confirmButtonText: 'Resultados de Campo',
        cancelButtonText: 'Resultados de Laboratorio'
    }).then((result) => {
        if (result.isConfirmed) {
            generarExcel('campo');
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            generarExcel('laboratorio');
        }
    });
    
});

function generarExcel(tipo) {
    const reportes = ListaReportes; 
    if (!reportes || !Array.isArray(reportes)) {
        console.error("ListaReportes no está definido o no es un arreglo.");
        return;
    }

    let datosPorEstacion = {};

    if (tipo === 'campo') {
        reportes.forEach(reporte => {
            function formatearFecha(fechaISO) {
                if (!fechaISO) return '';
                
                const fecha = new Date(fechaISO);
                const dia = String(fecha.getDate()).padStart(2, '0');
                const mes = String(fecha.getMonth() + 1).padStart(2, '0');
                const anio = fecha.getFullYear();
                
                return `${dia}-${mes}-${anio}`;
            }
            
            const resultadoCampo = reporte.resultadoCampo;
            if (resultadoCampo) {
                const nombreEstacion = reporte.estacion.nombreEstacion || 'Sin nombre';
                if (!datosPorEstacion[nombreEstacion]) {
                    datosPorEstacion[nombreEstacion] = [];
                }
                datosPorEstacion[nombreEstacion].push({
                    'Fecha Visita': formatearFecha(resultadoCampo.Fecha_Muestra),
                    'Hora': resultadoCampo.Hora || '',
                    'Temperatura Ambiente (°C)': resultadoCampo.TempAmbiente !== undefined ? resultadoCampo.TempAmbiente : '',
                    'Temperatura Agua (°C)': resultadoCampo.TempAgua !== undefined ? resultadoCampo.TempAgua : '',
                    'pH (U. de pH)': resultadoCampo.Ph !== undefined ? resultadoCampo.Ph : '',
                    'Oxígeno disuelto (mg/L)': resultadoCampo.Od !== undefined ? resultadoCampo.Od : '',
                    'Conductividad eléctrica (µS/cm)': resultadoCampo.Cond !== undefined ? resultadoCampo.Cond : '',
                    'Potencial Redox (mV)': resultadoCampo.Orp !== undefined ? resultadoCampo.Orp : '',
                    'Turbiedad (NTU)': resultadoCampo.Turb !== undefined ? resultadoCampo.Turb : '',
                    'Estado del tiempo': resultadoCampo.Tiempo || '',
                    'Apariencia': resultadoCampo.Apariencia || '',
                    'Olor': resultadoCampo.Olor || '',
                    'Color': resultadoCampo.Color || '',
                    'Altura (m)': resultadoCampo.Altura !== undefined ? resultadoCampo.Altura : '',
                    'H1 (m)': resultadoCampo.H1 !== undefined ? resultadoCampo.H1 : '',
                    'H2 (m)': resultadoCampo.H2 !== undefined ? resultadoCampo.H2 : '',
                    'Observación': resultadoCampo.Observacion || ''
                });
            }
        });
    } else if (tipo === 'laboratorio') {
        const datosLaboratorio = [];

        function formatearFecha(fecha) {
            if (!fecha) return 'Sin datos';
            
            const date = new Date(fecha);
            
            // Verificamos que la fecha sea válida
            if (isNaN(date)) return 'Fecha no válida';
        
            // Extraemos día, mes y año
            const dia = String(date.getDate()).padStart(2, '0');
            const mes = String(date.getMonth() + 1).padStart(2, '0'); // Los meses en JavaScript son 0-indexados
            const año = date.getFullYear();
        
            return `${dia}-${mes}-${año}`;
        }
        reportes.forEach(reporte => {
            const laboratorio = reporte.muestraCompuesta;
            const estacion = reporte.estacion;
            
            if (laboratorio && estacion) {
                const fechaMuestraOriginal = laboratorio.insitu?.Fecha_Muestra || 
                                     laboratorio.fisico?.Fecha_Muestra || 
                                     laboratorio.quimico?.Fecha_Muestra || 
                                     laboratorio.nutriente?.Fecha_Muestra || 
                                     laboratorio.metalAgua?.Fecha_Muestra || 
                                     laboratorio.MetalSedimental?.Fecha_Muestra || 
                                     laboratorio.biologico?.Fecha_Muestra || 
                                     null;
        
        const fechaMuestra = formatearFecha(fechaMuestraOriginal);

                const fila = {
                    'Fecha Muestra': fechaMuestra, // Usamos la fecha obtenida
                    'Estación': estacion.nombreEstacion || 'Sin nombre',
                    'Código': estacion.codigo || 'N/A',
                    'Temperatura Ambiente (°C)': laboratorio.insitu?.Temp_ambiente !== undefined ? laboratorio.insitu.Temp_ambiente : 'Sin datos',
                    'Temperatura Agua (°C)': laboratorio.insitu?.Tem_agua !== undefined ? laboratorio.insitu.Tem_agua : 'Sin datos',
                    'pH (U. de pH)': laboratorio.insitu?.PhInsitu !== undefined ? laboratorio.insitu.PhInsitu : 'Sin datos',
                    'Oxígeno disuelto (mg/L)': laboratorio.insitu?.Oxigeno_disuelto !== undefined ? laboratorio.insitu.Oxigeno_disuelto : 'Sin datos',
                    'Conductividad eléctrica (µS/cm)': laboratorio.insitu?.Conductiviidad_electrica !== undefined ? laboratorio.insitu.Conductiviidad_electrica : 'Sin datos',
                    'Potencial Redox (mV)': laboratorio.insitu?.OrpInsitu !== undefined ? laboratorio.insitu.OrpInsitu : 'Sin datos',
                    'Turbiedad (NTU)': laboratorio.insitu?.Turbiedad !== undefined ? laboratorio.insitu.Turbiedad : 'Sin datos',
                    'Caudal (m3/s)': laboratorio.fisico?.Caudal !== undefined ? laboratorio.fisico.Caudal : 'Sin datos',
                    'Clasificación caudal (Adim)': laboratorio.fisico?.ClasificacionCaudal || 'Sin clasificación',
                    'Número de verticales': laboratorio.fisico?.NumeroDeVerticales !== undefined ? laboratorio.fisico.NumeroDeVerticales : 'Sin datos',
                    'Color verdadero (UPC)': laboratorio.fisico?.ColorVerdaderoUPC || 'Sin color',
                    'Color triestimular 436 nm': laboratorio.fisico?.ColorTriestimular436nm || 'Sin color',
                    'Color triestimular 525 nm': laboratorio.fisico?.ColorTriestimular525nm || 'Sin color',
                    'Color triestimular 620 nm': laboratorio.fisico?.ColorTriestimular620nm || 'Sin color',
                    'Sólidos suspendidos totales (mg/L)': laboratorio.fisico?.SolidosSuspendidosTotales !== undefined ? laboratorio.fisico.SolidosSuspendidosTotales : 'Sin datos',
                    'Sólidos totales (mg/L)': laboratorio.fisico?.SolidosTotales !== undefined ? laboratorio.fisico.SolidosTotales : 'Sin datos',
                    'Sólidos volátiles totales (mg/L)': laboratorio.fisico?.SolidosVolatilesTotales !== undefined ? laboratorio.fisico.SolidosVolatilesTotales : 'Sin datos',
                    'Sólidos disueltos totales (mg/L)': laboratorio.fisico?.SolidosDisueltosTotales !== undefined ? laboratorio.fisico.SolidosDisueltosTotales : 'Sin datos',
                    'Sólidos fijos totales (mg/L)': laboratorio.fisico?.SolidosFijosTotales !== undefined ? laboratorio.fisico.SolidosFijosTotales : 'Sin datos',
                    'Sólidos sedimentables (ml/L-h)': laboratorio.fisico?.SolidosSedimentables !== undefined ? laboratorio.fisico.SolidosSedimentables : 'Sin datos',
                    'DBO5 (mg/L)': laboratorio.quimico?.Db05 !== undefined ? laboratorio.quimico.Db05 : 'Sin datos',
                    'DQO (mg/L)': laboratorio.quimico?.Dq0 !== undefined ? laboratorio.quimico.Dq0 : 'Sin datos',
                    'Hierro total (mg Fe/L)': laboratorio.quimico?.HierroTotal !== undefined ? laboratorio.quimico.HierroTotal : 'Sin datos',
                    'Sulfatos (mg/L)': laboratorio.quimico?.Sulfatos !== undefined ? laboratorio.quimico.Sulfatos : 'Sin datos',
                    'Sulfuros (mg/L)': laboratorio.quimico?.Sulfuros !== undefined ? laboratorio.quimico.Sulfuros : 'Sin datos',
                    'Clororus (mg/L)': laboratorio.quimico?.Cloruros !== undefined ? laboratorio.quimico.Cloruros : 'Sin datos',
                    'Grasas y/o aceites (mg/L)': laboratorio.quimico?.Grasa_Aceite !== undefined ? laboratorio.quimico.Grasa_Aceite : 'Sin datos',
                    'SAAM (mg/L)': laboratorio.quimico?.sustanciaActivaAzulMetileno !== undefined ? laboratorio.quimico.sustanciaActivaAzulMetileno : 'Sin datos',
                    'Fósforo total (mg P/L)': laboratorio.nutriente?.Fosforo_total !== undefined ? laboratorio.nutriente.Fosforo_total : 'Sin datos',
                    'Fosfato (mg P/L)': laboratorio.nutriente?.Fosfato !== undefined ? laboratorio.nutriente.Fosfato : 'Sin datos',
                    'Fósforo orgánico (mg P/L)': laboratorio.nutriente?.Fosforo_organico !== undefined ? laboratorio.nutriente.Fosforo_organico : 'Sin datos',
                    'Nitratos (mg N/L)': laboratorio.nutriente?.Nitratos !== undefined ? laboratorio.nutriente.Nitratos : 'Sin datos',
                    'Nitritos (mg N/L)': laboratorio.nutriente?.Nitritos !== undefined ? laboratorio.nutriente.Nitritos : 'Sin datos',
                    'Nitrógeno orgánico (mg N/L)': laboratorio.nutriente?.Nitrogeno_organico !== undefined ? laboratorio.nutriente.Nitrogeno_organico : 'Sin datos',
                    'Nitrógeno total Kjeldahl (mg N/L)': laboratorio.nutriente?.Nitrogeno_total_kjeldahl !== undefined ? laboratorio.nutriente.Nitrogeno_total_kjeldahl : 'Sin datos',
                    'Cadmio (mg Cd/L)': laboratorio.metalAgua?.Cadmio !== undefined ? laboratorio.metalAgua.Cadmio : 'Sin datos',
                    'Cobre (mg Cu/L)': laboratorio.metalAgua?.Cobre !== undefined ? laboratorio.metalAgua.Cobre : 'Sin datos',
                    'Cromo (mg Cr/L)': laboratorio.metalAgua?.Cromo !== undefined ? laboratorio.metalAgua.Cromo : 'Sin datos',
                    'Cromo hexavalente (mg Cr6+/L)': laboratorio.metalAgua?.Cromo_hexavalente !== undefined ? laboratorio.metalAgua.Cromo_hexavalente : 'Sin datos',
                    'Mercurio (mg Hg/L)': laboratorio.metalAgua?.Mercurio !== undefined ? laboratorio.metalAgua.Mercurio : 'Sin datos',
                    'Niquel (mg Ni/L)': laboratorio.metalAgua?.Niquel !== undefined ? laboratorio.metalAgua.Niquel : 'Sin datos',
                    'Plomo (mg Pb/L)': laboratorio.metalAgua?.Plomo !== undefined ? laboratorio.metalAgua.Plomo : 'Sin datos',
                    'Cadmio sedimentable (mg Cd/L)': laboratorio.MetalSedimental?.Cadmio_sedimentable !== undefined ? laboratorio.MetalSedimental.Cadmio_sedimentable : 'Sin datos',
                    'Cobre sedimentable (mg Cu/L)': laboratorio.MetalSedimental?.Cobre_sedimentable !== undefined ? laboratorio.MetalSedimental.Cobre_sedimentable : 'Sin datos',
                    'Cromo sedimentable (mg Cr/L)': laboratorio.MetalSedimental?.Cromo_sedimentable !== undefined ? laboratorio.MetalSedimental.Cromo_sedimentableMetalSedimento : 'Sin datos',
                    'Mercurio sedimentable (mg Hg/L)': laboratorio.MetalSedimental?.Mercurio_sedimentable !== undefined ? laboratorio.MetalSedimental.Mercurio_sedimentable : 'Sin datos',
                    'Plomo sedimentable (mg Pb/L)': laboratorio.MetalSedimental?.Plomo_sedimentable !== undefined ? laboratorio.MetalSedimental.Plomo_sedimentable : 'Sin datos',
                    'Escherichia coli (UFC)': laboratorio.biologico?.Escherichia_coli_ufc !== undefined ? laboratorio.biologico.Escherichia_coli_ufc : 'Sin datos',
                    'Coliformes totales (UFC)': laboratorio.biologico?.Coliformes_totales_ufc !== undefined ? laboratorio.biologico.Coliformes_totales_ufc : 'Sin datos',
                    'Escherichia coli (NMP/100mL)': laboratorio.biologico?.Escherichia_coli_npm !== undefined ? laboratorio.biologico.Escherichia_coli_npm : 'Sin datos',
                    'Coliformes totales (NMP/100mL)': laboratorio.biologico?.Coliformes_totales_npm !== undefined ? laboratorio.biologico.Coliformes_totales_npm : 'Sin datos',
                    'Riqueza algas': laboratorio.biologico?.Riquezas_algas !== undefined ? laboratorio.biologico.Riquezas_algas : 'Sin datos',
                    'Indice biológico BMWP': laboratorio.biologico?.Indice_biologico !== undefined ? laboratorio.biologico.Indice_biologico : 'Sin datos',
                    'Clasificación Indice biológico BMWP': laboratorio.biologico?.ClasificacionIBiologico !== undefined ? laboratorio.biologico.ClasificacionIBiologico : 'Sin datos',
                    'Observaciones (algas y macroinvertebrados)': laboratorio.biologico?.Observaciones !== undefined ? laboratorio.biologico.Observaciones : 'Sin datos',
                };
        
                datosLaboratorio.push(fila);
            }
        });
        


        datosPorEstacion['Laboratorio'] = datosLaboratorio; 
    }

    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Datos');

    const headerStyle = {
        fill: { 
            type: 'pattern', 
            pattern: 'solid', 
            fgColor: { argb: 'FFA2A4A5' } 
        },
        font: { 
            bold: true, 
            size: 10, 
            color: { argb: 'FF000000' } 
        },
        alignment: { horizontal: 'center' }
    };

    if (tipo === 'campo') {
        Object.keys(datosPorEstacion).forEach((nombreEstacion, index) => {
            if (index > 0) {
                worksheet.addRow([]);
            }
        
            const estacion = reportes.find(reporte => reporte.estacion.nombreEstacion === nombreEstacion)?.estacion;

        const codigo = estacion ? estacion.codigo : '';
        const municipioNombre = estacion && estacion.municipio ? estacion.municipio.nombre : '';

        // Formato: "nombreEstacion codigo (municipioNombre)"
        const textoEncabezado = `${nombreEstacion} - ${codigo} (${municipioNombre})`;

        // Crear la fila del encabezado con el texto formateado
        const filaEncabezado = worksheet.addRow([textoEncabezado]);
            filaEncabezado.font = { bold: true, size: 14 }; 
        
            worksheet.mergeCells(`A${filaEncabezado.number}:P${filaEncabezado.number}`);
        
            const datos = datosPorEstacion[nombreEstacion];
        
            const headers = [
                'Fecha Visita', 'Hora', 'Temperatura Ambiente (°C)', 'Temperatura Agua (°C)', 'pH (U. de pH)', 'Oxígeno disuelto (mg/L)',
                'Conductividad eléctrica (µS/cm)', 'Potencial Redox (mV)', 'Turbiedad (NTU)', 'Estado del tiempo',
                'Apariencia', 'Olor', 'Color', 'Altura (m)', 'H1 (m)', 'H2 (m)', 'Observación'
            ];
            const headerRow = worksheet.addRow(headers);
            headerRow.eachCell((cell) => {
                cell.font = headerStyle.font; 
                cell.fill = headerStyle.fill;
                cell.alignment = { ...headerStyle.alignment, wrapText: true };
            });
            worksheet.getRow(headerRow.number).height = 40;
        
            datos.forEach(data => {
                const fila = [
                    data['Fecha Visita'] || '',
                    data.Hora || '',
                    data['Temperatura Ambiente (°C)'] || '',
                    data['Temperatura Agua (°C)'] || '',
                    data['pH (U. de pH)'] || '',
                    data['Oxígeno disuelto (mg/L)'] || '',
                    data['Conductividad eléctrica (µS/cm)'] || '',
                    data['Potencial Redox (mV)'] || '',
                    data['Turbiedad (NTU)'] || '',
                    data['Estado del tiempo'] || '',
                    data.Apariencia || '',
                    data.Olor || '',
                    data.Color || '',
                    data['Altura (m)'] || '',
                    data['H1 (m)'] || '',
                    data['H2 (m)'] || '',
                    data.Observación || ''
                ];
                worksheet.addRow(fila); 
            });
        });
    }
    
    if (tipo === 'laboratorio') {
        const datosLaboratorio = datosPorEstacion['Laboratorio'];
    
        if (datosLaboratorio.length > 0) {
            const headers = [
                'Fecha Visita',
                'Estación',
                    'Código',
                    'Temperatura Ambiente (°C)',
                    'Temperatura Agua (°C)',
                    'pH (U. de pH)',
                    'Oxígeno disuelto (mg/L)',
                    'Conductividad eléctrica (µS/cm)',
                    'Potencial Redox (mV)',
                    'Turbiedad (NTU)',
                    'Caudal (m3/s)',
                    'Clasificación caudal (Adim)',
                    'Número de verticales',
                    'Color verdadero (UPC)',
                    'Color triestimular 436 nm',
                    'Color triestimular 525 nm',
                    'Color triestimular 620 nm',
                    'Sólidos suspendidos totales (mg/L)',
                    'Sólidos totales (mg/L)',
                    'Sólidos volátiles totales (mg/L)',
                    'Sólidos disueltos totales (mg/L)',
                    'Sólidos fijos totales (mg/L)',
                    'Sólidos sedimentables (ml/L-h)',
                    'DBO5 (mg/L)',
                    'DQO (mg/L)',
                    'Hierro total (mg Fe/L)',
                    'Sulfatos (mg/L)',
                    'Sulfuros (mg/L)',
                    'Clororus (mg/L)',
                    'Grasas y/o aceites (mg/L)',
                    'SAAM (mg/L)',
                    'Fósforo total (mg P/L)',
                    'Fosfato (mg P/L)',
                    'Fósforo orgánico (mg P/L)',
                    'Nitratos (mg N/L)',
                    'Nitritos (mg N/L)',
                    'Nitrógeno orgánico (mg N/L)',
                    'Nitrógeno total Kjeldahl (mg N/L)',
                    'Cadmio (mg Cd/L)',
                    'Cobre (mg Cu/L)',
                    'Cromo (mg Cr/L)',
                    'Cromo hexavalente (mg Cr6+/L)',
                    'Mercurio (mg Hg/L)',
                    'Niquel (mg Ni/L)',
                    'Plomo (mg Pb/L)',
                    'Cadmio sedimentable (mg Cd/L)',
                    'Cobre sedimentable (mg Cu/L)',
                    'Cromo sedimentable (mg Cr/L)',
                    'Mercurio sedimentable (mg Hg/L)',
                    'Plomo sedimentable (mg Pb/L)',
                    'Escherichia coli (UFC)',
                    'Coliformes totales (UFC)',
                    'Escherichia coli (NMP/100mL)',
                    'Coliformes totales (NMP/100mL)',
                    'Riqueza algas',
                    'Indice biológico BMWP',
                    'Clasificación Indice biológico BMWP',
                    'Observaciones (algas y macroinvertebrados)',
            ];
            
            const headerGroups = [
                'Punto de Monitoreo', 'Punto de Monitoreo', 'Punto de Monitoreo',
                'InSitu', 'InSitu', 'InSitu', 'InSitu', 'InSitu', 'InSitu', 'InSitu', 
                'Físicos', 'Físicos', 'Físicos', 'Físicos', 'Físicos', 'Físicos', 'Físicos', 'Físicos','Físicos', 'Físicos', 'Físicos', 'Físicos', 'Físicos',
                'Químicos', 'Químicos', 'Químicos', 'Químicos', 'Químicos', 'Químicos', 'Químicos', 'Químicos',
                'Nutrientes', 'Nutrientes', 'Nutrientes', 'Nutrientes', 'Nutrientes', 'Nutrientes', 'Nutrientes', 
                'Metal Agua', 'Metal Agua', 'Metal Agua', 'Metal Agua', 'Metal Agua', 'Metal Agua', 'Metal Agua',
                'Metal Sedimental', 'Metal Sedimental', 'Metal Sedimental', 'Metal Sedimental', 'Metal Sedimental', 
                'Biológico', 'Biológico', 'Biológico', 'Biológico', 'Biológico', 'Biológico', 'Biológico', 'Biológico'
            ];
            
            
            const headerGroupRow = worksheet.addRow(headerGroups);
            
            worksheet.mergeCells('A1:C1'); 
            worksheet.mergeCells('D1:J1'); 
            worksheet.mergeCells('K1:W1'); 
            worksheet.mergeCells('X1:AE1'); 
            worksheet.mergeCells('AF1:AL1'); 
            worksheet.mergeCells('AM1:AS1');  
            worksheet.mergeCells('AT1:AX1'); 
            worksheet.mergeCells('AY1:BF1');  
            headerGroupRow.eachCell((cell, colNumber) => {
                cell.font = { bold: true, size: 12 };
                cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true }; 
                cell.fill = {
                    type: 'pattern',
                    pattern: 'solid',
                    fgColor: { argb: 'FFA2A4A5' } 
                };

            });
            const headerRow = worksheet.addRow(headers);
            headerRow.eachCell((cell) => {
                cell.fill = headerStyle.fill; 
                cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true }; 
                worksheet.getRow(headerRow.number).height = 40; 
            });
    
            datosLaboratorio.forEach(data => {
                worksheet.addRow(Object.values(data));
            });
        }
    }
    
    
    worksheet.eachRow({ includeEmpty: true }, (row) => {
        row.eachCell({ includeEmpty: true }, (cell) => {
            cell.border = {
                top: { style: 'thin' },
                left: { style: 'thin' },
                bottom: { style: 'thin' },
                right: { style: 'thin' }
            };
        });
    });

    worksheet.columns.forEach(column => {
        column.width = 20; 
    });

    const fechaActual = new Date();
    const nombreCampaña = reportes[0].campaña ? reportes[0].campaña.nombreCampaña : 'Sin_Campaña'; 
    const tiposFuente = [...new Set(reportes.map(reporte => 
        reporte.estacion?.tipoFuente?.NombreTipoFuente || 'vacio'
    ))];
    
    // Formatear los tipos de fuente en un solo string con "y" si hay más de dos
    const TipoFuente = tiposFuente.length > 1 
        ? tiposFuente.slice(0, -1).join(', ') + ' y ' + tiposFuente[tiposFuente.length - 1]
        : tiposFuente[0] || 'vacio';
    
    console.log(TipoFuente);
        // const fechaCreacion = reportes[0].campaña && reportes[0].campaña.fecha_creacion ? new Date(reportes[0].campaña.fecha_creacion) : fechaActual;
    
    // Formatea la fecha de creación en el formato deseado (día-mes-año)
    // const fechaCreacionFormateada = `${fechaCreacion.getDate()}-${fechaCreacion.getMonth() + 1}-${fechaCreacion.getFullYear()}`;
    
    const nombreArchivo = tipo === 'campo' 
        ? `reportecampo_${nombreCampaña}_${TipoFuente}.xlsx` 
        : `reportelaboratorio_${nombreCampaña}_${TipoFuente}.xlsx`;
    
    workbook.xlsx.writeBuffer().then(buffer => {
        const blob = new Blob([buffer], { type: 'application/octet-stream' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = nombreArchivo;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    });
    
}

// function eliminarFilaReporte(idReporte) {
//     Swal.fire({
//         title: '¿Estás seguro?',
//         text: "¡No podrás revertir esta acción!",
//         icon: 'warning',
//         showCancelButton: true,
//         confirmButtonColor: '#3085d6',
//         cancelButtonColor: '#d33',
//         confirmButtonText: 'Sí, eliminar',
//         cancelButtonText: 'Cancelar'
//     }).then((result) => {
//         if (result.isConfirmed) {
//             console.log("URL completa para eliminar:", `http://localhost:5078/api/ReportesLaboratorio/EliminarReporteLaboratorio/${idReporte}`);

//             $.ajax({
//                 url: `http://localhost:5078/api/ReportesLaboratorio/EliminarReporteLaboratorio/${idReporte}`,
//                 type: 'DELETE',
//                 success: function (result) {
//                     Swal.fire('Eliminado!', 'El registro ha sido eliminado.', 'success');
//                     // Aquí puedes llamar a cargarCampañas() si necesitas refrescar los datos
//                 },
//                 error: function () {
//                     Swal.fire({
//                         icon: 'warning',
//                         text: 'La campaña no se puede eliminar debido a que tiene reportes relacionados.'
//                     });
//                 }
//             });
//         }
//     });
// }

    cargarExcelPorCampañas( )
    obtenerReportesLaboratorio();
    CargarEstaciones();

})
