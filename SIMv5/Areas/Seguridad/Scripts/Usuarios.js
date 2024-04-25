var editar = false;
var _hoy = new Date().toLocaleDateString("sp-CO");
var IdUsuario = -1;
var filtroTercero = "";
var Ingreso = "";
$(document).ready(function () {

    $("#LoadingPanel").dxLoadPanel({
        message: 'Procesando...',
        showIndicator: true,
        shading: true,
        shadingColor: "rgba(0,0,0,0.4)",
    });

    $("#grdListaUsuarios").dxDataGrid({
        dataSource: UsuariosDataSource,
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
        groupPanel: {
            visible: false,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'ID_USUARIO', width: '5%', caption: 'ID', alignment: 'center' },
            { dataField: 'GRUPO', width: '12%', caption: 'Grupo', dataType: 'string' },
            { dataField: 'S_LOGIN', width: '15%', caption: 'Nombre Usuario', dataType: 'string' },
            { dataField: 'NOMBRE', width: '23%', caption: 'Nombre', dataType: 'string' },
            { dataField: 'FUNCIONARIO', width: '25%', caption: 'Funcionario', dataType: 'string' },
            { dataField: 'VENCE', width: '6%', caption: 'Fecha Vence', dataType: 'date', format: 'dd/MM/yyyy' },
            { dataField: 'ESTADO', width: '6%', caption: 'Estado', dataType: 'string' },
            {
                alignment: 'center',
                caption: 'Ver',
                width: '60',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'user',
                        hint: 'Ver detalles del usuario',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Seguridad/api/UsuarioApi/DetalleUsuario";
                            $.getJSON(_Ruta, { IdUsuario: options.data.ID_USUARIO })
                                .done(function (data) {
                                    if (data != null) {
                                        showUsuario(data);
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
                caption: 'Editar',
                width: '60',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        hint: 'Editar usuario',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Seguridad/api/UsuarioApi/DetalleUsuario";
                            $.getJSON(_Ruta, { IdUsuario: options.data.ID_USUARIO })
                                .done(function (data) {
                                    if (data != null) {
                                        editar = true;
                                        Ingreso = String(data.FechaRegistro);
                                        IdUsuario = data.IdUsuario;
                                        Grupos.option("value", data.IdGrupo);
                                        Funcionarios.option("value", data.CodFuncionario)
                                        Nombres.option("value", data.Nombres);
                                        Apellidos.option("value", data.Apellidos);
                                        Login.option("value", data.Login);
                                        Correo.option("value", data.Email);
                                        Password.option("value", null);                                       
                                        var dateParts = data.FechaVence.split("/");
                                        var dateVence = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
                                        fecVence.option("value", dateVence);
                                        var valTipo = data.Tipo == "Superusuario" ? true : false;
                                        chkTipoUsr.option("value", valTipo);
                                        var valEstado = data.Estado == "Activo" ? true : false;
                                        chkEstado.option("value", valEstado);
                                        popupDatosUsr.show();
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
                caption: 'Asociar tercero',
                width: '60',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'tags',
                        hint: 'Asociar tercero al usuario',
                        onClick: function (e) {
                            IdUsuario = options.data.ID_USUARIO;
                            popupTerceros.show();
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupDet = null;

    var showUsuario = function (data) {
        Usuario = data;
        if (popupDet) {
            popupDet.option("contentTemplate", popupDetalle.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleUsr").dxPopup(popupDetalle).dxPopup("instance");
        }
        popupDet.show();
    };

    var popupDetalle = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Detalle del Usuario",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function (container) {
            var _funcionario = Usuario.Funcionario == null ? "" : Usuario.Funcionario;
            var _idtercero = Usuario.IdTercero == null ? "" : Usuario.IdTercero;
            var _doctercero = Usuario.DocTercero == null ? "" : Usuario.DocTercero;
            var _tercero = Usuario.NomTercero == null ? "" : Usuario.NomTercero;
            var _mail = Usuario.Email == null ? "" : Usuario.Email;
            var divIni = $("<div></div>");
            var Content = "<table class='table table-sm' style='font-size: 12px;'><thead><tr><th scope='col'>&nbsp;</th><th scope='col'>&nbsp;</th></tr></thead><tbody>";
            Content += "<tr><th scope='row'><b>Id de usuario :</b> </th><td>" + Usuario.IdUsuario + "</td></tr>";
            Content += "<tr><th scope='row'><b>Grupo : </b></th><td>" + Usuario.Grupo + "</td></tr>";
            Content += "<tr><th scope='row'><b>Funcionario : </b></th><td>" + _funcionario + "</td></tr>";
            Content += "<tr><th scope='row'><b>Nombres : </b></th><td>" + Usuario.Nombres + "</td></tr>";
            Content += "<tr><th scope='row'><b>Apellidos : </b></th><td>" + Usuario.Apellidos + "</td></tr>";
            Content += "<tr><th scope='row'><b>Correo electrónico : </b></th><td>" + _mail + "</td></tr>";
            Content += "<tr><th scope='row'><b>Usuario/login : </b></th><td>" + Usuario.Login + "</td></tr>";
            Content += "<tr><th scope='row'><b>Fecha registro : </b></th><td>" + Usuario.FechaRegistro + "</td></tr>";
            Content += "<tr><th scope='row'><b>Fecha vencimiento : </b></th><td>" + Usuario.FechaVence + "</td></tr>";
            Content += "<tr><th scope='row'><b>Tipo usuario : </b></th><td>" + Usuario.Tipo + "</td></tr>";
            Content += "<tr><th scope='row'><b>Estado : </b></th><td>" + Usuario.Estado + "</td></tr>";
            Content += "<tr><th scope='row'><b>Id tercero asociado : </b></th><td>" + _idtercero + "</td></tr>";
            Content += "<tr><th scope='row'><b>Documento tercero asociado : </b></th><td>" + _doctercero + "</td></tr>";
            Content += "<tr><th scope='row'><b>Nombre tercero asociado : </b></th><td>" + _tercero + "</td></tr>";
            Content += "<tr><th scope=´row´>&nbsp;</th><td>&nbsp;</td></tr>";
            divIni.html(Content);
            container.append(divIni);
            return container;
        }
    };

    var Grupos = $("#cboGrupo").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Seguridad/api/UsuarioApi/ListaGrupos");
                }
            })
        }),
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione"
    }).dxSelectBox("instance");

    var Funcionarios = $("#cboFuncionario").dxSelectBox({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "id",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "Seguridad/api/UsuarioApi/ListaFuncionarios");
                }
            })
        }),
        displayExpr: "valor",
        valueExpr: "id",
        searchEnabled: true,
        noDataText: "No hay datos para mostrar",
        placeholder: "Seleccione"
    }).dxSelectBox("instance");

    var Nombres = $("#txtNombres").dxTextBox({
        placeholder: "Ingrese el nombre del usuario...",
        value: ""
        }).dxValidator({
            validationGroup: "UsuarioGroup",
            validationRules: [{
                type: "required",
                message: "El nombre del usuario es obligatorio"
            }]
    }).dxTextBox("instance");

    var Apellidos = $("#txtApellidos").dxTextBox({
        placeholder: "Ingrese el apellido del usuario...",
        value: "",
    }).dxValidator({
        validationGroup: "UsuarioGroup",
        validationRules: [{
            type: "required",
            message: "El apellido del usuario es obligatorio"
        }]
    }).dxTextBox("instance");

    var Login = $("#txtLogin").dxTextBox({
        placeholder: "Ingrese el usuario/login...",
        value: "",
        onFocusIn: function () {
            if (!editar)  $("#lblLogin").text("");
        },
        onChange: function (e) {
            if (e.component.option("value") && !editar) {
                var _text = e.component.option("value");
                var _Ruta = $('#SIM').data('url') + "Seguridad/api/UsuarioApi/LoginDisponible";
                $.getJSON(_Ruta, { strLogin: _text }).done(function (data) {
                    if (!data) {
                        $("#lblLogin").text(_text + " no esta dispoble, favor ingrese uno diferente!");
                        e.component.option("value", "");
                        Password.option("value", "");
                    } else $("#lblLogin").text("");
                }).fail(function (jqxhr, textStatus, error) {
                   alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
                });
            }
        }
    }).dxValidator({
        validationGroup: "UsuarioGroup",
        validationRules: [{
            type: "required",
            message: "El usuario/login es obligatorio"
        }]
    }).dxTextBox("instance");

    var Correo = $("#txtMail").dxTextBox({
        placeholder: "Ingrese el correo electrónico...",
        value: ""
    }).dxValidator({
        validationRules: [{ type: 'email', message: 'El formato del correo electrónico no es válido' }]
    }).dxTextBox("instance");

    var Password = $("#txtPassword").dxTextBox({
        mode: "password",
        value: "",
        inputAttr: {
            autocomplete: 'new-password'
        },
        focusIn: function (e) {
            if (e.component.option("value") === "") {
                VerPass.option("visible", false);
                $("#lblVerPss").hide();
            }
        },
        onChange: function (e) {
            if (e.component.option("value") !== "") {
                VerPass.option("visible", true);
                VerPass.focus();
                $("#lblVerPss").show();
            } else {
                VerPass.option("visible", false);
                $("#lblVerPss").hide();
            }
        }
    }).dxTextBox("instance");

    var VerPass = $("#txtPasswordVer").dxTextBox({
        value: "",
        mode: "password",
        visible: false,
        inputAttr: {
            autocomplete: 'new-password'
        },
    }).dxValidator({
        validationRules: [{
            type: "compare",
            comparisonTarget: function () {
                var password = Password.option("value");
                if (password) {
                    return password;
                }
            },
            message: "La contraseña y su confirmación no son iguales."
        }]
    }).dxTextBox("instance");

    $("#chkVerPss").dxCheckBox({
        value: false,
        height: function () {
            return Password.option("height");
        },
        onOptionChanged: function (e) {
            if (e.value) {
                Password.option("mode", "text");
                VerPass.option("mode", "text");
            } else {
                Password.option("mode", "password");
                VerPass.option("mode", "password");
            }
        }
    });

    var fecVence = $("#fecVence").dxDateBox({
        type: "date",
        value: new Date(),
        showAnalogClock: false,
        displayFormat: "dd/MM/yyyy",
        onOpened: function (args) {
            let position = args.component._popup.option("position");
            position.my = "center";
            position.at = "left";
            args.component._popup.option("position", position);
        }
    }).dxValidator({
        validationGroup: "UsuarioGroup",
        validationRules: [{
            type: "required",
            message: "Debe ingresar la fecha de registro de la solicitud"
        }]
    }).dxDateBox("instance");

    var chkTipoUsr = $("#chkTipoUsr").dxCheckBox({
        value: false,
        width: 200,
        text: "Superusuario"
    }).dxCheckBox("instance");

    var chkEstado = $("#chkEstado").dxCheckBox({
        value: false,
        width: 200,
        text: "Activo"
    }).dxCheckBox("instance");

    $("#btnNuevoUsr").dxButton({
        stylingMode: "contained",
        text: "Nuevo Usuario",
        type: "success",
        width: 200,
        icon: 'user',
        onClick: function () {
            editar = false;
            IdUsuario = -1;
            $("#DivVerPass").hide();
            VerPass.option("visible", false);
            Grupos.option("value", null);
            Funcionarios.option("value", null)
            Nombres.option("value", null);
            Apellidos.option("value", null);
            Login.option("value", null);
            Correo.option("value", null);
            Password.option("value", null);
            Ingreso = _hoy;
            fecVence.option("value", new Date());
            chkTipoUsr.option("value", false);
            chkEstado.option("value", false);
            popupDatosUsr.show();
        }
    });

    $("#btnGuardaUsr").dxButton({
        stylingMode: "contained",
        text: "Guardar Usuario",
        type: "success",
        width: 200,
        icon: 'save',
        onClick: function () {
            DevExpress.validationEngine.validateGroup("UsuarioGroup");
            var params = {
                IdUsuario: IdUsuario,
                IdGrupo: Grupos.option("value"),
                Grupo: Grupos.option("text"),
                CodFuncionario: Funcionarios.option("value") ? Funcionarios.option("value") : "",
                Funcionario: Funcionarios.option("text") ? Funcionarios.option("text") : "",
                Nombres: Nombres.option("value"),
                Apellidos: Apellidos.option("value"),
                Email: Correo.option("value") ? Correo.option("value") : "" ,
                Login: !editar ? Login.option("value") : "",
                Password: !editar ? Password.option("value") : "",
                FechaRegistro: new Date().toISOString(),
                FechaVence: fecVence.option("value"),
                Tipo: chkTipoUsr.option("value") ? "S": "G",
                Estado: chkEstado.option("value") ? "A" : "I",
                IdTercero: null,
                DocTercero: "",
                NomTercero: ""
            };
            var _Ruta = $('#SIM').data('url') + "Seguridad/api/UsuarioApi/GuardaUsuario";
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
                        popupDatosUsr.hide();
                        $('#grdListaUsuarios').dxDataGrid({ dataSource: UsuariosDataSource });
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });  
        }
    });

    var popupDatosUsr = $("#PopupNuevoUsr").dxPopup({
        width: 1100,
        height: 650,
        hoverStateEnabled: true,
        title: "Datos del usuario",
        dragEnabled: true,
        onShown: function () {
            $("#txtRegistro").text(Ingreso);
        }
    }).dxPopup("instance");

    var txtNombre = $("#txtNombre").dxTextBox({
        placeholder: "Ingrese el nombre de la persona o empresa",
        value: ""
    }).dxTextBox("instance");

    var txtDocumento = $("#txtDocumento").dxTextBox({
        placeholder: "Ingrese el numero del documento",
        value: ""
    }).dxTextBox("instance");

    var gridTerceros = $("#grdTerceros").dxDataGrid({
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idTercero",
                loadMode: "raw",
                load: function () {
                    if (filtroTercero != "") {
                        return $.getJSON($('#SIM').data('url') + "Seguridad/api/UsuarioApi/BuscarTercero", { FiltroTercero: filtroTercero });
                    } else return null;
                }
            })
        }),
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
        showBorders: true,
        paging: {
            pageSize: 5
        },
        selection: {
            mode: 'single'
        },
        columns: [
            { dataField: 'idTercero', width: '10%', caption: 'Id', alignment: 'center' },
            { dataField: 'documentoN', width: '20%', caption: 'Documento', dataType: 'string' },
            { dataField: 'rSocial', width: '25%', caption: 'Nombre del Tercero', dataType: 'string' },
            { dataField: 'observacion', width: '10%', caption: 'Tipo', dataType: 'string' },
            { dataField: 'telefono', width: '15%', caption: 'Telefono', dataType: 'string' },
            { dataField: 'correo', width: '20%', caption: 'Correo', dataType: 'string' },
        ]
    }).dxDataGrid("instance");

    $("#btnSeleccionaTercero").dxButton({
        text: "Seleccionar Tercero",
        icon: "pin",
        type: "default",
        width: "190",
        onClick: function () {
            
            var TerceroSel = $("#grdTerceros").dxDataGrid('instance').getSelectedRowsData();
            if (TerceroSel.length > 0) {
                IdTercero = TerceroSel[0].idTercero;
                if (IdUsuario > 0) {
                    var result = DevExpress.ui.dialog.confirm('Esta seguro de asociar el tercero ' + TerceroSel[0].NomTercero + ' con el usuario seleccionado?', 'Usuarios');
                    result.done(function (dialogResult) {
                        if (dialogResult) {
                            var _Ruta = $('#SIM').data('url') + "Seguridad/Api/UsuarioApi/AsociarTercero";
                            $.getJSON(_Ruta, { IdUsuario: IdUsuario, IdTercero: IdTercero }).done(function (data) {
                                if (data.resp == "Error") DevExpress.ui.dialog.alert('Ocurrió un error ' + data.mensaje, 'Usuarios');
                                else {
                                    DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Usuarios');
                                    $('#grdListaUsuarios').dxDataGrid({ dataSource: UsuariosDataSource });
                                    popupTerceros.hide();
                                }
                            }).fail(function (jqxhr, textStatus, error) {
                                DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Usuarios');
                            });
                        }
                    });
                }       
            }
            else DevExpress.ui.dialog.alert('Aún faltan no ha seleccionado un tercero!!', 'Buscar tercero');

        }
    });

    $("#btnBuscaTercero").dxButton({
        text: "Buscar Tercero",
        icon: "search",
        type: "default",
        width: "170",
        onClick: function () {           
            if (txtDocumento.option("value").length > 0 || txtNombre.option("value").length > 0) {
                filtroTercero = txtDocumento.option("value") + ";" + txtNombre.option("value");
                $("#lbltercero").text("");
                gridTerceros.refresh();
            } else {
                DevExpress.ui.dialog.alert('Aún faltan datos para buscar el tercero de la solicitud!!', 'Buscar tercero');
            }
        }
    });

    var popupTerceros = $("#popupBuscarTercero").dxPopup({
        width: 900,
        height: 550,
        showTitle: true,
        title: "Buscar Terceros del SIM"
    }).dxPopup("instance");

});

var UsuariosDataSource = new DevExpress.data.CustomStore({
    key: "ID_USUARIO",
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
        $.getJSON($('#SIM').data('url') + 'Seguridad/Api/UsuarioApi/GetUsuarios', params)
            .done(function (response) {
                d.resolve(response.data, {
                    totalCount: response.totalCount
                });
        }).fail(function (jqxhr, textStatus, error) {
            alert('error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});

function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}