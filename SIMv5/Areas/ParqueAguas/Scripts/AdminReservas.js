var IdReserva = -1;

$(document).ready(function () {
    var IdRegistro = -1;

       //Reservas
    $("#GidListado").dxDataGrid({
        dataSource: ReservasDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 5
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'D_RESERVA', width: '5%', caption: 'Fecha Reserva', dataType:'date'},
            { dataField: 'S_OBSERVACIONES', width: '35%', caption: 'Responsable' },
            { dataField: 'S_NRO_COMPROBANTE', width: '10%', caption: 'Comprobante', dataType: 'string' },
            { dataField: 'N_NROVISITANTES', width: '10%', caption: 'Número Personas', dataType: 'number' },
            {
                dataField: 'B_POS', width: '20%', caption: 'Estado', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '0') {
                        return 'No se ha confirmado en taquilla';
                    }
                    if (cellInfo.value == '1') {
                        return 'Fue confirmada en taquilla, pero aún no ha sido facturada';
                    }
                    if (cellInfo.value == '1') {
                        return 'Facturada';
                    }
                }
            },
            {
                dataField: 'B_CANCELADA', width: '10%', caption: 'Cancelada', dataType: 'string', customizeText: function (cellInfo) {
                    if (cellInfo.value == '1') {
                        return 'Si';
                    }
                    else {
                        return 'No';
                    }
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/edit.png',
                        height: 20,
                        hint: 'Cancelar la reserva',
                        onClick: function (e) {

                            
                                if (confirm('Está seguro de cancelar esta reserva?')) {

                                    var _Ruta = $('#SIM').data('url') + "ParqueAguas/api/AdminReservasAPI/CancelarReserva";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data !== null) {
                                                IdRegistro = parseInt(data.id);
                                                $('#GidListado').dxDataGrid({ dataSource: ReservasDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Evento no esperado!');
                                        });
                               
                            }
                        }
                    }).appendTo(container);
                }
            },
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
               
            }
        }
    });

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la Serie Documental...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre de la Serie Documental es requerido!"
        }]
    }).dxTextBox("instance");

    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        value: "",
        height: 90
    }).dxTextArea("instance");

    var txtCodInterno = $("#txtCodInterno").dxTextBox({
        value: "",

    }).dxTextBox("instance");

    var chkActivo = $("#chkActivo").dxCheckBox({
        value: false,
        width: 80,
        text: "Si",

    }).dxCheckBox("instance");

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdRegistro;
            var nombre = txtNombre.option("value");
            var codInterno = txtCodInterno.option("value");
            var descripcion = txtDescripcion.option("value");
            var estado = chkActivo.option("value");
            var params = { id: id, nombre: nombre, descripcion: descripcion, habilitado: estado, codInterno: codInterno };
            var _Ruta = $('#SIM').data('url') + "AdministracionDocumental/api/AdminDocumentalAPI/GuardarSerieDocumental";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: SerieDocumentalDataSource });
                        $("#PopupNuevaSerie").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popup = $("#PopupNuevaSerie").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Serie Documental"
    }).dxPopup("instance");


});

var ReservasDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_OBSERVACIONES","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'ParqueAguas/api/AdminReservasAPI/GetReservas', {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false,
            CodFuncionario: 0
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});



