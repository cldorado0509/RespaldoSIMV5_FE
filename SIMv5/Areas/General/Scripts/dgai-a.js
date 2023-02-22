var myApp = angular.module('SIM', ['dx']);

//Globalize.culture("es");
//Globalize.locale(navigator.language || navigator.browserLanguage);

myApp.controller("DGAIndexController", function ($scope, $location, $http) {
    $('.my-cloak').removeClass('my-cloak');

    $scope.popDGASettings = {
        fullScreen: false,
        showTitle: true,
        title: 'DGA'
    };

    $scope.grdDGASettings = {
        dataSource: grdDGADataSource,
        allowColumnResizing: true,
        loadPanel: { text: 'Cargando Datos...' },
        noDataText: 'No Se Encontraron Registros',
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20, 50]
        },
        filterRow: {
            visible: true,
        },
        groupPanel: {
            visible: false,
        },
        editing: {
            editMode: 'row',
            editEnabled: false,
            removeEnabled: false,
            insertEnabled: false

        },
        initNewRow: function (rowInfo) {
            $scope.showTipoTercero();
        },
        onEditingStart: function (rowInfo) {
            /*window.open(window.location + '/Tercero/' + rowInfo.data.ID_TERCERO + '?vistaRetorno=true', '_blank');
            rowInfo.cancel = true;
            return;
            rowInfo.data.ID_TERCERO
            if (rowInfo.key) {
                rowInfo.cancel = true;
            }*/
        },
        selection: {
            mode: 'single'
        },
        columns: [
            {
                caption: 'Visualizar',
                width: '7%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    $('<img/>')//.addClass('glyphicon glyphicon-eye-open')
                        .attr('src', $('#app').data('url') + 'Content/Images/view.png')
                        .attr('width', '18px')
                        .attr('heiht', '20px')
                        .css('cursor', 'pointer')
                        .on('dxclick', function () {
                            window.location = $('#app').data('url') + 'General/DGA/DGA?id=' + options.data.ID_DGA;
                        })
                        .appendTo(container);
                }
            },
            {
                dataField: "ID_TERCERO",
                width: '8%',
                caption: 'Id Tercero',
                dataType: 'number',
            },
            {
                dataField: "S_TIPO_DOCUMENTO",
                width: '6%',
                caption: 'Tipo Doc',
                dataType: 'string',
            },
            {
                dataField: "N_DOCUMENTON",
                width: '10%',
                caption: 'Documento',
                dataType: 'number',
            },
            {
                dataField: 'S_TERCERO',
                width: '38%',
                caption: 'Empresa',
                dataType: 'string',
            },
            {
                dataField: 'ID_DGA',
                caption: 'Código',
                width: '8%',
                dataType: 'number',
            },
            {
                dataField: 'D_ANO',
                width: '7%',
                caption: 'Año',
                alignment: 'right',
                dataType: 'number',
            }, {
                dataField: 'S_ESTADO',
                width: '10%',
                caption: 'Estado',
                dataType: 'string',
            },
            {
                caption: 'Imprimir',
                width: '5%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.ID_ESTADO != 1) {
                        $('<img/>')//.addClass('glyphicon glyphicon-print')
                        .attr('src', $('#app').data('url') + 'Content/Images/print.png')
                        .attr('width', '28px')
                        .attr('heiht', '20px')
                        .css('padding-right', '10px')
                        .css('cursor', 'pointer')
                        .on('dxclick', function () {
                            /*popDGA.SetContentUrl($('#app').data('url') + 'General/DGA/PrintDGA?id=' + options.data.ID_DGA);
                            popDGA.Show();*/

                            var popDGA = $("#popDGA").dxPopup("instance");
                            popDGA.show();

                            $('#frmDGA').attr('src', null);
                            $('#frmDGA').attr('src', $('#app').data('url') + 'General/DGA/PrintDGA?id=' + options.data.ID_DGA);
                        })
                        .appendTo(container);
                    }
                    //if (options.data.ID_ESTADO == null || options.data.ID_ESTADO == 4) {
                    if (false) {
                        $('<img/>')
                        .attr('src', $('#app').data('url') + 'Content/Images/delete.png')
                        .attr('width', '28px')
                        .attr('heiht', '20px')
                        .css('padding-right', '10px')
                        .css('cursor', 'pointer')
                        .on('dxclick', function () {
                            var result = DevExpress.ui.dialog.confirm("Está Seguro de Anular el DGA Seleccionado?", "Confirmación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    $http.get($('#app').data('url') + 'General/api/DGAApi/AnularDGA?id=' + options.data.ID_DGA).success(function (data, status, headers, config) {
                                        if (data.tipoRespuesta === 'Error') {
                                            DevExpress.ui.dialog.alert(data.detalleRespuesta, 'Error');
                                        } else {
                                            /*popDGA.SetContentUrl($('#app').data('url') + 'General/DGA/PrintDGA?id=' + options.data.ID_DGA);
                                            popDGA.Show();

                                            $('#grdDGA').dxDataGrid('instance').refresh();*/

                                            var popDGA = $("#popDGA").dxPopup("instance");
                                            popDGA.show();

                                            $('#frmDGA').attr('src', null);
                                            $('#frmDGA').attr('src', $('#app').data('url') + 'General/DGA/PrintDGA?id=' + options.data.ID_DGA);
                                        }
                                    }).error(function (data, status, headers, config) {
                                        DevExpress.ui.dialog.alert('Error Procesando la Solicitud', 'Error');
                                    });
                                }
                            });
                        })
                        .appendTo(container);
                    } else if (options.data.ID_ESTADO == 6) {
                        $('<img/>')//.addClass('glyphicon glyphicon-plus-sign')
                        .attr('src', $('#app').data('url') + 'Content/Images/copiar.png')
                        .attr('width', '28px')
                        .attr('heiht', '20px')
                        .css('padding-right', '10px')
                        .css('cursor', 'pointer')
                        .on('dxclick', function () {
                            var result = DevExpress.ui.dialog.confirm("Está Seguro de Realizar una Copia del DGA Seleccionado?", "Confirmación");
                            result.done(function (dialogResult) {
                                if (dialogResult) {
                                    $http.get($('#app').data('url') + 'General/api/DGAApi/CopiarDGA?id=' + options.data.ID_DGA).success(function (data, status, headers, config) {
                                        if (data.tipoRespuesta === 'Error') {
                                            DevExpress.ui.dialog.alert(data.detalleRespuesta, 'Error');
                                        } else {
                                            DevExpress.ui.dialog.alert('DGA Copiado Satisfactoriamente.', 'Copia DGA');
                                            $('#grdDGA').dxDataGrid('instance').refresh();
                                            /*$('#grdDGA').dxDataGrid({
                                                dataSource: grdDGADataSource
                                            });*/
                                        }
                                    }).error(function (data, status, headers, config) {
                                        DevExpress.ui.dialog.alert('Error Procesando la Solicitud', 'Error');
                                    });
                                }
                            });
                        })
                        .appendTo(container);
                    }
                }
            }
        ],
    }
});

var grdDGADataSource = new DevExpress.data.CustomStore({
    load: function (loadOptions) {
        var d = $.Deferred();

        var filterOptions = loadOptions.filter ? loadOptions.filter.join(",") : "";
        var sortOptions = loadOptions.sort ? JSON.stringify(loadOptions.sort) : '[{"selector":"ID_TERCERO","desc":false},{"selector":"D_ANO","desc":true}]';
        var groupOptions = loadOptions.group ? JSON.stringify(loadOptions.group) : "";

        var skip = loadOptions.skip;
        var take = loadOptions.take;
        $.getJSON($('#app').data('url') + 'General/api/DGAApi/DGAs', {
            filter: filterOptions,
            sort: sortOptions,
            group: groupOptions,
            skip: skip,
            take: take,
            searchValue: '',
            searchExpr: '',
            comparation: '',
            tipoData: 'f',
            noFilterNoRecords: false
        }).done(function (data) {
            d.resolve(data.datos, { totalCount: data.numRegistros });
        }).fail(function (data) {
            d.resolve(null, { totalCount: 0 });
        });
        return d.promise();
    }
});