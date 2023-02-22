var IdExpediente = -1;
var NomExpediente = "";
var IdIUnidadDoc = -1;
var IdTomo = -1;

$(document).ready(function () {
    $("#btnBuscarExp").dxButton({
        text: "Buscar Expediente",
        icon: "search",
        type: "default",
        width: "190",
        onClick: function () {
            var _popup = $("#popupBuscaExp").dxPopup("instance");
            _popup.show();
            $('#BuscarExp').attr('src', $('#SIM').data('url') + 'Utilidades/BuscarExpediente?popup=true');
        }
    });

    $("#btnIndices").dxButton({
        text: "Indices Expediente",
        icon: "fields",
        hint: 'Indices del documento',
        visible: false,
        onClick: function () {
            var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/IndicesExpediente";
            $.getJSON(_Ruta, { IdExp: IdExpediente })
                .done(function (data) {
                    if (data != null) {
                        showIndices(data);
                    }
                }).fail(function (jqxhr, textStatus, error) {
                    DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Indices del documento');
                });
        }
    });

    $("#cmdShowExpedienteFlip").dxButton({
        text: "Ver Expediente ...",
        icon: "selectall",
        hint: 'Ver el Expediente Documental',
        visible: false,
        onClick: function () {
            verExpediente($('#SIM').data('url') + 'GestionDocumental/Expedientes/FlipExpediente?idExp=' + IdExpediente + '&IdTomo=' + IdTomo);
        }
    });

    $("#popupBuscaExp").dxPopup({
        width: 900,
        height: 800,
        showTitle: true,
        title: "Buscar Expediente"
    });

    var popupInd = null;

    var showIndices = function (data) {
        Indices = data;
        if (popupInd) {
            popupInd.option("contentTemplate", popupOptInd.contentTemplate.bind(this));
        } else {
            popupInd = $("#PopupIndicesExp").dxPopup(popupOptInd).dxPopup("instance");
        }
        popupInd.show();
    };

    var popupOptInd = {
        width: 700,
        height: "auto",
        hoverStateEnabled: true,
        title: "Indices del Expediente",
        visible: false,
        closeOnOutsideClick: true,
        contentTemplate: function () {
            var Content = "";
            $.each(Indices, function (key, value) {
                Content += "<p>" + value.INDICE + " : <span><b>" + value.VALOR + "</b></span></p>";
            });
            return $("<div />").append(
                $("<p><b>Indices expediente " + NomExpediente + "</b></p>"),
                $("<br />"),
                Content
            );
        }
    };    
});

function verExpediente(ruta) {
    var w = 1500;
    var h = 800;
    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : window.screenX;
    var dualScreenTop = window.screenTop != undefined ? window.screenTop : window.screenY;
    var width = w;
    var height = h;
    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
    var top = ((height / 2) - (h / 2)) + dualScreenTop;
    var title = "";

    var newWindow = window.open(ruta, title, 'scrollbars=yes,resizable=1,status=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

    if (window.focus) {
        newWindow.focus();
    }
}

function SeleccionaExp(Expediente) {
    var _popup = $("#popupBuscaExp").dxPopup("instance");
    _popup.hide();
    if (Expediente != "") {
        IdExpediente = Expediente;
        $("#PanelDer").addClass("hidden");
        var _Ruta = $('#SIM').data('url') + "GestionDocumental/api/ExpedientesApi/NombreExpediente";
        $.getJSON(_Ruta, { IdExp: IdExpediente })
            .done(function (data) {
                if (data != "") {
                    NomExpediente = data;
                    $("#lblExpediente").text(NomExpediente);
                    $("#btnIndices").dxButton("instance").option("visible", true);
                    $("#dxTreeView").dxTreeView({
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.CustomStore({
                                key: "ID",
                                loadMode: "raw",
                                load: function () {
                                    return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ArbolExpediente", { IdExp: IdExpediente });
                                }
                            })
                        }),
                        dataStructure: "plain",
                        keyExpr: "ID",
                        displayExpr: "NOMBRE",
                        parentIdExpr: "PADRE",
                        width: '100%',
                        onItemClick: function (e) {
                            var item = e.itemData;
                            $("#cmdShowExpedienteFlip").dxButton("instance").option("visible", false);
                            if (item.DOCS) {
                                var valores = item.ID.split(".");
                                IdTomo = valores[2];
                                IdIUnidadDoc = valores[3];
                                $("#ListaDocs").dxList({
                                    dataSource: new DevExpress.data.DataSource({
                                        store: new DevExpress.data.CustomStore({
                                            key: "Documento",
                                            loadMode: "raw",
                                            load: function () {
                                                return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ObtieneDocs", { IdUniDoc: IdIUnidadDoc, IdTomo: IdTomo });
                                            }
                                        })
                                    }),
                                    height: "100%",
                                    width: "100%",
                                    allowItemDeleting: false,
                                    itemDeleteMode: "toggle",
                                    showSelectionControls: true,
                                    scrollingEnabled: true,
                                    itemTemplate: function (data, index) {
                                        var divP = $("<div>");
                                        var div1 = $("<div>").addClass("image-container").appendTo(divP);
                                        $("<img>").attr("src", $('#SIM').data('url') + "Content/imagenes/doc.png").appendTo(div1);
                                        var div2 = $("<div>").addClass("info").appendTo(divP);
                                        $("<div>").text("Documento: " + data.Documento).appendTo(div2);
                                        $("<div>").text(data.Datos.substring(0, 30) + ' ...').appendTo(div2);
                                        $("<div>").text("Fecha digitliza: " + data.Fecha).appendTo(div2);
                                        return divP;
                                    },
                                    onContentReady: function (e) {
                                        var listitems = e.element.find('.dx-item');
                                        var tooltip = $('#tooltip').dxTooltip({
                                            width: 500,
                                            contentTemplate: function (contentElement) {
                                                contentElement.append(
                                                    $("<p style='indices'/>").text(contentElement.text)
                                                )
                                            }
                                        }).dxTooltip('instance');
                                        listitems.on('dxhoverstart', function (args) {
                                            tooltip.content().text($(this).data().dxListItemData.Datos);
                                            tooltip.show(args.target);
                                        });

                                        listitems.on('dxhoverend', function () {
                                            tooltip.hide();
                                        });
                                    },
                                    onItemClick: function (e) {
                                        var item = e.itemData;
                                        window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + item.Documento, "Documento " + item.Documento, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                                    }
                                });
                                $("#PanelDer").removeClass("hidden");
                            } else if (item.TOMO) {
                                var valores = item.ID.split(".");
                                IdTomo = valores[2];
                                $("#cmdShowExpedienteFlip").dxButton("instance").option("visible", true);
                                $("#ListaDocs").dxList({
                                    dataSource: new DevExpress.data.DataSource({
                                        store: new DevExpress.data.CustomStore({
                                            key: "Documento",
                                            loadMode: "raw",
                                            load: function () {
                                                return $.getJSON($("#SIM").data("url") + "GestionDocumental/api/ExpedientesApi/ObtieneDocs", { IdTomo: IdTomo });
                                            }
                                        })
                                    }),
                                    height: "100%",
                                    width: "100%",
                                    allowItemDeleting: false,
                                    itemDeleteMode: "toggle",
                                    showSelectionControls: true,
                                    scrollingEnabled: true,
                                    itemTemplate: function (data, index) {
                                        var divP = $("<div>");
                                        var div1 = $("<div>").addClass("image-container").appendTo(divP);
                                        $("<img>").attr("src", $('#SIM').data('url') + "Content/imagenes/doc.png").appendTo(div1);
                                        var div2 = $("<div>").addClass("info").appendTo(divP);

                                        $("<div>").text("Documento: " + data.Documento).appendTo(div2);
                                        $("<div>").html("<p style='indices'>" + data.Datos + "</p>").addClass("divInd").appendTo(div2);
                                        $("<div>").text("Fecha digitliza: " + data.Fecha).appendTo(div2);
                                        return divP;
                                    },
                                    onContentReady: function (e) {
                                        var listitems = e.element.find('.dx-item');
                                        var tooltip = $('#tooltip').dxTooltip({
                                            width: 500,
                                            contentTemplate: function (contentElement) {
                                                contentElement.append(
                                                    $("<p style='indices'/>").text(contentElement.text)
                                                )
                                            }
                                        }).dxTooltip('instance');
                                        listitems.on('dxhoverstart', function (args) {
                                            tooltip.content().text($(this).data().dxListItemData.Datos);
                                            tooltip.show(args.target);
                                        });

                                        listitems.on('dxhoverend', function () {
                                            tooltip.hide();
                                        });
                                    },
                                    onItemClick: function (e) {
                                        var item = e.itemData;
                                        window.open($('#SIM').data('url') + "Utilidades/LeeDoc?IdDocumento=" + item.Documento, "Documento " + item.Documento, "width= 900,height=800,scrollbars = yes, location = no, toolbar = no, menubar = no, status = no");
                                    }

                                });
                                $("#PanelDer").removeClass("hidden");
                            } else {
                                $("#PanelDer").addClass("hidden");
                            }
                        }
                    });
                } 
            }).fail(function (jqxhr, textStatus, error) {
                DevExpress.ui.dialog.alert('Ocurrió un error ' + textStatus + ' ' + error + ' ' + jqxhr.responseText, 'Asociar documento');
            }
            );
    } else alert("No se ha ingresado el codifo del expediente");
}
