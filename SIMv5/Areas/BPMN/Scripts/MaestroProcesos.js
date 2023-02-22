$(document).ready(function () {
    var IdRegistro = -1;
    var Proceso = null;
    var VersionProcesos = null;


    $("#GidListado").dxDataGrid({
        dataSource: ProcesosDataSource,
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
            { dataField: 'procesoId', width: '5%', caption: 'ID', alignment: 'center' },
            { dataField: 'nombreProceso', width: '30%', caption: 'Nombre', dataType: 'string' },
            { dataField: 'descripcion', width: '30%', caption: 'Descripcion', dataType: 'string' },
            { dataField: 'habilitado', caption: 'Habilitado', dataType: 'bool' },
            { dataField: 'versionNro', caption: 'Version', dataType: 'string' },
            { dataField: 'fechaInicioEstado', caption: 'Fecha Creacion', dataType: 'date', format: 'MMM dd yyyy' },
            //{ dataField: 'nombreEstado', caption="Estado", visible: true, allowSearch: true },
              {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: '../Content/Images/VerDetalle.png',
                        height: 20,
                        hint: 'Ver detalles del proceso',
                        onClick: function (e) {
                            var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/ObtenerVersionesProceso?idProceso=" + options.data.procesoId
                            $.getJSON(_Ruta)
                                .done(function (data) {
                                    if (data != null) {
                                        showProceso(options.data, data);
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
                    /*if (options.data.CODFUNCIONARIO == CodigoFuncionario) {*/
                    if (true) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/edit.png',
                            height: 20,
                            hint: 'Editar datos del proceso',
                            onClick: function (e) {
                                IdRegistro = parseInt(options.IdProceso);
                                txtNombre.option("value", options.data.nombreProceso);
                                txtDescripcion.option("value", options.data.descripcion);
                                console.log(options.data)
                            //    var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/ObtenerProceso";
                            //    $.getJSON(_Ruta,
                            //        {
                            //            IdProceso: options.data.ID_PROCESO
                            //        }).done(function (data) {
                            //            if (data != null) {
                                            
                            //                popup.show();
                            //            }
                            //        }).fail(function (jqxhr, textStatus, error) {
                            //            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
                            //        });
                                popupEditProceso.show()
                            }
                        }).appendTo(container);
                    }
                }
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/delete.png',
                            height: 20,
                            hint: 'Eliminar el proceso',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea eliminar el proceso ' + options.data.S_NOMBRE + '?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/EliminaProceso";
                                        $.getJSON(_Ruta,
                                            {
                                                objData: options.data.ID_PROCESO
                                            }).done(function (data) {
                                                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar proceso');
                                                else {
                                                    $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
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
            },
            {
                width: 40,
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
                        $('<div/>').dxButton({
                            icon: '../Content/Images/Enviar_Doc.png',
                            height: 20,
                            hint: 'Editar datos del proceso',
                            onClick: function (e) {
                                var result = DevExpress.ui.dialog.confirm('Desea registrar en Gestión Documental del SIM todos los documentos abiertos de este proceso?', 'Confirmación');
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        loadPanel.show();
                                        var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/RegistraPropuestas";
                                        $.getJSON(_Ruta, { IdProceso: options.data.ID_PROCESO })
                                            .done(function (data) {
                                                loadPanel.hide();
                                                if (data.RegistroCorrecto) {
                                                    DevExpress.ui.dialog.alert(data.Mensaje, 'Subir Propuestas');
                                                    $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                                                } else {
                                                    DevExpress.ui.dialog.alert('Ocurrió un error : ' + data.Mensaje, 'Subir Propuestas');
                                                }
                                            }).fail(function (jqxhr, textStatus, error) {
                                                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Subir Propuestas');
                                            });
                                    }
                                });
                            }
                        }).appendTo(container);
                    }
                }
            }
        ]
    });

    var popupDet = null;

    var showProceso = function (data, versiones) {
        Proceso = data;
        VersionProcesos = versiones

        if (popupDet) {
            popupDet.option("contentTemplate", popupOptions.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleProceso").dxPopup(popupOptions).dxPopup("instance");
        }
        popupDet.show();
    };

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre del proceso aqui...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "El nombre del proceso es obligatorio"
        }]
    }).dxTextBox("instance");

    var txtDescripcion = $("#txtDescripcion").dxTextArea({
        value: "",
        height: 90
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La descripcion del proceso es obligatorio"
        }]
    }).dxTextArea("instance");


    //var dpFechaCreacion = $("#dpFechaCreacion").dxDateBox({
    //    type: "datetime",
    //    value: new Date(),
    //    showAnalogClock: false,
    //    readOnly: true,
    //    onOpened: function (args) {
    //        let position = args.component._popup.option("position");
    //        position.my = "center";
    //        position.at = "left";
    //        args.component._popup.option("position", position);
    //    }
    //}).dxDateBox("instance");


    var dpFechaModificacion = $("#dpFechaModificacion").dxDateBox({
        type: "datetime",
     //   value: new Date(),
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
            message: "La fecha de inicio de recepción de propuestas es obligatorio"
        }]
    }).dxDateBox("instance");

  

    var chkHabilitado = $("#chkHabilitado").dxCheckBox({
        value: true,
        width: 80
     //   text: "Si",
        //onValueChanged: function (data) {
        //    if (data.value) {
        //        chkPptaEco.option("disabled", false);
        //        dpFechaApertura.option("disabled", false);
        //    } else {
        //        chkPptaEco.option("disabled", true);
        //        chkPptaEco.option("value", false);
        //        dpFechaApertura.option("disabled", true);
        //    }
        //}
    }).dxCheckBox("instance");

    var txtVersion = $("#txtVersion").dxTextBox({
      //  placeholder: "Ingrese el nombre del proceso aqui...",
        value: "",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "La version del proceso es obligatorio"
        }]
    }).dxTextBox("instance");

    var popupEditProceso = $("#PopupEditProceso").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Editar Proceso BPMN"
    }).dxPopup("instance");

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Detalle Proceso BPMN",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            return $("<div />").append(
                $(`<div class="text-center">` + Proceso.nombreProceso + `<br>` + Proceso.descripcion + `</div> `),
                $(prepararVresiones(VersionProcesos)),
                $("<div />").attr("id", "buttonContainer").css("visibility", Proceso.acualizaVersion != "N" ? "visible" : "hidden").dxButton({
                    text: "Crear Nueva Version",
                    onClick: function (e) {
                        var params = { FuncionarioId: CodigoFuncionario, ProcesoId: Proceso.procesoId, Nombre: '', Descripcion: '', Habilitado: '1', Version: 1 };
                        var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/InsertarNuevaVersion"
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
                                    $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                                    $("#PopupEditProceso").dxPopup("instance").hide();

                                    showProceso(Proceso, data);
                                }
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                            }
                        })
                    }
                }),

            );
        }
    };

    $("#btnEditar").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var nombre = txtNombre.option("value");
            var descripcion = txtDescripcion.option("value");
            var habilitado = chkHabilitado.option("value");
            var version = txtVersion.option("value");
            var params = { FuncionarioId: CodigoFuncionario, IdProceso: IdRegistro, Nombre: nombre, Descripcion: descripcion, Habilitado: habilitado ? "1" : "0", Version: version, FechaCreacion: '' };

            var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/CrearProcesos"
            console.log(_Ruta)
            console.log(JSON.stringify(params))

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
                        $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                        $("#PopupNuevoProceso").dxPopup("instance").hide();

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
            //  }
        }
    });

    $("#btnGuarda").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var nombre = txtNombre.option("value");
            var descripcion = txtDescripcion.option("value");
            var habilitado = chkHabilitado.option("value");
            var version = txtVersion.option("value");
            var params = { FuncionarioId: CodigoFuncionario, IdProceso: IdRegistro, Nombre: nombre, Descripcion: descripcion, Habilitado: habilitado ? "1" : "0", Version: version, FechaCreacion: '' };

            var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/CrearProcesos"
            console.log(_Ruta)
            console.log(JSON.stringify(params))

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
                        $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                        $("#PopupNuevoProceso").dxPopup("instance").hide();

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
            //  }
        }
    });


    var popup = $("#PopupNuevoProceso").dxPopup({
        width: 900,
        height: "auto",
        hoverStateEnabled: true,
        title: "Crear Nuevo Proceso BPMN"
    }).dxPopup("instance");

    var popupOptions = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Detalle Proceso BPMN",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            return $("<div />").append(
                $(`<div class="text-center">` + Proceso.nombreProceso + `<br>` + Proceso.descripcion + `</div> `),
                $(prepararVresiones(VersionProcesos)),
                $("<div />").attr("id", "buttonContainer").css("visibility", Proceso.acualizaVersion != "N" ? "visible" : "hidden").dxButton({
                    text: "Crear Nueva Version",
                    onClick: function (e) {
                        var params = { FuncionarioId: CodigoFuncionario, ProcesoId: Proceso.procesoId, Nombre: '', Descripcion: '', Habilitado:'1', Version: 1 };
                        var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/InsertarNuevaVersion"
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
                                    $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
                                    $("#PopupNuevoProceso").dxPopup("instance").hide();

                                    showProceso(Proceso, data);
                                }
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                            }
                        })
                    }
                }),
            
            );
        }
    };

    var prepararVresiones = function (versiones) {
        console.log(versiones)
        var elementos = ''
        for (var i = 0; i < versiones.length; i++) {
            elementos = elementos + '<tr>'
                elementos = elementos + '<th scope="row">' + versiones[i].versionNro + '</th>'
            elementos = elementos + '<td>' + versiones[i].nombreEstado + '</td>'
                elementos = elementos + '<td>' + versiones[i].fechaInicioEstado + '</td>'
            elementos = elementos + `<td><a href="/SIM/BPMN/ProcesosxArea/Index?procesoid=${versiones[i].procesoVersionId}&procesoestadoid=${versiones[i].procesoEstadoVersionId}">Detalle del Proceso</a></td>`
            elementos = elementos + '</tr>'
        }
        return `<table class="table">
                  <thead>
                    <tr>
                      <th scope="col">Version</th>
                      <th scope="col">Estado</th>
                      <th scope="col">Fecha Inicio</th>
                      <th scope="col">Ir al Modelo</th>
                    </tr>
                  </thead>
                  <tbody>${elementos}</tr>
                  </tbody>
                </table>
`
    }

    $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo Proceso",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtNombre.reset();
            txtDescripcion.reset();
            chkHabilitado.option("value", true);
            txtVersion.option("value", "1.0");
            //dpFechaCreacion.option("value", new Date());
            popup.show();
        }
    });

    $("#btnEditar").dxButton({
        stylingMode: "contained",
        text: "Editar Proceso",
        type: "success",
        width: 200,
        height: 30,
        icon: '../Content/Images/new.png',
        onClick: function () {
            IdRegistro = -1;
            txtNombre.reset();
            txtDescripcion.reset();
            chkHabilitado.option("value", true);
            txtVersion.option("value", "1.0");
            //dpFechaCreacion.option("value", new Date());
            popup.show();
        }
    });

    var loadPanel = $("#LoadingPanel").dxLoadPanel({
        shadingColor: "rgba(0,0,0,0.4)",
        visible: false,
        showIndicator: true,
        showPane: true,
        shading: true,
        closeOnOutsideClick: false
    }).dxLoadPanel("instance");
});


var ProcesosDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = (typeof loadOptions.skip != 'undefined' && loadOptions.skip != null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take != 'undefined' && loadOptions.take != null ? loadOptions.take : 0);
        var _Ruta = "https://sim.metropol.gov.co/BPMN/api/Procesos/ObtenerProcesos?habilitado=true"
        //$.getJSON(_Ruta, { ID: options.data.ID })
        
        $.getJSON(_Ruta,  {
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
            CodFuncionario: CodigoFuncionario
        }).done(function (data) {
            d.resolve(data, { totalCount:data.length });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});