
$(document).ready(function () {
    var ListaDepartamentos = [];
     
    // Inicializar el DataGrid con la lista de departamentos
    $("#gridContainer").dxDataGrid({
        dataSource: ListaDepartamentos,
        columnAutoWidth: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },

        columns: [
            { dataField: "id", caption: "CM" },
            { dataField: "nombreDepartamento", caption: "Nombre" },
            {
                caption: "Acciones",
                width: 100,
                cellTemplate: function (container, options) {
                    var editButton = $("<i>")
                        .addClass("fas fa-edit")
                        .attr("title", "Editar Departamento")
                        .css({ "cursor": "pointer", "margin-right": "10px" })
                        .click(function () {
                            EditDepartamento(options.data);
                        });

                    var deleteButton = $("<i>")
                        .addClass("fas fa-trash")
                        .attr("title", "Eliminar Departamento")
                        .css({ "cursor": "pointer", "margin-right": "10px" })
                        .click(function () {
                            EliminarDepartamento(options.data);
                        });

                    // Añadir los botones al contenedor
                    container.append(editButton).append(deleteButton);
                }
            }
        ],
        paging: {
            pageSize: 5 // Cambia este valor según sea necesario
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20], // Opciones de tamaño de página
            showInfo: true
        },
        filterRow: {
            visible: true
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        export: {
            enabled: true,
            fileName: "Departamentos"
        }
    });

    // Cargar datos al grid
    function cargarDatosAlGrid() {
        $.getJSON('http://localhost:5078/api/Departamento/ObtenerDepartamentos', function (data) {
            ListaDepartamentos = data.result;
            console.log(data);
            // $.getJSON('/REDRIO/api/REDRIO/GetMunicipiosAsync', function (data) {
            //     ListaDepartamentos = data; // Asignar datos a la lista
            $("#gridContainer").dxDataGrid("instance").option("dataSource", ListaDepartamentos);
        }).fail(function () {
            alert('Error al cargar municipios.');
        });
    }

    $('#btnGuardarDato').click(function () {
        var cm = $('#cm').val();
        var nombre = $('#nombre').val();

        if (cm && nombre) {
            var nuevoDato = { cm: cm, nombre: nombre };
            ListDepartamentos.push(nuevoDato);
            localStorage.setItem('ListDepartamentos', JSON.stringify(ListDepartamentos));
            cargarDatosAlGrid();
            $('#formNuevoDato')[0].reset();
            $('#modalNuevoDato').modal('hide');
        } else {
            alert("Por favor, complete todos los campos.");
        }
    });

    // Función para editar un departamento
    function EditDepartamento(data) {
        $('#editIndex').val(ListDepartamentos.findIndex(dep => dep.cm === data.cm));
        $('#editCm').val(data.cm);
        $('#editNombre').val(data.nombre);
        $('#modalEditarDato').modal('show');
    }

    $('#btnActualizarDato').click(function () {
        var index = $('#editIndex').val();
        var cm = $('#editCm').val();
        var nombre = $('#editNombre').val();

        ListDepartamentos[index] = { cm: cm, nombre: nombre };
        localStorage.setItem('ListDepartamentos', JSON.stringify(ListDepartamentos));
        cargarDatosAlGrid();
        $('#modalEditarDato').modal('hide');
    });

    // Función para eliminar un departamento
    function EliminarDepartamento(data) {
        

        Swal.fire({
            title: '¿Estás seguro?',
            text: "¿Esta acción no se puede deshacer!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                // Eliminar el elemento del array
                ListDepartamentos = ListDepartamentos.filter(dep => dep.cm !== data.cm);
                localStorage.setItem('ListDepartamentos', JSON.stringify(ListDepartamentos));

                // Refrescar la tabla
                $("#gridContainer").dxDataGrid("instance").refresh();
                Swal.fire('Eliminado!', 'El registro ha sido eliminado.', 'success');
            }

        });
    }

    // Cargar datos al iniciar
    cargarDatosAlGrid();
});
