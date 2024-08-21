$(document).ready(function () {
    $("#grdListaFuncionarios").dxDataGrid({
        dataSource: FuncionariosDataSource,
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
            { dataField: 'CODFUNCIONARIO', width: '7%', caption: 'ID', alignment: 'center' },
            { dataField: 'CEDULA', width: '7%', caption: 'Documento', dataType: 'string' },
            { dataField: 'NOMBRES', width: '15%', caption: 'Nombre', dataType: 'string' },
            { dataField: 'APELLIDOS', width: '15%', caption: 'Apellidos', dataType: 'string' },
            { dataField: 'CARGO', width: '15%', caption: 'Cargo', dataType: 'string' },
            { dataField: 'EMAIL', width: '15%', caption: 'Correo', dataType: 'string' },
            { dataField: 'OFICINA', width: '4%', caption: 'Oficina', dataType: 'string' },
            { dataField: 'EXTENSION', width: '4%', caption: 'Extension', dataType: 'string' },            
            { dataField: 'GRUPOTRABAJO', width: '15%', caption: 'Grupo Trabajo', dataType: 'string' },
            { dataField: 'ACTIVO', width: '3%', caption: 'Activo', dataType: 'string' },
            {
                alignment: 'center',
                caption: 'Ver',
                width: '60',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'user',
                        hint: 'Ver detalles del funcionario',
                        onClick: function (e) {
                            var _Ruta = $('#SIM').data('url') + "Seguridad/api/FuncionarioApi/DetalleFuncionario";
                            $.getJSON(_Ruta, { CodFuncionario: options.data.CODFUNCIONARIO })
                                .done(function (data) {
                                    if (data != null) {
                                        showFuncionario(data);
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
                caption: 'Firma',
                width: '60',
                cellTemplate: function (container, options) {
                    $('<div/>').dxButton({
                        icon: 'todo',
                        hint: 'Ver firma del funcionario',
                        onClick: function (e) {
                            popupFirmaFunc.show();
                        }
                    }).appendTo(container);
                }
            }
        ]
    });

    var popupDet = null;
    var showFuncionario = function (data) {
        Funcionario = data;
        if (popupDet) {
            popupDet.option("contentTemplate", popupDetalle.contentTemplate.bind(this));
        } else {
            popupDet = $("#PopupDetalleFunc").dxPopup(popupDetalle).dxPopup("instance");
        }
        popupDet.show();
    };

    var popupDetalle = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Detalle del Funcionario",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function (container) {
            var divIni = $("<div></div>");
            var Content = "<table class='table table-sm' style='font-size: 12px;'><thead><tr><th scope='col'>&nbsp;</th><th scope='col'>&nbsp;</th></tr></thead><tbody>";
            Content += "<tr><th scope='row'><b>Código del Funcionario :</b> </th><td>" + Funcionario.CodFuncionario + "</td></tr>";
            Content += "<tr><th scope='row'><b>Cédula : </b></th><td>" + Funcionario.Cedula + "</td></tr>";
            Content += "<tr><th scope='row'><b>Funcionario : </b></th><td>" + Funcionario.Nombre + ' ' + Funcionario.Apellido + "</td></tr>";
            Content += "<tr><th scope='row'><b>Cargo : </b></th><td>" + Funcionario.Cargo + "</td></tr>";
            Content += "<tr><th scope='row'><b>GrupoTarbajo : </b></th><td>" + Funcionario.GrupoTarbajo + "</td></tr>";
            Content += "<tr><th scope='row'><b>Oficina : </b></th><td>" + Funcionario.Oficina + "</td></tr>";
            Content += "<tr><th scope='row'><b>Extensión : </b></th><td>" + Funcionario.Extension + "</td></tr>";
            Content += "<tr><th scope='row'><b>Correo Electrónico : </b></th><td>" + Funcionario.Email + "</td></tr>";
            Content += "<tr><th scope='row'><b>Activo : </b></th><td>" + Funcionario.Estado + "</td></tr>";
            Content += "<tr><th scope='row'><b>Posee firma digital : </b></th><td>" + Funcionario.FirmaDigital + "</td></tr>";
            Content += "<tr><th scope='row'><b>Usuario firma digital : </b></th><td>" + Funcionario.UsrFirmaDigital + "</td></tr>";
            Content += "<tr><th scope='row'><b>Fecha vence firma : </b></th><td>" + Funcionario.FechaFirmaDigital + "</td></tr>";
            Content += "<tr><th scope='row'><b>Posee Firma : </b></th><td>" + Funcionario.PoseeFirma + "</td></tr>";
            Content += "<tr><th scope=´row´>&nbsp;</th><td>&nbsp;</td></tr>";
            divIni.html(Content);
            container.append(divIni);
            return container;
        }
    };

    var popupDatosFunc = $("#PopupNuevoFunc").dxPopup({
        width: 1100,
        height: 650,
        hoverStateEnabled: true,
        title: "Datos del funcionario",
        dragEnabled: true,
        onShown: function () {
            $("#txtRegistro").text(Ingreso);
        }
    }).dxPopup("instance");

    var popupFirmaFunc = $("#popupFirmaFunc").dxPopup({
        width: 1100,
        height: 650,
        hoverStateEnabled: true,
        title: "Firmsa del funcionario",
        dragEnabled: true
    }).dxPopup("instance");

    $('#firmaFuncionario').dxFileUploader({
        dialogTrigger: '#dropzone-external',
        dropZone: '#dropzone-external',
        multiple: false,
        allowedFileExtensions: ['.jpg', '.jpeg', '.gif', '.png'],
        uploadMode: 'instantly',
        uploadUrl: 'https://js.devexpress.com/Demos/NetCore/FileUploader/Upload',
        visible: false,
        onDropZoneEnter({ component, dropZoneElement, event }) {
            if (dropZoneElement.id === 'dropzone-external') {
                const items = event.originalEvent.dataTransfer.items;

                const allowedFileExtensions = component.option('allowedFileExtensions');
                const draggedFileExtension = `.${items[0].type.replace(/^image\//, '')}`;

                const isSingleFileDragged = items.length === 1;
                const isValidFileExtension = allowedFileExtensions.includes(draggedFileExtension);

                if (isSingleFileDragged && isValidFileExtension) {
                    toggleDropZoneActive(dropZoneElement, true);
                }
            }
        },
        onDropZoneLeave(e) {
            if (e.dropZoneElement.id === 'dropzone-external') { toggleDropZoneActive(e.dropZoneElement, false); }
        },
        onUploaded(e) {
            const { file } = e;
            const dropZoneText = document.getElementById('dropzone-text');
            const fileReader = new FileReader();
            fileReader.onload = function () {
                toggleDropZoneActive(document.getElementById('dropzone-external'), false);
                const dropZoneImage = document.getElementById('dropzone-image');
                dropZoneImage.src = fileReader.result;
            };
            fileReader.readAsDataURL(file);
            dropZoneText.style.display = 'none';
            uploadProgressBar.option({
                visible: false,
                value: 0,
            });
        },
        onProgress(e) {
            uploadProgressBar.option('value', (e.bytesLoaded / e.bytesTotal) * 100);
        },
        onUploadStarted() {
            toggleImageVisible(false);
            uploadProgressBar.option('visible', true);
        },
    });

    const uploadProgressBar = $('#upload-progress').dxProgressBar({
        min: 0,
        max: 100,
        width: '30%',
        showStatus: false,
        visible: false,
    }).dxProgressBar('instance');

    function toggleDropZoneActive(dropZone, isActive) {
        dropZone.classList.toggle('dropzone-active', isActive);
    }

    function toggleImageVisible(visible) {
        const dropZoneImage = document.getElementById('dropzone-image');
        dropZoneImage.hidden = !visible;
    }

    //document.getElementById('dropzone-image').onload = function () { toggleImageVisible(true); };
});

var FuncionariosDataSource = new DevExpress.data.CustomStore({
    key: "CODFUNCIONARIO",
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
        $.getJSON($('#SIM').data('url') + 'Seguridad/Api/FuncionarioApi/GetFuncionarios', params)
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
