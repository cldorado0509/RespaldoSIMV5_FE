var IdPersona = -1;
var IdSeleccionGrid = -1;
var RegistroSalida = -1;
var IdRegistro = -1;
var IdDep = -1;
var CodFun = -1;
var IdMotivoVisita = -1;
var IdVisita = -1;
var Busqueda = -1;
var PersonaGuardada = -1;
var Sw = -1;
var ConstF;
var Hoy = new Date().toDateString("yyyy/MM/dd");

var FechaInicial = new Date();

var w = FechaInicial.getDate() + '-' + (FechaInicial.getMonth() + 1) + '-' + FechaInicial.getFullYear();//esta variable es para comparar si el dia seleccionado en el calendario el dia actual, y asi activar o desactivar el boton que permite crear una visita

const today = new Date();

const year = today.getFullYear();

const month = `${today.getMonth() + 1}`.padStart(2, "0");

const day = `${today.getDate()}`.padStart(2, "0");

const q = [year, month, day].join("-");  //esta variable es para comparar si el dia seleccionado en el calendario el dia actual, y asi activar o desactivar el boton que permite crear una visita

var Fecha = FechaInicial.getDate() + '/' + (FechaInicial.getMonth() + 1) + '/' + FechaInicial.getFullYear();

var Hora = FechaInicial.getHours() + ':' + FechaInicial.getMinutes() + ':' + FechaInicial.getSeconds();

var mesReporte = month ;
var annoReporte = year;

$(document).ready(function ()
{

    //Calendario
    const zoomLevels = ['month', 'year', 'decade', 'century'];


    var calendar = $('#calendar-container').dxCalendar({

        value: new Date(),
        disabled: false,
        firstDayOfWeek: 0,
        zoomLevel: zoomLevels[0],
        dateSerializationFormat:"yyyy-MM-dd",

        onValueChanged(data) {
            w = data.value
            Hoy = data.value;
            GidListado.dxDataGrid("instance").option("dataSource", VisitaTerceroDataSource);
            BtnSalida.option("visible", false);
            IdPersona = -1;
            IdRegistro = -1;
            IdDep = -1;
            CodFun = -1;
            //IdMotivoVisita = -1;
            IdVisita = -1;
            PrimerNombre.option("disabled", true);
            SegundoNombre.option("disabled", true);
            PrimerApellido.option("disabled", true);
            SegundoApellido.option("disabled", true);
            Telefono.option("disabled", true);
            Email.option("disabled", true);
            Genero.option("disabled", true);
            Direccion.option("disabled", true);
            Cedula.reset();
            PrimerNombre.reset();
            SegundoNombre.reset();
            PrimerApellido.reset();
            SegundoApellido.reset();
            Telefono.reset();
            Email.reset();
            Genero.reset();
            Direccion.reset();
            GuardarP.option("visible", false);
            BtnSalida.option("visible", false);
            Cedula.option("disabled", false);
            $('#txtApellido1').dxValidator('instance').reset();
            $('#txtNombre1').dxValidator('instance').reset();
            $('#txtDocumento').dxValidator('instance').reset();

            if (w != q) {
                btnNuevo.option("visible", false);
            } else {
                btnNuevo.option("visible", true);
            }
            
        },


    }).dxCalendar('instance');


    //Datagrid
    var GidListado = $("#GidListado").dxDataGrid(
    { 
        dataSource: VisitaTerceroDataSource,
        allowColumnResizing: true,
        loadPanel: { enabled: true, text: 'Cargando Datos...' },
        noDataText: "Sin datos para mostrar",
            showBorders: true,

        export: {
                enabled: true
        },
        
        paging: {
            pageSize: 5
        },
        headerFilter: {
            visible: false,
            allowSearch: true,
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 300],
            showNavigationButtons: true,
        },
        filterRow: {
            visible: true,
            emptyPanelText: 'Arrastre una columna para agrupar'
        },
        selection: {
            mode: 'single'
        },
        hoverStateEnabled: true,
        remoteOperations: true,
        columns: [
            { dataField: 'IdVisitaTercero', width: '4.3%', caption: 'Codigo', alignment: 'left' },
            { dataField: 'FechaIngreso', width: '8.2%', caption: 'Fecha Ingreso', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy hh:mm:ss a'},
            { dataField: 'FechaSalida', width: '8.2%', caption: 'Fecha Salida', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy hh:mm:ss a'},
            { dataField: 'Documento', width: '6%', caption: 'Documento', alignment: 'left' },
            { dataField: 'NombreCompleto', width: '17%', caption: 'Nombre', alignment: 'center' },
           // { dataField: 'Carne', width: '4.1%', caption: 'Carne', alignment: 'left' },
            { dataField: 'Empresa', width: '11%', caption: 'Empresa', alignment: 'center' },
            { dataField: 'NombreDependencia', width: '11%', caption: 'Dependencia', alignment: 'center' },
            { dataField: 'NombreFuncionario', width: '17%', caption: 'Funcionario', alignment: 'center' },
            { dataField: 'NombreMotivoVisita', width: '11%', caption: 'Motivo Visita', alignment: 'center' },
            
            {
                width: '5%',
                caption: "Editar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'edit',
                        
                        
                        hint: 'Editar la información de la visita',
                        onClick: function () {
                            Sw = 1;
                            var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/ObtenerVisita";
                            $.getJSON(_Ruta,
                                { id:options.data.IdVisitaTercero
                                }).done(function (data) {
                                    if (data !== null) {
                                        ConstF = data.fechaIngreso;
                                        IdPersona = data.idPersona;
                                        IdDep = data.dependencia;
                                        IdVisita = data.idVisitaTercero;
                                        Dependencia.option("value", data.dependencia);
                                        Funcionario.option("value", data.codFuncionario);
                                        MotivoVisita1.option("value", data.motivoVisita);
                                        RegistroSalida = data.fechaSalida;
                                        //Carne.option("value", data.carne);
                                        //if (data.entrega == 1 )
                                        //    CarneDevuelto.option("value", true);
                                        //else CarneDevuelto.option("value", false);
                                        Empresa.option("value", data.empresa);
                                        Observacion.option("value", data.observacion);
                                        popupVisita.show();
                                    }
                                }).fail(function (jqxhr, textStatus, error) {
                                    DevExpress.ui.dialog.alert('Ocurrió un error');
                                });
                        }
                    }).appendTo(container);
                }
              
            },

            {
                width: '5%',
                caption: "Eliminar",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'remove',
                        
                        hint: 'Eliminar visita',
                        onClick: function (e) {
                            var result = DevExpress.ui.dialog.confirm('Desea eliminar el registro seleccionado?', 'Confirmación');
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    
                                   
                                    var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/EliminarVisita";
                                    var params = {
                                        idVisitaTercero: options.data.IdVisitaTercero, fechaIngreso: Hoy, fechaSalida: Hoy, carne: 0, dependencia: 0, motivoVisita: 0, observacion: ".", empresa: ".", entrega: ".", codFuncionario: 0, idPersona: 0
                                    };
                                    $.ajax({
                                        type: 'POST',
                                        url: _Ruta,
                                        contentType: "application/json",
                                        dataType: 'json',
                                        data: JSON.stringify(params),
                                        success: function (data) {
                                            if (data.resp === "Error") DevExpress.ui.dialog.alert('Ocurrió un error al eliminar registro seleccionado');
                                            else {
                                                IdRegistro = -1;
                                                IdPersona = -1;
                                                IdVisita = -1;
                                                GidListado.dxDataGrid("instance").option("dataSource", VisitaTerceroDataSource);
                                                DevExpress.ui.dialog.alert('Registro eliminado correctamente!');
                                                PrimerNombre.option("disabled", true);
                                                SegundoNombre.option("disabled", true);
                                                PrimerApellido.option("disabled", true);
                                                SegundoApellido.option("disabled", true);
                                                Telefono.option("disabled", true);
                                                Email.option("disabled", true);
                                                Genero.option("disabled", true);
                                                Direccion.option("disabled", true);
                                                Cedula.reset();
                                                PrimerNombre.reset();
                                                SegundoNombre.reset();
                                                PrimerApellido.reset();
                                                SegundoApellido.reset();
                                                Telefono.reset();
                                                Email.reset();
                                                Genero.reset();
                                                Direccion.reset();
                                                GuardarP.option("visible", false);
                                                $("#direc").hide();
                                                BtnSalida.option("visible", false);
                                                $('#txtDocumento').dxValidator('instance').reset();

                                                
                                            }
                                        },
                                        error: function (xhr, textStatus, errorThrown) {
                                            DevExpress.ui.dialog.alert('Ocurrió un error al eliminar el registro seleccionado');
                                        }
                                    });
                                }
                            });
                        }
                    }).appendTo(container);
                }
            },
            
        ],
            onSelectionChanged: function (selectedItems) {
                
                Busqueda = -1;
                IdSeleccionGrid = -1;
                
                var data = selectedItems.selectedRowsData[0];
                if (data) {
                    IdSeleccionGrid = data.Documento;
                    //IdVisita = data.IdVisitaTercero;
                    RegistroSalida = data.IdVisitaTercero;
                    if (data.FechaSalida != null) {
                        BtnSalida.option("disabled", true);
                    }
                    else BtnSalida.option("disabled", false);
                }
            
                var _Ruta = ($('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/Persona");

             $.getJSON(_Ruta,
                {
                    id: IdSeleccionGrid
                }).done(function (data) {

                    if (data.idPersona > 0) {
                       
                        Cedula.option("value", data.documento);
                       
                        PrimerApellido.option("value", data.primerApellido);
                        SegundoApellido.option("value", data.segundoApellido);
                        PrimerNombre.option("value", data.primerNombre);
                        SegundoNombre.option("value", data.segundoNombre);
                        Telefono.option("value", data.telefono);
                        Email.option("value", data.correo);
                        Genero.option("value", data.genero);
                        Direccion.option("value", data.direccion);
                        Direccion.option("disabled", false);
                        PrimerNombre.option("disabled", false);
                        SegundoNombre.option("disabled", false);
                        PrimerApellido.option("disabled", false);
                        SegundoApellido.option("disabled", false);
                        Telefono.option("disabled", false);
                        Email.option("disabled", false);
                        Genero.option("disabled", false);                     
                        GuardarP.option("visible", false);
                        BtnSalida.option("visible", true);
                        

                    } else {
                        IdSeleccionGrid = -1;
                        
                       
                    }

                });


        }
        });
   
    var GidReporte= $("#GidReporte").dxDataGrid(
        {
            dataSource: ReporteMesDataSource,
            allowColumnResizing: true,
            loadPanel: { enabled: true, text: 'Cargando Datos...' },
            noDataText: "Sin datos para mostrar",
            showBorders: true,

            export: {
                enabled: true
            },

            paging: {
                pageSize: 5
            },
            headerFilter: {
                visible: false,
                allowSearch: true,
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10],
                showNavigationButtons: true,
            },
            filterRow: {
                visible: true,
                emptyPanelText: 'Arrastre una columna para agrupar'
            },
            selection: {
                mode: 'single'
            },
            hoverStateEnabled: true,
            remoteOperations: true,
            columns: [
                { dataField: 'IdVisitaTercero', width: '4.3%', caption: 'Codigo', alignment: 'left' },
                { dataField: 'FechaIngreso', width: '8.2%', caption: 'Fecha Ingreso', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy hh:mm:ss a' },
                { dataField: 'FechaSalida', width: '8.2%', caption: 'Fecha Salida', alignment: 'center', dataType: 'date', format: 'dd/MM/yyyy hh:mm:ss a' },
                { dataField: 'Documento', width: '6%', caption: 'Documento', alignment: 'left' },
                { dataField: 'NombreCompleto', width: '17%', caption: 'Nombre', alignment: 'center' },
                // { dataField: 'Carne', width: '4.1%', caption: 'Carne', alignment: 'left' },
                { dataField: 'Empresa', width: '11%', caption: 'Empresa', alignment: 'center' },
                { dataField: 'NombreDependencia', width: '11%', caption: 'Dependencia', alignment: 'center' },
                { dataField: 'NombreFuncionario', width: '17%', caption: 'Funcionario', alignment: 'center' },
                { dataField: 'NombreMotivoVisita', width: '11%', caption: 'Motivo Visita', alignment: 'center' },

            ]
            
        });
    //TXT 

    var Cedula = $('#txtDocumento').dxNumberBox({
        min:1,
        disabled: false,
        showClearButton: true,
        placeholder: 'Ingresar documento',
       value: "",
        width: 250
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el Documento!"
        }]
    }).dxNumberBox("instance");
    var PrimerApellido = $('#txtApellido1').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar primer apellido',
        value: "",
        width: 250
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el apellido!"
        },
            {
                type: 'pattern',
                pattern: /^[^0-9]+$/,
                message: 'No uses digitos en el apellido',
            }, {
                type: 'stringLength',
                min: 2,
                message: 'El apellido debe tener al menos 2 caracteres',
            }
        ]
    }).dxTextBox("instance");
    var SegundoApellido = $('#txtApellido2').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar segundo apellido',
        value: "",
        width: 250,
       
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [
        {
            type: 'pattern',
            pattern: /^[^0-9]+$/,
            message: 'No uses digitos en el nombre',
        }
        ]
    }).dxTextBox("instance");
    var PrimerNombre = $('#txtNombre1').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar primer nombre',
        value: "",
        width: 270
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el Nombre!"
        },
          {
                type: 'pattern',
                pattern: /^[^0-9]+$/,
                message: 'No uses digitos en el nombre',
            }, {
                type: 'stringLength',
                min: 2,
                message: 'El nombre debe tener al menos 2 caracteres',
            }
        ]
    }).dxTextBox("instance");
    var SegundoNombre = $('#txtNombre2').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar segundo nombre',
        value: "",
        width: 250,
        
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [
        {
            type: 'pattern',
            pattern: /^[^0-9]+$/,
            message: 'No uses digitos en el nombre',
        }
        ]
    }).dxTextBox("instance");
    var Telefono = $('#txtTelefono').dxNumberBox({
        disabled: true,
        
        showClearButton: true,
        placeholder: 'Ingresar telefono',
        value: "",
        width: 250,
        
    }).dxValidator({
       validationGroup: "ValPersona",
        validationRules: [{
            type: 'pattern',
            pattern: /^([0-9]){7,10}$/,
            message: 'Ingrese un telefono con un formato correcto',
        }],
    }).dxNumberBox("instance");
    var Email = $('#txtEmail').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar email',
        value: "",
        width: 270,
      
    }).dxValidator({
        validationGroup: "ValPersona",
        validationRules: [{
            type: 'email',
            message: 'Ingrese un correo valido',
        }],
    }).dxTextBox("instance");
    var Carne = $('#txtCarne').dxNumberBox({
        showSpinButtons: true,
        showClearButton: true,
        placeholder: '',
        value: "1",
        default: "1",
        width: 250,
        visible: false,
        
    }).dxValidator({
        validationGroup: "",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el carne!"
        }]
    }).dxNumberBox("instance");
    var Empresa = $('#txtEmpresa').dxTextBox({

        showClearButton: true,
        placeholder: '',
        width: 500,

    }).dxTextBox("instance");
    var Direccion = $('#txtDireccion').dxTextBox({
        disabled: true,
        showClearButton: true,
        placeholder: 'Ingresar Direccion',
        value: "",
        width: 250,

    }).dxTextBox("instance");

    $('#txtNumero1').dxTextBox({

        showClearButton: true,
        placeholder: '',
        width: 70,
       
        
    });
    $('#txtNumero2').dxTextBox({

        showClearButton: true,
        placeholder: '',
        width: 70,

    });
    $('#txtPlaca').dxTextBox({

        showClearButton: true,
        placeholder: '',
        width: 70,

    });
    $('#txtInterior').dxTextBox({

        showClearButton: true,
        placeholder: '',
        width: 80,

    });
    $('#txtCodEpm').dxTextBox({

        showClearButton: true,
        placeholder: '',
        width: 200,

    });


    var annoReporteMes = $('#txtanno').dxNumberBox({
        showSpinButtons: true,
        showClearButton: true,
        placeholder: '',
        value: year,
        width: 250
        

    }).dxValidator({
        validationGroup: "",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el año!"
        }]
    }).dxNumberBox("instance");

    //Text Area
    var Observacion = $('#txtareaObservacion').dxTextArea({
        width: 500,
        value:"",
        height: 70,
    }).dxTextArea('instance');

    //CheckBox
    var CarneDevuelto= $('#CarneDevuelto').dxCheckBox({
        value: true,
        visible: false
    }).dxCheckBox('instance');
    
    //RadioButtons
    var Genero = $('#radioGenero').dxRadioGroup({
        disabled: true,
        items: ['M', 'F'],
        value: "",
        layout: 'horizontal',
    }).dxRadioGroup("instance");
   
    //Buttons
    var reporteMes = $("#btnReporteMes").dxButton({
        stylingMode: "contained",
        hint: "Reporte mensual",
        type: "success",
        width: 50,
        height: 36,
        icon: 'export',
        onClick: function () {
            popupReporte.show();
        }
    }).dxButton("instance");
    var btnNuevo = $("#btnNuevo").dxButton({
        stylingMode: "contained",
        text: "Nuevo",
        type: "success",
        width: 80,
        height: 36,
        icon: 'add',
        onClick: function () {


            if (IdPersona > 0 && Busqueda>0) {
                cargarFuncionarios()
                IdDep = -1;
                CodFun = -1;
                Sw = -1;
                Funcionario.getDataSource().reload();
                Dependencia.getDataSource().reload();
                funcionarioseleccionado = null
                IdRegistro = -1;
                MotivoVisita1.reset();
                popupVisita.show();
                Dependencia.reset();
                Funcionario.reset();
                Observacion.reset();
                Empresa.reset();
                
               // CarneDevuelto.reset();
                $('#MotivoVisita1').dxValidator('instance').reset();
                $('#treeBoxFuncionario').dxValidator('instance').reset();
               // $('#txtCarne').dxValidator('instance').reset();
            } else {
                
                 DevExpress.ui.dialog.alert('Debe ingresar una persona registrada');
                
            }
        }
    }).dxButton("instance");
    var GuardarP= $("#btnGuardarP").dxButton({
        visible: false,
       // stylingMode: "contained",
        text: "Guardar",
        type: "success",
        width: 80,
        height: 36,
        icon: 'save',
        validationGroup: "ValPersona",
       
       onClick: function (e) {

           var result = e.validationGroup.validate();
           if (result.isValid) {


               //DevExpress.validationEngine.validateGroup("ValPersona");


               var id = IdRegistro;
               var documento = Cedula.option("value");
               var primerApellido = PrimerApellido.option("value");
               var segundoApellido = SegundoApellido.option("value");
               var primerNombre = PrimerNombre.option("value");
               var segundoNombre = SegundoNombre.option("value");
               var telefono = Telefono.option("value");
               var email = Email.option("value");
               var genero = Genero.option("value");
               var direccion = Direccion.option("value");

               if (documento === "" || primerApellido === "" || primerNombre === "") return;
               var params = {};

               if (IdRegistro > 0) {
                   params = {
                       idPersona: id, documento: documento, primerNombre: primerNombre, segundoNombre: segundoNombre, primerApellido: primerApellido, segundoApellido: segundoApellido, genero: genero, telefono: telefono, correo: email, direccion: direccion
                   };
               } else {
                   params = {
                       documento: documento, primerNombre: primerNombre, segundoNombre: segundoNombre, primerApellido: primerApellido, segundoApellido: segundoApellido, genero: genero, telefono: telefono, correo: email, direccion: direccion
                   };
               }

               var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/GuardarPersona";
               $.ajax({
                   type: "POST",
                   dataType: 'json',
                   url: _Ruta,
                   data: JSON.stringify(params),
                   contentType: "application/json",
                   crossDomain: true,
                   headers: { 'Access-Control-Allow-Origin': '*' },
                   success: function (data) {

                       IdPersona = data.id;

                       if (IdRegistro <= 0) {
                           PrimerNombre.option("disabled", true);
                           SegundoNombre.option("disabled", true);
                           PrimerApellido.option("disabled", true);
                           SegundoApellido.option("disabled", true);
                           Telefono.option("disabled", true);
                           Email.option("disabled", true);
                           Genero.option("disabled", true);
                           Direccion.option("disabled", true);
                       } else {
                           PrimerNombre.option("disabled", false);
                           SegundoNombre.option("disabled", false);
                           PrimerApellido.option("disabled", false);
                           SegundoApellido.option("disabled", false);
                           Telefono.option("disabled", false);
                           Email.option("disabled", false);
                           Genero.option("disabled", false);
                           Direccion.option("disabled", false);
                       }
                       DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                       GuardarP.option("visible", false);
                       $('#GidListado').dxDataGrid({ dataSource: VisitaTerceroDataSource });



                   },
                   error: function (xhr, textStatus, errorThrown) {
                       DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                   }
               });

           } else DevExpress.ui.dialog.alert('Debe llenar correctamente los datos!');
        }
   }).dxButton("instance");
    var BtnSalida= $("#btnSalida").dxButton({
        visible: false,
        stylingMode: "contained",
        text: "Registrar Salida",
        type: "success",
        width: 150,
        height: 36,
        icon: 'arrowleft',

        onClick: function () {

            var FechaParaSalida = new Date();

            var DiaSalida = FechaParaSalida.getDate() + '/' + (FechaParaSalida.getMonth() + 1) + '/' + FechaParaSalida.getFullYear();
            var HoraSalida = FechaParaSalida.getHours() + ':' + FechaParaSalida.getMinutes() + ':' + FechaParaSalida.getSeconds();
           
            var id = RegistroSalida;
            var fecha = DiaSalida + " " + HoraSalida;
           
            params = {
                idVisitaTercero: id, fechaSalida: fecha
            };

            var _Ruta = ($('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/RegistrarSalida");
            $.ajax({
                type: "PUT",
                dataType: 'json',
                url: _Ruta,
                data: JSON.stringify(params),
                contentType: "application/json",
                crossDomain: true,
                headers: { 'Access-Control-Allow-Origin': '*' },
                success: function (data) {
                    if (data.resp === "Error") DevExpress.ui.dialog.alert('Seleccione un registro');
                    else {
                        DevExpress.ui.dialog.alert('Datos Guardados correctamente', 'Guardar Datos');
                        $('#GidListado').dxDataGrid({ dataSource: VisitaTerceroDataSource });
                        BtnSalida.option("visible", false);

                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    DevExpress.ui.dialog.alert('Ocurrió un problema : ' + textStatus + ' ' + errorThrown + ' ' + xhr.responseText, 'Guardar Datos');
                }
            });




        }
    
    }).dxButton("instance");
    $("#btnCancelar").dxButton({
        text: "Cancelar",
        type: "default",
        height: 30,
        onClick: function () {
            Sw = -1;
            popupVisita.hide();
            
            MotivoVisita1.reset();
            Dependencia.reset();
            Funcionario.reset();
            Observacion.reset();
            Empresa.reset();
            //Carne.reset();
            //CarneDevuelto.reset();
            $('#MotivoVisita1').dxValidator('instance').reset();
        }
    });
    $("#btnCancelar1").dxButton({
        text: "Cancelar",
        type: "default",
        height: 30,
        onClick: function () {
            popupDireccion.hide();
        }
    });
    $("#btnCancelar2").dxButton({
        text: "Cancelar",
        type: "default",
        height: 30,
        onClick: function () {
            popupImprimir.hide();
        }
    });
   var limpiar= $("#btnLimpiar").dxButton({
        hint: "Limpiar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'clear',


        onClick: function () {
            IdPersona = -1;
            IdRegistro = -1;
            IdDep = -1;
            CodFun = -1;
            IdMotivoVisita = -1;
            IdVisita = -1;
            PrimerNombre.option("disabled", true);
            SegundoNombre.option("disabled", true);
            PrimerApellido.option("disabled", true);
            SegundoApellido.option("disabled", true);
            Telefono.option("disabled", true);
            Email.option("disabled", true);
            Genero.option("disabled", true);
            Direccion.option("disabled", true);
            Cedula.reset();
            PrimerNombre.reset();
            SegundoNombre.reset();
            PrimerApellido.reset();
            SegundoApellido.reset();
            Telefono.reset();
            Email.reset();
            Genero.reset();
            Direccion.reset();
            GuardarP.option("visible", false);
            BtnSalida.option("visible", false);
            Cedula.option("disabled", false);
            $('#txtApellido1').dxValidator('instance').reset();
            $('#txtNombre1').dxValidator('instance').reset();
            $('#txtDocumento').dxValidator('instance').reset();
           
           
        }
   }).dxButton("instance");
    $("#btnBuscar").dxButton({
        hint: "Buscar",
        stylingMode: "contained",
        text: "",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            Busqueda = 1;
            IdRegistro = -1;
            funcionarioseleccionado = null
            cargarFuncionarios()
            

            var _Ruta = ($('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/Persona");

            $.getJSON(_Ruta,
                {
                    id: Cedula.option("value")
                }).done(function (data) {

                    if (data.idPersona > 0) {

                        IdPersona = data.idPersona;
                        IdRegistro = data.idPersona;
                        PrimerApellido.option("value", data.primerApellido);
                        SegundoApellido.option("value", data.segundoApellido);
                        PrimerNombre.option("value", data.primerNombre);
                        SegundoNombre.option("value", data.segundoNombre);
                        Telefono.option("value", data.telefono);
                        Email.option("value", data.correo);
                        Genero.option("value", data.genero);
                        Direccion.option("value", data.direccion);
                        
                      
                        Cedula.option("disabled", true);
                        PrimerNombre.option("disabled", false);
                        SegundoNombre.option("disabled", false);
                        PrimerApellido.option("disabled", false);
                        SegundoApellido.option("disabled", false);
                        Telefono.option("disabled", false);
                        Email.option("disabled", false);
                        Genero.option("disabled", false);
                        Direccion.option("disabled", false);

                        /*$("#direc").show();*/
                       
                        GuardarP.option("visible", true);


                       
                    } else {
                        IdPersona = -1;
                        IdRegistro = -1;
                        PrimerNombre.reset();
                        SegundoNombre.reset();
                        PrimerApellido.reset();
                        SegundoApellido.reset();
                        Telefono.reset();
                        Email.reset();
                        Genero.reset();
                        Cedula.option("disabled", true);
                        PrimerNombre.option("disabled", false);
                        SegundoNombre.option("disabled", false);
                        PrimerApellido.option("disabled", false);
                        SegundoApellido.option("disabled", false);
                        Telefono.option("disabled", false);
                        Email.option("disabled", false);
                        Genero.option("disabled", false);
                        Direccion.option("disabled", false);
                        /*$("#direc").show();*/
                        GuardarP.option("visible", true);
                        


                        DevExpress.ui.dialog.alert('Documento no registrado!');
                    }

                }).fail(function (jqxhr, textStatus, error) {
                    IdPersona = -1;
                    IdRegistro = -1;
                    DevExpress.ui.dialog.alert('Ingrese un documento!');
                    PrimerNombre.option("disabled", true);
                    SegundoNombre.option("disabled", true);
                    PrimerApellido.option("disabled", true);
                    SegundoApellido.option("disabled", true);
                    Telefono.option("disabled", true);
                    Email.option("disabled", true);
                    Genero.option("disabled", true);
                    Direccion.option("disabled", true);
                    PrimerNombre.reset();
                    SegundoNombre.reset();
                    PrimerApellido.reset();
                    SegundoApellido.reset();
                    Telefono.reset();
                    Email.reset();
                    Genero.reset();
                    Direccion.reset();
                    GuardarP.option("visible", false);
                    $("#direc").hide();
                });

        }
    });
   var guardarVisista = $("#btnGuardarVisita").dxButton({
        text: "Guardar",
        type: "default",
        height: 30,
       onClick: function () {

           if (MotivoVisita1.option("value") == null || Funcionario.option("value") == null) {

               DevExpress.validationEngine.validateGroup("VisitaGroup");
               DevExpress.ui.dialog.alert('Seleccione los campos requeridos');
           } else {

               var FechaGuardar = new Date();
               var DiaGuardar = new Date().toDateString("yyyy/MM/dd");
               var HoraGuardar = FechaGuardar.getHours() + ':' + FechaGuardar.getMinutes() + ':' + FechaGuardar.getSeconds();
               var HoyGuardar = DiaGuardar + " " + HoraGuardar;
              
               var id = IdVisita;
               var idP = IdPersona;
               var codF = CodFun;
               var idMv = IdMotivoVisita;
               var idD = IdDep;
               var entrada = HoyGuardar;
               var carne = 1;
               var obs = Observacion.option("value");
               var empresa = Empresa.option("value");
               var entrega = 1;

               //if (CarneDevuelto.option("value") == true) {
               //    var entrega = 1;
               //} else {
               //    var entrega = 0;
               //} 

               if (Sw <=0) {
                   entrada = HoyGuardar
               } else {
                  entrada = ConstF
               } 


               if (idD === "" || idMv === "" || codF === "") return;
               var params = {
                   idVisitaTercero: id, fechaIngreso: entrada, fechaSalida: RegistroSalida, carne: carne, dependencia: idD, motivoVisita: idMv, observacion: obs, empresa: empresa, entrega: entrega, codFuncionario: codF, idPersona: idP
               };

               var _Ruta = $('#SIM').data('url') + "AtencionUsuarios/api/VisitaTerceroApi/GuardarVisita";
               $.ajax({
                   type: "POST",
                   dataType: 'json',
                   url: _Ruta,
                   data: JSON.stringify(params),
                   contentType: "application/json",
                   crossDomain: true,
                   headers: { 'Access-Control-Allow-Origin': '*' },

                   success: function (data) {

                       DevExpress.ui.dialog.alert('Datos Guardados correctamente');
                       $('#GidListado').dxDataGrid({ dataSource: VisitaTerceroDataSource });
                       $("#PopupNuevo").dxPopup("instance").hide();
                       funcionarioseleccionado = null

                       ///
                       IdPersona = -1;
                       IdRegistro = -1;
                       IdDep = -1;
                       RegistroSalida = -1;
                       //CodFun = -1;
                       //IdMotivoVisita = -1;
                       IdVisita = -1;
                       PrimerNombre.option("disabled", true);
                       SegundoNombre.option("disabled", true);
                       PrimerApellido.option("disabled", true);
                       SegundoApellido.option("disabled", true);
                       Telefono.option("disabled", true);
                       Email.option("disabled", true);
                       Genero.option("disabled", true);
                       Direccion.option("disabled", true);
                       Cedula.reset();
                       PrimerNombre.reset();
                       SegundoNombre.reset();
                       PrimerApellido.reset();
                       SegundoApellido.reset();
                       Telefono.reset();
                       Email.reset();
                       Genero.reset();
                       Direccion.reset();
                       GuardarP.option("visible", false);
                       BtnSalida.option("visible", false);
                       Cedula.option("disabled", false);

                   },
                   error: function (xhr, textStatus, errorThrown) {
                       DevExpress.ui.dialog.alert('Ocurrió un problema al guardar Datos');
                   }
               });
           }
       }
   }).dxButton("instance");

    $("#btnBuscarReporte").dxButton({
        hint: "Buscar fecha reporte",
        stylingMode: "contained",
        type: "success",
        width: 40,
        height: 36,
        icon: 'find',
        onClick: function () {
            
            mesReporte = boxMesReporte.option("value");
            annoReporte = annoReporteMes.option("value");
            $('#GidReporte').dxDataGrid({ dataSource: ReporteMesDataSource });

         
        }
    });



    //SelectBox
    var Dependencia = $('#treeBoxDependencia').dxSelectBox(
    {
       width: 500,
            dataSource: new DevExpress.data.DataSource(
            {
                 store: new DevExpress.data.CustomStore(
                 {
                    key: "idDepencencia",
                    loadMode: "raw",
                         load: function ()
                         {                          
                             return  $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/VisitaTerceroApi/GetDependenciaAsync")                        
                         }
                 })
                }),

               displayExpr: "nombre",
               valueExpr: "idDepencencia",
               searchEnabled: true,
      
            onValueChanged: function (data) {
                
                IdDep = data.value;
               

                if (data.value == -50) {
                    funcionarioseleccionado = null;
                    Funcionario.option("dataSource", funcionarios)
                    Funcionario.reset()
                } else {

                    var listaUsuarioxDependencia = []

                    console.log(funcionarios)

                    for (var i = 0; i < funcionarios.length; i++) {
                        element = funcionarios[i]
                        if (element.IdDependencia == IdDep)
                            listaUsuarioxDependencia.push(element)
                    }

                    Funcionario.option("dataSource", listaUsuarioxDependencia)

                }

       }
       
    }).dxValidator(
       {
        validationGroup: "ProcesoGroup",
       
        }).dxSelectBox("instance");


    var funcionarios = []
    var funcionarioseleccionado = null

    function getFuncionario(value) {
        for (var i = 0; i < funcionarios.length; i++) {
            let element = funcionarios[i]

            if (element.codFuncionario == value)
                return element
        }
    }

    function cargarFuncionarios() {
        var res = $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/VisitaTerceroApi/GetFuncionarioAsync?dep=" + IdDep);
                    res.then(data => {
                        
                        funcionarios = data
                        Funcionario.option("dataSource", funcionarios)
                    })
    }

    cargarFuncionarios()

    var Funcionario = $('#treeBoxFuncionario').dxSelectBox({
        width: 500,
       
        
        displayExpr: "nombres",
        valueExpr: "codFuncionario",
        searchEnabled: true,

        onValueChanged: function (data) {

            CodFun = data.value;
            /*console.log(data)*/
           
            if (IdDep <= 0) {
                if (data.value != null) {
                    funcionarioseleccionado = data.value


                    let itemFunc = getFuncionario(data.value)

                    /* console.log(IdDep)*/
                    Dependencia.option("value", itemFunc.IdDependencia);

                }
            }

        }
    }).dxValidator({
        validationGroup: "VisitaGroup",
        validationRules: [{
            type: "required",
            message: "Debe ingresar el funcionario!"
        }]
    }).dxSelectBox("instance");

    $('#treeBoxMunicipio').dxSelectBox({
            width: 200,
            
            placeholder: '',

        });
    $('#treeBoxViaP').dxSelectBox({
         width: 140,
           
            placeholder: '',

        });       
    $('#treeBoxLetra1').dxSelectBox({
            width: 85,
            
            placeholder: '',

        });
    $('#treeBoxSentido1').dxSelectBox({
            width: 90,
            
            placeholder: '',

        });
    $('#treeBoxViaS').dxSelectBox({
            width: 140,
           
            placeholder: '',

        });        
    $('#treeBoxLetra2').dxSelectBox({
            width: 85,
            
            placeholder: '',

        });
    $('#treeBoxSentido2').dxSelectBox({
            width: 90,
           
            placeholder: '',

    });

    var boxMesReporte = $('#treeBoxMesReporte').dxSelectBox({
        width: 200,
        dataSource: [{ id: 1, value: 'Enero' }, { id: 2, value: 'Febrero' }, { id: 3, value: 'Marzo' }, { id: 4, value: 'Abril' }, { id: 5, value: 'Mayo' }, { id: 6, value: 'Junio' }, { id: 7, value: 'Julio' }, { id: 8, value: 'Agosto' }, { id: 9, value: 'Septiembre' }, { id: 10, value: 'Octubre' }, { id: 11, value: 'Noviembre' }, { id: 12, value: 'Diciembre' }],
        displayExpr: "value",
        valueExpr: "id",
        value:1
       

    }).dxSelectBox("instance");;
   


    var MotivoVisita1 = $("#MotivoVisita1").dxSelectBox({
        width: 500,
        dataSource: new DevExpress.data.DataSource({
            store: new DevExpress.data.CustomStore({
                key: "idMotivoVisita",
                loadMode: "raw",
                load: function () {
                    return $.getJSON($("#SIM").data("url") + "AtencionUsuarios/api/VisitaTerceroApi/MotivoVisita1async");
                }
            })
        }),
        displayExpr: "nombre",
        valueExpr: "idMotivoVisita",
        searchEnabled: true,

        onValueChanged: function (data) {
            IdMotivoVisita = data.value
        }
    }).dxValidator({
        validationGroup: "VisitaGroup",
        validationRules: [{
            type: "required",
            message: "Seleccione el motivo de la visita!"
        }]
    }).dxSelectBox("instance");


    //Popups
   

    var popupVisita = $("#PopupNuevo").dxPopup({
        width: 900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Visitas",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");

    var popupDireccion = $("#PopupDireccion").dxPopup({
        width: 1500,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Agregar dirección",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },

    }).dxPopup("instance");

    var popupReporte = $("#PopupReporte").dxPopup({
        width: 1900,
        height: "auto",
        dragEnabled: true,
        resizeEnabled: false,
        hoverStateEnabled: true,
        title: "Reporte Mes",
        onShowing: function () {
            $('body').css('overflow', 'hidden');
        },
        onHiding: function () {
            $('body').css('overflow', 'auto');
        },
    }).dxPopup("instance");
   
});

var VisitaTerceroDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FechaIngreso","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);
       
       
        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/VisitaTerceroApi/Visitas', {
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
            fecha: Hoy            
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});


var ReporteMesDataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();
        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"FechaIngreso","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";
        var skip = (typeof loadOptions.skip !== 'undefined' && loadOptions.skip !== null ? loadOptions.skip : 0);
        var take = (typeof loadOptions.take !== 'undefined' && loadOptions.take !== null ? loadOptions.take : 0);


        $.getJSON($('#SIM').data('url') + 'AtencionUsuarios/api/VisitaTerceroApi/ReporteMes', {
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
            mes: mesReporte,
            anno: annoReporte
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });

        }).fail(function (jqxhr, textStatus, error) {
            alert('Error cargando datos: ' + textStatus + ", " + jqxhr.responseText);
        });
        return d.promise();
    }
});


// Notas


/*  reporte.reset();                                              para poner en blanco los campos*/
/*  alert(MotivoVisita.option("value"));                          para mostrar alertas, del valor que lleva*/
/*  $('#MotivoVisita').dxValidator('instance').reset();           para limpiar los validadores al reiniciar el popup*/

