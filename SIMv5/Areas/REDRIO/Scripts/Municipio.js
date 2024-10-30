$(document).ready(function () {
    var listaDatosMunicipio = [];
     
    function inicializarGrid() {
        $("#gridMunicipios").dxDataGrid({
            dataSource: listaDatosMunicipio,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            columns: [
                { dataField: "IdMunicipio", caption: "ID" },
                { dataField: "nombreMunicipio", caption: "Nombre Municipio" },
                // { dataField: "departamento.nombreDepartamento", caption: "Departamento" }, 
                {
                    caption: "Acciones",
                    width: 100,
                    cellTemplate: function (container, options) {
                        // Crear botones de editar y eliminar
                        $("<i>").addClass("fas fa-edit")
                            .attr("title", "Editar")
                            .css("cursor", "pointer")
                            .appendTo(container)
                            .click(function () {
                                editarFilaMunicipio(options.data);
                            });
    
                        $("<i>").addClass("fas fa-trash")
                            .attr("title", "Eliminar")
                            .css("cursor", "pointer")
                            .appendTo(container)
                            .click(function () {
                                eliminarFilaMunicipio(options.data);
                            });
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
    


   
    function cargarMunicipios() {
        var urlBase = $("#SIM").data("url");
        var urlCompleta = urlBase + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerMunicipios";
    
        console.log("URL llamada: ", urlCompleta);
    
        $.getJSON(urlCompleta, function (data) {
            listaDatosMunicipio = data.result;
            console.log(listaDatosMunicipio);
    
            $("#gridMunicipios").dxDataGrid("instance").option("dataSource", listaDatosMunicipio);
        }).fail(function () {
            alert('Error al cargar municipios.');
            console.log(urlBase);
        });
    }
   

    inicializarGrid();
    cargarMunicipios();

    $('#btnGuardarDato').click(function () {
        var cm = $('#cm').val();
        var nombre = $('#nombre').val();
        var Departamento = $('#Departamento').val();

        if (!cm || !nombre || !Departamento) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        var nuevoDato = { cm, nombre, Departamento };

        $.ajax({
            url: '/REDRIO/CrearMunicipio',
            type: 'POST',
            data: nuevoDato,
            success: function (result) {
                if (result.success) {
                    cargarMunicipios();
                    $('#modalNuevoDato').modal('hide');
                    $('#formNuevoDato')[0].reset();
                } else {
                    alert('Error al guardar municipio.');
                }
            }
        });
    });

    function editarFilaMunicipio(data) {
        $('#editCm').val(data.cm);
        $('#editNombre').val(data.nombre);
        $('#editDepartamento').val(data.Departamento);
        $('#modalEditarDato').modal('show');
    }

    $('#btnActualizarDato').click(function () {
        var cm = $('#editCm').val();
        var nombre = $('#editNombre').val();
        var Departamento = $('#editDepartamento').val();

        if (!nombre || !Departamento) {
            alert("Por favor, complete todos los campos.");
            return;
        }

        var datoActualizado = { cm, nombre, Departamento };

        $.ajax({
            url: '/REDRIO/ActualizarMunicipio',
            type: 'POST',
            data: datoActualizado,
            success: function (result) {
                if (result.success) {
                    cargarMunicipios();
                    $('#modalEditarDato').modal('hide');
                } else {
                    alert('Error al actualizar municipio.');
                }
            }
        });
    });

    function eliminarFilaMunicipio(data) {
        Swal.fire({
            title: '�Est�s seguro?',
            text: "�No podr�s revertir esta acci�n!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'S�, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/REDRIO/EliminarMunicipio',
                    type: 'POST',
                    data: { id: data.cm },
                    success: function (result) {
                        if (result.success) {
                            cargarMunicipios();
                            Swal.fire('Eliminado!', 'El registro ha sido eliminado.', 'success');
                        } else {
                            alert('Error al eliminar municipio.');
                        }
                    }
                });
            }
        });
    }

    var ListDepartamentos = JSON.parse(localStorage.getItem('ListDepartamentos')) || [];
    function poblarSelectDepartamentos(selectElementId) {
        var selectElement = $('#' + selectElementId);
        selectElement.empty();
        selectElement.append('<option value="">Seleccione un departamento</option>');
        ListDepartamentos.forEach(function (departamento) {
            selectElement.append('<option value="' + departamento.nombre + '">' + departamento.nombre + '</option>');
        });
    }

    poblarSelectDepartamentos('Departamento');
    poblarSelectDepartamentos('editDepartamento');
});
