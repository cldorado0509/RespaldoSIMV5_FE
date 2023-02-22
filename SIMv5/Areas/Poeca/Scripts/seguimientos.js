$(function () {

    //console.log("Seg:", seguimientos);
    //console.log("Acc:", acciones);

    var accionSeleccionada, isClonedRow = false;


    var $ddlAcciones = $("#ddlAcciones");
    $.each(accionesDisponibles, function () {
        let medidaReducida = this.medida;
        if (this.medida.length > 30) {
            medidaReducida = this.medida.substr(0, 35) + "...";
        }

        $ddlAcciones.append($("<option />").val(this.id)
            .text(`Sector: ${this.sector} - Medida: ${medidaReducida} - Acción: ${this.accion} - Nivel: ${this.nivel} - Producto: ${this.producto} - Meta: ${this.metaPropuesta} - Valoración: ${this.valoracionEconomica} - Otros: ${this.otrosRecursos}`));
    });

    let seguimientosDataSource = $.map(seguimientos, (seguimiento) => {
        let accionDeSeguimiento = acciones.find(x => x.id === seguimiento.ID_INFO_ACCION);
        let seguimientoXAccion = { ...accionDeSeguimiento, ...seguimiento };
        console.log(seguimientoXAccion);
        return seguimientoXAccion;
    });

    $("#gridSeguimientos").dxDataGrid({
        dataSource: seguimientosDataSource,
        allowColumnResizing: true,
        //loadPanel: { enabled: true, text: 'Cargando Datos...' },
        //noDataText: "Sin datos para mostrar",
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
            visible: false,
            width: 240,
            placeholder: "Buscar..."
        },
        selection: {
            mode: 'single'
        },
        editing: {
            allowDeleting: showActions,
            allowUpdating: showActions,
            useIcons: true
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'sector', width: '5%', caption: 'Sector', dataType: 'string', allowEditing: false },
            { dataField: 'medida', width: '5%', caption: 'Medida', dataType: 'string', allowEditing: false },
            { dataField: 'accion', width: '5%', caption: 'Acción', dataType: 'string', allowEditing: false },
            { dataField: 'nivel', width: '5%', caption: 'Nivel', dataType: 'string', allowEditing: false },
            { dataField: 'producto', width: '8%', caption: 'Producto', dataType: 'string', allowEditing: false },
            { dataField: 'metaPropuesta', width: '4%', caption: 'Meta', dataType: 'string', allowEditing: false },
            { dataField: 'periodicidad', width: '6%', caption: 'Periodicidad', dataType: 'string', allowEditing: false },
            { dataField: 'cargoResponsable', width: '9%', caption: 'Responsable', dataType: 'string', allowEditing: false },
            { dataField: 'valoracionEconomica', width: '9%', caption: 'Valoración ($)', dataType: 'string', allowEditing: false },
            { dataField: 'N_SEGUIMIENTO_META', width: '8%', caption: 'Seguimiento', dataType: 'string' },
            { dataField: 'N_VALORACION_ECONOMICA', width: '11%', caption: 'Recusos Invertidos ($)', dataType: 'string' },
            { dataField: 'S_OBSERVACIONES', width: '14%', caption: 'Observaciones', dataType: 'string' },
            //{
            //    type: "buttons",
            //    width: 110,
            //    buttons: ["edit", "delete", {
            //        hint: "Clone",
            //        icon: "unselectall",
            //        visible: function (event) {
            //            return !event.row.isEditing;
            //        },
            //        onClick: function (event) {
            //            var clonedItem = $.extend({}, event.row.data);

            //            seguimientosDataSource.splice(e.row.rowIndex, 0, clonedItem);
            //            event.component.refresh(true);
            //            event.event.preventDefault();
            //        }
            //    }]
            //}
        ],

        onCellPrepared: function (e) {
            if (e.rowType == "data" && e.column.command == "edit" && showActions) {
                var cellElement = e.cellElement;

                //var cloneBtn = $("<a class='dx-link dx-link-clone dx-icon-unselectall'></a>");
                //cloneBtn.on("dxclick", (args) => {
                //    var datagrid = $("#gridSeguimientos");
                //    isClonedRow = true;
                //    accionSeleccionada = e.data;
                //    datagrid.dxDataGrid("addRow");
                //});
                //cellElement.append(cloneBtn);
            }
        },

        onInitNewRow: (event) => {
            var urlParams = new URLSearchParams(window.location.search);

            event.data.sector = accionSeleccionada.sector;
            event.data.medida = accionSeleccionada.medida;
            event.data.accion = accionSeleccionada.accion;
            event.data.nivel = accionSeleccionada.nivel;
            event.data.producto = accionSeleccionada.producto;
            event.data.periodicidad = accionSeleccionada.periodicidad;
            event.data.metaPropuesta = accionSeleccionada.metaPropuesta;
            event.data.cargoResponsable = accionSeleccionada.cargoResponsable;
            event.data.valoracionEconomica = accionSeleccionada.valoracionEconomica;
            event.data.id = accionSeleccionada.id;
            event.data.ID_INFO_ACCION = accionSeleccionada.id;
            event.data.ID_EPISODIO = urlParams.get("episodio");

            if (isClonedRow) {
                event.data.N_VALORACION_ECONOMICA = accionSeleccionada.N_VALORACION_ECONOMICA;
                event.data.N_SEGUIMIENTO_META = accionSeleccionada.N_SEGUIMIENTO_META;
                event.data.S_OBSERVACIONES = accionSeleccionada.S_OBSERVACIONES;
                isClonedRow = false;
            }
        },

        onRowInserting: (event) => {
            //console.log(event.data);

            let deferred = $.Deferred();
            $.ajax({
                url: "SeguimientoPlan/Crear",
                type: "POST",
                data: JSON.stringify(event.data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (response) {
                    //alert(response.responseText);
                    deferred.reject();
                },
                success: function (response) {
                    //alert(response);
                    deferred.resolve();
                    window.location.reload();
                }
            });

            event.cancel = deferred.promise();
            
        },

        onRowUpdating: (event) => {
            //console.log(event.key, event.oldData, event.newData);

            let { id, ...data } = { ...event.oldData, ...event.newData };

            let deferred = $.Deferred();
            $.ajax({
                url: "SeguimientoPlan/Editar",
                type: "POST",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (response) {
                    //alert(response.responseText);
                    deferred.reject()
                },
                success: function (response) {
                    //alert(response);
                    deferred.resolve()
                }
            });

            event.cancel = deferred.promise();
        },

        onRowRemoving: (event) => {
            //console.log(event.data);

            let deferred = $.Deferred();

            $.ajax({
                url: "SeguimientoPlan/Eliminar/" + event.data.ID,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (response) {
                    //alert(response.responseText);
                    deferred.reject();
                },
                success: function (response) {
                    //alert(response);
                    deferred.resolve();
                }
            });

            event.cancel = deferred.promise();
        },


        onContentReady: function (e) {
            $("td.dx-command-edit[role='columnheader']").text("Acciones");
            //e.component.columnOption("command:edit", "caption", "Acciones");
        }
    });

     

    $('#btnContinuarCreacion').click(function () {
        accionSeleccionada = acciones.find(x => x.id === parseInt($ddlAcciones.val()));
        $("#gridSeguimientos").dxDataGrid("addRow");

        $('#escogeAccion').modal('hide');
    });
});

//{ dataField: 'B_SOBRE_SELLADO', width: '7%', caption: 'Sobre Sellado', dataType: 'string' },
            //{ dataField: 'D_CIERREPROPUESTAS', width: '13%', caption: 'Fecha cierre propuestas', dataType: 'date', format: 'MMM dd yyyy HH:mm' },
            //{ dataFiled: "CODFUNCIONARIO", dataType: 'number', visible: false, allowSearch: false },
            //{
            //    width: 40,
            //    alignment: 'center',
            //    cellTemplate: function (container, options) {
            //        $('<div/>').dxButton({
            //            icon: '../Content/Images/VerDetalle.png',
            //            height: 20,
            //            hint: 'Ver detalles del proceso',
            //            onClick: function (e) {
            //                var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/ObtenerProceso";
            //                $.getJSON(_Ruta, { IdProceso: options.data.ID_PROCESO })
            //                    .done(function (data) {
            //                        if (data != null) {
            //                            var OptSellado = data.SobreSellado == "1" ? "SI" : "NO";
            //                            data.PptaEconomica = data.PptaEconomica == "1" ? "SI" : "NO";
            //                            data.SobreSellado = OptSellado;
            //                            var DatoMod = data.Modalidad.split(";");
            //                            data.Modalidad = DatoMod[1];
            //                            showProceso(data);
            //                        }
            //                    }).fail(function (jqxhr, textStatus, error) {
            //                        DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
            //                    });
            //            }
            //        }).appendTo(container);
            //    }
            //},
            //{
            //    width: 40,
            //    alignment: 'center',
            //    cellTemplate: function (container, options) {
            //        if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
            //            $('<div/>').dxButton({
            //                icon: '../Content/Images/edit.png',
            //                height: 20,
            //                hint: 'Editar datos del proceso',
            //                onClick: function (e) {
            //                    var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/ObtenerProceso";
            //                    $.getJSON(_Ruta,
            //                        {
            //                            IdProceso: options.data.ID_PROCESO
            //                        }).done(function (data) {
            //                            if (data != null) {
            //                                IdRegistro = parseInt(data.IdProceso);
            //                                txtNombre.option("value", data.Nombre);
            //                                txtObjeto.option("value", data.Objeto);
            //                                var FecInicia = data.FechaInicio != null ? new Date(data.FechaInicio) : "";
            //                                dpFechaInicia.option("value", FecInicia);
            //                                var FecCierre = data.FechaCierre != "" ? new Date(data.FechaCierre) : "";
            //                                dpFechaCierre.option("min", FecInicia);
            //                                dpFechaCierre.option("value", FecCierre);
            //                                var OptSellado = data.SobreSellado == "1" ? true : false;
            //                                var OptPptaEco = data.PptaEconomica == "1" ? true : false;
            //                                chkSellado.option("value", OptSellado);
            //                                chkPptaEco.option("value", OptPptaEco);
            //                                var DatoMod = data.Modalidad.split(";");
            //                                var DataModalidad = { "IdModalidad": parseInt(DatoMod[0]), "Modalidad": DatoMod[1] };
            //                                cbModalidad.option("value", DataModalidad);
            //                                var FecApertura = data.FechaApertura != "" ? new Date(data.FechaApertura) : new Date();
            //                                dpFechaApertura.option("value", FecApertura);
            //                                if (OptSellado) dpFechaApertura.option("disabled", false);
            //                                else dpFechaApertura.option("disabled", true);
            //                                var FecAprEco = data.FechaApeEco != "" ? new Date(data.FechaApeEco) : new Date();
            //                                dpFechaAprEco.option("value", FecAprEco);
            //                                if (OptPptaEco) dpFechaAprEco.option("disabled", false);
            //                                else dpFechaAprEco.option("disabled", true);
            //                                $("#lblPropuestas").html(data.Propuestas);
            //                                popup.show();
            //                            }
            //                        }).fail(function (jqxhr, textStatus, error) {
            //                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
            //                        });
            //                }
            //            }).appendTo(container);
            //        }
            //    }
            //},
            //{
            //    width: 40,
            //    alignment: 'center',
            //    cellTemplate: function (container, options) {
            //        if (options.data.CODFUNCIONARIO == CodigoFuncionario) {
            //            $('<div/>').dxButton({
            //                icon: '../Content/Images/Delete.png',
            //                height: 20,
            //                hint: 'Eliminar el proceso',
            //                onClick: function (e) {
            //                    var result = DevExpress.ui.dialog.confirm('Desea eliminar el proceso ' + options.data.S_NOMBRE + '?', 'Confirmación');
            //                    result.done(function (dialogResult) {
            //                        if (dialogResult) {
            //                            var _Ruta = $('#SIM').data('url') + "Contratos/api/PropuestaApi/EliminaProceso";
            //                            $.getJSON(_Ruta,
            //                                {
            //                                    objData: options.data.ID_PROCESO
            //                                }).done(function (data) {
            //                                    if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar proceso');
            //                                    else {
            //                                        $('#GidListado').dxDataGrid({ dataSource: ProcesosDataSource });
            //                                    }
            //                                }).fail(function (jqxhr, textStatus, error) {
            //                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar proceso');
            //                                });
            //                        }
            //                    });
            //                }
            //            }).appendTo(container);
            //        }
            //    }
            //}