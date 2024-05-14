var idRecord = -1;
var _familiaId = 0;
var _departamentoId = 0;
var _historiaId = 0;
var _consecutivoActa = "";
$(document).ready(function () {


    $('#asistente').accordion({
        collapsible: true,
        animationDuration: 500,
        multiple: false,
    });

    $("#btnAddRow").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            appendRow();
        }
    }).dxButton("instance");

    $("#GridListado").dxDataGrid({
        dataSource: gridDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        groupPanel: {
            visible: false,
            emptyPanelText : "Arrastre una columna acá!"
        },
        export: {
            enabled: true,
            allowExportSelectedData: true,
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
            { dataField: 'RegistroCAVId', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'ActoAdministrativo', width: '10%', caption: 'Consecutivo de acta', alignment: 'center' },
            { dataField: 'NumeroIdentificacion', width: '10%', caption: 'Nro Identificación', alignment: 'center' },
            { dataField: 'NombreComun', width: '10%', caption: 'Nombre Cómun' },
            { dataField: 'NombreCientifico', width: '10%', caption: 'Nombre Científico' },
            { dataField: 'TipoEstadoIndividuo', width: '10%', caption: 'Estado del Individuo' },
            { dataField: 'FechaLLegada', width: '5%', caption: 'Fecha Ingreso', dataType: 'date' },
            { dataField: 'Sexo', width: '10%', caption: 'Sexo' },
            { dataField: 'Activo', width: '5%', caption: 'Activo', dataType: 'boolean', alignment: 'center' },
            {
                visible: canEdit,
                width: 60,
                caption: "Editar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 30,
                        width: 30,
                        hint: 'Editar la información relacionada con el registro seleccionado',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GetIngresoCAVAsync";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.RegistroCAVId
                                }).done(function (data) {
                                    if (data !== null) {
                                        idRecord = data.RegistroCAVId;
                                        _consecutivoActa = data.ActoAdministrativo;
                                        txtActoAdmin.option("value", data.ActoAdministrativo);
                                        txtNombreEntrega.option("value", data.Tenedor);
                                        txtCedulaEntrega.option("value", data.IdentificacionTenedor);
                                        txtDireccionEntrega.option("value", data.DireccionTenedor);
                                        txtTelefonoEntrega.option("value", data.TelefonoTenedor);
                                        txtObservacion.option("value", data.Observacion);
                                        if (data.optSensiblizacion === 'S') {
                                            optSensiblizacion.option("value", "Si");
                                        }
                                        else {
                                            optSensiblizacion.option("value", "No");
                                        }
                                        txtDireccionProcedencia.option("value", data.DireccionProcedencia);
                                        txtBarrioProcedencia.option("value", data.BarrioProcedencia);
                                        cboDepartamentoProcedencia.option("value", data.CodigoDepartamento);
                                        cboMunicipioProcedencia.option("value", data.CodigoMunicipio);
                                        txtLongitud.option("value", data.Longitud);
                                        txtLatitud.option("value", data.Latitud);
                                        txtAltidud.option("value", data.Altitud);
                                        popupRegistro.show();
                                        eliminarTabla();
                                        appendRow();

                                        var idFamilia = "cboFamilia3";
                                        var idEspecie = "cboEspecie3";
                                        var idNumero = "txtNumero3";
                                        var idCantidad = "txtCantidad3";
                                        var idPrefijo = "txtPrefijo3";
                                        var idBtnEliminar = "btnEliminar3";
                                        var cbofamilia = $("#" + idFamilia).dxSelectBox("instance");
                                        var cboEspecieR = $("#" + idEspecie).dxSelectBox("instance");
                                        var txtCantidad = $("#" + idCantidad).dxNumberBox("instance");
                                        var txtPrefijo = $("#" + idPrefijo).dxTextBox("instance");
                                        var btnEliminarRow = $("#" + idBtnEliminar).dxButton("instance");

                                        cbofamilia.option("value", data.FamiliaFaunaId);
                                        cboEspecieR.option("value", data.EspecieFaunaId);
                                        var txtNroIdentificacion = $("#" + idNumero).dxTextBox("instance");
                                        txtNroIdentificacion.option("value", data.NumeroIdentificacion);
                                        txtCantidad.option("readOnly", true);
                                        txtPrefijo.option("readOnly", true);
                                        btnEliminarRow.option("disabled", true);

                                        txtNombreFuncionario.option("value", data.FuncionarioResponsable);
                                        txtCedulaFuncionario.option("value", data.CedulaProcedimiento);
                                        cboEntidadProcedimiento.option("value", data.CodigoAutoridadRemision);
                                        txtNombreResponsable.option("value", data.FuncionarioResponsable);
                                        txtCedulaResponsable.option("value", data.CedulaResponsable);
                                        txtCargoResponsable.option("value", data.CargoFuncionarioResponsable);
                                        txtNombreConstancia.option("value", data.FuncionarioConstanciaCAV);
                                        txtCedulaConstancia.option("value", data.CedulaFuncionarioConstanciaCAV);
                                        txtCargoConstancia.option("value", data.CargoFuncionarioConstancia);


                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canDelete,
                width: 60,
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 30,
                        width: 30,
                        hint: 'Eliminar el registro seleccionado',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/EliminarTipoAdquisicionAsync?Id=" + options.data.RegistroCAVId;
                                    $.ajax({
                                        type: 'DELETE',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'text',
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: gridDataSource });
                                                DevExpress.ui.dialog.alert('Registro eliminado correctamente!');

                                            }
                                        },
                                        error: function (xhr, textStatus, errorThrown) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                        }
                                    });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canRead,
                width: 60,
                caption: "Historia Clínica",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'folder',
                        height: 30,
                        width: 30,
                        hint: 'Historia Clínica del Individuo',
                        onClick: function (e) {
                            popupHistoria.show();
                            txtNumeroIdentificacion.option("value", options.data.NumeroIdentificacion);
                            txtNombreComunEspecie.option("value", options.data.NombreComun);
                            txtNombreCientificoEspecie.option("value", options.data.NombreCientifico);
                            txtFechaLlegadaIndividuo.option("value", options.data.FechaLLegada);
                            txtSexoIndividuo.option("value", options.data.Sexo);

                            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GetHistoriaClinicaIndividuoAsync?Id=" + options.data.RegistroCAVId;
                            $.getJSON(_Ruta,
                             {
                                 Id: options.data.RegistroCAVId
                             }).done(function (data) {
                             if (data !== null) {
                                 _historiaId = data.historiaId;
                                 if (_historiaId > 0) {
                                     txtObservacionHistoria.option("value", data.observacion);
                                     txtEstudiante.option("value", data.estudiante);
                                     cboFuncionarioResponsable.option("value", data.funcionarioResponsable);
                                     $("#btnNuevoExamen").dxButton("instance").option("visible", true);
                                 }
                                 else{
                                     txtObservacionHistoria.option("value", "");
                                     txtEstudiante.option("value", "");
                                     $("#btnNuevoExamen").dxButton("instance").option("visible", false);
                                  }
                                 $('#gridExamenes').dxDataGrid({ dataSource: gridExamenesDataSource });
                             }
                            }).fail(function (jqxhr, textStatus, error) {
                              DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                            });
                        }
                    }).appendTo(container);
                }
            },
           
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                idRecord = data.RegistroCAVId;
                _consecutivoActa = data.actoAdministrativo;

              
            }
        }
    });
    
    $("#gridExamenes").dxDataGrid({
        dataSource: gridExamenesDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        groupPanel: {
            visible: false,
            emptyPanelText: "Arrastre una columna acá!"
        },
        export: {
            enabled: true,
            allowExportSelectedData: true,
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
            { dataField: 'ExamenLaboratorioId', width: '5%', caption: 'Id', alignment: 'center' },
            { dataField: 'NumeroPrueba', width: '10%', caption: 'Número de Prueba', alignment: 'center' },
            { dataField: 'Fecha', width: '5%', caption: 'Fecha', dataType: 'date' },
            { dataField: 'Observacion', width: '10%', caption: 'Observación' },
            {
                visible: canEdit,
                width: 60,
                caption: "Editar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        caption: "Editar",
                        height: 30,
                        width: 30,
                        hint: 'Editar la información relacionada con el registro seleccionado',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GetIngresoCAVAsync";
                            $.getJSON(_Ruta,
                                {
                                    Id: options.data.RegistroCAVId
                                }).done(function (data) {
                                    if (data !== null) {
                                        idRecord = data.RegistroCAVId;
                                       
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Evento no esperado!');
                                });
                        }
                    }).appendTo(container);
                }
            },
            {
                visible: canDelete,
                width: 60,
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        height: 30,
                        width: 30,
                        hint: 'Eliminar el registro seleccionado',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/EliminarTipoAdquisicionAsync?Id=" + options.data.RegistroCAVId;
                                    $.ajax({
                                        type: 'DELETE',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'text',
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Eliminar registro seleccionado');
                                            else {
                                                $('#GidListado').dxDataGrid({ dataSource: gridDataSource });
                                                DevExpress.ui.dialog.alert('Registro eliminado correctamente!');

                                            }
                                        },
                                        error: function (xhr, textStatus, errorThrown) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Eliminar registro seleccionado');
                                        }
                                    });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            }
        ],
        onSelectionChanged: function (selectedItems) {
            var data = selectedItems.selectedRowsData[0];
            if (data) {
                idRecord = data.RegistroCAVId;
                _consecutivoActa = data.actoAdministrativo;
            }
        }
    });
        
    $("#btnGuardarIngresoCAV").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {

            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = idRecord;
            var actoAdmin = txtActoAdmin.option("value");
            var especieFaunaId = 0;
            var nombreComun = "";
            var nombreCientifico = "";
            var tipoEstadoIndividuoId = 0;
            var direccionProcedencia = txtDireccionProcedencia.option("value");
            var barrioProcedencia = txtBarrioProcedencia.option("value");
            var codigoMunicipioProcedencia = cboMunicipioProcedencia.option("value");
            var longitud = txtLongitud.option("value");
            var latitud = txtLatitud.option("value");
            var altitud = txtAltidud.option("value");
            var nombreTenedor = txtNombreEntrega.option("value");
            var cedulaTenedor = txtCedulaEntrega.option("value");
            var direccionTenedor = txtDireccionEntrega.option("value");
            var telefonoTenedor = txtTelefonoEntrega.option("value");
            var declaracion = txtObservacionEntrega.option("value"); 
            var entidadProcedimientoId = cboEntidadProcedimiento.option("value");
            var funcionarioProcedimiento = txtNombreFuncionario.option("value");
            var cedulaFuncionarioProcedimiento = txtCedulaFuncionario.option("value");
            var funcionarioResponsable = txtNombreResponsable.option("value"); 
            var cedulaFuncionarioResponsable = txtCedulaResponsable.option("value");
            var cargoFuncionarioResponsable = txtCargoResponsable.option("value");

            var funcionarioConstanciaCAV = txtNombreConstancia.option("value");
            var cedulaFuncionarioConstanciaCAV = txtCedulaConstancia.option("value");
            var cargoFuncionarioConstanciaCAV = txtCargoConstancia.option("value");

            var fechaEntrega = txtFechaProcedencia.option("value");
            var horaEntrega = txtHoraProcedencia.option("value");
            var sensibilización = optSensiblizacion.option("value");
            var observacion = txtObservacion.option("value");
            
            if (actoAdmin === "") return;

            var jsonObj = [];

            var tbl = document.getElementById('tblIndividuos');
            var totalr = tbl.rows.length;

            for (var i = 0; i < tbl.rows.length - 1; i++) {
                var item = {};
                
                var oCells = tbl.rows[i].cells;
                var cellLength = oCells.length;
                var tot = totalr + i;
                var idFamilia = "cboFamilia" + tot; 
                var idEspecie = "cboEspecie" + tot;
                var idPrefijo = "txtPrefijo" + tot;
                var idNumero = "txtNumero" + tot;
                var idCantidad = "txtCantidad" + tot;
                var idTiempoCautiverio = "cboTiempoCautiverio" + tot;
                var cbofamilia = $("#" + idFamilia).dxSelectBox("instance");
                var cboEspecieR = $("#" + idEspecie).dxSelectBox("instance");
                var txtPrefijo = $("#" + idPrefijo).dxTextBox("instance");
                var txtNroIdentificacion = $("#" + idNumero).dxTextBox("instance");
                var txtCantidad = $("#" + idCantidad).dxNumberBox("instance");
                var cboTiempoCautiverio = $("#" + idTiempoCautiverio).dxSelectBox("instance");

                var isEditing = false;
                if (id > 0 && txtCantidad.option("readOnly") === true) {
                    isEditing = true;
                }
                item["isEdit"] = isEditing;
                item["registroCAVId"] = id;
                item["actoAdministrativo"] = actoAdmin;
                item["direccionProcedencia"] = direccionProcedencia;
                item["barrioProcedencia"] = barrioProcedencia;
                item["codigoMunicipio"] = codigoMunicipioProcedencia;
                item["longitud"] = longitud;
                item["latitud"] = latitud;
                item["altitud"] = altitud;
                item["nombreComun"] = nombreComun;
                item["nombreCientifico"] = nombreCientifico;
                item["especieFaunaId"] = cboEspecieR.option("value");
                item["prefijo"] = txtPrefijo.option("value");
                item["numeroIdentificacion"] = txtNroIdentificacion.option("value");
                item["cantidad"] = txtCantidad.option("value");
                item["tiempoId"] = cboTiempoCautiverio.option("value");
                item["tenedor"] = nombreTenedor;
                item["identificacionTenedor"] = cedulaTenedor;
                item["direccionTenedor"] = direccionTenedor;
                item["telefonoTenedor"] = telefonoTenedor;
                item["declaracion"] = declaracion;
                item["codigoAutoridadRemision"] = entidadProcedimientoId;
                item["funcionarioProcedimiento"] = funcionarioProcedimiento;
                item["cedulaProcedimiento"] = cedulaFuncionarioProcedimiento;
                item["funcionarioResponsable"] = funcionarioResponsable;
                item["cedulaResponsable"] = cedulaFuncionarioResponsable;
                item["cargoFuncionarioResponsable"] = cargoFuncionarioResponsable;
                item["funcionarioConstanciaCAV"] = funcionarioConstanciaCAV;
                item["cedulaFuncionarioConstanciaCAV"] = cedulaFuncionarioConstanciaCAV;
                item["cargoFuncionarioConstancia"] = cargoFuncionarioConstanciaCAV;
                item["fechaEntrega"] = fechaEntrega;
                item["horaEntrega"] = horaEntrega;
                item["sensibilización"] = sensibilización;
                item["observacion"] = observacion;

                jsonObj.push(item);
            }

            var params =
            {
                "registroCAVs": jsonObj
            };

            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GuardarLoteRegistrosIngresoCAVAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.IsSuccess === false && data.Resul === '') return;
                    else {
                        DevExpress.ui.dialog.alert('Expediente Ambiental Creado/Actualizado correctamente con el CM:' + data.Result.IdGenerated, 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: ExpedientesDataSource });
                        $('#popupNuevoExpediente').dxPopup("instance").hide();
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });


            $('#GridListadoIndividuos').dxDataGrid({ dataSource: gridIndividuosDataSource });
            popupRegistro.hide();
        }
    }).dxButton("instance");

    $("#btnNuevoExamen").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            popupExamen.show();
            appendRow();
        }
    }).dxButton("instance");
         
    var txtActoAdmin = $("#txtActoAdmin").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");

    var txtDireccionProcedencia = $("#txtDireccionProcedencia").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");

    var txtBarrioProcedencia = $("#txtBarrioProcedencia").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");

    var txtFechaProcedencia = $("#txtFechaProcedencia").dxDateBox({
        type: "date",
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
            message: "Debe ingresar la fecha de la recepción de los individuos"
        }]
    }).dxDateBox("instance");

    var txtHoraProcedencia = $("#txtHoraProcedencia").dxDateBox({
        type: "time",
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
            message: "Debe ingresar la fecha de la recepción de los individuos"
        }]
    }).dxDateBox("instance");

    var txtLongitud = $("#txtLongitud").dxNumberBox({
        format: '##.###### Coordenada X',
        value: -75.000000,
        min: -76,
        max: -75
    }).dxNumberBox("instance");

    var txtLatitud = $("#txtLatitud").dxNumberBox({
        format: '#.###### Coordenada Y',
        value: 6.000000,
        min: 6,
        max: 7,
    }).dxNumberBox("instance");

    var txtAltidud = $("#txtAltidud").dxNumberBox({
    }).dxNumberBox("instance");

    var cboDepartamentoProcedencia = $("#cboDepartamentoProcedencia").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "departamentoId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerDepartamentosAsync");
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboMunicipioDs = cboMunicipioProcedencia.getDataSource();
                _departamentoId = data.value;
                cboMunicipioDs.reload();
                cboMunicipioProcedencia.option("value", null);
            }
        },
        displayExpr: "nombre",
        valueExpr: "departamentoId",
  
        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar la Familia!"
        }]
    }).dxSelectBox("instance");

    var cboMunicipioProcedencia = $("#cboMunicipioProcedencia").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "municipioId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerMunicipiosAsync?idDepartamento=" + _departamentoId);
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "municipioId",
        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el municipio!"
        }]
    }).dxSelectBox("instance");


    var txtNombreEntrega = $("#txtNombreEntrega").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el nombre de la persona que realiza la entraga!"
        }]
    }).dxTextBox("instance");

    var txtDireccionEntrega = $("#txtDireccionEntrega").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar la dirección de residencia de la persona que realiza la entraga!"
        }]
    }).dxTextBox("instance");

    var txtCedulaEntrega = $("#txtCedulaEntrega").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el número de documento de identidad de la persona que realiza la entraga!"
        }]
    }).dxTextBox("instance");

    var txtTelefonoEntrega = $("#txtTelefonoEntrega").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");
    
    var txtObservacionEntrega = $("#txtObservacionEntrega").dxTextArea({
        value: "",
        readOnly: false,
        height: 180
    }).dxTextArea("instance");

    const priorities = ['Si', 'No'];
    var optSensiblizacion = $('#optSensiblizacion').dxRadioGroup({
        items: priorities,
        value: priorities[1],
        layout: 'horizontal',
    }).dxRadioGroup("instance");
        
    var txtNombreFuncionario = $("#txtNombreFuncionario").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el nombre del Funcionario de la Autoridad Ambiental/Fuerza Pública que realiza el Procedimiento!"
        }]
    }).dxTextBox("instance");

    var txtCedulaFuncionario = $("#txtCedulaFuncionario").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar la cédula del Funcionario de la Autoridad Ambiental/Fuerza Pública que realiza el Procedimiento!"
        }]
    }).dxTextBox("instance");


    var cboEntidadProcedimiento = $("#cboEntidadProcedimiento").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "autoridadId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerAutoridadesAsync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "autoridadId",
        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el municipio!"
        }]
    }).dxSelectBox("instance");

    var txtNombreResponsable = $("#txtNombreResponsable").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el nombre del Responsable emergencias/Estación de paso"
        }]
    }).dxTextBox("instance");

    var txtCedulaResponsable = $("#txtCedulaResponsable").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar la cédula del Responsable emergencias/Estación de paso!"
        }]
    }).dxTextBox("instance");

    var txtCargoResponsable = $("#txtCargoResponsable").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");

    var txtNombreConstancia = $("#txtNombreConstancia").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator ({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el nombre del Responsable emergencias/Estación de paso"
        }]
    }).dxTextBox("instance");

    var txtCedulaConstancia = $("#txtCedulaConstancia").dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar la cédula del Responsable emergencias/Estación de paso!"
        }]
    }).dxTextBox("instance");

    var txtCargoConstancia = $("#txtCargoConstancia").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");
    
    var cboDisposicionIndividuo = $("#cboDisposicionIndividuo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "tipoDestinoId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerDisposicionesIndividuosAsync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "tipoDestinoId",

        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar la Familia!"
        }]
    }).dxSelectBox("instance");

    var txtObservacion = $("#txtObservacion").dxTextArea({
        value: "",
        readOnly: false,
        height: 100
    }).dxTextArea("instance");
    
    var txtNumeroIdentificacion = $("#txtNumeroIdentificacion").dxTextBox({
        value: "",
        readOnly: true,
    }).dxTextBox("instance");

    var txtNombreComunEspecie = $("#txtNombreComunEspecie").dxTextBox({
        value: "",
        readOnly: true,
    }).dxTextBox("instance");

    var txtNombreCientificoEspecie = $("#txtNombreCientificoEspecie").dxTextBox({
        value: "",
        readOnly: true,
    }).dxTextBox("instance");

    var txtFechaLlegadaIndividuo = $("#txtFechaLlegadaIndividuo").dxTextBox({
        value: "",
        readOnly: true,
    }).dxTextBox("instance");

    var txtSexoIndividuo = $("#txtSexoIndividuo").dxTextBox({
        value: "",
        readOnly: true,
    }).dxTextBox("instance");

    var txtObservacionHistoria = $("#txtObservacionHistoria").dxTextArea({
        value: "",
        readOnly: false,
        height: 60
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar alguna observación relacionada con la Historia Clínica!"
        }]
    }).dxTextArea("instance");

    var txtEstudiante = $("#txtEstudiante").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");

    var cboFuncionarioResponsable = $("#cboFuncionarioResponsable").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idAbogado",
                loadMode: "raw",
                load: function () {
                    var datos = $.getJSON($("#SIM").data("url") + "ExpedienteAmbiental/api/ExpedientesAmbApi/ObtenerAbogadosAsync");
                    return datos;
                }
            })
        }),
        displayExpr: "nombre",
        searchEnabled: true,
        valueExpr: "idAbogado",
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el Abogado!"
        }]
    }).dxSelectBox("instance");


    var txtFechaExamen = $("#txtFechaExamen").dxDateBox({
        value: "",
        readOnly: false,
    }).dxDateBox("instance");

    var txtObservacionesExamen = $("#txtObservacionesExamen").dxTextArea({
        value: "",
        readOnly: false,
        height: 100
    }).dxTextArea("instance");

    var txtNroExamen = $("#txtNroExamen").dxTextBox({
        value: "",
        readOnly: false,
    }).dxTextBox("instance");
    
    var btnGuardarHistoria = $("#btnGuardarHistoria").dxButton({
        text: "Guardar",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            DevExpress.validationEngine.validateGroup("ProcesoGroup");
            var id = idRecord;

            var observacionHistoria = txtObservacionHistoria.option("value");
            if (observacionHistoria === "") return;

            var funcionarioResponsableId = cboFuncionarioResponsable.option("value");
            var estudiante = txtEstudiante.option("value");

            var params = {
                historiaId: _historiaId, registroCAVId: id, observacion: observacionHistoria, funcionarioResponsable: funcionarioResponsableId, estudiante: estudiante
            };

            var _Ruta = $('#SIM').data('url') + "CAV/api/CAVApi/GuardarHistoriaAsync";
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.IsSuccess === false) DevExpress.ui.dialog.alert('Ocurrió un error ' + data.Message, 'Guardar Datos');
                    else {
                        DevExpress.ui.dialog.alert('Historian Creada/Actualizada correctamente', 'Guardar Datos');
                        $('#gridExamenes').dxDataGrid({ dataSource: gridExamenesDataSource });
                        $("#btnNuevoExamen").dxButton("instance").option("visible", true);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });
        }
    }).dxButton("instance");

    var popupRegistro = $("#popupRegistro").dxPopup({
        width: 1300,
        height: 800,
        dragEnabled: true,
        resizeEnabled: true,
        hoverStateEnabled: true,
        title: "Formato Recepción Fauna Silvestre"
    }).dxPopup("instance");

    var popupHistoria = $("#popupHistoria").dxPopup({
        width: 1400,
        height: 800,
        fullScreen : true,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Historia Clínica del individuo"
    }).dxPopup("instance");

    var popupEntrega = $("#popupEntrega").dxPopup({
        width: 1400,
        height: 800,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Información de quién entrega"
    }).dxPopup("instance");

    var popupEspecie = $("#popupEspecie").dxPopup({
        width: 1400,
        height: 800,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Información de la Fauna"
    }).dxPopup("instance");

    var popupExamen = $("#popupExamen").dxPopup({
        width: 1400,
        height: 800,
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Exámen de Laboratorio"
    }).dxPopup("instance");
    
    $("#btnNuevoIngresoCAV").dxButton({
        text: "Nuevo",
        type: "success",
        height: 30,
        width: 100,
        icon: 'add',
        onClick: function () {
            idRecord = -1;
            _consecutivoActa = "-1";
            popupRegistro.show();
            appendRow();
        }
    }).dxButton("instance");

});

// Adiciona una Fila a la tabla de Individuos
function appendRow() {
    var tbl = document.getElementById('tblIndividuos');
    row = tbl.insertRow(tbl.rows.length);
    row.id = "row" + (tbl.rows.length + 1);
    createCellCausaIngreso(row.insertCell(0), "cboCausaIngreso" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellFamilia(row.insertCell(1), "cboFamilia" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellEspecie(row.insertCell(2), "cboEspecie" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellPrefijo(row.insertCell(3), "txtPrefijo" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellNumero(row.insertCell(4), "txtNumero" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellCantidad(row.insertCell(5), "txtCantidad" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellTiempoCautiverio(row.insertCell(6), "cboTiempoCautiverio" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
    createCellbtnEliminar(row.insertCell(7), "btnEliminar" + (tbl.rows.length + 1), 'row', (tbl.rows.length + 1));
}

const eliminarFila = (key) => {
    const table = document.getElementById('tblIndividuos')
    const row = document.getElementById('row' + key)
    if (table.rows.length <= 2) {
        DevExpress.ui.notify({ message: "No se pueden eliminar todas las filas de la tabla!", width: 1000, shading: true }, "warning", 2000);
        return;
    }
    row.remove();
}

const eliminarTabla = () => {
    const table = document.getElementById('tblIndividuos');
    if (table.rows.length > 1) {
        table.deleteRow(1);
    }
}
function createCellCausaIngreso(cell, id, style, key) {
    var div = document.createElement('div');
    div.id =  id;
    cell.appendChild(div);  

    var cboCausaIngreso = $("#" + id).dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "causaIngresoId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerCausasIngresoAsync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "causaIngresoId",
        width: 200,

        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar  la Causa de Ingreso!"
        }]
    }).dxSelectBox("instance");
}

function createCellFamilia(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    cell.appendChild(div);

    var cboFamilia = $("#" + id).dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "familiaFaunaId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerFamiliasAsync");
                }
            })
        }),
        onValueChanged: function (data) {
            if (data.value != null) {
                var cboEspecieS = $("#cboEspecie" + key).dxSelectBox("instance");
                var cboEspecieDs = cboEspecieS.getDataSource();
                _familiaId = data.value;
                cboEspecieDs.reload();
                cboEspecieS.option("value", null);
            }
        },
        displayExpr: "nombre",
        valueExpr: "familiaFaunaId",
        width: 200,

        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar la Familia!"
        }]
    }).dxSelectBox("instance");
}

function createCellEspecie(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    cell.appendChild(div);

    var cboEspecie = $("#" + id).dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "especieFaunaId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerEspeciesAsync?idFamilia=" + _familiaId);
                }
            })
        }),
        displayExpr: "nombreComun",
        valueExpr: "especieFaunaId",
        width: 200,

        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar la Especie!"
        }]
    }).dxSelectBox("instance");

}

function createCellPrefijo(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    div.style.width = "50px";
    cell.appendChild(div);

    var txtPrefijo = $("#" + id).dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el Prefijo para la identificación del(los) individo(s)!"
        }]
    }).dxTextBox("instance");

}

function createCellNumero(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    div.style.width = "100px";
    cell.appendChild(div);

    var txtNumero = $("#" + id).dxTextBox({
        value: "",
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar el Número de Identificación del(los) individo(s)!"
        }]
    }).dxTextBox("instance");

}

function createCellCantidad(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    cell.appendChild(div);

    var txtCantidad = $("#" + id).dxNumberBox({
        value: 1,
        min: 1,
        max: 2000,
        readOnly: false,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe proporcionar la cantidad de individuos a registrar!"
        }]
    }).dxNumberBox("instance");
}

function createCellTiempoCautiverio(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    cell.appendChild(div);

    var cboTiempo = $("#" + id).dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "tiempoId",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "CAV/api/CAVApi/ObtenerTiemposCautiverioAsync");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "tiempoId",
        width: 200,

        searchEnabled: true,
    }).dxValidator({
        validationGroup: "ProcesoGroup",
        validationRules: [{
            type: "required",
            message: "Debe seleccionar el tiempo en Cautiverio!"
        }]
    }).dxSelectBox("instance");
}

function createCellbtnEliminar(cell, id, style, key) {
    var div = document.createElement('div');
    div.id = id;
    cell.appendChild(div);

    var btnEliminarRow = $("#" + id).dxButton({
        text: "",
        type: "danger",
        height: 30,
        width: 30,
        icon: 'remove',
        onClick: function () {
            eliminarFila(key);
        }
    }).dxButton("instance");



}

function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}

var gridDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var params = {};
        [
            "filter",
            "group",
            "groupSummary",
            "parentIds",
            "requireGroupCount",
            "requireTotalCount",
            "searchExpr",
            "searchOperation",
            "searchValue",
            "select",
            "sort",
            "skip",
            "take",
            "totalSummary",
            "userData"
        ].forEach(function (i) {
            if (i in loadOptions && isNotEmpty(loadOptions[i])) {
                params[i] = JSON.stringify(loadOptions[i]);
            }
        });
        $.getJSON($('#SIM').data('url') + 'CAV/api/CAVApi/GetIngresosCAVAsync', params).done(function (data) {
            if (data === null) {
                alert('La consulta no retornó ningún dato!');
            }
            d.resolve(data.data, { totalCount: data.totalCount });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

gridExamenesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var params = {};
        [
            "filter",
            "group",
            "groupSummary",
            "parentIds",
            "requireGroupCount",
            "requireTotalCount",
            "searchExpr",
            "searchOperation",
            "searchValue",
            "select",
            "sort",
            "skip",
            "take",
            "totalSummary",
            "userData"
           
            
        ].forEach(function (i) {
            if (i in loadOptions && isNotEmpty(loadOptions[i])) {
                params[i] = JSON.stringify(loadOptions[i]);
            }

        });
       

       
        $.getJSON($('#SIM').data('url') + 'CAV/api/CAVApi/GetExamenesHistoriaAsync', { params, historiaId: _historiaId }).done(function (data) {
            if (data === null) {
                alert('La consulta no retornó ningún dato!');
                return;
            }
            d.resolve(data.data, { totalCount: data.totalCount });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        
        return d.promise();
    }
});

