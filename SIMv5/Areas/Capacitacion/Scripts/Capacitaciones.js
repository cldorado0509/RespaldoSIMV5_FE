var IdEvento = -1;

$(document).ready(function () {
    var Evento = null;

    $("#grdListaCapac").dxDataGrid({
        dataSource: CapacitacionDataSource,
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
            { dataField: 'ID_EVENTO', width: '5%', caption: 'Código', alignment: 'center' },
            { dataField: 'EVENTO', width: '15%', caption: 'Capacitación', dataType: 'string' },
            { dataField: 'LUGAR', width: '30%', caption: 'Lugar del Evento', dataType: 'string' },
            { dataField: 'FECHA', width: '30%', caption: 'Fecha', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'fields',
                        hint: 'Ver detalles de la capacitación',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Capacitacion/api/CapacitacionApi/DetalleCapacitacion";
                            $.getJSON(_Ruta, { IdEvento: options.data.ID_EVENTO })
                                .done(function (data) {
                                    if (data != null) {
                                        showCapacitacion(data);
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
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
                        hint: 'Editar datos de la capacitación',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Capacitacion/api/CapacitacionApi/DetalleCapacitacion";
                            $.getJSON(_Ruta,
                                {
                                    IdEvento: options.data.ID_EVENTO
                                }).done(function (data) {
                                    if (data != null) {
                                        IdEvento = parseInt(data.IdEvento);
                                        txtNombre.option("value", data.Evento);
                                        txtLugar.option("value", data.Lugar);
                                        var Fecha = data.Fecha != null ? new Date(data.Fecha) : "";
                                        dpFecha.option("value", Fecha);
                                        txtDuracion.option("value", data.Duracion);
                                        txtResponsable.option("value", data.Responsable);
                                        txtCapacidad.option("value", data.Capacidad);
                                        txtContacto.option("value", data.Contacto);
                                        txtCorreoContacto.option("value", data.CorreoContacto);
                                        txtUrl.option("value", data.Url);
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
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'trash',
                        hint: 'Eliminar capacitación',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar la capacitación ' + options.data.EVENTO + '?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "Capacitacion/api/CapacitacionApi/EliminaCapacitacion";
                                    $.getJSON(_Ruta,
                                        {
                                            IdEvento: options.data.ID_EVENTO
                                        }).done(function (data) {
                                            if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar capacitación');
                                            else {
                                                $('#grdListaCapac').dxDataGrid({ dataSource: CapacitacionDataSource });
                                            }
                                        }).fail(function (jqxhr, textStatus, error) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar capacitación');
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
                        icon: 'group',
                        hint: 'Inscritos en la capacitación',
                        onClick: function (e) {
                            IdEvento = options.data.ID_EVENTO;
                            showInscritos();
                        }
                    }).appendTo(container);
                }
            },
            {
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'print',
                        hint: 'Reporte Inscritos en la capacitación',
                        onClick: function (e) {
                            IdEvento = options.data.ID_EVENTO;
                            var popupRep = $("#PopupReporte").dxPopup("instance");
                            popupRep.option("contentTemplate", popupOpcRepto.contentTemplate.bind(IdEvento));
                            $('#PopupReporte').css({ 'visibility': 'visible' });
                            $("#PopupReporte").fadeTo("slow", 1);
                            popupRep.show();
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupDet = null;
    var popupIns = null;

    var showCapacitacion = function (data) {
        Evento = data;
        if (popupDet) {
            popupDet.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleCapac").dxPopup(popupOptions).dxPopup("instance");
        }
        popupDet.show();
    };

    var showInscritos = function () {
        if (popupIns) {
            popupIns.show();
        } else {
            popupIns = $("#PopupInscritos").dxPopup("instance");
            popupIns.show();
        }   
    };

    $("#PopupReporte").dxPopup({
        fullScreen: true,
        showTitle: true,
        title: "Reporte de los inscritos a la capacitación",
        dragEnabled: false,
        closeOnOutsideClick: true
    });

    var popupOpcRepto = {
        fullScreen: true,
        title: 'Reporte de los inscritos a la capacitación',
        closeOnOutsideClick: true,
        visible: false,
        contentTemplate: function (container) {
            var _ruta = $('#SIM').data('url') + 'Utilidades/Documento?url=' + $('#SIM').data('url') + "Capacitacion/Capacitacion/ReporteInscritos?id=" + IdEvento;
            $("<iframe>").attr("src", _ruta).attr("width", "100%").attr("height", "100%").appendTo(container);
        }
    };

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la capacitación aqui...",
        value: "",
    }).dxValidator({
        validationGroup: "CapacitacionGroup",
        validationRules: [{
            type: "required",
            message: "El nombre de la capacitación es obligatorio"
        }]
    }).dxTextBox("instance");

    var txtLugar = $("#txtLugar").dxTextBox({
        placeholder: "Ingrese el lugar donde será la capacitación aqui...",
        value: "",
    }).dxValidator({
        validationGroup: "CapacitacionGroup",
        validationRules: [{
            type: "required",
            message: "El lugar de la capacitación es obligatorio"
        }]
    }).dxTextBox("instance");

    var dpFecha = $("#dpFecha").dxDateBox({
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
        validationGroup: "CapacitacionGroup",
        validationRules: [{
            type: "required",
            message: "La fecha de la capacitación es obligatorio"
        }]
        }).dxDateBox("instance");

    var txtDuracion = $("#txtDuracion").dxNumberBox({
        placeholder: "Ingrese la duracion en minutos",
    }).dxValidator({
        validationGroup: "CapacitacionGroup",
        validationRules: [{
            type: "required",
            message: "La duración en minutos es un dato obligatorio"
        }]
        }).dxNumberBox("instance");

    var txtCapacidad = $("#txtCapacidad").dxNumberBox({
        placeholder: "Ingrese la capacidad de inscritos para la capacitación",
    }).dxValidator({
        validationGroup: "CapacitacionGroup",
        validationRules: [{
            type: "required",
            message: "La capacidad de inscritos es un dato obligatorio"
        }]
    }).dxNumberBox("instance");

    var txtResponsable = $("#txtResponsable").dxTextBox({
        placeholder: "Ingrese el responsable de la capacitacion aqui...",
        value: "",
        valueChangeEvent: "keyup",
        onValueChanged: function (data) {
            txtContacto.option("value", data.value);
        }
    }).dxTextBox("instance");

    var txtContacto = $("#txtContacto").dxTextBox({
        placeholder: "Ingrese el contacto de la capacitacion aqui...",
        value: "",
    }).dxTextBox("instance");

    var txtCorreoContacto = $("#txtCorreoContacto").dxTextBox({
        placeholder: "Ingrese el correo electrónico de contacto de la capacitacion aqui...",
        value: "",
    }).dxTextBox("instance");

    var txtUrl = $("#txtUrl").dxTextBox({
        placeholder: "Ingrese la dirección URL de la capacitación si es virtual",
        value: "",
    }).dxTextBox("instance");

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        onClick: function () {
            DevExpress.validationEngine.validateGroup("CapacitacionGroup");
            var nombre = txtNombre.option("value");
            var lugar = txtLugar.option("value");
            var fecha = dpFecha.option("value");
            var duracion = txtDuracion.option("value");
            var responsable = txtResponsable.option("value");
            var capacidad = txtCapacidad.option("value");
            var contacto = txtContacto.option("value");
            var correoContacto = txtCorreoContacto.option("value");
            var url = txtUrl.option("value");
            var params = { IdEvento: IdEvento, Evento: nombre, Lugar: lugar, Fecha: fecha, Duracion: duracion, Responsable: responsable, Capacidad: capacidad, Contacto: contacto, CorreoContacto: correoContacto, Url: url, Inscritos: 0 };
            var _Ruta = $('#SIM').data('url') + "Capacitacion/api/CapacitacionApi/GuardaCapacitacion";
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
                        $('#grdListaCapac').dxDataGrid({ dataSource: CapacitacionDataSource });
                        $("#PopupNuevaCapac").dxPopup("instance").hide();
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
        text: "Nueva Capacitación",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdEvento = -1;
            txtNombre.reset();
            txtLugar.reset();
            dpFecha.option("value", new Date());
            txtDuracion.reset();
            txtResponsable.reset();
            txtCapacidad.reset();
            txtContacto.reset();
            txtCorreoContacto.reset();
            txtUrl.reset();
            popup.show();
        }
    });

    var popup = $("#PopupNuevaCapac").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Evento de Capacitación"
    }).dxPopup("instance");

    var popupIns = $("#PopupInscritos").dxPopup({
        fullScreen: true,
        dragEnabled: false,
        height: undefined, function() { return $(window).height() * 0.8 },
        hoverStateEnabled: true,
        title: "Inscritos al Evento de Capacitación",
        onShown: function (e) {
            $("#grdListaInscritos").dxDataGrid({
                dataSource: InscritosDataSource,
                allowColumnResizing: true,
                loadPanel: { enabled: true, text: 'Cargando Datos...' },
                noDataText: "Sin datos para mostrar",
                showBorders: true,
                paging: { pageSize: 8 },
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
                "export": {
                    enabled: true,
                    fileName: 'lISTADO_INSCRITOS'
                },
                hoverStateEnabled: true,
                remoteOperations: true,
                scrolling: {
                    showScrollbar: 'always'
                },
                columns: [
                    { dataField: 'ID_PARTICIPANTE', dataType: 'number', visible: false, allowSearch: false },
                    { dataField: 'DOCUMENTO', width: '10%', caption: 'Documento', alignment: 'center' },
                    { dataField: 'NOMBRE', width: '20%', caption: 'Nombre', dataType: 'string' },
                    { dataField: 'APELLIDOS', width: '20%', caption: 'Apellidos', dataType: 'string' },
                    { dataField: 'MUNICIPIO', width: '15%', caption: 'Municipio', dataType: 'string' },
                    { dataField: 'SECTOR', width: '10%', caption: 'Sector', dataType: 'string' },
                    { dataField: 'EMPRESA', width: '10%', caption: 'Empresa', dataType: 'string' },
                    { dataField: 'TELEFONO', width: '5%', caption: 'Teléfono', dataType: 'string' },
                    { dataField: 'CORREO', width: '10%', caption: 'Correo Electrónico', dataType: 'string' },
                    {
                        alignment: 'center',
                        cellTemplate: function (container, options) {
                            $('<div/>').dxButton({
                                icon: 'clear',
                                hint: 'Retirar la persona de la capacitación',
                                onClick: function (e) {
                                    var result = DevExpress.ui.dialog.confirm('Realmente desea retirar a ' + options.data.NOMBRE + ' de esta capacitación?', 'Confirmación');
                                    result.done(function (dialogResult) {
                                        if (dialogResult) {
                                            var _Ruta = $('#SIM').data('url') + "Capacitacion/api/CapacitacionApi/QuitaInscrito";
                                            $.getJSON(_Ruta, { IdEvento: IdEvento, IdParticipante: options.data.ID_PARTICIPANTE })
                                                .done(function (data) {
                                                    if (data != null) {
                                                        grid.dxDataGrid({ dataSource: InscritosDataSource });
                                                    }
                                                }).fail(function (jqxhr, textStatus, error) {
                                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
                                                });
                                        }
                                    });
                                }
                            }).appendTo(container);
                        }
                    }
                ]
            });
        }
    }).dxPopup("instance");

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Evento de capacitación",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            return $("<div />").append(
                $("<p>Nombre de la capacitación : <span><b>" + Evento.Evento + "</b></span></p>"),
                $("<p>Lugar de realización : <span><b>" + Evento.Lugar + "</b></span></p>"),
                $("<p>Fecha : <span><b>" + Evento.Fecha + "</b></span></p>"),
                $("<p>Duración de la capcitación (Minutos) : <span><b>" + Evento.Duracion + "</b></span></p>"),
                $("<p>Responsable del evento : <span><b>" + Evento.Responsable + "</b></span></p>"),
                $("<p>Contacto del evento : <span><b>" + Evento.Contacto + "</b></span></p>"),
                $("<p>Correo del contacto : <span><b>" + Evento.CorreoContacto + "</b></span></p>"),
                $("<p>Capacidad permitida : <span><b>" + Evento.Capacidad + "</b></span></p>"),
                $("<p>Dirección Url del evento (virtual) : <span><b>" + Evento.Url + "</b></span></p>"),
                $("<br />"),
                $("<p>Total inscritos hasta la fecha : <span><b>" + Evento.Inscritos + "</b></span></p>")
            );
        }
    };
});

var CapacitacionDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FECHA","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Capacitacion/api/CapacitacionApi/ObtieneCapacitaciones', {
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
    }
});

var InscritosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"DOCUMENTO","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        $.getJSON($('#SIM').data('url') + 'Capacitacion/api/CapacitacionApi/ObtieneInscritos', {
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
            IdEvento: IdEvento
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});