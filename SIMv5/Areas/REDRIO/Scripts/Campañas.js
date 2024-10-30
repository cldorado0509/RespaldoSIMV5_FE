    $(document).ready(function () {
        var listaDatoscampaña = [];

        $('#btnVolverAgregar').click(function () {
            $('#vistaAgregar').hide();   
            $('#vistaPrincipal').show(); 
        });

        $('#btnVolverEditar').click(function () {
            $('#vistaEditar').hide();   
            $('#vistaPrincipal').show(); 
        });
        function cargarcampañas() {
            var urlBase = $("#SIM").data("url");
            var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ObtenerCampañas";
        
            console.log("URL llamada: ", urlCompleta);
        
            $.getJSON(urlCompleta, function (data) {
                listaDatoscampaña = data.result;
                console.log(listaDatoscampaña);
        
                $("#gridContainer").dxDataGrid("instance").option("dataSource", listaDatoscampaña);
            }).fail(function () {
                alert('Error al cargar campañas.');
            });
        }
    
        function cargarFases(selectedFaseId, isEditing = false) {
            var urlBase = $("#SIM").data("url");
            var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ObtenerFases";
        
            console.log("URL llamada: ", urlCompleta);
        
            $.getJSON(urlCompleta, function (data) {
                console.log("Datos de Fase:", data); 
                
                var faseSelect = isEditing ? $('#editFase') : $('#Fase');
                faseSelect.empty(); 
                faseSelect.append($('<option>', { value: '', text: 'Seleccione una fase' }));
                
                if (data.result && data.result.length > 0) {
                    $.each(data.result, function (index, fase) {
                        faseSelect.append($('<option>', { 
                            value: fase.idFase,
                            text: fase.nombreFase
                        }));
                    });
                } else {
                    console.log("No se encontraron fases."); 
                }
                
                if (selectedFaseId) {
                    faseSelect.val(selectedFaseId);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log("Error al cargar fases:", textStatus, errorThrown); 
                alert('Error al cargar las fases.'); 
            });
        }
        
        
        

        
        function inicializarGrid() {
            $("#gridContainer").dxDataGrid({
                dataSource: listaDatoscampaña,
                columnAutoWidth: true,
                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                columns: [
                    {
                        caption: "ID",
                        cellTemplate: function(container, options) {
                            container.text(options.rowIndex + 1);
                        }
                    },
                    // { dataField: "nombreCampaña", caption: "Nombre de campaña" },
                    { dataField: "fase.nombreFase", caption: "Fase" },
                    { dataField: "descripcion", caption: "Descripción" },
                    { dataField: "fecha_inicial", caption: "Fecha Inicio", dataType: "date" },
                    { dataField: "fecha_final", caption: "Fecha Final", dataType: "date" },
                    {
                        caption: "Acciones",
                        width: 150,
                        cellTemplate: function (container, options) {
                            var editButton = $("<i>")
                                .addClass("fas fa-edit")
                                .attr("title", "Editar campaña")
                                .css({ "cursor": "pointer", "margin-right": "10px" })
                                .click(function () {
                                    editarFilacampaña(options.data);
                                });

                            var deleteButton = $("<i>")
                                .addClass("fas fa-trash")
                                .attr("title", "Eliminar campaña")
                                .css({ "cursor": "pointer", "margin-right": "10px" })
                                .click(function () {
                                    eliminarFilacampaña(options.data);
                                });
                                
                                var uploadButton = $("<i>")
                                .addClass("fas fa-file-upload")
                                .attr("title", "Cargar archivo Excel")
                                .css({ "cursor": "pointer", "margin-right": "10px" })
                                .click(function () {
                                    $('#cargarExcelModal').modal('show');
                                    
                            
                                    $('#btnCargarExcel').data('campaignId', options.data.idCampaña);
                                    $('#btnCargarExcel2').data('campaignId', options.data.idCampaña);

                                });
                            
                                var labButton = $("<i>")
                                .addClass("fas fa-vial")
                                .attr("title", "Ver resultados de laboratorio")
                                .css({ "cursor": "pointer", "margin-right": "10px" })
                                .click(function () {
                                    window.location.href = '/SIM/REDRIO/REDRIO/ReporteSuperficial?idCampa=' + options.data.idCampaña;;
                                });
                            
        
                            container.append(editButton).append(deleteButton).append(uploadButton).append(labButton);
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
                filterRow: {
                    visible: true
                },
                searchPanel: {
                    visible: true,
                    placeholder: "Buscar..."
                },
                export: {
                    enabled: true,
                    fileName: "campañas"
                }
            });
        }
        
        // $('#btnCargarExcel').click(function () {
        //     var campaignId = $(this).data('campaignId');
        //     cargarArchivoExcel('laboratorio', campaignId); 
        // });
        $('#btnCargarExcel').click(function () {
            var campaignId = $(this).data('campaignId');
            cargarArchivoExcel('laboratorio', campaignId); 
            console.log(campaignId);
            
        });

        $('#btnCargarExcel2').click(function () {
            var campaignId = $(this).data('campaignId');
            cargarArchivoExcel('campo', campaignId);
            console.log(campaignId);
        });

        
        $('#modalExcel').on('hidden.bs.modal', function () {
            $('#cargarExcelModal').modal('show');
        });

        $('#btnAgregar').click(function () {
            $('#vistaAgregar').show(); 
            $('#vistaEditar').hide();   
            $('#vistaPrincipal').hide(); 
            $('#formNuevoDato')[0].reset(); 
            cargarFases(); 
        });

        $('#btnGuardarDato').click(function () {
            // Obtener los valores de los campos de entrada
            // var NombreCampaña = $('#nombreCampaña').val();
            var IdFase = $('#Fase').val();
            var Descripcion = $('#Descripcion').val();  
            var Fecha_inicial = $('#Fecha_inicio').val();
            var Fecha_final = $('#Fecha_fin').val();
        
            // Validar que la fecha inicial no sea mayor que la fecha final
            if (new Date(Fecha_inicial) > new Date(Fecha_final)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear campaña: la fecha inicial no puede ser mayor a la final.'
                });
                return; // Salir de la función si hay error
            }
        
            // Validar que todos los campos requeridos estén llenos
            if (
                // !NombreCampaña ||
                 !Descripcion || !IdFase) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear campaña: todos los campos son requeridos.'
                });
                return; // Salir de la función si hay error
            }
        
            var nuevaCampaña = { 
                // nombreCampaña: NombreCampaña, 
                idFase: parseInt(IdFase), 
                descripcion: Descripcion,
                fecha_inicial: Fecha_inicial,
                fecha_final: Fecha_final
            };
        
            var urlBase = $("#SIM").data("url");
            var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarCampaña";
        
            $.ajax({
                url: urlCompleta,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(nuevaCampaña), 
                success: function (result) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Campaña agregada',
                        text: result.message 
                    });
                    $('#formNuevoDato')[0].reset();
                    $('#vistaPrincipal').show();
                    $('#vistaAgregar').hide();
                    cargarcampañas();

                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo agregar la campaña: ' + xhr.responseText
                    });
                }
            });
        });
        

        function editarFilacampaña(data) {
            $('#editid').val(data.idCampaña); 
            $('#editnombreCampaña').val(data.nombreCampaña);
            cargarFases(data.idFase, true);         
            $('#editDescrip').val(data.descripcion);        
            $('#editFecha_inicio').val(data.fecha_inicial.split('T')[0]); 
            $('#editFecha_fin').val(data.fecha_final.split('T')[0]); 
        
            $('#vistaEditar').show();   
            $('#vistaPrincipal').hide(); 
            $('#vistaAgregar').hide();   
        }
        
        
        
        
        $('#btnActualizarDato').click(function () {
            var idCampaña = $('#editid').val();
            var nombreCampaña = $('#editnombreCampaña').val();
            var idFase = $('#editFase').val();
            var descripcion = $('#editDescrip').val();
            var fecha_inicial = $('#editFecha_inicio').val();
            var fecha_final = $('#editFecha_fin').val();

            if (!idCampaña) {
                alert('ID de campaña no encontrado.');
                return;
            }

            var campañaActualizada = { idCampaña, nombreCampaña, idFase, descripcion, fecha_inicial, fecha_final };

            var urlBase = $("#SIM").data("url");
            var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/EditarCampaña/" + idCampaña; 

            $.ajax({
                url: urlCompleta,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(campañaActualizada),
                success: function (response) {
                    if (response.isSuccess) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Actualización exitosa',
                        }).then(() => {
                            cargarcampañas(); 
                            $('#vistaEditar').hide();
                            $('#formEditarDato')[0].reset(); 
                            $('#vistaPrincipal').show(); 
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
                        text: 'Error al actualizar la campaña. Inténtelo nuevamente.'
                    });
                }
            });
            
        });

        function eliminarFilacampaña(data) {
            Swal.fire({
                title: '¿Estás seguro?',
                text: "¡No podrás revertir esta acción!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    var urlBase = $("#SIM").data("url");
                    var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/EliminarCampana/" + data.idCampaña;
                    console.log("URL completa para eliminar:", urlCompleta);

                    $.ajax({
                        url: urlCompleta,
                        type: 'DELETE',
                        success: function (result) {
                            Swal.fire('Eliminado!', 'El registro ha sido eliminado.', 'success');
                            cargarcampañas();
                        },
                        error: function () {
                            Swal.fire({
                                icon: 'warning',
                                // title: 'Error',
                                text: 'La campaña no se puede eliminar debido a que tiene reportes relacionados.'
                            });
                        }
                    });
                }
            });
        }
        

        var rowsWithErrors = [];

        function verificarCodigoEnBackend(codigo) {
            if (!codigo) {
                console.error('Código es undefined o vacío');
                return Promise.resolve({ exists: false });
            }

            return $.ajax({
                url: 'http://localhost:5078/api/estacion/ObtenerEstacionCodigo/' + codigo,
                type: 'GET',
                dataType: 'json'
            }).then(function (response) {
                console.log('Respuesta del backend para el código ' + codigo + ':', response);
                return { exists: response.isSuccess }; 
            }).catch(function (xhr) {
                if (xhr.status === 404) {
                    console.error('Código no encontrado:', codigo);
                    return { exists: false }; 
                } else {
                    console.error('Error al verificar el código:', codigo);
                }
                return { exists: false };  
            });
        }
        
        function verificarCodigosEstacion(excelData) {
            rowsWithErrors = []; 
            var verificationPromises = []; 
            var codigosNoEncontrados = []; 
        
            excelData.forEach(function (row, index) {
                var codigoEstacion = row.Código;
        
                if (!codigoEstacion) {
                    console.error('El código en la fila ' + index + ' es undefined o vacío. Fila:', row);
                    rowsWithErrors.push(index); 
                    return; 
                }
        

                var promise = verificarCodigoEnBackend(codigoEstacion).then(function (response) {
                    if (!response.exists) {
                        console.log('El código no existe:', codigoEstacion);
                        codigosNoEncontrados.push({ codigo: codigoEstacion, rowIndex: index });
                    }
                }).catch(function () {
                    console.error('Error al verificar el código:', codigoEstacion);
                    codigosNoEncontrados.push({ codigo: codigoEstacion, rowIndex: index });
                });
        
                verificationPromises.push(promise); 
            });
        
            Promise.all(verificationPromises).then(function () {
                console.log("Verificación completa. Códigos no encontrados:", codigosNoEncontrados.map(c => c.codigo));
        
                excelData.forEach(function (row, index) {
                    codigosNoEncontrados.forEach(function(error) {
                        if (row.Código === error.codigo) {
                            rowsWithErrors.push(index); 
                        }
                    });
                });
        
                if (rowsWithErrors.length > 0) { 
                    
                    alert('Algunas estaciones no están registradas. Corrija el archivo y vuelva a intentarlo.');
                    $('#botonProcesar').hide(); 
                    $("#botonProcesar2").hide();
                } else {
                    console.log("Todas las verificaciones completadas sin errores.");
                }
        
                const dataGridInstance = $("#excelGridContainer").dxDataGrid("instance");
                dataGridInstance.option("dataSource", excelData); 
                dataGridInstance.refresh(); 
            }).catch(function (error) {
                console.error("Error al procesar las verificaciones:", error);
            });
        }
        
    function agregarInsitu(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarInsitu";

        return $.ajax({
            url: urlCompleta,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('insitu agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar insitu: ', xhr.status);
            return null;
        });
    }

    function agregarFisico(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarFisico";
        console.log('fisico:', datos);

        return $.ajax({
            url: urlCompleta,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('Físico agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar Físico: ', xhr.status);
            return null;
        });
    }

    function agregarQuimico(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarQuimico";

        return $.ajax({
            url: urlCompleta,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('Quimico agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar Quimico: ', xhr.status);
            return null;
        });
    }

    function agregarNutriente(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarNutriente";

        return $.ajax({
            url: urlCompleta,  
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('Nutriente agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar Nutriente: ', xhr.status);
            return null;
        });
    }

    function agregarMetalAgua(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarMetal";

        return $.ajax({
            url: urlCompleta,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('MetalAgua agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar MetalAgua: ', xhr.status);
            return null;
        });
    }

    function agregarMetalSedimento(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarMetalSedimental";

        return $.ajax({
            url: urlCompleta,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('MetalSedimental agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar MetalSedimental: ', xhr.status);
            return null;
        });
    }

    function agregarBiologico(datos) {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarBiologico";

        return $.ajax({
            url: urlCompleta,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(datos)
        }).then(function(response) {
            console.log('Biologico agregado exitosamente', response);
            return response;
        }).catch(function(xhr) {
            console.error('Error al agregar Biologico: ', xhr.status);
            return null;
        });
    }
    var procesosCompletos = 0; 

    function guardarDatosEnLocalStorage(tipo, excelData) {
        var datos = JSON.parse(localStorage.getItem(`excelData${tipo}`)) || [];
        datos.push(...excelData);
        
        // Asegúrate de que estás guardando en las claves correctas
        if (tipo === 'Laboratorio') {
            localStorage.setItem('excelDataLaboratorio', JSON.stringify(datos)); 
        } else if (tipo === 'campo') {
            localStorage.setItem('excelDatacampo', JSON.stringify(datos)); 
        }
        
        // Guarda los datos en una clave general si es necesario
        localStorage.setItem('excelData', JSON.stringify(datos));
    }
    function cargarDatosDesdeLocalStorage(tipo) {
        // Recupera los datos almacenados según el tipo (Laboratorio o campo)
        var datos = JSON.parse(localStorage.getItem(`excelData${tipo}`)) || [];
        
        // Verifica si se recuperaron datos correctamente
        if (datos.length > 0) {
            console.log(`Datos cargados desde excelData${tipo}:`, datos);
        } else {
            console.warn(`No hay datos almacenados para el tipo ${tipo}`);
        }
    
        return datos;
    }
    
    $('#botonProcesar').click(function() {
        var excelData = $("#excelGridContainer").dxDataGrid("instance").option("dataSource");
        procesarDatosPorRango(excelData);
    
        guardarDatosEnLocalStorage('Laboratorio', excelData);
    
        if (file) {
            guardarExcel(file);
        } else {
            console.error('No se ha cargado ningún archivo Excel');
        }
    
        $('#modalExcel').dialog('close');
        var campaignId = $('#btnCargarExcel').data('campaignId');
        $('#btnCargarExcel2').prop('disabled', true);

        $('#btnCargarExcel').after('<p>Excel de Laboratorio cargado exitosamente</p>');
        $('#btnCargarExcel').prop('disabled', true);
        mostrarBotonFinalizar();

    
    });
    
    
    
    $('#botonProcesar2').click(function() {
        var excelData = $("#excelGridContainer").dxDataGrid("instance").option("dataSource");
        procesarDatosPorRango2(excelData);
        guardarDatosEnLocalStorage('campo', excelData);
        localStorage.setItem('excelDatacampo', JSON.stringify(excelData)); 
        if (file) {
            guardarExcel(file);
        } else {
            console.error('No se ha cargado ningún archivo Excel');
        }
        $('#modalExcel').dialog('close');
        var campaignId = $('#btnCargarExcel2').data('campaignId');
        $('#btnCargarExcel').prop('disabled', true);
        $('#btnCargarExcel2').after('<p>Excel de Campo cargado exitosamente</p>');
        
        mostrarBotonFinalizar();
        
        $('#btnCargarExcel2').prop('disabled', true);
    });
    
    function mostrarBotonFinalizar() {
        if ($('#botonFinalizarArea').length === 0) {
            if ($('#btnCargarExcel').siblings('p').length > 0 || $('#btnCargarExcel2').siblings('p').length > 0) {
                var targetElement;
    
                if ($('#btnCargarExcel').siblings('p').length > 0) {
                    targetElement = $('#btnCargarExcel').siblings('p');
                } else if ($('#btnCargarExcel2').siblings('p').length > 0) {
                    targetElement = $('#btnCargarExcel2').siblings('p');
                }
    
                // Mostrar overlay y loading
                $('#loadingOverlay').show();
    
                // Agregar un retraso de 30 segundos antes de mostrar el botón "Finalizar"
                setTimeout(function() {
                    // Ocultar overlay y loading
                    $('#loadingOverlay').hide();
    
                    targetElement.append('<div style="margin-top: 12px; margin-right:80px"><button id="botonFinalizarArea" class="btn btn-primary">Finalizar</button></div>');
    
                    // Agregar el evento click al botón después de que se haya añadido al DOM
                    $('#botonFinalizarArea').off('click').on('click', function() {
                        var datosLaboratorio = cargarDatosDesdeLocalStorage('Laboratorio');
                        var datosCampo = cargarDatosDesdeLocalStorage('campo');
                    
                        if (datosLaboratorio.length > 0) {
                            console.log('Procesando datos de Laboratorio:', datosLaboratorio);
                        }
                    
                        if (datosCampo.length > 0) {
                            console.log('Procesando datos de Campo:', datosCampo);
                        }
                        var campaignId = $('#btnCargarExcel').data('campaignId');
                        console.log(campaignId);
                        obtenerDatosReporte(campaignId).then(function(datosReporte) {
                        });
                    
                        if (localStorage.getItem('excelDataLaboratorio')) {
                            localStorage.removeItem('excelDataLaboratorio');
                            localStorage.removeItem('excelData');
                            console.log('Datos de Laboratorio eliminados del localStorage');
                        }
                    
                        if (localStorage.getItem('excelDatacampo')) {
                            localStorage.removeItem('excelDatacampo');
                            localStorage.removeItem('excelData');
                            console.log('Datos de Campo eliminados del localStorage');
                        }
                    
                        $('#btnCargarExcel').siblings('p').remove();
                        $('#btnCargarExcel2').siblings('p').remove();
                        $('#botonFinalizarArea').remove();
                        $('#modalExcel').dialog('close');
                        $('#btnCargarExcel').prop('disabled', false);
                        $('#btnCargarExcel2').prop('disabled', false);
                    });
                }, 10000); 

            }
        }
    }
    
    
    $('#cargarExcelModal').on('shown.bs.modal', function () {
        $('#btnCargarExcel').show();
        $('#btnCargarExcel2').show(); 

        $('#btnCargarExcel').prop('disabled', false);
        $('#btnCargarExcel2').prop('disabled', false);
    });
    
        var finalizado = false;

    $('#cargarExcelModal').on('hide.bs.modal', function (e) {
    if (!finalizado && $('#botonFinalizarArea').is(':visible')) {
        e.preventDefault();
        Swal.fire({
        icon: 'error',
        title: 'Debes finalizar la tarea primero',
        text: 'Por favor, haz clic en el botón de finalizar para continuar.'
        });
    } else {
        finalizado = false; 
    }
    
    });
    
    
        function guardarAgregarResultadoLaboratorio(datos) {
            var urlBase = $("#SIM").data("url");
            var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarResultadoCampo";

            return $.ajax({
                url: urlCompleta,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos)
                
            }).then(function(response) {
                console.log(' Laboratorio campo, Datos guardados exitosamente');
                return response;
            }).catch(function(xhr) {
                console.error('Error al guardar datos: ', xhr.status);
                console.error(xhr.responseText);        
            });
        }
        function convertirFecha(fechaStr) {
            if (!fechaStr) return null; 
            const partes = fechaStr.split('/');
            const dia = parseInt(partes[0], 10);
            const mes = parseInt(partes[1], 10) - 1;
            const año = 2000 + parseInt(partes[2], 10);
            return new Date(año, mes, dia);
        }
        async function procesarDatosPorRango(excelData) {

            var campaignId = $('#btnCargarExcel').data('campaignId');
        
            for (const row of excelData) {
                // Rango Insitu
                var insitu = {
                    Temp_ambiente: row["Temperatura Ambiente (°C)"]|| null,
                    Tem_agua: row["Temperatura Agua (°C)"]|| null,
                    PhInsitu: row["pH (U de pH)"]|| null,
                    Oxigeno_disuelto: row["Oxígeno disuelto (mg/L)"]|| null,
                    Conductiviidad_electrica: row["Conductividad eléctrica (µS/cm)"]|| null,
                    OrpInsitu: row["Potencial Redox (mV)"]|| null,
                    Turbiedad: row["Turbiedad (NTU)"]|| null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseInsitu = await agregarInsitu(insitu);
                console.log(responseInsitu);
                row.IdInsitu = responseInsitu.result.idInsitu;
                
        
                // Rango Físico
                var VarFisico = {
                    Caudal: row["Caudal (m3/s)"] || null,
                    ClasificacionCaudal: row["Clasificación caudal (Adim)"] || null,
                    NumeroVerticales: row["Número de verticales"] || null,
                    ColorVerdaderoUPC: row["Color verdadero (UPC)"] || null,
                    ColorTriestimular436nm: row["Color triestimular 436 nm"] || null,
                    ColorTriestimular525nm: row["Color triestimular 525 nm"] || null,
                    ColorTriestimular620nm: row["Color triestimular 620 nm"] || null,
                    SolidosSuspendidosTotales: row["Sólidos suspendidos totales (mg/L)"] || null,
                    SolidosTotales: row["Sólidos totales (mg/L)"] || null,
                    SolidosVolatilesTotales: row["Sólidos volátiles totales (mg/L)"] || null,
                    SolidosDisueltosTotales: row["Sólidos disueltos totales (mg/L)"] || null,
                    SolidosFijosTotales: row["Sólidos fijos totales (mg/L)"] || null,
                    SolidosSedimentables: row["Sólidos sedimentables (ml/L-h)"] || null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseFisico = await agregarFisico(VarFisico);
                row.IdFisico = responseFisico.result.idFisico;
        
                // Rango Químico
                var quimico = {
                    Db05: row["DBO5 (mg/L)"] || null,
                    Dq0: row["DQO (mg/L)"] || null,
                    HierroTotal: row["Hierro total (mg Fe/L)"] || null,
                    Sulfatos: row["Sulfatos (mg/L)"] || null,
                    Sulfuros: row["Sulfuros (mg/L)"] || null,
                    Clororus: row["Clororus (mg/L)"] || null,
                    Grasa_Aceite: row["Grasas y/o aceites (mg/L)"] || null,
                    sustanciaActivaAzulMetileno: row["SAAM (mg/L)"] || null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseQuimico = await agregarQuimico(quimico);
                row.IdQuimico = responseQuimico.result.idQuimico;
        
                // Rango Nutriente
                var nutriente = {
                    Fosforo_total: row["Fósforo total (mg P/L)"] || null,
                    Fosfato: row["Fosfato (mg P/L)"] || null,
                    Fosforo_organico: row["Fósforo orgánico (mg P/L)"] || null,
                    Nitratos: row["Nitratos (mg N/L)"] || null,
                    Nitritos: row["Nitritos (mg N/L) "] || null,
                    Nitrogeno_organico: row["Nitrógeno orgánico (mg N/L)"] || null,
                    Nitrogeno_total_kjeldahl: row["Nitrógeno total Kjeldahl (mg N/L)"] || null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseNutriente = await agregarNutriente(nutriente);
                row.IdNutriente = responseNutriente.result.idNutriente;
        
                // Rango Metal Agua
                var metalAgua = {
                    Cadmio: row["Cadmio (mg Cd/L)"] || null,
                    Cobre: row["Cobre (mg Cu/L)"] || null,
                    Cromo: row["Cromo (mg Cr/L)"] || null,
                    Cromo_hexavalente: row["Cromo hexavalente (mg Cr6+/L)"] || null,
                    Mercurio: row["Mercurio (mg Hg/L)"] || null,
                    Niquel: row["Niquel (mg Ni/L)"] || null,
                    Plomo: row["Plomo (mg Pb/L)"] || null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseMetalAgua = await agregarMetalAgua(metalAgua);
                row.IdMetalAgua = responseMetalAgua.result.idMetalAgua;
        
                // Rango Metal Sedimento
                var metalSedimento = {
                    Cadmio_sedimentable: row["Cadmio sedimentable (mg Cd/L)"] || null,
                    Cobre_sedimentable: row["Cobre sedimentable (mg Cu/L)"] || null,
                    Cromo_sedimentable: row["Cromo sedimentable (mg Cr/L)"] || null,
                    Mercurio_sedimentable: row["Mercurio sedimentable (mg Hg/L)"] || null,
                    Plomo_sedimentable: row["Plomo sedimentable (mg Pb/L)"] || null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseMetalSedimento = await agregarMetalSedimento(metalSedimento);
                row.idMetalSedimental = responseMetalSedimento.result.idMetalSedimental;
        
                // Rango Biológico
                var biologico = {
                    Escherichia_coli: row["Escherichia coli (UFC)"] || null,
                    Coliformes_totales_ufc: row["Coliformes totales (UFC)"] || null,
                    Escherichia_coli_npm: row["Escherichia coli (NMP/100mL)"] || null,
                    Coliformes_totales_npm: row["Coliformes totales (NMP/100mL)"] || null,
                    Riquezas_algas: row["Riqueza algas"] || null,
                    Indice_biologico: row["Indice biológico BMWP"] || null,
                    ClasificacionIBiologico: row["Clasificación Indice biológico BMWP"] || null,
                    Observaciones: row ["Observaciones (algas y macroinvertebrados)"] || null,
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"])
                };
                const responseBiologico = await agregarBiologico(biologico);
                row.IdBiologico = responseBiologico.result.idBiologico;
            }
        
                localStorage.setItem('excelDataLaboratorio', JSON.stringify(excelData));
                obtenerDatos(campaignId).then(datosReporte => {
                    agregarMuestraCompuesta(datosReporte).then(idMuestraCompuesta => {
                        excelData.forEach((obj, index) => {
                            obj.idMuestraCompuesta = idMuestraCompuesta[index];
                        });
                        
                        localStorage.setItem('excelDataLaboratorio', JSON.stringify(excelData));
                    });
                        });
                        
        }
        async function procesarDatosPorRango2(excelData) {
            var campaignId = $('#btnCargarExcel2').data('campaignId');
        
            for (const row of excelData) {
                // Campo
                var Campo = {
                    Fecha_Muestra: convertirFecha(row["Fecha Visita"]),
                    Hora: row["Hora"]|| null || undefined,
                    TempAmbiente: row["Temperatura Ambiente (°C)"]|| null || undefined,
                    TempAgua: row["Temperatura Agua (°C)"]|| null || undefined,
                    Ph: row["pH (U de pH)"]|| null || undefined,
                    Od: row["Oxígeno disuelto (mg/L)"]|| null || undefined,
                    Conductiviidad_electrica: row["Conductividad eléctrica (µS/cm)"]|| null || undefined,
                    Orp: row["Potencial"]|| null || undefined,
                    Turb: row["Turbiedad (NTU)"]|| null || undefined,
                    Tiempo: row["Estado del tiempo"]|| null || undefined,
                    Apariencia: row["Apariencia"]|| null || undefined,
                    Olor: row["Olor"]|| null || undefined,
                    Color: row["Color"]|| null || undefined,
                    Altura: row["Altura (m)"]|| null || '',
                    H1: row["H1 (m)"]|| null || '',
                    H2: row["H2 (m)"]|| null || '',
                    Observacion: row ["Observación"]|| null || '',
                };
                const response = await guardarAgregarResultadoLaboratorio(Campo);
                row.idCampo = response.result.idCampo;
                            console.log(excelData)
            }
        
            localStorage.setItem('excelDatacampo', JSON.stringify(excelData));
        }

        function verificarColumnasExcel(tipoCarga, columnas) {
            var columnasRequeridas = [];
        
            if (tipoCarga === 'laboratorio') {
                columnasRequeridas = [
                    "Código",
                    "Fecha Visita",
                    "Temperatura Ambiente (°C)",
                    "Temperatura Agua (°C)",
                    "pH (U de pH)",
                    "Oxígeno disuelto (mg/L)",
                    "Conductividad eléctrica (µS/cm)",
                    "Potencial Redox (mV)",
                    "Turbiedad (NTU)",
                    "Clasificación caudal (Adim)",
                    "Número de verticales",
                    "Color verdadero (UPC)",
                    "Color triestimular 436 nm",
                    "Color triestimular 525 nm",
                    "Color triestimular 620 nm",
                    "Sólidos suspendidos totales (mg/L)",
                    "Sólidos totales (mg/L)",
                    "Sólidos volátiles totales (mg/L)",
                    "Sólidos disueltos totales (mg/L)",
                    "Sólidos fijos totales (mg/L)",
                    "Sólidos sedimentables (ml/L-h)",
                    "DBO5 (mg/L)",
                    "DQO (mg/L)",
                    "Hierro total (mg Fe/L)",
                    "Sulfatos (mg/L)",
                    "Sulfuros (mg/L)",
                    "Clororus (mg/L)",
                    "Grasas y/o aceites (mg/L)",
                    "SAAM (mg/L)",
                    "Fósforo total (mg P/L)",
                    "Fosfato (mg P/L)",
                    "Fósforo orgánico (mg P/L)",
                    "Nitratos (mg N/L)",
                    "Nitritos (mg N/L)",
                    "Nitrógeno orgánico (mg N/L)",
                    "Nitrógeno total Kjeldahl (mg N/L)",
                    "Cadmio (mg Cd/L)",
                    "Cobre (mg Cu/L)",
                    "Cromo (mg Cr/L)",
                    "Cromo hexavalente (mg Cr6+/L)",
                    "Mercurio (mg Hg/L)",
                    "Niquel (mg Ni/L)",
                    "Plomo (mg Pb/L)",
                    "Cadmio sedimentable (mg Cd/L)",
                    "Cobre sedimentable (mg Cu/L)",
                    "Cromo sedimentable (mg Cr/L)",
                    "Mercurio sedimentable (mg Hg/L)",
                    "Plomo sedimentable (mg Pb/L)",
                    "Escherichia coli (UFC)",
                    "Coliformes totales (UFC)",
                    "Escherichia coli (NMP/100mL)",
                    "Coliformes totales (NMP/100mL)",
                    "Riqueza algas",
                    "Indice biológico BMWP",
                    "Clasificación Indice biológico BMWP",
                    "Observaciones (algas y macroinvertebrados)"
                ];
            } else if (tipoCarga === 'campo') {
                columnasRequeridas = [
                    "Código",
                    "Fecha Visita",
                    "Hora",
                    "Temperatura Ambiente (°C)",
                    "Temperatura Agua (°C)",
                    "pH (U de pH)",
                    "Oxígeno disuelto (mg/L)",
                    "Conductividad eléctrica (µS/cm)",
                    "Potencial Redox (mV)",
                    "Turbiedad (NTU)",
                    "Estado del tiempo",
                    "Apariencia",
                    "Olor",
                    "Color",
                    "Altura (m)",
                    "H1 (m)",
                    "H2 (m)",
                    "Observación"
                ];
            }
        
            var columnasFaltantes = columnasRequeridas.filter(function (columna) {
                return !columnas.includes(columna);
            });
        
            if (columnasFaltantes.length > 0) {
                alert("El archivo Excel es incorrecto. Faltan las siguientes columnas: " + columnasFaltantes.join(", "));
                return false;
            }
        
            return true;
        }
        var file;
        var rowsWithErrors = []; 

        function cargarArchivoExcel(tipoSeleccionado) {
            localStorage.removeItem('excelDataLaboratorio');
            localStorage.removeItem('excelDatacampo');
            localStorage.removeItem('excelData');
            let tipoCarga = tipoSeleccionado.toLowerCase();  
            console.log('Tipo de carga seleccionada:', tipoCarga); 
            if (tipoSeleccionado === 1) {
                tipoCarga = 'laboratorio';
            } else if (tipoSeleccionado === 2) {
                tipoCarga = 'campo';
            }
            var input = document.createElement('input');
            input.type = 'file';
            input.accept = '.xlsx, .xls';
            
            function esDecimal(valor) {
                return /^-?\d+(\.\d+)?$/.test(valor);
            }
            function validarDatosExcel(excelData) {
                const columnasValidar = [
                    "Temperatura Ambiente (°C)",
                    "Temperatura Agua (°C)",
                    "pH (U de pH)",
                    "Oxígeno disuelto (mg/L)",
                    "Conductividad eléctrica (µS/cm)",
                    "Potencial Redox (mV)",
                    "Turbiedad (NTU)",
                    "Clasificación caudal (Adim)",
                    "Número de verticales",
                    "Color verdadero (UPC)",
                    "Color triestimular 436 nm",
                    "Color triestimular 525 nm",
                    "Color triestimular 620 nm",
                    "Sólidos suspendidos totales (mg/L)",
                    "Sólidos totales (mg/L)",
                    "Sólidos volátiles totales (mg/L)",
                    "Sólidos disueltos totales (mg/L)",
                    "Sólidos fijos totales (mg/L)",
                    "Sólidos sedimentables (ml/L-h)",
                    "DBO5 (mg/L)",
                    "DQO (mg/L)",
                    "Hierro total (mg Fe/L)",
                    "Sulfatos (mg/L)",
                    "Sulfuros (mg/L)",
                    "Clororus (mg/L)",
                    "Grasas y/o aceites (mg/L)",
                    "SAAM (mg/L)",
                    "Fósforo total (mg P/L)",
                    "Fosfato (mg P/L)",
                    "Fósforo orgánico (mg P/L)",
                    "Nitratos (mg N/L)",
                    "Nitritos (mg N/L)",
                    "Nitrógeno orgánico (mg N/L)",
                    "Nitrógeno total Kjeldahl (mg N/L)",
                    "Cadmio (mg Cd/L)",
                    "Cobre (mg Cu/L)",
                    "Cromo (mg Cr/L)",
                    "Cromo hexavalente (mg Cr6+/L)",
                    "Mercurio (mg Hg/L)",
                    "Niquel (mg Ni/L)",
                    "Plomo (mg Pb/L)",
                    "Cadmio sedimentable (mg Cd/L)",
                    "Cobre sedimentable (mg Cu/L)",
                    "Cromo sedimentable (mg Cr/L)",
                    "Mercurio sedimentable (mg Hg/L)",
                    "Plomo sedimentable (mg Pb/L)",
                    "Escherichia coli (UFC)",
                    "Coliformes totales (UFC)",
                    "Escherichia coli (NMP/100mL)",
                    "Coliformes totales (NMP/100mL)",
                    "Riqueza algas",
                    "Indice biológico BMWP",
                    "Temperatura Ambiente (°C)",
                    "Temperatura Agua (°C)",
                    "pH (U de pH)",
                    "Oxígeno disuelto (mg/L)",
                    "Conductividad eléctrica (µS/cm)",
                    "Potencial Redox (mV)",
                    "Turbiedad (NTU)",
                    "Hora"
                ];
            
                let rowsWithErrors = []; 
        const errors = [];
        
        excelData.forEach((row, index) => {
            columnasValidar.forEach(columna => {
                const valor = row[columna];
                if (columna === "Hora") {
                    const horaValida = /^([01]\d|2[0-3]):([0-5]\d)$/.test(valor);
                    if (valor && !horaValida) {
                        console.error(`Valor inválido en fila ${index + 1}, columna "${columna}": ${valor}`);
                        errors.push({ row: index, column: columna });
                        rowsWithErrors.push(index);
                    }
                } else if (valor && !esDecimal(valor.toString())) {
                    console.error(`Valor inválido en fila ${index + 1}, columna "${columna}": ${valor}`);
                    errors.push({ row: index, column: columna });
                    rowsWithErrors.push(index);
                }
            });
        });
        
        if (rowsWithErrors.length > 0) {
            // Swal.fire({
            //     icon: 'warning',
            //     text: 'Algunas filas contienen valores inválidos. Por favor, revisa las filas resaltadas.'
            // });
            alert('Algunas filas contienen valores inválidos. Por favor, revisa las filas resaltadas.');
            $('#botonProcesar').hide(); 
            $("#botonProcesar2").hide();

        }
        
        return errors; 
    }
            input.addEventListener('change', function(event) {
                 file = event.target.files[0];
                if (file) {
                    var reader = new FileReader();
                    reader.onload = function(e) {
                        var data = new Uint8Array(e.target.result);
                        var workbook = XLSX.read(data, { type: 'array' });
                        
                        var firstSheetName = workbook.SheetNames[0];
                        var worksheet = workbook.Sheets[firstSheetName];
                        var range = XLSX.utils.decode_range(worksheet['!ref']);
                        var columns = [];
                        
                        for (var C = range.s.c; C <= range.e.c; ++C) {
                            var cell = worksheet[XLSX.utils.encode_cell({ r: range.s.r, c: C })];
                            columns.push(cell ? cell.v : '');
                        }
                        
                        if (!verificarColumnasExcel(tipoCarga, columns)) {
                            return;
                        }
                        
                        var excelData = [];
                        for (var R = range.s.r + 1; R <= range.e.r; ++R) {
                            var row = {};
                            var isEmptyRow = true;
                            
                            for (var C = range.s.c; C <= range.e.c; ++C) {
                                var cell = worksheet[XLSX.utils.encode_cell({ r: R, c: C })];
                                var columnName = columns[C];
                                var cellValue = (cell && cell.v) ? cell.v : '';
                                
                                var cellValue = '';
                                if (cell) {
                                    if (columnName === 'Fecha Visita' && cell.t === 'n') {
                                        var dateObject = XLSX.SSF.parse_date_code(cell.v);
                                        if (dateObject) {
                                            cellValue = ('0' + dateObject.d).slice(-2) + '/' + ('0' + dateObject.m).slice(-2) + '/' + (dateObject.y % 100);
                                            console.log('Fecha procesada:', cellValue);
                                        } else {
                                            console.error('Error al convertir fecha:', cell.v);
                                        }
                                    
                                                                                                        
                                    } else if (columnName === 'Hora' && cell.t === 'n') {
                                        var dateObject = XLSX.SSF.parse_date_code(cell.v);
                                        if (dateObject) {
                                            cellValue = ('0' + dateObject.H).slice(-2) + ':' + ('0' + dateObject.M).slice(-2);
                                        } else {
                                            cellValue = cell.v;
                                        }
                                    } else if (typeof cell.v === 'string' && cell.v.trim() === '#N/A') {
                                        cellValue = '';
                                    } else if (typeof cell.v === 'string' && cell.v.trim() === '') {
                                        cellValue = '';
                                    } else {
                                        cellValue = cell.v;
                                    
                                    }
                                }
                                
                                row[columnName] = cellValue;
                                
                                if (cellValue !== '') {
                                    isEmptyRow = false;
                                }
                            }
                            
                            if (!isEmptyRow) {
                                excelData.push(row);
                            }
                        }
                        
                        console.log('Datos procesados:', excelData);
                        const errors = validarDatosExcel(excelData);
                        var data = new Uint8Array(e.target.result);
                        verificarCodigosEstacion(excelData);
                        $("#excelGridContainer").empty();
                        
                        var columnas = columns.map(key => ({
                            dataField: key,
                            caption: key,
                            allowFiltering: true,
                            allowSorting: true,
                            dataType: (key === "Fecha Visita" ? "date" : (key === "Hora" ? "string" : (key === "pH (U. de pH)" ? "number" : "string")))
                        }));
                        
                        $("#excelGridContainer").dxDataGrid({
                            dataSource: excelData,
                            columns: columnas,
                            columnAutoWidth: true,
                            wordWrapEnabled: true,
                            showBorders: true,
                            paging: {
                                pageSize: 10
                            },
                            // pager: {
                            //     showPageSizeSelector: true,
                            //     allowedPageSizes: [5, 10, 20],
                            //     showInfo: true
                            // },
                            filterRow: {
                                visible: true
                            },
                            headerFilter: {
                                visible: true
                            },
                            searchPanel: {
                                visible: true,
                                width: 240,
                                placeholder: "Buscar..."
                            },
                            export: {
                                enabled: true,
                                fileName: "DatosExcel"
                            },
                            onRowPrepared: function(info) {
                                if (info.rowType === "data") {
                                    var rowIndex = excelData.indexOf(info.data); 
                        
                                    if (rowsWithErrors.includes(rowIndex)) {
                                        $(info.rowElement).css("background-color", "red"); 
                                    }
                                    errors.forEach(error => {
                                        if (error.row === rowIndex) {
                                            const columnIndex = columns.indexOf(error.column);
                                            const cellElement = $(info.rowElement).find(`td:nth-child(${columnIndex + 1})`); 
                                            cellElement.css("background-color", "orange"); 
                                            $('#botonProcesar').hide();
                                            $('#botonProcesar2').hide();
                                        }
                                    });
                                }
                            },
                        });
        
                        $('#modalExcel').dialog({
                            modal: true,
                            width: '90%',
                            height: 600,
                            title: "Datos del archivo Excel",
                            close: function() {
                                $("#excelGridContainer").dxDataGrid("instance").option("dataSource", []);
                                $('#botonProcesar').hide();
                                $('#botonProcesar2').hide();
                                var dataGridInstance = $("#excelGridContainer").dxDataGrid("instance");
                                if (dataGridInstance) {
                                    dataGridInstance.dispose();
                                }
                            }
                        });
        
                        if (excelData.length > 0) {
                            console.log('Tipo de carga procesada: ' + tipoCarga);
        
                            $('#botonProcesar').hide();
                            $('#botonProcesar2').hide();
        
                            if (tipoCarga === 'laboratorio') {
                                $('#botonProcesar').show();
                            } else if (tipoCarga === 'campo') {
                                $('#botonProcesar2').show();
                            }
                        } else {
                            $('#botonProcesar').hide();
                            $('#botonProcesar2').hide();
                        }
                    };
                    reader.readAsArrayBuffer(file);
                }
            });
        
            input.click();
        }

    var campaignId = $('#btnCargarExcel').data('campaignId');

    var excelData = JSON.parse(localStorage.getItem('excelData'));



    function agregarReporte(datos) {
        
        if (Array.isArray(datos)) {
            var promises = datos.map(function(dato) {
                var data = {
                    idCampaña: dato.idCampaña,
                    idEstacion: dato.idEstacion,
                    idResultadoCampo: dato.idResultadoCampo
                };
                
                if (dato.idMuestraCompuesta) {
                    data.idMuestraCompuesta = dato.idMuestraCompuesta;
                }
                var urlBase = $("#SIM").data("url");
                var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarReporteLaboratorio";
        
                
                return $.ajax({
                    url: urlCompleta,
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(data)
                });
            });
            
            Promise.all(promises).then(function(responses) {
                console.log('Reportes agregados exitosamente');
                console.log(responses);
                Swal.fire({
                    icon: 'success',
                    title: 'success',
                    text: 'Excel Agregado con exito'
                });
                // location.reload()
            }).catch(function(xhr) {
                console.error('Error al agregar reportes: ', xhr.status);
                console.log(datos);
            });
        } else {
            console.error('datos no es un arreglo');
        }
    }
    function agregarMuestraCompuesta(datos) {
        console.log(datos);
        
        if (Array.isArray(datos)) {
            var promises = datos.map(function(dato) {
                var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarMuestraCompuesta";

                return $.ajax({
                    url: urlCompleta,
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(dato)
                });
            });
            
            return Promise.all(promises).then(function(responses) {
                console.log('Muestras compuestas agregadas exitosamente');
                console.log(responses);
                
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
        async function obtenerIdEstacion(codigo) {
            try {
                const response = await $.ajax({
                    url: 'http://localhost:5078/api/estacion/ObtenerEstacionCodigo/' + codigo,
                    type: 'GET',
                    dataType: 'json'
                });
                return response.result.idEstacion;
            } catch (error) {
                console.error('Error al obtener ID de estación: ', error);
                return null;
            }
        }

        async function obtenerDatosReporte(campaignId) {
            var excelDataLaboratorio = JSON.parse(localStorage.getItem('excelDataLaboratorio')) || [];
            var excelDataCampo = JSON.parse(localStorage.getItem('excelDatacampo')) || [];
            
            var datosReporte = [];
            
            for (const datosCampo of excelDataCampo) {
                var reporteCampo = {
                    idCampaña: campaignId,
                    idEstacion: await obtenerIdEstacion(datosCampo.Código),
                    idResultadoCampo: datosCampo.idCampo,
                    idMuestraCompuesta: null, 
                    datosCampo: datosCampo,
                    datosLaboratorio: null
                };
                
                datosReporte.push(reporteCampo);
            }
            
            for (const datosLaboratorio of excelDataLaboratorio) {
                var reporteLaboratorio = {
                    idCampaña: campaignId,
                    idEstacion: await obtenerIdEstacion(datosLaboratorio.Código),
                    idResultadoCampo: null,
                    idMuestraCompuesta: datosLaboratorio.idMuestraCompuesta,
                    datosCampo: null,
                    datosLaboratorio: datosLaboratorio
                };
                
                datosReporte.push(reporteLaboratorio);
            }
            
            if (Array.isArray(datosReporte)) {
                console.log('Datos para agregar reporte:', datosReporte);
                agregarReporte(datosReporte); 
                return datosReporte;
            } else {
                console.error('datosReporte no es un arreglo');
                return [];
            }
        }
        
        
function guardarExcel(file) {
    var campaignId = $('#btnCargarExcel').data('campaignId') || 1; 
    var formData = new FormData();
    formData.append('file', file);            
    formData.append('nombreUsuario', 'null'); 
    formData.append('IdCampaña', campaignId); 

    // console.log('Datos a enviar:', {
    //     IdCampaña: campaignId,
    //     nombreUsuario: 'null',
    //     file: file
    // });

    var urlBase = $("#SIM").data("url");
    var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/AgregarHistorialExcel";

    $.ajax({
        url: 'http://localhost:5078/api/HistorialExcel/AgregarHistorialExcel',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log('Excel guardado exitosamente', response);
        },
        error: function (xhr) {
            console.error('Error al guardar Excel: ', xhr.status, xhr.responseText);
        }
    });
}
async function obtenerDatos(campaignId) {
    var excelData = JSON.parse(localStorage.getItem('excelDataLaboratorio'));

    if (!Array.isArray(excelData)) {
        console.error('excelData no es un arreglo o es nulo:', excelData);
        return []; 
    }

    console.log('Datos de localStorage:', excelData);
    
    var datosReporte = [];
    
    for (const row of excelData) {
        var idInsitu = row.IdInsitu;
        var idFisico = row.IdFisico;
        var idQuimico = row.IdQuimico;
        var idNutriente = row.IdNutriente;
        var idMetalAgua = row.IdMetalAgua;
        var idMetalSedimental = row.idMetalSedimental;
        var idBiologico = row.IdBiologico;

        datosReporte.push({
            idInsitu: idInsitu,
            idFisico: idFisico,
            idQuimico: idQuimico,
            idNutriente: idNutriente,
            idMetalAgua: idMetalAgua,
            idMetalSedimental: idMetalSedimental, 
            idBiologico: idBiologico,
        });
    }
    
    return datosReporte;
}
$('#dropdownList a').click(function (e) {
    e.preventDefault(); // Evitar redirección
    var plantillaSeleccionada = $(this).data('value');
    
    // Aquí puedes usar el mismo código de descarga
    var rutaArchivo;
    if (plantillaSeleccionada === "plantilla1") {
        rutaArchivo = "/SIM/wwwroot/uploads/plantillas/PlantillaLABORATORIO.xlsx";
    } else if (plantillaSeleccionada === "plantilla2") {
        rutaArchivo = "/SIM/wwwroot/uploads/plantillas/PlantillaReporteCampo.xlsx";
    }

    if (rutaArchivo) {
        var enlaceDescarga = document.createElement('a');
        enlaceDescarga.href = rutaArchivo;
        enlaceDescarga.download = rutaArchivo.split('/').pop();
        enlaceDescarga.click();
    }
});


        
        cargarcampañas();
        cargarFases()
        inicializarGrid();
    });
