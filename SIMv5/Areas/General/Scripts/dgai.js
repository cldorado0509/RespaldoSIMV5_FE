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
            insertEnabled: true

        },
        onInitNewRow: function (rowInfo) {
            rowInfo.cancel = true;
            window.location = $('#app').data('url') + 'General/DGA/DGA';
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
                caption: 'Editar / Visualizar',
                width: '20%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.D_FREPORTE == null) {
                        $('<img/>')//.addClass('glyphicon glyphicon-pencil')
                            .attr('src', $('#app').data('url') + 'Content/Images/edit.png')
                            .attr('width', '18px')
                            .attr('heiht', '20px')
                            .css('cursor', 'pointer')
                            .on('dxclick', function () {
                                //alert(JSON.stringify(options.data));
                                window.location = $('#app').data('url') + 'General/DGA/DGA?id=' + options.data.ID_DGA;
                            })
                            .appendTo(container);
                    } else {
                        $('<img/>')//.addClass('glyphicon glyphicon-eye-open')
                            .attr('src', $('#app').data('url') + 'Content/Images/view.png')
                            .attr('width', '18px')
                            .attr('heiht', '20px')
                            .css('cursor', 'pointer')
                            .on('dxclick', function () {
                                //alert(JSON.stringify(options.data));
                                window.location = $('#app').data('url') + 'General/DGA/DGA?id=' + options.data.ID_DGA;
                            })
                            .appendTo(container);
                    }
                }
            },
            {
                dataField: 'ID_DGA',
                caption: 'Código',
                width: '20%',
                dataType: 'number',
            },
            {
                dataField: 'D_ANO',
                width: '20%',
                caption: 'Año',
                alignment: 'right',
                dataType: 'number',
            }, {
                dataField: 'S_ESTADO',
                width: '20%',
                caption: 'Estado',
                dataType: 'string',
            },
            {
                caption: 'Enviar / Imprimir',
                width: '20%',
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.data.ID_ESTADO == 1) {
                        $('<img/>')//.addClass('glyphicon glyphicon-new-window')
                            .attr('src', $('#app').data('url') + 'Content/Images/send.png')
                            .attr('width', '18px')
                            .attr('heiht', '20px')
                            .css('cursor', 'pointer')
                            .on('dxclick', function () {
                                var result = DevExpress.ui.dialog.confirm("Está Seguro de Enviar el DGA Seleccionado?", "Confirmación");
                                result.done(function (dialogResult) {
                                    if (dialogResult) {
                                        $http.get($('#app').data('url') + 'General/api/DGAApi/SendDGA?id=' + options.data.ID_DGA).success(function (data, status, headers, config) {
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
                    } else {
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