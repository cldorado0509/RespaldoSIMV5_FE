$(document).ready(function () {
    var listaDatosEstacion = [];

    $('#btnVolverAgregar').click(function () {
        $('#vistaAgregar').hide();   
        $('#vistaPrincipal').show(); 
    });

    $('#btnVolverEditar').click(function () {
        $('#vistaEditar').hide();   
        $('#vistaPrincipal').show(); 
    });
    $('#btnAgregar').click(function () {
        $('#vistaAgregar').show(); 
        $('#vistaEditar').hide();   
        $('#vistaPrincipal').hide(); 
        $('#formNuevoDato')[0].reset(); 
        cargarMunicipio(); 
        cargarTipoFuente()
    });
    function cargarEstaciones() {
        // var urlBase = $("#SIM").data("url");
        // var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ObtenerEstaciones";
        var urlCompleta = 'http://localhost:5078/api/Estacion/ObtenerEstaciones'
        console.log("URL llamada: ", urlCompleta);
        
        $.getJSON(urlCompleta, function (data) {
            listaDatosEstacion = data.result;
            console.log(listaDatosEstacion);
    
            $("#gridContainer").dxDataGrid("instance").option("dataSource", listaDatosEstacion);
        }).fail(function () {
            alert('Error al cargar Estaciones.');
        });
    }

    function cargarTipoFuente(selectedTipoFuenteId, isEditing = false) {
        // var urlBase = $("#SIM").data("url");
        // var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ObtenerTipoFuente";
        var urlCompleta= 'http://localhost:5078/api/tipoFuente/ObtenerTiposFuentes'
        console.log("URL llamada: ", urlCompleta);
    
        $.getJSON(urlCompleta, function (data) {
            console.log("Datos de TipoFuente:", data); 
            
            var TipoFuentelect = isEditing ? $('#editTipoFuente') : $('#TipoFuente');
            TipoFuentelect.empty(); 
            TipoFuentelect.append($('<option>', { value: '', text: 'Seleccione una TipoFuente' }));
            
            if (data.result && data.result.length > 0) {
                $.each(data.result, function (index, TipoFuente) {
                    TipoFuentelect.append($('<option>', { 
                        value: TipoFuente.idTipoFuente,
                        text: TipoFuente.nombreTipoFuente
                    }));
                });
            } else {
                console.log("No se encontraron TipoFuente."); 
            }
            
            if (selectedTipoFuenteId) {
                TipoFuentelect.val(selectedTipoFuenteId);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("Error al cargar TipoFuente:", textStatus, errorThrown); 
            alert('Error al cargar las TipoFuente.'); 
        });
    }
    function cargarMunicipio(selectedMunicipioId, isEditing = false) {
        // var urlBase = $("#SIM").data("url");
        // var urlCompleta = urlBase + "REDRIO/api/REDRIOApi/ObtenerMunicipio";
        var urlCompleta= 'http://localhost:5078/api/Municipio/ObtenerMunicipios'
        console.log("URL llamada: ", urlCompleta);
    
        $.getJSON(urlCompleta, function (data) {
            console.log("Datos de Municipio:", data); 
            
            var Municipiolect = isEditing ? $('#editMunicipio') : $('#Municipio');
            Municipiolect.empty(); 
            Municipiolect.append($('<option>', { value: '', text: 'Seleccione una Municipio' }));
            
            if (data.result && data.result.length > 0) {
                $.each(data.result, function (index, Municipio) {
                    Municipiolect.append($('<option>', { 
                        value: Municipio.idMunicipio,
                        text: Municipio.nombre
                    }));
                });
            } else {
                console.log("No se encontraron Municipio."); 
            }
            
            if (selectedMunicipioId) {
                Municipiolect.val(selectedMunicipioId);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("Error al cargar Municipio:", textStatus, errorThrown); 
            alert('Error al cargar las Municipio.'); 
        });
    }
    
    
    

    
    function inicializarGrid() {
        $("#gridContainer").dxDataGrid({
            dataSource: listaDatosEstacion,
            columnAutoWidth: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            columns: [
                {
                    caption: "ID",
                    cellTemplate: function(container, options) {
                        container.text(options.rowIndex + 1);
                    }
                },
                { dataField: "nombreEstacion", caption: "Nombre de Estacion" },
                { dataField: "codigo", caption: "Codigo" },
                { dataField: "tipoFuente.nombreTipoFuente", caption: "TipoFuente" },
                { dataField: "municipio.nombre", caption: "Municipio" },
                {
                    caption: "Acciones",
                    width: 150,
                    cellTemplate: function (container, options) {
                        var editButton = $("<i>")
                            .addClass("fas fa-edit")
                            .attr("title", "Editar Estacion")
                            .css({ "cursor": "pointer", "margin-right": "10px" })
                            .click(function () {
                                editarFilaEstacion(options.data);
                            });

                        var deleteButton = $("<i>")
                            .addClass("fas fa-trash")
                            .attr("title", "Eliminar Estacion")
                            .css({ "cursor": "pointer", "margin-right": "10px" })
                            .click(function () {
                                eliminarFilaEstacion(options.data);
                            });
                            
                            
                        
                            var viewButton = $("<i>")
                            .addClass("fas fa-eye")
                            .attr("title", "Ver Detalles")
                            .css({ "cursor": "pointer", "margin-right": "10px" })
                            .click(function () {
                                abrirModalDetalles(options.data);
                            });
                    
                        container.append(editButton).append(deleteButton).append(viewButton);
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
                fileName: "Estaciones"
            }
        });
    }
    function formatearFecha(fecha) {
        if (!fecha) return "N/A";
    
        const fechaObj = new Date(fecha);
    
        if (isNaN(fechaObj)) return "N/A";
    
        const dia = fechaObj.getDate().toString().padStart(2, '0');
        const mes = (fechaObj.getMonth() + 1).toString().padStart(2, '0'); 
        const año = fechaObj.getFullYear();
        return `${dia}/${mes}/${año}`;
    }
    function abrirModalDetalles(data) {
        console.log("Nombre Estación antes de decodificar:", data.nombreEstacion);
    
        let nombreEstacion;
        try {
            nombreEstacion = decodeURIComponent(data.nombreEstacion);
        } catch (e) {
            console.error("Error al decodificar nombreEstacion:", e);
            nombreEstacion = data.nombreEstacion;
        }
    
        $("#modalIdEstacion").text(data.idEstacion);
        $("#modalNombreEstacion").text(nombreEstacion);
        $("#modalFechaCreacion").text(formatearFecha(data.fecha_creacion));
        $("#modalGradosLatitud").text(data.grados_latitud || "N/A");
        $("#modalGradosLongitud").text(data.grados_longitud || "N/A");
        $("#modalMinutosLatitud").text(data.minutos_latitud || "N/A");
        $("#modalMinutosLongitud").text(data.minutos_longitud || "N/A");
        $("#modalSegundosLatitud").text(data.segundos_latitud || "N/A");
        $("#modalSegundosLongitud").text(data.segundos_longitud || "N/A");
    
        $('#detallesModal').modal('show');
    }
    

    $("#btnGuardarDato").click(function () {
        guardarNuevaEstacion();
        console.log('HOLA');
        
    });

    function guardarNuevaEstacion() {
        // Obtener los valores del formulario
        const nuevaEstacion = {
            nombreEstacion: $("#nombreEstacion").val(),
            idTipoFuente: $("#TipoFuente").val(),
            idMunicipio: $("#Municipio").val(),
            Codigo: $("#Codigo").val(),
            Elevacion: $("#Elevacion").val(),
            Grados_latitud: $("#GradosLatitud").val(),
            Grados_longitud: $("#GradosLongitud").val(),
            Minutos_latitud: $("#MinutosLatitud").val(),
            Minutos_longitud: $("#MinutosLongitud").val(),
            Segundos_latitud: $("#SegundosLatitud").val(),
            Segundos_longitud: $("#SegundosLongitud").val(),
        };
    
        // Validación básica (puedes mejorarla según los requisitos)
        if (!nuevaEstacion.nombreEstacion || !nuevaEstacion.idTipoFuente || !nuevaEstacion.idMunicipio) {
            swal.fire({
                icon: 'warning',
                text: 'Todos los campos son requeridos'
            })
            return;
        }
    
        // Aquí deberías realizar la solicitud AJAX para guardar la nueva estación
        $.ajax({
            type: 'POST',
            url: 'http://localhost:5078/api/estacion/AgregarEstacion', // URL del API para guardar
            contentType: 'application/json',
            data: JSON.stringify(nuevaEstacion),
            success: function (response) {
                swal.fire({
                    icon: 'success',
                    text: 'Se guardo exitosamente'
                });
                cargarEstaciones();
                $('#vistaAgregar').hide();
                $('#vistaPrincipal').show();
                inicializarGrid();
            },
            error: function (error) {
                console.log("Error al guardar la nueva estación:", error);
                swal.fire({
                    icon: 'error',
                    text: 'Ocurrio un error al intentar guardar, por favor verifique la información'
                })
                }
        });
    }


    function editarFilaEstacion(data) {
     

        $("#editIdEstacion").val(data.idEstacion);
        $("#editNombreEstacion").val(data.nombreEstacion);
        $("#editCodigo").val(data.codigo);
        cargarTipoFuente(data.idTipoFuente, true);         
        cargarMunicipio(data.idMunicipio, true);         
        $("#editGradosLatitud").val(data.grados_latitud || "N/A");
        $("#editGradosLongitud").val(data.grados_longitud || "N/A");
        $("#editMinutosLatitud").val(data.minutos_latitud || "N/A");
        $("#editMinutosLongitud").val(data.minutos_longitud || "N/A");
        $("#editSegundosLatitud").val(data.segundos_latitud || "N/A");
        $("#editSegundosLongitud").val(data.segundos_longitud || "N/A");
        $('#vistaEditar').show();   
        $('#vistaPrincipal').hide(); 
        $('#vistaAgregar').hide();   
    }

    $('#btnActualizarDato').click(function () {
        const idEstacion = $("#editIdEstacion").val();
        console.log('Valor de idEstacion:', idEstacion); 
    
        const estacionEditada = {
            idEstacion: idEstacion,
            nombreEstacion: $("#editNombreEstacion").val() || null,
            codigo: $("#editCodigo").val() || null,
            idTipoFuente: $("#editTipoFuente").val() || null,
            idMunicipio: $("#editMunicipio").val() || null,
            grados_latitud: $("#editGradosLatitud").val() || null,
            grados_longitud: $("#editGradosLongitud").val() || null,
            minutos_latitud: $("#editMinutosLatitud").val() || null,
            minutos_longitud: $("#editMinutosLongitud").val() || null,
            segundos_latitud: $("#editSegundosLatitud").val() || null,
            segundos_longitud: $("#editSegundosLongitud").val() || null
        };
    
        
    
        $.ajax({
            type: 'PUT',
            url: 'http://localhost:5078/api/estacion/ActualizarEstacion/' + estacionEditada.idEstacion,
            contentType: 'application/json',
            data: JSON.stringify(estacionEditada),
            success: function (response) {
            swal.fire({
                icon: 'success',
                text: 'Se actualizo exitosamente'
            })
            cargarEstaciones();
                $('#vistaEditar').hide();
                $('#vistaPrincipal').show();
                inicializarGrid();
            },
            error: function (error) {
                console.log("Error al actualizar la estación:", error);
                swal.fire({
                    icon: 'error',
                    text: 'Ocurrio un error al Actulizar verifique la información'
                })
            }
        });
    });

    function eliminarFilaEstacion(data) {
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

                $.ajax({
                    url: 'http://localhost:5078/api/estacion/EliminarEstacion/' + data.idEstacion,
                    type: 'DELETE',
                    success: function (result) {
                        Swal.fire('Eliminado!', 'El registro ha sido eliminado.', 'success');
                        cargarEstaciones();
                    },
                    error: function () {
                        Swal.fire({
                            icon: 'warning',
                            // title: 'Error',
                            text: 'La Estación no se puede eliminar debido a que tiene reportes relacionados.'
                        });
                    }
                });
            }
        });
    }
    
    
    
    cargarEstaciones();
    cargarMunicipio();
    cargarTipoFuente();
    inicializarGrid();

})