
//Funcion para subir los documentos al grid 
function SubirDoC(idTramite, idRequisito, idTercero, idInstala) {
    $("#pantallaCargarImg").dialog(
    {
        width: 500,
        height: 200,
        modal: true,
        close: function () {
            //some code
        }
    });
    $("#fotos").attr("src", "../Tramites/NuevoTramite/SubirDocumento?idTramite=" + idTramite + "&idRequisito=" + idRequisito + "&idTercero=" + idTercero + "&idInstalacion=" + idInstala);


};

var idRequ = 0
var idInst = 0;
//Obtenemos la información del grid
//if ($scope.selectedTab == 3) {
function mostrarRequisitos(idTram, idInst, idRequ) {

    $.ajax({
        type: "POST",
        url: '../Tramites/NuevoTramite/GetRequisitosXTramite',
        data: { id: idTram, idInstalacion: idInst, idRequisito: idRequ }, //id del tramite
        success: function (response) {
            var obj = JSON.parse(response);
            gridRequisitos(obj);         

            //for (var i = 0; i < obj.length; i++) {
            //    if (obj[i].ID_ESTADO == 2) 
            //        parent.$("#cargaDocu" + obj[i].ID_REQUISITO).attr('src', '/SIM/Content/Images/aprobado.png');
            //}

        }, error: function (request, status, error) {
            alert(request.responseText);
        }

    });
}


//Se muestran los requisitos cargados
function mostrarRequisitosCargados(idTram, idInst, idRequ) {

    $.ajax({
        type: "POST",
        url: '../NuevoTramite/GetRequisitosXTramite',
        data: { id: idTram, idInstalacion: idInst, idRequisito: idRequ }, //id del tramite
        success: function (response) {
            var obj = JSON.parse(response);
            //var grid = parent.$("#GrdRequisitos").dxDataGrid('instance');
            //grid.refresh();
            gridRequisitos(obj);

        }, error: function (request, status, error) {
            alert(request.responseText);
        }

    });
}

//
//


//Ver el documento adjutado al requisito
function VerRequisito(idTram, idReq, idTerce, idInst) {

    window.open('NuevoTramite/AbrirDocumento?idTram=' + idTram + '&idReq=' + idReq + '&idTerce=' + idTerce + '&idInst=' + idInst);
}

//Eliminar el documento cargado
function EliminarRequisito(idTram, idReq, idTerce, idInst) {


    $.confirm({
        text: "¿Estas seguro que deseas eliminar el documento?",
        title: "Confirmar",
        confirm: function(button) {
            $.ajax({
                type: "POST",
                url: '../Tramites/NuevoTramite/DeleteRequisitoxTramite',
                data: { idTram: idTram, idReq: idReq, idTerce: idTerce, idInst: idInst },
                success: function (response) {

                    
                    alert('Documento eliminado correctamente.');

                    mostrarRequisitosCargados(idTram, idInst, idReq);
                   
                    parent.$("#cargaDocu" + idReq).attr('src', '/SIM/Content/Images/EditarVisita.png')

                }, error: function (request, status, error) {
                    alert(request.responseText);
                }
            });
        },
        confirmButton: "Si",
        cancelButton: "No",
        cancel: function(button) {
            // nothing to do
        },
       
    });

    //$.confirm({
    //    title: 'Confirmar!',
    //    content: '¿Estas seguro que deseas eliminar el documento?',
    //    buttons: {
    //        Aceptar: function () {
    //            $.ajax({
    //                type: "POST",
    //                url: '../Tramites/NuevoTramite/DeleteRequisitoxTramite',
    //                data: { idTram: idTram, idReq: idReq, idTerce: idTerce, idInst: idInst },
    //                success: function (response) {

    //                    $.alert({
    //                        title: '',
    //                        content: 'Documento eliminado correctamente.',
    //                    });
    //                    parent.$("#cargaDocu" + idReq).attr('src', '/SIM/Content/Images/EditarVisita.png')

    //                }, error: function (request, status, error) {
    //                    alert(request.responseText);
    //                }
    //            });
    //        },
    //        Cancelar: function () {

    //        }
    //    }
    //});



}


function gridRequisitos(jsonRequisitos) {

    var tercero = $('#app').data('idtercero');
    var instalacion = $('#app').data('idinstalacion');
    var idTramite = $('#app').data('idtramite');

    $("#GrdRequisitos").dxDataGrid({
        dataSource: jsonRequisitos,
        filterRow: { visible: false },
        columns: [
       { dataField: 'ID_REQUISITO', visible: false, allowGrouping: true, caption: '', width: '10%', allowFiltering: true, dataType: 'number', allowEditing: false },
       { dataField: 'REQUISITO', caption: 'Requisito', allowGrouping: true, width: '55%', dataType: 'string', allowEditing: false },
       { dataField: 'ID_ESTADO', visible: false, allowGrouping: true, caption: '', width: '10%', allowFiltering: true, dataType: 'number', allowEditing: false },
       { dataField: 'NOMBRE_ESTADO', caption: 'Estado',  alignment: 'center', allowGrouping: true, width: '15%', dataType: 'string', allowEditing: false },
       { dataField: 'ID_TRAMITE', visible: false, caption: '', allowGrouping: true, width: '20%', dataType: 'string', allowEditing: false },
         { dataField: 'FORMATO', visible: false, caption: '', allowGrouping: true, width: '20%', dataType: 'string', allowEditing: false },
          { dataField: 'ID_INSTALACION', visible: false, caption: '', allowGrouping: true, width: '20%', dataType: 'number', allowEditing: false },
             {
                 dataField: 'cargar', alignment: 'center', allowGrouping: true, caption: 'Cargar Documento', width: '10%', allowEditing: false, cellTemplate: function (container, options) {
                     if (options.data.ID_ESTADO != null && options.data.ID_ESTADO == 2)
                         {
                         $('<img src="/SIM/Content/Images/aprobado.png" style="width:25px;height:25px" class="btnGuardar" id="cargaDocu' + options.data.ID_REQUISITO + '" />')

                                     .attr('src', options.value)
                                     .appendTo(container);
                     }
                     else
                     {
                         $('<img src="/SIM/Content/Images/EditarVisita.png" style="width:25px;height:25px" class="btnGuardar" id="cargaDocu' + options.data.ID_REQUISITO + '" />')

                                   .attr('src', options.value)
                                   .appendTo(container);
                     }
                 }
             },
                       {
                           dataField: 'verDoc', alignment: 'center', allowGrouping: true, caption: 'Ver', width: '10%', allowEditing: false, cellTemplate: function (container, options) {

                               $('<img src="/SIM/Content/Images/Ver_Doc.png" style="width:25px;height:25px" class="btnGuardar" id="verDocureq' + options.data.ID_REQUISITO + 'tra' + options.data.ID_TRAMITE + 'terc' + tercero + '" />')

                                               .attr('src', options.value)
                                               .appendTo(container);
                           }
                       }
             , {
                 dataField: 'eliminar', alignment: 'center', allowGrouping: true, caption: 'Eliminar', width: '10%', allowEditing: false, cellTemplate: function (container, options) {

                     $('<img src="/SIM/Content/Images/delete.png" style="width:25px;height:25px" class="btnGuardar"  />')

                                 .attr('src', options.value)
                                 .appendTo(container);
                 }
             }

        ],
        height: 300,

        setCellValue: function (rowData, value) {
            rowData.ID_ESTADO = 2;
            rowData.NOMBRE_ESTADO = "Aprobado";
        },

        onCellClick: function (e) {

            var idTramite = e.data.ID_TRAMITE;
            var idRequisito = e.data.ID_REQUISITO;
            var idTercero = tercero
            var idInstala = e.data.ID_INSTALACION;
            var idEstado = e.ID_ESTADO;

            var tipoBoton = e.columnIndex;
            switch (tipoBoton) {

                case 2: //subir doc
                    SubirDoC(idTramite, idRequisito, idTercero, idInstala);
                    break;

                case 3: //ver  documento

                    VerRequisito(idTramite, idRequisito, idTercero, idInstala);

                    break;

                case 4: //eliminar documento

                    EliminarRequisito(idTramite, idRequisito, idTercero, idInstala);
                    break;

            }

        },
        scrolling: {
            mode: 'virtual',
            scrollByContent: false,
            scrollByThumb: false,
            showScrollbar: 'never'
        },

        //columnChooser: { enabled: false },
        //allowColumnReordering: true,
        //sorting: { mode: 'single' },
        //pager: { visible: true },
        //paging: { pageSize: 6 },
        //filterRow: { visible: false },
        onCellPrepared: function (cellInfo) {

            if (cellInfo.rowType == "data" && cellInfo.column.dataField === 'cargarDoc') {

                if (cellInfo.row.key.URL == "") {
                    cellInfo.cellElement.addClass('btnEditar');
                    cellInfo.column.allowEditing = false;

                } else {


                    cellInfo.cellElement.addClass('btnGuardar');

                }


            }
        }

    });

};

