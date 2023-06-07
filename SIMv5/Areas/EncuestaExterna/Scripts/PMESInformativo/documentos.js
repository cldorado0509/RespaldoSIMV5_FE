var idOrganizacion = -1;
var idTipoComunicacion = -1;
var tipoComunicacion = '';
var archivosDocumento = null;
var idArchivo = 0;
var archivoActualizar = null;
var opcionesLista = [];
var cargando = false;
var idTercero = 0;
var itemSeleccionadoTercero = false;

var listoMostrar = false;

$(document).ready(function () {
    $("#loadPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $('#idTipoComunicacion').dxSelectBox({
        items: null,
        width: '100%',
        placeholder: 'Seleccionar Tipo de Comunicación',
        showClearButton: false,
        readOnly: ($('#app').data('ro') == 'S')
    });

    $('#tipoComunicacion').dxTextBox({
        width: '100%',
        placeholder: 'Tipo de Comunicación',
        readOnly: ($('#app').data('ro') == 'S')
    });

    $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/ObtenerTiposComunicacion').done(function (data) {
        $('#idTipoComunicacion').dxSelectBox({
            items: data,
            displayExpr: 'S_TIPOCOMUNICACION',
            valueExpr: 'COD_TIPOCOMUNICACION',
            placeholder: 'Seleccionar Tipo de Comunicación',
            showClearButton: false,
            onValueChanged: function (data) {
                idTipoComunicacion = data.value;
            },
            onSelectionChanged: function (data) {
                if (data.selectedItem.COD_TIPOCOMUNICACION == -1) {
                    $('#pnlTipoComunicacion').show();
                } else {
                    $('#pnlTipoComunicacion').hide();
                }
            }
        });

        $.getJSON($('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/ObtenerDatosDocumentoPublicacion', {
            id: $('#app').data('id'),
        }).done(function (data) {
            $('.my-cloak').removeClass('my-cloak');

            archivosDocumento = data.Archivos;
            $("#grdArchivos").dxDataGrid({
                dataSource: archivosDocumento
            });

            cargando = true;

            if (data.IdTercero != null && data.IdTercero > 0) {
                cboTercerosDataSource.store().clear();
                cboTercerosDataSource.store().insert({ ID_TERCERO: $('#app').data('tercero'), S_RSOCIAL: $('#app').data('nombretercero') });
                cboTercerosDataSource.load();
            }

            $('#organizacion').dxSelectBox('instance').option('value', data.IdTercero);

            selectedItem = $('#organizacion').dxSelectBox('instance').option('selectedItem');

            $('#idTipoComunicacion').dxSelectBox({
                value: data.IdTipoComunicacion
            });

            $('#idTipoComunicacion').dxSelectBox('instance').option('value', data.IdTipoComunicacion);

            selectedItem = $('#idTipoComunicacion').dxSelectBox('instance').option('selectedItem');

            if (selectedItem && (selectedItem.COD_TIPOCOMUNICACION == -1 || selectedItem.COD_TIPOCOMUNICACION == null)) {
                $('#pnlTipoComunicacion').show();
                $('#tipoComunicacion').dxTextBox('instance').option('value', data.TipoComunicacion);

            } else {
                $('#pnlTipoComunicacion').hide();
            }

            $('#descripcion').dxTextBox({
                value: data.Descripcion,
                width: '100%',
                maxLength: 240,
                readOnly: ($('#app').data('ro') == 'S')
            });

            archivosDocumento = data.Archivos;
            $("#grdArchivos").dxDataGrid({
                dataSource: archivosDocumento
            });

            listoMostrar = true;
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
    }).fail(function (jqxhr, textStatus, error) {
        alert('error cargando datos: ' + textStatus + ", " + error);
    });
    
    if ($('#app').data('ro') == 'N') {
        $('#descripcionArchivo').dxTextBox({
            width: '100%',
            maxLength: 120,
        });

        $('#agregarArchivo').dxButton({
            icon: 'upload',
            text: 'Cargar Archivo',
            width: '100%',
            onClick: function () {
                if ($('#descripcionArchivo').dxTextBox('instance').option('value') == null || $('#descripcionArchivo').dxTextBox('instance').option('value').trim() == '') {
                    MostrarNotificacion('alert', null, 'Descripción de Archivo Requerida');
                } else {
                    var fileUploader = $('#fileDocumento').dxFileUploader('instance');
                    fileUploader._isCustomClickEvent = true;
                    fileUploader._$fileInput.click();
                }
            }
        });

        $('#fileDocumento').dxFileUploader({
            name: 'file',
            uploadUrl: $('#app').data('url') + 'EncuestaExterna/PMESInformativo/CargarArchivo',
            selectButtonText: 'Seleccionar Archivo (*.*)',
            labelText: 'o arrastre un archivo aquí',
            showFileList: false,
            visible: false,
            onValueChanged: function (e) {
                var url = e.component.option('uploadUrl');

                if (archivoActualizar != null) {
                    idArchivo = archivoActualizar.ID_INFORMATIVO_DOC_ARCHIVOS;

                    url = updateQueryStringParameter(url, 'ida', idArchivo);
                } else {
                    idArchivo = 0;

                    archivosDocumento.forEach(archivosDocumento => {
                        if (archivosDocumento.ID_INFORMATIVO_DOC_ARCHIVOS > idArchivo) {
                            idArchivo = archivosDocumento.ID_INFORMATIVO_DOC_ARCHIVOS;
                        }
                    });

                    idArchivo++;

                    url = updateQueryStringParameter(url, 'ida', idArchivo);
                }

                e.component.option('uploadUrl', url);
            },
            onUploadStarted: function (e) {
                $("#loadPanel").dxLoadPanel('instance').show();
            },
            onUploadAborted: function (e) {
                $("#loadPanel").dxLoadPanel('instance').hide();
            },
            onUploaded: function (e) {
                var orden = 0;

                var descripcion = $('#descripcionArchivo').dxTextBox('instance').option('value');

                $('#descripcionArchivo').dxTextBox('instance').option('value', '');

                archivosDocumento.forEach(archivosDocumento => {
                    if (archivosDocumento.N_ORDEN > orden) {
                        orden = archivosDocumento.N_ORDEN;
                    }
                });

                orden++;

                if (archivoActualizar == null)
                    archivosDocumento.push({ ID_INFORMATIVO_DOC_ARCHIVOS: idArchivo, S_DESCRIPCION: descripcion, S_ACTIVO: 'S', N_ORDEN: orden, ID_DOCUMENTO: 0, MODIFICADO: 'N', ARCHIVONUEVO: 'S', ARCHIVOACTUALIZADO: 'N', S_NOMBRE_ARCHIVO: e.file.name });
                else {
                    archivosDocumento.forEach(archivosDocumento => {
                        if (archivosDocumento.ID_INFORMATIVO_DOC_ARCHIVOS == idArchivo) {
                            archivosDocumento.MODIFICADO = 'S';
                            archivosDocumento.ARCHIVOACTUALIZADO = 'S';
                            archivosDocumento.S_NOMBRE_ARCHIVO = e.file.name;
                        }
                    });
                }

                archivoActualizar = null;

                $("#grdArchivos").dxDataGrid({
                    dataSource: archivosDocumento
                });

                $("#loadPanel").dxLoadPanel('instance').hide();
            },
            onUploadError: function (e) {
                $("#loadPanel").dxLoadPanel('instance').hide();
                archivoActualizar = null;

                MostrarNotificacion('alert', null, 'Error Subiendo Archivo: ' + e.request.responseText);
            },
        });
    }

    $('#popTercero').dxPopup({
        showTitle: true,
        deferRendering: false,
        title: 'Seleccionar Tercero',
        onShown: function () {
            //$('#grdTercero').dxDataGrid('instance').option('dataSource', terceroDataSource);

            $('#grdTercero').dxDataGrid({
                dataSource: terceroDataSource,
                allowColumnResizing: true,
                loadPanel: { text: 'Cargando Datos...' },
                paging: {
                    pageSize: 10
                },
                pager: {
                    showPageSizeSelector: false
                },
                filterRow: {
                    visible: true,
                },
                groupPanel: {
                    visible: false,
                },
                editing: {
                    editEnabled: false,
                    removeEnabled: false,
                    insertEnabled: false
                },
                selection: {
                    mode: 'single'
                },
                columns: [
                    {
                        dataField: 'ID_TERCERO',
                        caption: 'ID_TERCERO',
                        visible: false,
                        dataType: 'number'
                    },
                    {
                        dataField: 'N_DOCUMENTON',
                        width: '25%',
                        caption: 'DOCUMENTO',
                        visible: true,
                        dataType: 'number'
                    },
                    {
                        dataField: 'S_RSOCIAL',
                        width: '75%',
                        caption: 'RAZON SOCIAL',
                        visible: true,
                        dataType: 'string'
                    }
                ],
                onSelectionChanged: function (selecteditems) {
                    var data = selecteditems.selectedRowsData[0];
                    itemSeleccionadoTercero = true;
                    idTercero = data.ID_TERCERO;
                    nombreTercero = data.S_RSOCIAL;

                    var popup = $('#popTercero').dxPopup('instance');
                    popup.hide();
                }
            });
        },
        onHidden: function () {
            if (itemSeleccionadoTercero) {
                itemSeleccionadoTercero = false;

                cboTercerosDataSource.store().clear();
                cboTercerosDataSource.store().insert({ ID_TERCERO: idTercero, S_RSOCIAL: nombreTercero });
                cboTercerosDataSource.load();

                $('#organizacion').dxSelectBox('instance').option('dataSource', cboTercerosDataSource);
                $('#organizacion').dxSelectBox('instance').option('value', idTercero);
            }
        },
    });

    cboTercerosDataSource.store().clear();
    cboTercerosDataSource.store().insert({ ID_TERCERO: $('#app').data('tercero'), S_RSOCIAL: $('#app').data('nombretercero') });
    cboTercerosDataSource.load();

    $('#organizacion').dxSelectBox({
        dataSource: cboTercerosDataSource,
        valueExpr: 'ID_TERCERO',
        displayExpr: 'S_RSOCIAL',
        placeholder: '(Todos los Terceros)',
        value: idTercero,
        readOnly: ($('#app').data('ro') == 'S'),
        onOpened: function () {
            $('#organizacion').dxSelectBox('instance').close();
            var popup = $('#popTercero').dxPopup('instance');
            popup.show();
        }
    });

    $("#grdArchivos").dxDataGrid({
        dataSource: null,
        allowColumnResizing: true,
        height: '100%',
        width: '100%',
        loadPanel: { text: 'Cargando Datos...' },
        paging: {
            enabled: false
        },
        filterRow: {
            visible: false,
        },
        groupPanel: {
            visible: false,
            allowColumnDragging: false,
        },
        editing: {
            mode: 'cell',
            allowUpdating: ($('#app').data('ro') == 'N'),
            allowDeleting: false,
            allowAdding: false,
            useIcons: true
        },
        columns: [
            {
                dataField: "ID_INFORMATIVO_DOC_ARCHIVOS",
                dataType: 'number',
                visible: false,
            }, {
                dataField: "S_DESCRIPCION",
                caption: 'DESCRIPCION',
                dataType: 'string',
                allowEditing: false,
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    switch (cellInfo.data.N_TIPO) {
                        case 1: // Principal
                            cellElement.css('font-weight', 'bold');
                            break;
                        case 2: // Adjunto
                        default:
                            cellElement.css('font-weight', 'normal');
                            break;
                    }

                    if (cellInfo.data.MODIFICADO == 'S') {
                        cellElement.css('font-style', 'italic');
                    }

                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.S_DESCRIPCION);
                },
            }, {
                dataField: 'N_ORDEN',
                caption: 'ORDEN',
                alignment: 'center',
                width: '10%',
                dataType: 'number',
                visible: true,
                cellTemplate: function (cellElement, cellInfo) {
                    cellElement.css('text-align', 'center');

                    switch (cellInfo.data.N_TIPO) {
                        case 1: // Principal
                            cellElement.css('font-weight', 'bold');
                            break;
                        case 2: // Adjunto
                        default:
                            cellElement.css('font-weight', 'normal');
                            break;
                    }

                    if (cellInfo.data.MODIFICADO == 'S') {
                        cellElement.css('font-style', 'italic');
                    }

                    if (cellInfo.data.S_ACTIVO == 'N') {
                        cellElement.css('color', 'red');
                    }

                    cellElement.html(cellInfo.data.N_ORDEN);
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: ($('#app').data('ro') == 'N'),
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'upload',
                            width: '100%',
                            onClick: function () {
                                archivoActualizar = { ID_INFORMATIVO_DOC_ARCHIVOS: cellInfo.data.ID_INFORMATIVO_DOC_ARCHIVOS, N_TIPO: cellInfo.data.N_TIPO };

                                var fileUploader = $('#fileDocumento').dxFileUploader('instance');
                                fileUploader._isCustomClickEvent = true;
                                fileUploader._$fileInput.click(); 
                            }
                        });
                    }
                },
            }, {
                alignment: 'center',
                width: '5%',
                hint: 'Descargar Documento',
                visible: true,
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'download',
                            width: '100%',
                            onClick: function () {
                                window.open($('#app').data('url') + 'EncuestaExterna/PMESInformativo/DescargarDocumento?id=' + cellInfo.data.ID_INFORMATIVO_DOC_ARCHIVOS + '&n=' + cellInfo.data.ARCHIVONUEVO + '&a=' + cellInfo.data.ARCHIVOACTUALIZADO + '&f=' + utf8_to_b64(cellInfo.data.S_NOMBRE_ARCHIVO), '_ArchivoDescarga');
                            }
                        });
                    }
                },
            }, {
                alignment: 'center',
                width: '5%',
                visible: ($('#app').data('ro') == 'N'),
                allowEditing: false,
                cellTemplate: function (cellElement, cellInfo) {
                    if (cellInfo.data.S_ACTIVO == 'S') {
                        var div = document.createElement("div");
                        cellElement.get(0).appendChild(div);

                        $(div).dxButton({
                            icon: 'trash',
                            width: '100%',
                            onClick: function () {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro de Eliminar el Archivo ?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        cellInfo.data.S_ACTIVO = 'N';
                                        cellInfo.data.MODIFICADO = 'S';

                                        $("#grdArchivos").dxDataGrid({
                                            dataSource: archivosDocumento
                                        });
                                    }
                                });
                            }
                        });
                    }
                },
            }
        ],
        onEditorPreparing: function (e) {
            if (e.dataField === "N_ORDEN" && e.parentType === "dataRow") {
                e.editorOptions.readOnly = (e.value == 0 || e.row.data.S_ACTIVO == 'N');
            }
        },
        onRowUpdated: function (e) {
            e.data.MODIFICADO = 'S';
        },
    });

    if ($('#app').data('ro') == 'N') {
        $('#almacenar').dxButton(
            {
                icon: '',
                text: 'Almacenar',
                width: '200px',
                type: 'success',
                elementAttr: {
                    style: "float: right;"
                },
                onClick: function (params) {
                    if (ValidarDatosMinimos()) {
                        $("#loadPanel").dxLoadPanel('instance').show();

                        $.postJSON(
                            $('#app').data('url') + 'EncuestaExterna/api/PMESInformativoApi/AlmacenarDatosPublicacion', {
                                id: $('#app').data('id'),
                                IdTipoComunicacion: $('#idTipoComunicacion').dxSelectBox('instance').option('value'),
                                TipoComunicacion: $('#tipoComunicacion').dxTextBox('instance').option('value'),
                                IdTercero: $('#organizacion').dxSelectBox('instance').option('value'),
                                Descripcion: $('#descripcion').dxTextBox('instance').option('value'),
                                Archivos: archivosDocumento,
                        }
                        ).done(function (data) {
                            $("#loadPanel").dxLoadPanel('instance').hide();

                            var respuesta = data.split(':');

                            if (respuesta[0] == 'OK') {
                                var mensaje = 'Publicación Almacenada Satisfactoriamente.' + (respuesta[2] != '' ? '<br><br>ATENCION: ' + respuesta[2] : '');
                                var result = MostrarNotificacion('alert', null, mensaje);
                                result.done(function () {
                                    //window.location.href = $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?id=' + respuesta[1] + '&c=@DateTime.Now.ToString("HHmmss")';
                                    parent.CerrarPopupDocumento();
                                });
                            } else {
                                var result = MostrarNotificacion('alert', null, 'ERROR: ' + respuesta[2]);

                                result.done(function () {
                                    /*if (respuesta[1] != '0') {
                                        window.location.href = $('#app').data('url') + 'Tramites/ProyeccionDocumento/Documento?id=' + respuesta[1] + '&c=@DateTime.Now.ToString("HHmmss")';
                                    }*/
                                    parent.CerrarPopupDocumento();
                                });
                            }
                        }).fail(function (jqxhr, textStatus, error) {
                            $("#loadPanel").dxLoadPanel('instance').hide();
                            MostrarNotificacion('alert', null, 'ERROR DE INVOCACION: ' + textStatus + ", " + error);
                            MostrarNotificacion('alert', null, 'Se pudieron generar datos inconsistentes. Por favor cerrar esta ventana y revisar si la publicación fue registrada.');
                        });
                    }
                }
            }
        );
    }
});

function ValidarDatosMinimos()
{
    if ($('#descripcion').dxTextBox('instance').option('value') == null || $('#descripcion').dxTextBox('instance').option('value').trim() == '') {
        MostrarNotificacion('alert', null, 'Descripción Requerida.');
        return false;
    }

    return true;
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

let cboTercerosDataSource = new DevExpress.data.DataSource([]);

let terceroDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"S_RSOCIAL","desc":false}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#app').data('url') + 'General/api/TerceroApi/Terceros', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'l',
            noFilterNoRecords: true
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + error);
        });
        return d.promise();
    },
    byKey: function (key, extra) {
        return key.toString();
    },
});

$.postJSON = function (url, data) {
    var o = {
        url: url,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    };
    if (data !== undefined) {
        o.data = JSON.stringify(data);
    }
    return $.ajax(o);
};


function MostrarNotificacion(typeDialog, typeMsg, msg) {
    if (typeDialog === 'alert') {
        return DevExpress.ui.dialog.alert(msg, 'Proyección Documento');
    } else {
        return DevExpress.ui.notify(msg, typeMsg, 3000);
    }
}

function utf8_to_b64(str) {
    return window.btoa(unescape(encodeURIComponent(str)));
}

function b64_to_utf8(str) {
    return decodeURIComponent(escape(window.atob(str)));
}