$(document).ready(function () {
    var IdRegistro = -1;

    $("#GidListado").dxDataGrid({
        dataSource: MonitoreosDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        export: {
            enabled: true,
            fileName: "Reporte",
            allowExportSelectedData: false
        },
        paging: {
            pageSize: 10
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
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID', width: '10%', caption: 'Código', alignment: 'center' },
            { dataField: 'S_PLACA', width: '10%', caption: 'Placa', dataType: 'string' },
            { dataField: 'ID_ETIQUETA', width: '10%', caption: 'Etiqueta Id', dataType: 'string' },
            { dataField: 'D_MONITOREO', width: '15%', caption: 'F. Monitoreo', dataType: 'date', format: 'shortDateShortTime' },
            { dataField: 'S_EMPRESA', caption: 'Empresa', dataType: 'string' },
            { dataField: 'N_VALOR_MONITOREO', width: '10%', caption: 'Valor Monitoreo', dataType: 'number' },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../../Content/Images/edit.png',
                        height: 20,
                        hint: 'Editar el Monitoreo',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "EtiquetasAmbientales/api/RevisionApi/ObtenerMonitoreo";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        IdRegistro = parseInt(data.id);
                                        txtPlaca.option("value", data.placa);
                                        var FecMonitoreo = data.fechaMonitoreo != null ? new Date(data.fechaMonitoreo) : "";
                                        dpMonitoreo.option("value", FecMonitoreo);
                                        txtValor.option("value", data.valorMonitoreo);
                                        txtRPM.option("value", data.rPMRalenti);
                                        txtEmpresa.option("value", data.empresa);
                                        txtKilometraje.option("value", data.kilometraje);
                                        txtObservaciones.option("value", data.observaciones);
                                        txtFoto1.option("value", data.foto1);
                                        txtFoto2.option("value", data.foto2);
                                        txtFoto3.option("value", data.foto3);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../../Content/Images/Delete.png',
                        height: 20,
                        hint: 'Eliminar el Monitoreo',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el Monitoreo : ' + options.data.S_PLACA + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "EtiquetasAmbientales/api/RevisionApi/EliminarMonitoreo";
                                    $.getJSON(_Ruta,
                                        {
                                            objData: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar el Monitoreo');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: CategoriaDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar el Monitoreo');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }
        ]
    });


    var txtPlaca = $("#txtPlaca").dxTextBox({
        placeholder: "Ingrese la Placa del Vehículo...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La Placa es obligatoria!"
        }]
    }).dxTextBox("instance");

    var dpMonitoreo = $("#dpMonitoreo").dxDateBox({
        type: "datetime",
        value: new Date(),
        showAnalogClock: false,
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La fecha del Monitoreo es obligatoria!"
        }]
    }).dxDateBox("instance");

    var txtRPM = $("#txtRPM").dxNumberBox({
        placeholder: "Ingrese las RPM a Ralenti...",
        format: "#,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Valor de las RPM a Ralenti es obligatorio!"
        }]
    }).dxNumberBox("instance");

    var txtKilometraje = $("#txtKilometraje").dxNumberBox({
        placeholder: "Ingrese el Kilometraje...",
        format: "#,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El Valor del Kilometraje es obligatorio!"
        }]
    }).dxNumberBox("instance");

    var txtValor = $("#txtValor").dxNumberBox({
        placeholder: "Ingrese el valor del Monitoreo!...",
        format: "#,##0.##",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El valor del Monitoreo es obligatorio!"
        }]
    }).dxNumberBox("instance");

    var txtEmpresa = $("#txtEmpresa").dxTextBox({
        placeholder: "Ingrese el nombre de la Empresa...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La Empresa es obligatoria!"
        }]
    }).dxTextBox("instance");


    var txtObservaciones = $("#txtObservaciones").dxTextArea({
        value: "",
        height: 90,
    }).dxTextArea("instance");

    var txtFoto1 = $("#txtFoto1").dxTextBox({
        placeholder: "Ingrese la ruta de la foto 1...",
        value: "",
    }).dxTextBox("instance");

    var txtFoto2 = $("#txtFoto2").dxTextBox({
        placeholder: "Ingrese la ruta de la foto 2...",
        value: "",
    }).dxTextBox("instance");

    var txtFoto3 = $("#txtFoto3").dxTextBox({
        placeholder: "Ingrese la ruta de la foto 3...",
        value: "",
    }).dxTextBox("instance");
  

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = IdRegistro;
            var idEtiqueta = 1;
            var kilometraje = txtKilometraje.option("value");
            var placa = txtPlaca.option("value");
            var fechaMon = dpMonitoreo.option("value");
            var rpm = txtRPM.option("value");
            var valor = txtValor.option("value");
            var empresa = txtEmpresa.option("value");
            var observaciones = txtObservaciones.option("value");
            var foto1 = txtFoto1.option("value");
            var foto2 = txtFoto2.option("value");
            var foto3 = txtFoto3.option("value");
            var coordenadaX = 0;
            var coordenadaY = 0;
            var usuario = ".";
            var estado = "1";

            var params = { id: id, idEtiqueta: idEtiqueta, placa: placa, kilometraje: kilometraje, fechaMonitoreo: fechaMon, coordenadaX: coordenadaX, coordenadaY: coordenadaY, foto1: foto1, foto2: foto2, foto3: foto3, empresa: empresa, observaciones: observaciones, usuario: usuario, estado: estado, rPMRalenti: rpm, valorMonitoreo: valor};
            var _Ruta = $('#SIM').data('url') + "EtiquetasAmbientales/api/RevisionApi/GuardarMonitoreo";
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
                        $('#GidListado').dxDataGrid({ dataSource: CategoriaDataSource });
                        $("#PopupNuevoMonitoreo").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var popup = $("#PopupNuevoMonitoreo").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Categoría"
    }).dxPopup("instance");


  
});

var MonitoreosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_PLACA","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'EtiquetasAmbientales/api/RevisionApi/GetMonitoreos', {
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
            //CodFuncionario: CodigoFuncionario
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});