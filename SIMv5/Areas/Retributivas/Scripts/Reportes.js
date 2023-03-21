var IdTercero = 4542;
var urrl = $("#SIM").data("url") + "Retributivas/api/ReportesApi";
var _option
const now = new Date();

    // ******************************************
    //   
    //  TAB 1 - Tributary
    //
    // ******************************************
$(document).ready(function () {

    var codigoTributary = $("#codigoTributary").dxTextBox({
        placeholder: "Ingrese aqui la cantidad del caudal del afluente en metros cúbicos.",
        value: ""
    }).dxTextBox("instance");

    var txtNameTributary = $("#txtNameTributary").dxTextBox({
        placeholder: "Ingrese aqui el nombre del afluente...",
        value: ""
    }).dxTextBox("instance");

    var numberCaudal = $("#numberCaudal").dxTextBox({
        placeholder: "Ingrese aqui la cantidad del caudal del afluente en metros cúbicos.",
        value: ""
    }).dxTextBox("instance");

    var numberArea = $("#numberArea").dxTextBox({
        placeholder: "Ingrese aqui el area del afluente en metros cuadrados.",
        value: ""
    }).dxTextBox("instance");

    var numberlongitud = $("#numberlongitud").dxTextBox({
        placeholder: "Ingrese aqui la longitud...",
        value: ""
    }).dxTextBox("instance");

    var cmbTipoCuenca = $("#cmbTipoCuenca").dxTextBox({
        placeholder: "seleccione aqui el tipo de cuenca.",
        value: ""
    }).dxTextBox("instance");

    var cmbMunicipio = $("#cmbMunicipio").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID_MUNI",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/loadCounty");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "ID_MUNI",
        searchEnabled: true
    }).dxSelectBox("instance");

    $("#btnTributary").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = tributaryId;
            var codigo = codigoTributary.option("value");
            var name = txtNameTributary.option("value");
            var Caudal = numberCaudal.option("value");
            var area = numberArea.option("value");
            var Longitud = numberlongitud.option("value");
            var tipo = cmbTipoCuenca.option("value");
            var municipio = cmbMunicipio.option("value");
            var params = {
                ID: id,
                CODIGO: codigo,
                NOMBRE: name,
                AREA: area,
                CAUDAL: Caudal,
                LONGITUD: Longitud,
                TSIMTASA_TIPO_CUENCAS_ID: tipo,
                ID_MUNICIPIO: municipio
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/InsertTributary";
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
                        $('#grdTributary').dxDataGrid({ dataSource: myTributary });
                        popupTributary.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddATributary = $("#btnAddATributary").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
            tributaryId = null;
            codigoTributary.option("value", null);
            txtNameTributary.option("value", null);
            numberCaudal.option("value", null);
            numberArea.option("value", null);
            numberlongitud.option("value", null);
            cmbTipoCuenca.option("value", null);
            cmbMunicipio.option("value", null);
            option = true;
            popupTributary.show();
        }
    });

    $("#grdTributary").dxDataGrid({
        dataSource: myTributary,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "ID", width: '3%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'CODIGO', width: '8%', caption: 'CODIGO' },
            { dataField: 'NOMBRE', width: '20%', caption: 'NOMBRE' },
            { dataField: 'CAUDAL', width: '10%', caption: 'CAUDAL "Q"', format: '#,##0.## l/s' },
            { dataField: 'AREA', width: '8%', caption: 'AREA', format: '#,##0.## m2'},
            { dataField: 'LONGITUD', width: '10%', caption: 'LONGITUD', format: '#,##0.## m2'},
            { dataField: 'TSIMTASA_TIPO_CUENCAS_ID', width: '12%', caption: 'TIPO CUENCA' },
            { dataField: 'ID_MUNICIPIO', width: '10%', caption: 'MUNICIPIO' },
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/loadTributary";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        tributaryId = parseInt(data.ID);
                                        codigoTributary.option("value", data.CODIGO);
                                        txtNameTributary.option("value", data.NOMBRE);
                                        numberCaudal.option("value", data.CAUDAL);
                                        numberArea.option("value", data.AREA);
                                        numberlongitud.option("value", data.LONGITUD);
                                        cmbTipoCuenca.option("value", data.TSIMTASA_TIPO_CUENCAS_ID);
                                        cmbMunicipio.option("value", data.ID_MUNICIPIO);
                                        option = false;
                                        popupTributary.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/EmpresasApi/RemoveTributary";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdProducts').dxDataGrid({ dataSource: myProducts });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                }

            }

        ]
    });

    var popupTributary = $("#popupTributary").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "administrador de afluentes"
    }).dxPopup("instance");

    // ******************************************
    //   
    //  TAB 2 - Shedding
    //
    // ******************************************



    var cmbBasin = $("#cmbBasin").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/AllTributary";
                    return $.getJSON(_Ruta);
                },


            })
        }),
        pickerType: 'rollers',
        displayExpr: "CuencaMunicipio",
        valueExpr: "ID",
        searchEnabled: true
    }).dxSelectBox("instance");


    var cmbDownloadTypeShedding = $("#cmbDownloadTypeShedding").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Tipo_Descarga_Id",
                loadMode: "raw",
                load: function () {
                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/GetDownloadType";
                    return $.getJSON(_Ruta);
                },
            })
        }),
        pickerType: 'rollers',
        displayExpr: "Tipo_Descarga",
        valueExpr: "Tipo_Descarga_Id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmdWasteWater = $("#cmdWasteWater").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Tipo_Agua_residual_Id",
                loadMode: "raw",
                load: function () {
                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/GetWasteWater";
                    return $.getJSON(_Ruta);
                },
            })
        }),
        pickerType: 'rollers',
        displayExpr: "Tipo_Agua_Residual",
        valueExpr: "Tipo_Agua_residual_Id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var yearValidity = $("#yearValidity").dxNumberBox({
        placeholder: "Ingrese aqui los años de vigencia",
        format: '#### AÑOS',
        value: "",
        showSpinButtons: true,
        min: 0,
        max: 10,
    }).dxNumberBox("instance");


    var numberResolution = $("#numberResolution").dxNumberBox({
        placeholder: "Ingrese el numero de resolucion que auotorza su uso",
        showSpinButtons: true,
        min: 0,
        value: ""

    }).dxNumberBox("instance");

    var apodo = $("#apodo").dxTextBox({
        placeholder: "Ingrese un nombre para identificar este punto de vetimiento",
        showSpinButtons: true,
        value: ""
    }).dxTextBox("instance");

    var dateResolution = $("#dateResolution").dxDateBox({
        pickerType: 'rollers',
        value: now
    }).dxDateBox("instance");

    var yearValidity = $("#yearValidity").dxNumberBox({
        placeholder: "Ingrese aqui los años de vigencia",
        format: '#### AÑOS',
        value: "",
        showSpinButtons: true,
        min: 0,
        max: 10,
    }).dxNumberBox("instance");

    var numberLatitude = $("#numberLatitude").dxNumberBox({
        placeholder: "Ingrese aqui la coordenada de latitud para el vertimiento",
        showSpinButtons: true,
        format: ' #.####',
        min: 5.0001,
        max: 6.9999,
        value: ""
    }).dxNumberBox("instance");

    var numberLongitude = $("#numberLongitude").dxNumberBox({
        placeholder: "Ingrese aqui la coordenada de Longitud para el vertimiento",
        showSpinButtons: true,
        format: '##.####',
        max: -75.0001,
        min: -76.9999,
        value: ""
    }).dxNumberBox("instance");

    var numberFlow = $("#numberFlow").dxNumberBox({
        placeholder: "Ingrese el caudal autorizado",
        showSpinButtons: true,
        format: '#### l/s',
        min: 0,
        value: ""
    }).dxNumberBox("instance");

    var numberInstallation = $("#numberInstallation").dxNumberBox({
        placeholder: "Ingrese ingrese el numero de la instalación",
        showSpinButtons: true,
        min: 0,
        value: ""
    }).dxNumberBox("instance");

    $("#btnShedding").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var _id = SheddingId;
            var Basin = cmbBasin.option("value");
            var DownloadType = cmbDownloadTypeShedding.option("value");
            var WasteWater = cmdWasteWater.option("value");
            var Resolution = numberResolution.option("value");
            var nickShedding = apodo.option("value");
            var DateResolution = dateResolution.option("value");
            var YearResolution = yearValidity.option("value");
            var latitude = numberLatitude.option("value");
            var Longitude = numberLongitude.option("value");
            var Flow = numberFlow.option("value");
            var params = {
                ID: _id,
                TIPO_DESCARGA: DownloadType,
                TIPO_AGUA_RESIDUAL: WasteWater,
                NO_RESOLUCION : Resolution,
                FECHA_RESOLUCION : DateResolution,
                ANOS_VIGENCIA : YearResolution,
                LONGITUD : Longitude,
                LATITUD : latitude,
                CAUDAL_AUTORIZADO : Flow,
                TSIMTASA_CUENCAS_ID1: Basin,
                NICK : nickShedding,
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/InsertShedding";
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
                        $('#grdShedding').dxDataGrid({ dataSource: myShedding });
                        popupShedding.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddAShedding = $("#btnAddAShedding").dxButton({
        icon: 'plus',
        text: 'Ingresar Puntos de Vertmientos',
        hint: 'Registre sus puntos de vertimiento',
        onClick: function (e) {
            SheddingId = null;
            cmbBasin.option("value", null);
            cmbDownloadTypeShedding.option("value", null);
            cmdWasteWater.option("value", null);
            numberResolution.option("value", null);
            dateResolution.option("value", null);
            yearValidity.option("value", null);
            numberLatitude.option("value", null);
            numberLongitude.option("value", null);
            numberFlow.option("value", null);
            option = true;
            popupShedding.show();
        }
    });

    $("#grdShedding").dxDataGrid({
        dataSource: myShedding,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "Id_Cuenca_Tercero", width: '3%', caption: 'ID', dataType: 'Number', visible: false, alignment: 'center' },
            { dataField: "Id_Tercero", dataType: 'Number', visible: false },
            { dataField: 'Cuenca_Id', width: '5%', caption: 'Código', visible: false, alignment: 'center'  },
            { dataField: 'Nick', width: '20%', caption: 'Identificación del Vertimiento', alignment: 'center' },
            { dataField: 'Cuenca', width: '20%', caption: 'Fuente receptora', alignment: 'center' },
            { dataField: 'Municipio', width: '10%', caption: 'Municipio', alignment: 'center' },
            { dataField: 'Longitud', width: '10%', caption: 'Longitud', dataType: 'Number', visible: true, alignment: 'center' },
            { dataField: 'Latitud', width: '10%', caption: 'Latitud', alignment: 'center' },
            { dataField: 'Caudal', width: '10%', caption: 'Caudal (Q) L/S', alignment: 'center' },
            { dataField: 'Tipo_Descarga', width: '10%', caption: 'Tipo de Descarga', alignment: 'center' },
            { dataField: 'Tipo_Agua_Residual', width: '10%', caption: 'Tipo de Agua Residual', alignment: 'center' },
            { dataField: 'No_Resolucion', width: '8%', caption: 'No. Resolución', dataType: 'Number', visible: true, alignment: 'center' },
            { dataField: 'Fecha_Resolucion', width: '8%', caption: 'Fecha Resolución', dataType: 'date', alignment: 'center' },
            { dataField: 'Agnos_Vigencia', width: '8%', caption: 'Vigencia (Años)', alignment: 'center' },
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Registro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/loadShedding";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.Id_Cuenca_Tercero
                                }).done(function (data) {
                                    if (data !== null) {
                                        SheddingId = parseInt(data.Id_Cuenca_Tercero);
                                        cmbBasin.option("value", data.Cuenca_Id);
                                        cmbDownloadTypeShedding.option("value", data.Tipo_Descarga_Id);
                                        cmdWasteWater.option("value", data.Tipo_Agua_Residual_Id);
                                        numberResolution.option("value", data.No_Resolucion);
                                        yearValidity.option("value", data.Agnos_Vigencia);
                                        dateResolution.option("value", data.Fecha_Resolucion);
                                        numberLatitude.option("value", data.Latitud);
                                        numberLongitude.option("value", data.Longitud);
                                        numberFlow.option("value", data.Caudal);
                                        //numberInstallation.option("value", data.Id_Instalacion);
                                        option = false;
                                        popupShedding.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/RemoveShedding";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Cuenca_Tercero
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar Punto de Vertimiento');
                                            else {
                                                $('#grdShedding').dxDataGrid({ dataSource: myShedding });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                }

            }

        ]
    });

    var popupShedding = $("#popupShedding").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Registro Puntos de Vertimiento"
    }).dxPopup("instance");


    // ******************************************
    //   
    //  TAB 3 - Biochemical Oxygen Demand - BOD
    //
    // ******************************************

    var intYearBOD = $("#intYearBOD").dxTextBox({
        placeholder: "Ingrese aqui el año",
        value: ""
    }).dxTextBox("instance");

    var intRateBOD = $("#intRateBOD").dxTextBox({
        placeholder: "Ingrese aqui tarifa de SST",
        value: ""
    }).dxTextBox("instance");

    var stretchRiverBOD = $("#stretchRiverBOD").dxTextBox({
        placeholder: "Seleccione el Tramo del Rio",
        value: ""
    }).dxTextBox("instance");

    $("#btnBOD").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = BODId;
            var anoBOD = intYearBOD.option("value");
            var rateBOD = intRateBOD.option("value");
            var StretchBOD = stretchRiverBOD.option("value");
            var params = {
                ID: id,
                ANO: anoBOD,
                TARIFA: rateBOD,
                TSIMTASAS_FACTOR_AMBIENTAL_ID: StretchBOD
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/InsertBOD";
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
                        $('#grdBOD').dxDataGrid({ dataSource: myBOD });
                        popupBOD.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddABOD = $("#btnAddABOD").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
            BODId = null;
            intYearBOD.option("value", null);
            intRateBOD.option("value", null);
            stretchRiverBOD.option("value", null);
            option = true;
            popupBOD.show();
        }
    });

    $("#grdBOD").dxDataGrid({
        dataSource: myBOD,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "ID", width: '10%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'ANO', width: '20%', caption: 'AÑO' },
            { dataField: 'TARIFA', width: '50%', caption: 'TAFIFA' },
            { dataField: 'TSIMTASAS_FACTOR_AMBIENTAL_ID', width: '10%', caption: 'FACTOR AMBIENTAL' },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/loadBOD";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        TSBOD = parseInt(data.ID);
                                        intYearBOD.option("value", data.ANO);
                                        intRateBOD.option("value", data.TARIFA);
                                        stretchRiverBOD.option("value", data.TSIMTASAS_FACTOR_AMBIENTAL_ID);
                                        option = false;
                                        popupBOD.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/RemoveBOD";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdBOD').dxDataGrid({ dataSource: myBOD });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                }

            }

        ]
    });

    var popupBOD = $("#popupBOD").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tarifa Solidos Suspendidos Totales"
    }).dxPopup("instance");



    // ******************************************
    //   
    //  TAB 4 - Total suspended solids - TSS
    //
    // ******************************************

    var intYearTSS = $("#intYearTSS").dxTextBox({
        placeholder: "Ingrese aqui el año",
        value: ""
    }).dxTextBox("instance");

    var intRateTSS = $("#intRateTSS").dxTextBox({
        placeholder: "Ingrese aqui tarifa de SST",
        value: ""
    }).dxTextBox("instance");

    var stretchRiverTSS = $("#stretchRiverTSS").dxTextBox({
        placeholder: "Seleccione el Tramo del Rio",
        value: ""
    }).dxTextBox("instance");

    $("#btnTSS").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var id = TSSId;
            var anoTSS = intYearTSS.option("value");
            var rateTSS = intRateTSS.option("value");
            var StretchTSS = stretchRiverTSS.option("value");
            var params = {
                ID: id,
                ANO: anoTSS,
                TARIFA: rateTSS,
                TSIMTASAS_FACTOR_AMBIENTAL_ID: StretchTSS
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/InsertTSS";
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
                        $('#grdTSS').dxDataGrid({ dataSource: myTSS });
                        popupTSS.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddATSS = $("#btnAddATSS").dxButton({
        icon: 'plus',
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
            TSSId = null;
            intYearTSS.option("value", null);
            intRateTSS.option("value", null);
            stretchRiverTSS.option("value", null);
            option = true;
            popupTSS.show();
        }
    });

    $("#grdTSS").dxDataGrid({
        dataSource: myTSS,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 10
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

        columns: [
            { dataField: "ID", width: '10%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'ANO', width: '20%', caption: 'AÑO' },
            { dataField: 'TARIFA', width: '50%', caption: 'TAFIFA' },
            { dataField: 'TSIMTASAS_FACTOR_AMBIENTAL_ID', width: '10%', caption: 'FACTOR AMBIENTAL' },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/loadTSS";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        TSSId = parseInt(data.ID);
                                        intYearTSS.option("value", data.ANO);
                                        intRateTSS.option("value", data.TARIFA);
                                        stretchRiverTSS.option("value", data.TSIMTASAS_FACTOR_AMBIENTAL_ID);
                                        option = false;
                                        popupTSS.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/RemoveTSS";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdTSS').dxDataGrid({ dataSource: myTSS });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                }

            }

        ]
    });

    var popupTSS = $("#popupTSS").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tarifa Solidos Suspendidos Totales"
    }).dxPopup("instance");


    // ************************************************
    //   
    //  Reports not connected to the public sewer. - RNCPS
    //
    // ***********************************************

    //var cmbTributaryname = $("#cmbTributaryname").dxSelectBox({
    //    dataSource: myTributaryFactory,
    //    displayExpr: "NOMBRE",
    //    valueExpr: "ID",
    //    searchEnabled: true
    //}).dxSelectBox("instance");



    var cmbTributaryname = $("#cmbTributaryname").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Vertimiento_Id",
                loadMode: "raw",
                load: function () {
                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/LoadTributaryFactory";
                    return $.getJSON(_Ruta, { IdTercero }).done(function (data) {
                        apodoReport.option("value", data.Nick);
                    })

                }

                
            })
        }),
        displayExpr: "Vertimiento",
        valueExpr: "Vertimiento_Id",
        searchEnabled: true,

        onValueChanged(data) {
                apodoReport.option("value", data.Nick);
            }

    }).dxSelectBox("instance");

    var IdTributaryFactory;

    var apodoReport = $("#apodoReport").dxTextBox({
        value: "Nick",
        readOnly: true,
        hoverStateEnabled: false,
    }).dxTextBox("instance");

    var monthReport = $("#monthReport").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Mes_Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/MonthsReports");
                }
            })
        }),
        displayExpr: "Mes",
        valueExpr: "Mes_Id",
        searchEnabled: true
    }).dxSelectBox("instance");
        
    var yearReport = $("#yearReport").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Agno",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/AgnosReports");
                }
            })
        }),
        displayExpr: "Agno",
        valueExpr: "Agno",
        searchEnabled: true
    }).dxSelectBox("instance");


    //var cmbDownloadType = $("#cmbDownloadType").dxSelectBox({
    //    dataSource: new DevExpress.data.DataSource({
    //        store: new DevExpress.data.CustomStore({
    //            key: "Tipo_Descarga_Id",
    //            loadMode: "raw",
    //            load: function () {
    //                return $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/GetDownloadType");
    //            }
    //        })
    //    }),
    //    displayExpr: "Tipo_Descarga",
    //    valueExpr: "Tipo_Descarga_Id",
    //    searchEnabled: true
    //}).dxSelectBox("instance");

    var flowAverage = $("#flowAverage").dxNumberBox({
        placeholder: "Regsitre el flujo promedio del vertimiento",
        showSpinButtons: true,
        min: 0,
        value: "",
        format: '#0.## Litros/Segundo',
    }).dxNumberBox("instance");

    var sheddingDays = $("#sheddingDays").dxNumberBox({
        placeholder: "Número de dias de vertimiento al mes",
        value: "",
        min: 0,
        max: 31,
        format: '#0.## Dias',
        showSpinButtons: true,
    }).dxNumberBox("instance");

    var sheddingHours = $("#sheddingHours").dxNumberBox({
        placeholder: "ingrese las horas de vertimiento",
        format: '# Horas',
        placeholder: "Número de horas de vertimiento diarias",
        value: "",
        showSpinButtons: true,
        min: 0,
        max: 24,
    }).dxNumberBox("instance");

    var dboReport = $("#dboReport").dxNumberBox({
        placeholder: "Ingrese el reporte de la Demanda Bioquimica de Oxigeno",
        value: "",
        showSpinButtons: true,
        min: 0,
        format: '###0.## Mg/litro'
    }).dxNumberBox("instance");

    var sstReport = $("#sstReport").dxNumberBox({
        placeholder: "Ingrese el reporte de los Solidos Suspendidos Totales",
        value: "",
        showSpinButtons: true,
        min: 0,
        format: '###0.## Mg/litro'
    }).dxNumberBox("instance");

    $("#btnReports").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var _id = ReportId;
            var ID_TERCERO = IdTercero;

                var month = monthReport.option("value");
                var tributaryName = cmbTributaryname.option("value");
                var year = yearReport.option("value");
                option = null;



            //var month = monthReport.option("value").Id;
            //var year = new Date(yearReport.option("value")).getFullYear();
            
            var average = flowAverage.option("value");
            var hours = sheddingHours.option("value");
            var days = sheddingDays.option("value");
            var dbo = dboReport.option("value");
            var sst = sstReport.option("value");
            //var tipo = cmbDownloadType.option("value");
            
            var params = {
                ID_REPORTE: _id,
                VERTIMIENTO_ID: tributaryName,
                AGNO: year,
                MES_ID: month,
                CAUDAL: average,
                HORAS_DESCARGAS_DIA: hours,
                DIAS_DESCARGAS_MES: days,
                SST: sst,
                DBO: dbo,
                ID_TERCERO,
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/InsertReport";
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
                        $('#grdReports').dxDataGrid({ dataSource: myReports });
                        popupReports.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });

    var btnAddReports = $("#btnAddReports").dxButton({
        icon: 'plus',
        text: "Ingresar Autoreporte",
        hint: 'Ingresar Autoreporte',
        onClick: function (e) {
            ReportId = null;
            cmbTributaryname.option("value", null);
            monthReport.option("value", null);
            yearReport.option("value", null);
            //cmbDownloadType.option("value", null);
            //sheddingAmount.option("value", null);
            flowAverage.option("value", null);
            sheddingHours.option("value", null);
            sheddingDays.option("value", null);
            dboReport.option("value", null);
            sstReport.option("value", null);
            //option = true;
            popupReports.show();
        }
    });

    const grdReports =  $("#grdReports").dxDataGrid({
        dataSource: myReports,
        repaintChangesOnly: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 6
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [6, 12, 24, 36, 48, 60]
        },
        allowColumnReordering: true,
        allowColumnResizing: true,
        onCellPrepared: function (e) {
            if (e.rowType === "data" && e.column.dataField === "Tipo_Reporte") {
                e.cellElement.css("color", e.data.Tipo_Reporte == 'Calculado' ? "green" :
                    e.data.Tipo_Reporte == 'Promedio' ? "orange" :
                        e.data.Tipo_Reporte == 'Presuntivo' ? "red" : "black");

            };

            if (e.rowType === "data" && e.column.dataField === "Estado_Reporte") {
                e.cellElement.css("color", e.data.Estado_Reporte == 'Completo' ? "green" :
                    e.data.Estado_Reporte == 'No Reportado' ? "orange" :
                        e.data.Estado_Reporte == 'Incompleto' ? "red" : "black");

            };

            if (e.rowType === "data" && e.column.dataField === "Tipo_Agua_Residual") {
                e.cellElement.css("color", e.data.Tipo_Agua_Residual == 'Doméstica' ? "green" :
                    e.data.Estado_Reporte == 'No Doméstica' ? "red" : "black");
            };

            if (e.rowType === "data" && e.column.dataField === "Tipo_Descarga") {
                e.cellElement.css("color", e.data.Tipo_Descarga == 'Periódica Regular' ? "green" :
                    e.data.Tipo_Reporte == 'Periódica Irregular' ? "orange" :
                        e.data.Tipo_Reporte == 'Continua' ? "red" : "black");

            };
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
        wordWrapEnabled: true,

        columns: [
            { dataField: "Id_Reporte", dataType: 'Number', visible: false },
            { dataField: 'Nick', width: '10%', caption: 'Identificación del Vertimiento', alignment: 'center' },
            { dataField: 'Vertimiento', width: '10%', caption: 'Corriente de Agua', alignment: 'center' },
            { dataField: 'Agno', width: '4%', caption: 'Año', alignment: 'center' },
            { dataField: 'Mes', width: '4%', caption: 'Mes', visible: true, alignment: 'center' },
            //{ dataField: 'Tipo_Descarga', width: '8%', caption: 'Tipo de Descarga', alignment: 'center' },
            { dataField: 'Tipo_Agua_Residual', width: '8%', caption: 'Agua Residual', alignment: 'center' },
            { dataField: 'Caudal', width: '8%', caption: 'Caudal (Q) L/S', alignment: 'center' },
            { dataField: 'Horas_Descargas_Dia', width: '8%', caption: 'No. Horas/Día', dataType: 'horas', visible: true, alignment: 'center' },
            { dataField: 'Dias_Descargas_Mes', width: '8%', caption: 'No. Dias/Mes', alignment: 'center' },
            { dataField: 'dbo', width: '8%', caption: 'DBO mgO2/litro', alignment: 'center' },
            { dataField: 'sst', width: '8%', caption: 'SST mg/litro', alignment: 'center' },
            { dataField: 'Estado_Reporte', width: '8%', caption: 'Estado', alignment: 'center', cssClass: 'resaltado', },
            { dataField: 'Radicado', width: '5%', caption: 'No. Radicado' },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Registro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/loadReport";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.Id_Reporte
                                }).done(function (data) {
                                    if (data !== null) {
                                        ReportId = parseInt(data.Id_Reporte);
                                        cmbTributaryname.option("value", data.Vertimiento_Id);
                                        monthReport.option("value", data.Mes_Id);
                                        yearReport.option("value", data.Agno);
                                        cmbDownloadType.option("value", data.Tipo_Descarga_Id);
                                        //sheddingAmount.option("value", data.No_Descargas_Dia);
                                        flowAverage.option("value", data.Caudal);
                                        sheddingHours.option("value", data.Horas_Descargas_Dia);
                                        sheddingDays.option("value", data.Dias_Descargas_Mes);
                                        dboReport.option("value", data.dbo);
                                        sstReport.option("value", data.sst);
                                        //option = false;
                                        popupReports.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/ReportesApi/RemoveReport";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Reporte
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdReports').dxDataGrid({ dataSource: myReports });
                                            //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                }

            }

        ],
       
    });

    var popupReports = $("#popupReports").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Reportes SNCAP"
    }).dxPopup("instance");


});

//   TAB 1 - Tributary
var myTributary = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/Tributary", {
            filter: loadOptions.filter ? JSON.stringify(filterOptions) : '',
            sort: sortOptions,
            group: JSON.stringify(groupOptions),
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },

});

//   TAB 2 - Shedding
var myShedding = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var Id_Tercero = IdTercero;

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/Shedding", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },

});

//  TAB 3 - Biochemical Oxygen Demand - BOD
var myBOD = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/BOD", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },

});

//  TAB 4 - Total suspended solids - TSS
var myTSS = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/TSS", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },

});

//  TAB 5 - Reports not connected to the public sewer. - RNCPS
var myReports = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var Id_Tercero = IdTercero;

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/Reports", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },
});


var myTributaryFactory = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var Id_Tercero = IdTercero;
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/TributaryFactory", {
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
            Id_Tercero,
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },
});


var myMonths = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/Mounths", {
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
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },
});



var myAgnosReports = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        $.getJSON($("#SIM").data("url") + "Retributivas/api/ReportesApi/AgnosReports", {
        }).done(function (data) {
            return d.promise();
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
    },
});