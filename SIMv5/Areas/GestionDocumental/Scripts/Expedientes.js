var IdExpediente = -1;
var CodSerieDoc = -1;
var CodSubSerieDoc = -1;
var CodUnidadDocumental = -1;
var indicesSerieDocumentalStore = null;
var editar = false;

$(document).ready(function () {

    $("#grdListaExp").dxDataGrid({
        dataSource: ExpedientesDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
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
            { dataField: 'ID_EXPEDIENTE', width: '5%', caption: 'Identificador', alignment: 'center' },
            { dataField: 'TIPO', width: '20%', caption: 'Tipo de Expediente', dataType: 'string' },
            { dataField: 'NOMBRE', width: '25%', caption: 'Nombre del Expediente', dataType: 'string' },
            { dataField: 'CODIGO', width: '10%', caption: 'Código Asignado', dataType: 'string' },
            { dataField: 'FECHACREA', width: '10%', caption: 'Fecha', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            { dataField: 'ANULADO', width: '5%', caption: 'Anulado', dataType: 'string' },
            { dataField: 'ESTADO', width: '5%', caption: 'Último Estado', dataType: 'string' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Ver detalles del expediente',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/DetalleExpediente";
                            $.getJSON(_Ruta, { IdExpediente: options.data.ID_EXPEDIENTE })
                                .done(function (data) {
                                    if (data != null) {
                                        showExpediente(data);
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar proceso');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar datos del expediente',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/EditarExpediente";
                            $.getJSON(_Ruta,
                                {
                                    IdExpediente: options.data.ID_EXPEDIENTE
                                }).done(function (data) {
                                    if (data != null) {
                                        editar = true;
                                        indicesSerieDocumentalStore = null;
                                        IdExpediente = parseInt(data.IdExpediente);
                                        cboSerie.option("value", data.IdSerieDoc);
                                        var cboSubSerieDs = cboSubSerie.getDataSource();
                                        CodSerieDoc = data.IdSerieDoc;
                                        cboSubSerieDs.reload(); 
                                        cboSubSerie.option("value", data.IdSubSerieDoc);
                                        var cboUniDocDs = cboUnidadDoc.getDataSource();
                                        CodSubSerieDoc = data.IdSubSerieDoc;
                                        cboUniDocDs.reload(); 
                                        cboUnidadDoc.option("value", data.IdUnidadDoc);
                                        txtNombre.option("value", data.Nombre);
                                        txtCodigo.option("value", data.Codigo);
                                        txtDescrip.option("value", data.Descripcion);
                                        chkAnulado.option("disabled", false);
                                        chkActivo.option("value", data.Estado == "A" ? true : false);
                                        chkAnulado.option("value", data.Estado == "N" ? true : false);
                                        txtUltEstado.option("visible", true);
                                        txtUltEstado.option("value", data.UltEstado);
                                        AsignarIndices(data.Indices);
                                        popup.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar proceso');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'bookmark',
                        hint: 'Administrar estado del expediente',
                        onClick: function (e) {
                            IdExpediente = options.data.ID_EXPEDIENTE;
                            //$('#grdEstado').dxDataGrid({ dataSource: EstadoExpDataSource });
                            popupEst.show();
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'trash',
                        hint: 'Eliminar expediente',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el expediente de tipo ' + options.data.TIPO + ' con el nombre ' + options.data.NOMBRE + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/EliminaExpediente";
                                    $.getJSON(_Ruta,
                                        {
                                            IdExpediente: options.data.ID_EXPEDIENTE
                                        }).done(function (data) {
                                            if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar expediente');
                                            else {
                                                $('#grdListaExp').dxDataGrid({ dataSource: ExpedientesDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Eliminar expediente');
                                        });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'folder',
                        hint: 'Administrar carpetas del expediente',
                        onClick: function (e) {
                            IdExpediente = options.data.ID_EXPEDIENTE;
                            var Tomos = $("#popTomos").dxPopup("instance");
                            Tomos.show();
                            $('#frmTomos').attr('src', $('#SIM').data('url') + 'GestionDocumental/Expedientes/Tomos?IdExp=' + IdExpediente + '&c=@DateTime.Now.ToString("HHmmss")');
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupDet = null;

    $("#popTomos").dxPopup({
        fullScreen: true,
        onHiding: function (e) {
            $('#frmTomos').attr('src', '');
        }
    });

    var showExpediente = function (data) {
        Expediente = data;
        if (popupDet) {
            popupDet.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleExp").dxPopup(popupOptions).dxPopup("instance");
        }
        popupDet.show();
    };

    var cboSerie = $("#cboSerie").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODSERIE_DOCUMENTAL",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ListaSeries");
                }
            })
        }),
        displayExpr: "NOMBRE",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onOpened: function (e) {
            setTimeout(function () {
                e.component._popup.option('resizeEnabled', true);
                e.component._popup.option('width', e.component._popup.option('width') + 50);
            }, 10);
        },
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboSubSerieDs = cboSubSerie.getDataSource();
                CodSerieDoc = data.value.CODSERIE_DOCUMENTAL;
                cboSubSerieDs.reload(); 
                cboSubSerie.option("value", null);
            } else CodSerieDoc = -1;
        }
    }).dxSelectBox("instance");

    var cboSubSerie = $("#cboSubSerie").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODSUBSERIE",
                loadMode: "raw",
                load: function () {
                    if (typeof CodSerieDoc === 'undefined') CodSerieDoc = -1;
                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ListaSubSeries", { CodSerie: CodSerieDoc });
                }
            })
        }),
        displayExpr: "NOMBRE",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onOpened: function (e) {
            setTimeout(function () {
                e.component._popup.option('resizeEnabled', true);
                e.component._popup.option('width', e.component._popup.option('width') + 50);
            }, 10);
        },
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboUniDocDs = cboUnidadDoc.getDataSource();
                CodSubSerieDoc = data.value.CODSUBSERIE;
                cboUniDocDs.reload();
                cboUnidadDoc.option("value", null);
            } else CodSubSerieDoc = -1;
        }
    }).dxSelectBox("instance");
    
    var cboUnidadDoc = $("#cboUnidadDoc").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "CODSERIE",
                loadMode: "raw",
                load: function () {
                    if (typeof CodSubSerieDoc === 'undefined') CodSubSerieDoc = -1;
                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ListaUnidades", { CodSubSerie: CodSubSerieDoc });
                }
            })
        }),
        displayExpr: "NOMBRE",
        valueExpr: "CODSERIE",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione",
        onOpened: function (e) {
            setTimeout(function () {
                e.component._popup.option('resizeEnabled', true);
                e.component._popup.option('width', e.component._popup.option('width') + 50);
            }, 10);
        },
        onValueChanged: function (data) {
            if (!editar) {
                CodUnidadDocumental = data.value;
                CargarIndices();
            }
        }
    }).dxValidator({
        validationGroup: "ExpedienteGroup",
        validationRules: [{
            type: "required",
            message: "La unidad documental es obligatoria"
        }]
    }).dxSelectBox("instance");

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre del expediente...",
        value: "",
    }).dxValidator({
        validationGroup: "ExpedienteGroup",
        validationRules: [{
            type: "required",
            message: "El nombre del expediente es obligatorio"
        }]
    }).dxTextBox("instance");

    var txtCodigo = $("#txtCodigo").dxTextBox({
        placeholder: "Ingrese el código para el expediente...",
        value: "",
    }).dxTextBox("instance");

    var txtDescrip = $("#txtDescripcion").dxTextArea({
        value: "",
        height: 90,
        placeholder: "Ingrese una descripción para el expediente...",
        value: "",
    }).dxTextArea("instance");

    var chkActivo = $("#chkActivo").dxCheckBox({
        value: true,
        width: 80,
        text: "Activo",
        onValueChanged: function (data) {
            if (data.value) {
                chkAnulado.option("value", false);
            } else {
                chkAnulado.option("value", true);
            }
        }
    }).dxCheckBox("instance");

    var chkAnulado = $("#chkAnulado").dxCheckBox({
        value: false,
        width: 80,
        text: "Anulado",
        onValueChanged: function (data) {
            if (data.value) {
                chkActivo.option("value", false);
            } else {
                chkActivo.option("value", true);
            }
        }
    }).dxCheckBox("instance");

    var txtUltEstado = $("#txtUltEstado").dxTextBox({
        placeholder: "Ingrese el último estado del expediente",
        value: "",
        disabled: true
    }).dxTextBox("instance");

    function AsignarIndices(indices) {
        opcionesLista = [];

        indicesSerieDocumentalStore = new DevExpress.data.LocalStore({
            key: 'CODINDICE',
            data: indices,
            name: 'indicesSerieDocumental'
        });

        indices.forEach(function (valor, indice, array) {
            if (valor.TIPO == 5 && valor.ID_LISTA != null) {

                if (opcionesLista.findIndex(ol => ol.idLista == valor.ID_LISTA) == -1) {
                    opcionesLista.push({ idLista: valor.ID_LISTA, datos: null, cargado: false });
                }
            }
        });

        if (opcionesLista.length == 0) {
            CargarGridIndices();
        } else {
            opcionesLista.forEach(function (valor, indice, array) {
                $.getJSON($('#SIM').data('url') + 'Tramites/api/ProyeccionDocumentoApi/ObtenerIndiceValoresLista?id=' + valor.idLista).done(function (data) {
                    var index = opcionesLista.findIndex(ol => ol.idLista == valor.idLista);
                    opcionesLista[index].datos = data;
                    opcionesLista[index].cargado = true;

                    var finalizado = true;

                    opcionesLista.forEach(function (v, i, a) {
                        finalizado = finalizado && v.cargado;
                    });

                    if (finalizado) {
                        CargarGridIndices();
                    }
                });
            });
        }
    }

    function CargarIndices() {
        var URL = $('#SIM').data('url') + 'GestionDocumental/api/ExpedientesApi/ObtenerIndicesSerieDocumental';
        $.getJSON(URL, {
            codSerie: CodUnidadDocumental,
        }).done(function (data) {
            AsignarIndices(data);
        });
    }

    function CargarGridIndices() {
        $("#GridIndices").dxDataGrid({
            dataSource: indicesSerieDocumentalStore,
            allowColumnResizing: true,
            allowSorting: false,
            showRowLines: true,
            rowAlternationEnabled: true,
            showBorders: true,
            height: '100%',
            loadPanel: { text: 'Cargando Datos...' },
            paging: {
                pageSize: 0,
            },
            pager: {
                showPageSizeSelector: false,
            },
            filterRow: {
                visible: false,
            },
            groupPanel: {
                visible: false,
            },
            editing: {
                mode: "cell",
                allowUpdating: true,
                allowAdding: false,
                allowDeleting: false

            },
            selection: {
                mode: 'single'
            },
            scrolling: {
                showScrollbar: 'always',
            },
            wordWrapEnabled: true,
            columns: [
                {
                    dataField: "CODINDICE",
                    dataType: 'number',
                    visible: false,
                }, {
                    dataField: "INDICE",
                    caption: 'INDICE',
                    dataType: 'string',
                    width: '40%',
                    visible: true,
                    allowEditing: false
                }, {
                    dataField: 'VALOR',
                    caption: 'VALOR',
                    dataType: 'string',
                    allowEditing: true,
                    cellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 0: // TEXTO
                            case 1: // NUMERO
                            case 3: // HORA
                            case 8: // DIRECCION
                                if (cellInfo.data.VALOR != null) {
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 2: // FECHA
                                if (cellInfo.data.VALOR != null) {
                                    //cellElement.html(cellInfo.data.VALOR.getDate() + '/' + (cellInfo.data.VALOR.getMonth() + 1) + '/' + cellInfo.data.VALOR.getFullYear());
                                    cellElement.html(cellInfo.data.VALOR);
                                }
                                break;
                            case 4: // BOOLEAN
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR == 'S' ? 'SI' : 'NO');
                                break;
                            default: // OTRO
                                if (cellInfo.data.VALOR != null)
                                    cellElement.html(cellInfo.data.VALOR);
                                break;
                        }
                    },
                    editCellTemplate: function (cellElement, cellInfo) {
                        switch (cellInfo.data.TIPO) {
                            case 1: // NUMERO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);
                                if (cellInfo.data.MINIMO.length && cellInfo.data.MAXIMO.length > 0) {
                                    $(div).dxNumberBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        min: cellInfo.data.MINIMO,
                                        max: cellInfo.data.MAXIMO,
                                        showSpinButtons: false,
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                } else {
                                    $(div).dxNumberBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        showSpinButtons: false,
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                }
                                break;
                            case 0: // TEXTO
                            case 3: // HORA
                            case 8: // DIRECCION
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                            case 2: // FECHA
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);
                                var fecha = null;
                                if (cellInfo.data.VALOR != null && cellInfo.data.VALOR.trim() != '') {
                                    var partesFecha = cellInfo.data.VALOR.split('/');
                                    fecha = new Date(parseInt(partesFecha[2]), parseInt(partesFecha[1]) - 1, parseInt(partesFecha[0]));
                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        value: fecha,
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                } else {
                                    $(div).dxDateBox({
                                        type: 'date',
                                        width: '100%',
                                        displayFormat: 'dd/MM/yyyy',
                                        onValueChanged: function (e) {
                                            //cellInfo.setValue(e.value);
                                            cellInfo.setValue(e.value.getDate() + '/' + (e.value.getMonth() + 1) + '/' + e.value.getFullYear());
                                        },
                                    });
                                }
                                break;
                            case 4: // SI/NO
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxSelectBox({
                                    dataSource: siNoOpciones,
                                    width: '100%',
                                    displayExpr: "Nombre",
                                    valueExpr: "ID",
                                    placeholder: "[SI/NO]",
                                    value: cellInfo.data.VALOR,
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                        $("#grdIndices").dxDataGrid("saveEditData");
                                    },
                                });
                                break;
                            case 5: // Lista
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                let itemsLista = opcionesLista[opcionesLista.findIndex(ol => ol.idLista == cellInfo.data.ID_LISTA)].datos;

                                if (cellInfo.data.ID_LISTA != null) {
                                    $(div).dxSelectBox({
                                        items: itemsLista,
                                        width: '100%',
                                        placeholder: "[SELECCIONAR OPCION]",
                                        value: (cellInfo.data.VALOR == null ? null : itemsLista.findIndex(ls => ls.NOMBRE == cellInfo.data.VALOR) > 0 ? itemsLista[itemsLista.findIndex(ls => ls.NOMBRE == cellInfo.data.VALOR)].ID : null),
                                        displayExpr: 'NOMBRE',
                                        valueExpr: 'ID',
                                        searchEnabled: true,
                                        onValueChanged: function (e) {
                                            cellInfo.data.ID_VALOR = e.value;
                                            cellInfo.setValue(itemsLista[itemsLista.findIndex(ls => ls.ID == e.value)].NOMBRE);
                                            $("#grdIndices").dxDataGrid("saveEditData");
                                        },
                                    });
                                } else {
                                    $(div).dxTextBox({
                                        value: cellInfo.data.VALOR,
                                        width: '100%',
                                        onValueChanged: function (e) {
                                            cellInfo.setValue(e.value);
                                        },
                                    });
                                }
                                break;
                            default: // Otro
                                var div = document.createElement("div");
                                cellElement.get(0).appendChild(div);

                                $(div).dxTextBox({
                                    value: cellInfo.data.VALOR,
                                    width: '100%',
                                    onValueChanged: function (e) {
                                        cellInfo.setValue(e.value);
                                    },
                                });
                                break;
                        }
                    }
                }, {
                    dataField: "OBLIGA",
                    caption: 'R',
                    width: '40px',
                    dataType: 'string',
                    visible: true,
                    allowEditing: false,
                    cellTemplate: function (cellElement, cellInfo) {
                        cellElement.css('text-align', 'center');
                        cellElement.css('color', 'red');
                        if (cellInfo.data.OBLIGA == 1)
                            cellElement.html('*');

                    }
                }
            ]
        });
    }

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ExpedienteGroup");
            var UnidadDoc = cboUnidadDoc.option("value");
            var nombre = txtNombre.option("value");
            var Codigo = txtCodigo.option("value");
            var Descrip = txtDescrip.option("value");
            var Activo = chkActivo.option("value");
            var Anulado = chkAnulado.option("value");
            var Estado = Activo ? "A" : Anulado ? "N" : "";
            var Indices = indicesSerieDocumentalStore._array;
            //Cerrado = Cerrado ? "1" : "0";
            var params = { IdUnidadDoc: UnidadDoc, IdExpediente: IdExpediente, Nombre: nombre, Codigo: Codigo, Descripcion: Descrip, Anulado: Estado, Indices: Indices };
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/GuardaExpediente";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                beforeSend: function () { },
                success: function (data) {
                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#grdListaExp').dxDataGrid({ dataSource: ExpedientesDataSource });
                        $("#PopupNuevoExp").dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });           
        }
    });

    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo Expediente",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            indicesSerieDocumentalStore = null;
            IdExpediente = -1;
            editar = false;
            cboSerie.option("value", null);
            cboSubSerie.option("value", null);
            cboUnidadDoc.option("value", null);
            txtDescrip.option("value", null);
            txtUltEstado.option("visible", false);
            chkAnulado.option("disabled", true);
            txtNombre.reset();
            txtCodigo.reset();
            popup.show();
        }
    });

    var popup = $("#PopupNuevoExp").dxPopup({
        width: 1100,
        height: 600,
        hoverStateEnabled: true,
        title: "Expediente",
        dragEnabled: true,
        onShown: function () {
            if (!editar) {
                indicesSerieDocumentalStore = null;
                //CargarGridIndices();
                CargarIndices();
                //    gridInstance = $("#GridIndices").dxDataGrid('instance');
                //    gridInstance.option("dataSource", indicesSerieDocumentalStore);
            } else {
                CargarGridIndices();
            }
        }  
    }).dxPopup("instance");

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Detalle del Expediente",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            var TablaIndi = "<table class='table table-sm' style='font-size: 11px;'><thead><tr><th scope='col'>NOMBRE DEL ÍNDICE</th><th scope='col'>VALOR</th></tr></thead><tbody>";
            Expediente.Indices.forEach(function (indice, index) {
                TablaIndi += "<tr><th scope='row'>" + indice.INDICE + "</th><td>" + indice.VALOR + "</td></tr>";
            });
            return $("<div />").append(
                $("<p>Unidad Documental : <span><b>" + Expediente.UnidadDoc + "</b></span></p>"),
                $("<p>Nombre del expediente : <span><b>" + Expediente.Nombre + "</b></span></p>"),
                $("<p>Código asignado : <span><b>" + Expediente.Codigo + "</b></span></p>"),
                $("<p>Descripción : <span><b>" + Expediente.Descripcion + "</b></span></p>"),
                $("<p>Funcionario que lo creó : <span><b>" + Expediente.Responsable + "</b></span></p>"),
                $("<p>Fecha de creación : <span><b>" + Expediente.FechaCrea + "</b></span></p>"),
                $("<p>Expediente anulado? : <span><b>" + Expediente.Anulado + "</b></span></p>"),
                $("<p>Último estado expediente : <span><b>" + Expediente.UltEstado + "</b></span></p>"),
                $("<br />"),
                $("<p>Cantidad de carpetas : <span><b>" + Expediente.Tomos + "</b></span></p>"),
                $("<p>Cantidad documentos : <span><b>" + Expediente.Documentos + "</b></span></p><br />"),
                $("<div id='tbalaInd'>" + TablaIndi + "</div>")
            );
        }
    };

     var EstadoDataSource = {
        store: new DevExpress.data.CustomStore({
            key: "ID_ESTADO",
            loadMode: "raw",
            load: function () {
                return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ListaEstadosExp");
            }
        })
    }

    var popupEst = $("#PopupEstados").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Estados Expediente",
        onShown: function () {
            $("#grdEstado").dxDataGrid({
                dataSource: EstadoExpDataSource,
                keyExpr: "ID_ESTADOEXPEDIENTE",
                allowColumnResizing: true,
                showBorders: true,
                paging: {
                    enabled: false
                },
                editing: {
                    mode: "row",
                    allowAdding: true,
                    useIcons: true
                }, columns: [
                    {
                        dataField: "ID_ESTADOEXPEDIENTE", dataType: 'number', visible: false
                    },
                    {
                        dataField: "ESTADO", caption: 'Estado', dataType: 'string', width: '35%',
                        cellTemplate: function (cellElement, cellInfo) {
                            cellElement.html(cellInfo.data.ESTADO);
                        },
                        editCellTemplate: function (cellElement, cellInfo) {
                            var div = document.createElement("div");
                            cellElement.get(0).appendChild(div);
                            $(div).dxSelectBox({
                                dataSource: EstadoDataSource,
                                placeholder: "Seleccione",
                                value: cellInfo.data.ESTADO,
                                displayExpr: "ESTADO",
                                valueExpr: "ID_ESTADO",
                                onValueChanged: function (e) {
                                    cellInfo.setValue(e.value);
                                }
                            });
                        }
                    },
                    {
                        dataField: "FUNCIONARIO", caption: "Funcionario", dataType: "string", width: "35%", allowEditing: false
                    },
                    {
                        dataField: "FECHAINI", caption: "Fecha Inicial", dataType: "date", width: "15%", format: "MMM dd, yyyy"
                    },
                    {
                        dataField: "FECHAFIN", caption: "Fecha Final", dataType: "date", width: "15%", format: "MMM dd, yyyy"
                    }
                ]
            });
        }
    }).dxPopup("instance");
});

var ExpedientesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FECHACREA","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/ExpedientesApi/ObtieneExpedientes', {
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
    }
});

var EstadoExpDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'GestionDocumental/api/ExpedientesApi/EstadosExpediente', {
            skip: skip,
            take: take,
            IdExp: IdExpediente
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    },
    insert: function (key, values) {
        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/InsertaEstadoExp";
        var params = { IdExpediente: IdExpediente, IdEstado: key.ESTADO, FechaIni: key.FECHAINI };
        $.ajax({
            type: "POST",
            dataType: 'json',
            url: _Ruta,
            data: JSON.stringify(params),
            contentType: "application/json",
            beforeSend: function () { },
            success: function (data) {
                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Guardar Datos');
                else {
                    DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                    $('#grdListaExp').dxDataGrid({ dataSource: ExpedientesDataSource });
                    $("#grdEstado").dxDataGrid({ dataSource: EstadoExpDataSource });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
            }
        });   
    }
});