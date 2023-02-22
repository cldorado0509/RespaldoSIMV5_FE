var IdTercero = 310993;
var urrl = '/api/AdministradorApi';

var _option

    // ******************************************
    //   
    //  TAB 1 - Tributary - 
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
        value: "",
        format: '#,###.## Litros/Segundo'
    }).dxTextBox("instance");

    var numberArea = $("#numberArea").dxTextBox({
        placeholder: "Ingrese aqui el area del afluente en Kilometros cuadrados.",
        value: "",
        format: '#,###.## Km2'

    }).dxTextBox("instance");

    var numberlongitud = $("#numberlongitud").dxTextBox({
        placeholder: "Ingrese aqui la longitud en Kilometros",
        value: "",
        format: '#,###.## Km'
    }).dxTextBox("instance");

    var cmbMunicipio = $("#cmbMunicipio").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID_MUNI",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/loadCounty");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "ID_MUNI",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmbTramo = $("#cmbTramo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/getTramos");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "ID",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmbTipoCuenca = $("#cmbTipoCuenca").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/getBasinType");
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "ID",
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
            var tramo = cmbTramo.option("value");
            var params = {
                ID: id,
                CODIGO: codigo,
                NOMBRE: name,
                AREA: area,
                CAUDAL: Caudal,
                LONGITUD: Longitud,
                TSIMTASA_TIPO_CUENCAS_ID: tipo,
                ID_MUNICIPIO: municipio,
                TSIMTASA_TRAMOS_ID: tramo,
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" + "/InsertTributary";
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
            cmbTramo.option("value", null);
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
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100, 200]
        },
        allowColumnReordering: true,
        allowColumnResizing: true,
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
            { dataField: 'CAUDAL', width: '10%', caption: 'CAUDAL "Q"', format: '#,###.## L/S'},
            { dataField: 'AREA', width: '8%', caption: 'AREA', format: '#,###.## Km2' },
            { dataField: 'LONGITUD', width: '10%', caption: 'LONGITUD', format: '#,###.## Km' },
            { dataField: 'TSIMTASA_TIPO_CUENCAS_ID', width: '12%', caption: 'TIPO', visible: false },
            { dataField: 'TIPO', width: '12%', caption: 'TIPO' },
            { dataField: 'TSIMTASA_TRAMOS_ID', width: '10%', caption: 'TRAMO_ID', visible: false },
            { dataField: 'TRAMO', width: '10%', caption: 'TRAMO' },
            { dataField: 'MUNICIPIO', width: '10%', caption: 'MUNICIPIO' },
            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" + "/loadTributary";
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
                                        cmbTramo.option("value", data.TSIMTASA_TRAMOS_ID);
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
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" +"/RemoveTributary";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
                                            else {
                                                $('#grdTributary').dxDataGrid({ dataSource: myTributary });
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


    var cmbBasin = $("#cmbBasin").dxTextBox({
        placeholder: "Seleccione el Afluente",
        value: ""
    }).dxTextBox("instance");

    var cmbUseType = $("#cmbUseType").dxTextBox({
        placeholder: "Seleccione el tipo de USO.",
        value: ""
    }).dxTextBox("instance");

    var numberResolution = $("#numberResolution").dxTextBox({
        placeholder: "Ingrese el numero de resolucion que auotorza su uso",
        value: ""
    }).dxTextBox("instance");

    var dateResolution = $("#dateResolution").dxTextBox({
        placeholder: "Ingrese aqui la fecha de la resolucion",
        value: ""
    }).dxTextBox("instance");

    var yearValidity = $("#yearValidity").dxTextBox({
        placeholder: "Ingrese aqui los años de vigencia",
        value: ""
    }).dxTextBox("instance");

    var numberLatitude = $("#numberLatitude").dxTextBox({
        placeholder: "Ingrese aqui la coordenada de latitud para el vertimiento",
        value: ""
    }).dxTextBox("instance");

    var numberLongitude = $("#numberLongitude").dxTextBox({
        placeholder: "Ingrese aqui la coordenada de Longitud para el vertimiento",
        value: ""
    }).dxTextBox("instance");

    var numberFlow = $("#numberFlow").dxTextBox({
        placeholder: "Ingrese el caudal autorizado",
        value: ""
    }).dxTextBox("instance");

    var numberInstallation = $("#numberInstallation").dxTextBox({
        placeholder: "Ingrese ingrese el numero de la instalación",
        value: ""
    }).dxTextBox("instance");

    $("#btnShedding").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var _id = SheddingId;
            var _idTercero = IdTercero;
            var Basin = cmbBasin.option("value");
            var UseType = cmbUseType.option("value");
            var Resolution = numberResolution.option("value");
            var DateResolution = dateResolution.option("value");
            var YearResolution = yearValidity.option("value");
            var latitude = numberLatitude.option("value");
            var Longitude = numberLongitude.option("value");
            var Flow = numberFlow.option("value");
            var Installation = numberInstallation.option("value");
            var params = {
                ID: _id,
                TIPO_USO : UseType,
                NO_RESOLUCION : Resolution,
                FECHA_RESOLUCION : DateResolution,
                AÑOS_VIGENCIA : YearResolution,
                LONGITUD : Longitude,
                LATITUD : latitude,
                CAUDAL_AUTORIZADO : Flow,
                ID_INSTALACION : Installation,
                TSIMTASA_CUENCAS_ID1 : Basin,
                ID_TERCERO : _idTercero

            };
            var _Ruta = urrl + "/InsertShedding";
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
        hint: 'ingresar Regsitro de Producción',
        onClick: function (e) {
            SheddingId = null;
            idTercero = IdTercero;
            cmbBasin.option("value", null);
            cmbUseType.option("value", null);
            numberResolution.option("value", null);
            dateResolution.option("value", null);
            yearValidity.option("value", null);
            numberLatitude.option("value", null);
            numberLongitude.option("value", null);
            numberFlow.option("value", null);
            numberInstallation.option("value", null);
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
            { dataField: "ID", width: '3%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'TSIMTASA_CUENCAS_ID1', width: '10%', caption: 'Afluente' },
            { dataField: 'TIPO_USO', width: '10%', caption: 'Tipo de Uso' },
            { dataField: 'NO_RESOLUCION', width: '7%', caption: 'No. Resolución', dataType: 'Number', visible: true },
            { dataField: 'FECHA_RESOLUCION', width: '10%', caption: 'Fecha Resolucion' },
            { dataField: 'AÑOS_VIGENCIA', width: '7%', caption: 'Vigencia' },
            { dataField: 'LONGITUD', width: '8%', caption: 'Longitud', dataType: 'Number', visible: true },
            { dataField: 'LATITUD', width: '8%', caption: 'latitud' },
            { dataField: 'CAUDAL_AUTORIZADO', width: '8%', caption: 'Caudal' },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Regsitro de Producción',
                        onClick: function (e) {
                            var _Ruta = urrl + "/loadShedding";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.ID
                                }).done(function (data) {
                                    if (data !== null) {
                                        SheddingId = parseInt(data.ID);
                                        cmbBasin.option("value", data.Nombre);
                                        cmbUseType.option("value", data.Nombre);
                                        numberResolution.option("value", data.Nombre);
                                        yearValidity.option("value", data.Nombre);
                                        dateResolution.option("value", data.Nombre);
                                        numberLatitude.option("value", data.Nombre);
                                        numberLongitude.option("value", data.Nombre);
                                        numberFlow.option("value", data.Nombre);
                                        numberInstallation.option("value", data.Descripcion);
                                        option = false;
                                        popupProducts.show();
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
                                    var _Ruta = urrl + "/RemoveShedding";
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

    var popupShedding = $("#popupShedding").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Regsitro de Puntos de Vertimiento"
    }).dxPopup("instance");



    // ******************************************
    //   
    //  TAB 10 - Parameter Environment
    //
    // ******************************************

    var intYearParameter = $("#intYearParameter").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Agno",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/AgnosReports");
                }
            })
        }),
        displayExpr: "Agno",
        valueExpr: "Agno",
        searchEnabled: true
    }).dxSelectBox("instance");


    var intRateParameter = $("#intRateParameter").dxTextBox({
        placeholder: "Ingrese aqui tarifa ",
        value: ""
    }).dxTextBox("instance");

    var enviromentParameter = $("#enviromentParameter").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "parameterMI",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/GetParametro");
                }
            })
        }),
        displayExpr: "Name_Parametro",
        valueExpr: "Id_Parametro",
        searchEnabled: true,
        placeholder: "Seleccione El Pareametro Ambiental",
    }).dxSelectBox("instance");

    $("#btnParameter").dxButton({
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
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" + "/InsertBOD";
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

    var btnAddAParameter = $("#btnAddAParameter").dxButton({
        icon: 'plus',
        hint: 'ingresar nueva tarifa',
        onClick: function (e) {
            BODId = null;
            intYearParameter.option("value", null);
            intRateParameter.option("value", null);
            enviromentParameter.option("value", null);
            option = true;
            popupParameter.show();
        }
    });

    $("#grdParameter").dxDataGrid({
        dataSource: myParameter,
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
            { dataField: "ID", width: '5%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'ANO', width: '5%', caption: 'AÑO' },
            { dataField: 'TARIFA', width: '15%', caption: 'TAFIFA', format: '$ #,###.#0', },
            { dataField: 'NOMBRE', width: '50%', caption: 'FACTOR AMBIENTAL'  },
            { dataField: 'ABREVIATURA', width: '10%', caption: 'ABREVIATURA'  }

            //{
            //    alignment: 'center', caption: 'Funciones',
            //    cellTemplate: function (container, options) {
            //        $('<div/>').dxButton({
            //            icon: 'edit',
            //            hint: 'Editar Regsitro de Producción',
            //            onClick: function (e) {
            //                var _Ruta = urrl + "/loadBOD";
            //                $.getJSON(_Ruta,
            //                    {
            //                        Id: options.data.ID
            //                    }).done(function (data) {
            //                        if (data !== null) {
            //                            TSBOD = parseInt(data.ID);
            //                            intYearBOD.option("value", data.ANO);
            //                            intRateBOD.option("value", data.TARIFA);
            //                            stretchRiverBOD.option("value", data.TSIMTASAS_FACTOR_AMBIENTAL_ID);
            //                            option = false;
            //                            popupBOD.show();
            //                        }
            //                    }).fail(function (jqxhr, textStatus, error) {
            //                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

            //                    });
            //            }
            //        }).appendTo(container);

            //        $('<div/>').dxButton({
            //            icon: 'remove',
            //            onClick: function (e) {
            //                var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
            //                result.done(function (dialogResult) {
            //                    if (dialogResult) {
            //                        var _Ruta = urrl + "/RemoveBOD";
            //                        $.getJSON(_Ruta,
            //                            {
            //                                Id: options.data.ID
            //                            }).done(function (data) {
            //                                if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar la Produccion');
            //                                else {
            //                                    $('#grdBOD').dxDataGrid({ dataSource: myBOD });
            //                                }
            //                            }).fail(function (jqxhr, textStatus, error) {
            //                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar la Empresa');
            //                            });
            //                    }
            //                });
            //            }
            //        }).appendTo(container);

            //    }

        /*    }*/

        ]
    });

    var popupParameter = $("#popupParameter").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Registrar Nueva Tarifa"
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
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" + "/InsertBOD";
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
                            var _Ruta = urrl + "/loadBOD";
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
                                    var _Ruta = urrl + "/RemoveBOD";
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
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" + "/InsertTSS";
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
                            var _Ruta = urrl + "/loadTSS";
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
                                    var _Ruta = urrl + "/RemoveTSS";
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


    // ******************************************
    //   
    //  TAB 5 - Reports not connected to the public sewer. - RNCPS
    //
    // ******************************************

    var cmbTributaryname = $("#cmbTributaryname").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Vertimiento_Id",
                loadMode: "raw",
                load: function () {
                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/LoadTributaryFactory";
                    return $.getJSON(_Ruta, { IdTercero });
                },
            })
        }),
        displayExpr: "Vertimiento",
        valueExpr: "Vertimiento_Id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmbTypeSettlement = $("#cmbTypeSettlement").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Tipo_Reporte_Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/TypeSettlement");
                }
            })
        }),
        displayExpr: "Tipo_Reporte",
        valueExpr: "Tipo_Reporte_Id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var cmbReportStatus = $("#cmbReportStatus").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Estado_Reporte_Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/ReportStatus");
                }
            })
        }),
        displayExpr: "Estado_Reporte",
        valueExpr: "Estado_Reporte_Id",
        searchEnabled: true
    }).dxSelectBox("instance");

    var monthReport = $("#monthReport").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Mes_Id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/MonthsReports");
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
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/AgnosReports");
                }
            })
        }),
        displayExpr: "Agno",
        valueExpr: "Agno",
        searchEnabled: true
    }).dxSelectBox("instance");

    var Name_Tercero = $("#Name_Tercero").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id_Tercero",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/GetTerceros");
                }
            })
        }),
        displayExpr: "Name_Tercero",
        valueExpr: "Id_Tercero",
        searchEnabled: true,
        disabled: false,
        placeholder: "Seleccione la Empresa",
        onValueChanged: function (e) {
            IdTercero = e.value,
            $('#cmbTributaryname').dxSelectBox({
                dataSource: new DevExpress.data.DataSource({
                    store: new DevExpress.data.CustomStore({
                        key: "Vertimiento_Id",
                        loadMode: "raw",
                        load: function () {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/LoadTributaryFactory";
                            return $.getJSON(_Ruta, { IdTercero });
                        },
                    })
                })
            });
        }
    }).dxSelectBox("instance");

    var sheddingAmount = $("#sheddingAmount").dxNumberBox({
        placeholder: "Ingrese la candidad de vertimiientos",
        showSpinButtons: true,
        min: 0,
        value: ""
    }).dxNumberBox("instance");

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
        format: '###0.## mgO2/litro'
    }).dxNumberBox("instance");

    var sstReport = $("#sstReport").dxNumberBox({
        placeholder: "Ingrese el reporte de los Solidos Suspendidos Totales",
        value: "",
        showSpinButtons: true,
        min: 0,
        format: '###0.## mg/litro'
    }).dxNumberBox("instance");

    $("#btnReports").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            DevExpress.ui.notify("The Contained button was clicked");
            var _id = ReportId;


            if (option) {
                var month = monthReport.option("value");
                var tributaryName = cmbTributaryname.option("value");
                var year = yearReport.option("value");
                var tipoReport = cmbTypeSettlement.option("value");
                var reportStatus = cmbReportStatus.option("value");
                var IdTercero = Name_Tercero.option("value");

                option = null;
            } else {
                var month = monthReport.option("value");
                var tributaryName = cmbTributaryname.option("value");
                var year = yearReport.option("value");
                var tipoReport = cmbTypeSettlement.option("value");
                var reportStatus = cmbReportStatus.option("value");
                var IdTercero = Name_Tercero.option("value");

                option = null;
            }

            var ID_TERCERO = IdTercero;

            //var month = monthReport.option("value").Id;
            //var year = new Date(yearReport.option("value")).getFullYear();

            var amount = sheddingAmount.option("value");
            var average = flowAverage.option("value");
            var hours = sheddingHours.option("value");
            var days = sheddingDays.option("value");
            var dbo = dboReport.option("value");
            var sst = sstReport.option("value");

            var params = {
                ID_REPORTE: _id,
                VERTIMIENTO_ID: tributaryName,
                AGNO: year,
                MES_ID: month,
                CAUDAL: average,
                NO_DESCARGAS_DIA: amount,
                HORAS_DESCARGAS_DIA: hours,
                DIAS_DESCARGAS_MES: days,
                SST: sst,
                DBO: dbo,
                ID_TERCERO,
                ESTADO_REPORTE_ID: reportStatus,
                TIPO_REPORTE_ID: tipoReport,
            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/InsertReport";
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
            sheddingAmount.option("value", null);
            flowAverage.option("value", null);
            sheddingHours.option("value", null);
            sheddingDays.option("value", null);
            dboReport.option("value", null);
            sstReport.option("value", null);
            cmbTypeSettlement.option("value", 2);
            cmbReportStatus.option("value", 4);
            option = true;
            popupReports.show();
        }
    });

    const grdReports = $("#grdReports").dxDataGrid({
        dataSource: myReports,
        repaintChangesOnly: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100, 200]
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
        },
        export: {
            enabled: true,
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
        columnChooser: {
            enabled: true,
            mode: 'select',
        },

        hoverStateEnabled: true,
        wordWrapEnabled: true,

        columns: [
            { dataField: "Id_Reporte", dataType: 'Number', visible: false },
            { dataField: "Name_Tercero", caption: 'empresa', visible: true, width: '10%', },
            { dataField: 'Vertimiento', width: '8%', caption: 'Corriente de Agua', alignment: 'center' },
            { dataField: 'Agno', width: '4%', caption: 'Año', alignment: 'center' },
            { dataField: 'Mes', width: '5%', caption: 'Mes', visible: true, alignment: 'center' },
            { dataField: 'Dias_Descargas_Mes', width: '3%', caption: 'Dias', alignment: 'center' },
            { dataField: 'Horas_Descargas_Dia', width: '3%', caption: 'Horas', dataType: 'horas', visible: true, alignment: 'center' },
            { dataField: 'Caudal', width: '5%', caption: 'Caudal (Q) Litros/Segundo', alignment: 'center', format: '###0.##' },
            //{ dataField: 'Caudal', width: '5%', caption: 'Caudal (Q) Litros/Segundo', alignment: 'center', format: '###0.## l/s' },
            { dataField: 'Tipo_Reporte', width: '5%', caption: 'Tipo Liquidacion', alignment: 'center', cssClass: 'resaltado', },
            { dataField: 'Estado_Reporte', width: '5%', caption: 'Estado', alignment: 'center', cssClass: 'resaltado', },

            { dataField: 'dbo', width: '5%', caption: 'DBO mg/litro', alignment: 'center', format: '###0.##' },
            { dataField: 'dbokgm', width: '5%', caption: 'DBO Kg/Mes', alignment: 'center', format: '###0.##' },
            { dataField: 'billing_dbo', width: '7%', caption: 'Tasa DBO', visible: true, format: 'currency', },


            { dataField: 'sst', width: '5%', caption: 'SST mg/litro', alignment: 'center', format: '###0.##' },
            { dataField: 'sstkgm', width: '5%', caption: 'SST Kg/Mes', alignment: 'center', format: '###0.##' },
            { dataField: 'billing_sst', width: '7%', caption: 'Tasa SST', visible: true, format: 'currency', },

            //{ dataField: 'dbo', width: '5%', caption: 'DBO mgO2/litro', alignment: 'center', format: '###0.## mg/l' },
            //{ dataField: 'dbokgm', width: '5%', caption: 'DBO Kg/Mes', alignment: 'center', format: '###0.## Kg/Mes' },
            //{ dataField: 'sst', width: '5%', caption: 'SST mg/litro', alignment: 'center', format: '###0.## mg/l' },
            //{ dataField: 'sstkgm', width: '5%', caption: 'SST Kg/Mes', alignment: 'center', format: '###0.## Kg/Mes' },


            { dataField: 'Radicado', width: '5%', caption: 'No. Radicado', visible: false },

            { dataField: 'total_billing', width: '7%', caption: 'Liquidación', visible: true, format: 'currency',  },


            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'money',
                        type: 'success',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('desea generar la liquidación', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/billing";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Reporte
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, ' al generar la liquidación');
                                            else {
                                                $('#grdReports').dxDataGrid({ dataSource: myReports });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Generar liquidacion');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        //

                        icon: 'edit',
                        hint: 'Editar Registro de Producción',

                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/loadReport";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.Id_Reporte
                                }).done(function (data) {
                                    if (data !== null) {
                                        ReportId = parseInt(data.Id_Reporte);
                                        IdTercero = parseInt(data.Id_Tercero);
                                        Name_Tercero.option("value", data.Id_Tercero);
                                        cmbTributaryname.option("value", data.Vertimiento_Id);
                                        monthReport.option("value", data.Mes_Id);
                                        yearReport.option("value", data.Agno);
                                        cmbReportStatus.option("value", data.Estado_Reporte_Id);
                                        cmbTypeSettlement.option("value", data.Tipo_Reporte_Id);
                                        sheddingAmount.option("value", data.No_Descargas_Dia);
                                        flowAverage.option("value", data.Caudal);
                                        sheddingHours.option("value", data.Horas_Descargas_Dia);
                                        sheddingDays.option("value", data.Dias_Descargas_Mes);
                                        dboReport.option("value", data.dbo);
                                        sstReport.option("value", data.sst);

                                        option = false;
                                        popupReports.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');

                                });
                        }
                    }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: 'remove',
                        //disabled: claimppal.IsInRole('VADMINISTRADOR'),
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar éste registro?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/RemoveReport";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Reporte
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminando el reporte');
                                            else {
                                                $('#grdReports').dxDataGrid({ dataSource: myReports });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminando el reporte');
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

    // ******************************************
    //   
    //  TAB 6 - Quinqueños
    //
    // ******************************************

    var intYearStart = $("#intYearStart").dxTextBox({
        placeholder: "Ingrese aqui el año",
        value: ""
    }).dxTextBox("instance");

    var intYearEnd = $("#intYearEnd").dxTextBox({
        placeholder: "Ingrese aqui tarifa de SST",
        value: ""
    }).dxTextBox("instance");

    var noAcuerdo = $("#noAcuerdo").dxTextBox({
        placeholder: "Seleccione el Tramo del Rio",
        value: ""
    }).dxTextBox("instance");

    $("#btnSaveQuinqueno").dxButton({
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
            var _Ruta = urrl + "/InsertTSS";
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

    var btnAddQuinqueno = $("#btnAddQuinqueno").dxButton({
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

    $("#grdQuinqueno").dxDataGrid({
        dataSource: myQuinqueno,
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
            { dataField: 'ID_QUINQUENO', width: '5%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'DESCRIPCION', width: '10%', caption: 'Quinqueño' },
            { dataField: 'INICIO', width: '10%', caption: 'Inicio' },
            { dataField: 'TERMINA', width: '10%', caption: 'Termina' },
            { dataField: 'ACUERDO', width: '10%', caption: 'Acuerdo' },
        ]
    });

    var popupQuinqueno = $("#popupQuinqueno").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tarifa Solidos Suspendidos Totales"
    }).dxPopup("instance");






    // ******************************************
    //   
    //  TAB 7 - Periodo
    //
    // ******************************************

    var intPeriodStart = $("#intPeriodStart").dxTextBox({
        placeholder: "Ingrese aqui el año",
        value: ""
    }).dxTextBox("instance");

    var intPeriodEnd = $("#intPeriodEnd").dxTextBox({
        placeholder: "Ingrese aqui tarifa de SST",
        value: ""
    }).dxTextBox("instance");


    $("#btnSavePeriod").dxButton({
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
            var _Ruta = urrl + "/InsertTSS";
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

    var btnAddPeriod = $("#btnAddPeriod").dxButton({
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

    $("#grdPeriod").dxDataGrid({
        dataSource: myPeriod,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
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
            { dataField: 'ID_PERIODO', width: '20%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'NO_PERIODO', width: '20%', caption: 'Periodo' },
            { dataField: 'INICIA', width: '20%', caption: 'Inicio' },
            { dataField: 'TERMINA', width: '20%', caption: 'Termina' },
            { dataField: 'QUINQUENO', width: '20%', caption: 'Quinqueño' },

        ]
    });

    var popupPeriod = $("#popupPeriod").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tarifa Solidos Suspendidos Totales"
    }).dxPopup("instance");








    // ******************************************
    //   
    //  TAB 3 - METAS INDIVIDUALES
    //
    // ******************************************

    var IdMetaIndividual = $("#IdMetaIndividual").dxNumberBox({
        value: 0,
        //disabled: true,

    }).dxNumberBox("instance");

    var numTargetMI = $("#numTargetMI").dxNumberBox({
        placeholder: "Ingrese la Meta",
        showSpinButtons: true,
        format: '#,###.## Kg/Año',
        min: 0,
        value: ""
    }).dxNumberBox("instance");

    var intLoadObtainedMI = $("#intLoadObtainedMI").dxNumberBox({
        placeholder: "Ingrese la Carga Obtenida",
        showSpinButtons: true,
        format: '#,###.## Kg/Año',
        min: 0,
        value: ""
    }).dxNumberBox("instance");

    var meetsMI = $("#meetsMI").dxSelectBox({
        items: ["Sin Evaluar", "Cumple", "No Cumple" ],
    }).dxSelectBox("instance");

    var enterpriseMI = $("#enterpriseMI").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "Id_Tercero",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/GetTerceros");
                }
            })
        }),
        displayExpr: "Name_Tercero",
        valueExpr: "Id_Tercero",
        searchEnabled: true,
        disabled: false,
        placeholder: "Seleccione la Empresa",
    }).dxSelectBox("instance");

    var periodMI = $("#periodMI").dxSelectBox({
        //items: [0, 1, 2, 3, 4, 5],
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "parameterMI",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/getPeriodQuinqueño");
                }
            })
        }),
        displayExpr: "concatenado",
        valueExpr: "ID_PERIODO",
        searchEnabled: true,
        placeholder: "Seleccione El Periodo",
    }).dxSelectBox("instance");

    var parameterMI = $("#parameterMI").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "parameterMI",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/GetParametro");
                }
            })
        }),
        displayExpr: "Name_Parametro",
        valueExpr: "Id_Parametro",
        searchEnabled: true,
        placeholder: "Seleccione El Pareametro Ambiental",

    }).dxSelectBox("instance");

    //var regionalFactorMI = $("#regionalFactorMI").dxSelectBox({

    //    dataSource: new DevExpress.data.DataSource({
    //        store: new DevExpress.data.CustomStore({
    //            key: "ID_FACTOR_REGIONAL",
    //            loadMode: "raw",
    //            //Parametro_Ambiental_Id : parameterMI.option("value"),
    //            load: function () {
    //                return $.getJSON($("#SIM").data("url") + "Retributivas/api/AdministradorApi/getRegionalFactorSingle");
    //            }
    //        })
    //    }),
    //    displayExpr: "FACTOR",
    //    valueExpr: "ID_FACTOR_REGIONAL",
    //    searchEnabled: true,
    //    placeholder: "Seleccione Factor Regional",

    //}).dxSelectBox("instance");

    var regionalFactorMI = $("#regionalFactorMI").dxNumberBox({
        placeholder: "Ingrese el factor",
        showSpinButtons: true,
        format: '#,###.#0',
        min: 1,
        max: 5.5,
        value: 1
    }).dxNumberBox("instance");

    $("#btnSaveMetaIndividual").dxButton({
        stylingMode: "contained",
        text: "Guardar",
        type: "default",
        width: 120,
        onClick: function () {
            //DevExpress.ui.notify("The Contained button was clicked");
            var _id = MetaId;

            var _Id_Meta_individual = _id;
            var _Carga_Obtenida = intLoadObtainedMI.option("value");
            var _Cumple_Meta = meetsMI.option("value");
            var _Id_Tercero = enterpriseMI.option("value");
            var _Name_Tercero = enterpriseMI.option("value");
            var _Periodo_Id = periodMI.option("value");
            var _Meta = numTargetMI.option("value");

            var _Periodo = periodMI.option("value");
            var _Parametro_Ambiental_Id = parameterMI.option("value");
            var _Parametro_Ambiental = parameterMI.option("value");
            var _Factor_Regional_Id = regionalFactorMI.option("value");
            var _Factor_Regional = regionalFactorMI.option("value");
            var params = {
                ID_META_INDIVIDUAL: _Id_Meta_individual,
                META: _Meta,
                CARGA_OBTENIDA: _Carga_Obtenida,
                CUMPLE_META: _Cumple_Meta,
                ID_TERCERO: _Id_Tercero,
                NAME_TERCERO: _Name_Tercero,
                PERIODO_ID: _Periodo_Id,
                PERIODO: _Periodo,
                PARAMETRO_AMBIENTAL_ID: _Parametro_Ambiental_Id,
                PARAMETRO_AMBIENTAL: _Parametro_Ambiental,
                FACTOR_REGIONAL_ID: _Factor_Regional_Id,
                FACTOR_REGIONAL: _Factor_Regional,

            };
            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi" + "/InsertMI";
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
                        $('#grdMetaIndividual').dxDataGrid({ dataSource: myGoalIndividual });
                        popupMetaIndividual.hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    });
    

    var btnAddMetaIndividual = $("#btnAddMetaIndividual").dxButton({
        icon: 'plus',
        hint: 'Ingresar Meta individual',
        onClick: function (e) {
            MetaId = null;
            numTargetMI.option("value", null);
            meetsMI.option("value", null);
            enterpriseMI.option("value", null);
            periodMI.option("value", null);
            parameterMI.option("value", null);
            regionalFactorMI.option("value", null);
            popupMetaIndividual.show();
        }
    });

    $("#grdMetaIndividual").dxDataGrid({
        dataSource: myGoalIndividual,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100, 200]
        },
        allowColumnReordering: true,
        allowColumnResizing: true,
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
            { dataField: 'Id_Meta_individual', width: '3%', caption: 'ID', dataType: 'Number',  visible: true },
            { dataField: 'Meta', width: '8%', caption: 'Meta', format: '#,###.## Kg/Año', },
            { dataField: 'Carga_Obtenida', width: '8%', caption: 'Carga Obtenida', format: '#,###.## Kg/Año', },
            { dataField: 'Cumple_Meta', width: '10%', caption: 'Cumplimiento' },
            { dataField: 'Name_Tercero', width: '20%', caption: 'Tercero' },
            { dataField: 'Periodo', width: '15%', caption: 'Peiodo' },
            { dataField: 'Parametro_Ambiental', width: '15%', caption: 'Parametro ambiental' },
            { dataField: 'Factor_Regional', width: '8%', caption: 'Factor Regional', format: '#,###.##', },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {

                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar Registro de Producción',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/loadMI";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.Id_Meta_individual
                                }).done(function (data) {
                                    if (data !== null) {
                                        MetaId = parseInt(data.Id_Meta_individual);
                                        numTargetMI.option("value", data.Meta);
                                        intLoadObtainedMI.option("value", data.Carga_Obtenida);
                                        meetsMI.option("value", data.Cumple_Meta);
                                        enterpriseMI.option("value", data.Id_Tercero);
                                        periodMI.option("value", data.Periodo_Id);
                                        parameterMI.option("value", data.Parametro_Ambiental_Id);
                                        regionalFactorMI.option("value", data.Factor_Regional_Id);
                                         option = false;
                                         popupMetaIndividual.show();
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
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/RemoveMI";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Meta_individual
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminando el reporte');
                                            else {
                                                $('#grdMetaIndividual').dxDataGrid({ dataSource: myGoalIndividual });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminando el reporte');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);

                     $('<div/>').dxButton({
                         icon: 'datafield',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea calcular la carga obtenida de esta Empresa?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/getCargaObtenidaMI";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Meta_individual
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'No existe una Meta definida, No se pudeo calcular la Carga');
                                            else {
                                                $('#grdMetaIndividual').dxDataGrid({ dataSource: myGoalIndividual });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'No se pudeo calcular la Carga');
                                        });
                                }
                            });
                        }
                     }).appendTo(container);

                    $('<div/>').dxButton({
                        icon: '../Content/icons/FactorRegional50x50px.png',
                        margin: '30px auto',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea Ajustar el Factor Regional del Periodo seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/calcularFR";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.Id_Meta_individual
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, ' al ajustar el factor Regional del Periodo');
                                            else {
                                                $('#grdMetaIndividual').dxDataGrid({ dataSource: myGoalIndividual });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Ajustar Factor Regioal');
                                        }
                                        );
                                }
                            });
                        },
                    }).appendTo(container);

                }

            }


        ]
    });

    var popupMetaIndividual = $("#popupMetaIndividual").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Metas Individuales"
    }).dxPopup("instance");


    // ******************************************
    //   
    //  TAB 2 - METAS GRUPALES
    //
    // ******************************************

    var intPeriodStartg = $("#intPeriodStartG").dxTextBox({
        placeholder: "Ingrese aqui el año",
        value: ""
    }).dxTextBox("instance");

    var intPeriodEndg = $("#intPeriodEndG").dxTextBox({
        placeholder: "Ingrese aqui tarifa de SST",
        value: ""
    }).dxTextBox("instance");


    $("#btnSaveGroupsGoal").dxButton({
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
            var _Ruta = urrl + "/InsertTSS";
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

    var btnAddGroupsGoal = $("#btnAddGroupsGoal").dxButton({
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

    $("#grdGroupsGoal").dxDataGrid({
        dataSource: myGroupsGoal,
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
            { dataField: 'id_meta_grupal', width: '5%', caption: 'Id', dataType: 'Number', visible: true },
            { dataField: 'meta', width: '10%', caption: 'Meta', format: "#,##0.## kg/año" },
            { dataField: 'periodo', width: '10%', caption: 'Periodo' },
            { dataField: 'parametro_ambiental', width: '10%', caption: 'Parametro' },
            { dataField: 'tsimtasa_tramo_id', width: '10%', caption: 'Tramo' },
        
        ]
    });

    var popupGroupsGoal = $("#popupGroupsGoal").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tarifa Solidos Suspendidos Totales"
    }).dxPopup("instance");


    // ******************************************
    //   
    //  TAB 1 - Factor Regional
    //
    // ******************************************

    var intFR = $("#intPeriodStartG").dxTextBox({
        placeholder: "Ingrese aqui el año",
        value: ""
    }).dxTextBox("instance");

    var intFR2 = $("#intPeriodEndG").dxTextBox({
        placeholder: "Ingrese aqui tarifa de SST",
        value: ""
    }).dxTextBox("instance");


    $("#btnSaveFR").dxButton({
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
            var _Ruta = urrl + "/InsertTSS";
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

    var btnAddFR = $("#btnAddFR").dxButton({
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

    $("#grdFR").dxDataGrid({
        dataSource: myRegionalFactor,
        repaintChangesOnly: true,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100, 200]
        },
        allowColumnReordering: true,
        allowColumnResizing: true,
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
            { dataField: 'ID_FACTOR_REGIONAL', width: '3%', caption: 'ID', dataType: 'Number', visible: true },
            { dataField: 'ANO', width: '5%', caption: 'Año' },
            { dataField: 'FACTOR', width: '5%', caption: 'Factor', format: '#,##0.######', },
            { dataField: 'CARGA_OBTENIDA', width: '7%', caption: 'Carga Obtenida', format: '#,##0.####### Kg', },
            { dataField: 'RESOLUCION', width: '5%', caption: 'Resolución' },
            { dataField: 'CUMPLE_META', width: '10%', caption: 'Cumple Meta' },
            { dataField: 'NOMBRE_TRAMO', width: '10%', caption: 'Tramos' },
            { dataField: 'TSIMTASA_TRAMOS_ID', width: '3%', caption: 'Tramos', visible: false },
            { dataField: 'NOMBRE', width: '20%', caption: 'Parametro Ambiental' },
            { dataField: 'TSIMTASA_PERIODOS_ID', width: '3%', caption: 'Periodos', visible: false  },
            { dataField: 'PERIODO', width: '15%', caption: 'Periodos' },

            {
                alignment: 'center', caption: 'Funciones',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/icons/FactorRegional50x50px.png',
                        //icon: 'percent',
                        //text: 'Factor',
                        //type: 'success',
                        //title: 'Calcular Factor Regional',
                        margin: '30px auto',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea Ajustar el Factor Regional del Periodo seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/getAjustarFR";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID_FACTOR_REGIONAL
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, ' al ajustar el factor Regional del Periodo');
                                            else {
                                                $('#grdFR').dxDataGrid({ dataSource: myRegionalFactor });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Ajustar Factor Regioal');
                                        }
                                    );
                                }
                            });
                        },
                    }).appendTo(container);
        
                    $('<div/>').dxButton({
                        icon: 'datafield',

                        //type: 'success',
                        margin: '30px auto',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea calcular la carga obtenida del Periodo seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Retributivas/api/AdministradorApi/getCargaObtenida";
                                    $.getJSON(_Ruta,
                                        {
                                            Id: options.data.ID_FACTOR_REGIONAL
                                        }).done(function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, ' al ajustar el factor Regional del Periodo');
                                            else {
                                                $('#grdFR').dxDataGrid({ dataSource: myRegionalFactor });
                                                //    $('#grdReports').dxDataGrid.refresh();
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Ajustar Factor Regioal');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);


                }

            }

        ]
    });

    var popupFR = $("#popupFR").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Tarifa Solidos Suspendidos Totales"
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
        $.getJSON($("#SIM").data("url") + urrl + "/Tributary", {
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

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/Shedding", {
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

//  TAB 10 - Parameter Enviroment
var myParameter = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/getParameter", {
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
        $.getJSON($("#SIM").data("url") + urrl + "/BOD", {
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
        $.getJSON($("#SIM").data("url") + urrl + "/TSS", {
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

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/Reports", {
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

//  TAB 6 - Quinqueño
var myQuinqueno = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/getQuinqueno", {
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


//  TAB 6 - Peridodo
var myPeriod  = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/getPeriod", {
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



//  TAB 3 - METAS INDIVIDUALES
var myGoalIndividual = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/getGoalIndividual", {
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





//  TAB 2 - METAS GROUPALES
var myGroupsGoal = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/getGroupsGoal", {
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




//  TAB 2 - factor Regional
var myRegionalFactor = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : "";
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($("#SIM").data("url") + urrl + "/getRegionalFactor", {
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


