$(document).ready(function () {
    // Init
    $(".ui .checkbox").checkbox();
    $(".ui.loading.segment > div").hide();

    $("#btnAddSection").click(function () {
        AddContentDialog(
            "section",
            addSectionDialogLabels,
            "ask",
            parseInt($("#sectionId").val()),
            function (err) {
                if (!err) {
                    loadSectionContent(true);
                }
            }
        );
    });

    $("#btnAddPublication").click(function () {
        AddContentDialog(
            "publication",
            addPublicationDialogLabels,
            "ask",
            parseInt($("#sectionId").val()),
            function (err) {
                if (!err) {
                    loadSectionContent(true);
                }
            }
        );
    });

    $.ajax({
        url: "../SectionAdm/GetTree",
        dataType: "json",
        success: function (d) {
            let oldParentSectionId = parseInt($("#parentSectionIdOld").val());
            let isMain = Boolean($("#isMain").val());

            if (!d.error) {
                let dropDown = SectionTreeToDropdown(d.data, oldParentSectionId, parseInt($("#sectionId").val()));
                if (!addEnable || (!oldParentSectionId && isMain))
                    dropDown.addClass("disabled");
                $(".eight.wide:eq(0)").append(dropDown);
            }
            else
                console.error("Error", d.msg);
        },
        error: function (e) {
            console.error(e);
        }
    });

    loadSectionContent(true, addEnable);

    $("#btnSave").click(function () {
        $(this).addClass("loading");
        BlockInteraction(true, "btnSave");
        $("#frmNew").addClass("loading").form("validate form");
    });

    $("#frmNew").form({
        fields: {
            spanishTitle: {
                identifier: "spanishTitle",
                rules: [
                    { type: "empty", prompt: "Ingrese el nombre de la sección." },
                ]
            },
            position: {
                identifier: "position",
                optional: true,
                rules: [
                    { type: "integer[0...]", prompt: "El valor de posición debe ser un numero entero positivo." }
                ]
            },
            CssClass: {
                identifier: "CssClass",
                optional: true,
                rules: [
                    { type: "regExp[^\s*(-?[_a-zA-Z]+[_-a-zA-Z0-9]*\s*)*$]", prompt: "Escriba nombres de clase css válidos separados por espacios." }
                ] 
            },
            redirectTo: {
                identifier: "redirectTo",
                optional: true,
                rules: [
                    { type: "url", prompt: "Escriba una dirección de redirección válida." }
                ]
            }
        },
        onSuccess: function (e) {
            updateSection(function (err, data) {
                $("#btnSave,#frmNew").removeClass("loading");
                BlockInteraction(false, "btnSave");

                if (err || data.error) {
                    AddResultMessage(err.message || err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    //TODO: Ask if return
                }
            });
        },
        onFailure: function () {
            $("#btnSave,#frmNew").removeClass("loading");
            BlockInteraction(false, "btnSave");
        }
    });
});

function loadSectionContent(includePublications, canDelete) {
    includePublications = includePublications === undefined ? true : includePublications;

    $("#sectionContent > table").remove();
    $("#sectionContent > div").hide();
    $("#sectionContent").removeClass("placeholder").addClass("loading");

    $.ajax({
        url: "../SectionAdm/GetTree?sectionId=" + $("#sectionId").val() + "&includePublications=" + includePublications,
        dataType: "json",
        success: function (d) {
            if (!d.error) {
                if (d.data.children.length > 0) {
                    $("#sectionContent > div").hide();
                    $("#sectionContent").append(SectionTreeToTable(d.data, 0, canDelete, { publicationDelete: deletePublication, sectionDelete: deleteSection })).removeClass("loading").addClass("basic");
                } else {
                    $("#sectionContent > div").show();
                    $("#sectionContent").removeClass("loading").addClass("placeholder");
                }
            }
            else {
                console.error("Error: ", d.msg);
                $("#sectionContent > div").show();
                $("#sectionContent").removeClass("loading").addClass("placeholder");
            }
        },
        error: function (e) {
            console.error(e);
            $("#sectionContent > div").show();
            $("#sectionContent").removeClass("loading").addClass("placeholder");
        }
    });
}

function updateSection(callback) {
    $.ajax({
        type: "POST",
        url: $("#frmNew").attr("action"),
        data: $("#frmNew").serialize(),
        dataType: "json",
        success: function (data) {
            if (!data.error) {
                callback(null, data);
            } else {
                callback(data);
            }
        },
        error: function (error) {
            callback(error);
        }
    });
}

function deleteSection(sectionId, row, isMain) {

    if (isMain) {
        SimpleAlert("err", "Imposible eliminar", "No es posible eliminar una sección principal.");
        return;
    }

    row.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");

    SimpleDialog("warn", "Borrar sección", "Se borrará la sección. ¿Desea continuar?", function (approve, element) {
        if (approve) {
            $.ajax({
                type: "POST",
                url: "Delete",
                data: "sectionId=" + sectionId,
                dataType: "json",
                success: function (d) {
                    if (d.error) {
                        AddResultMessage(d.msg || d.message, "err");
                    } else {
                        AddResultMessage(d.msg || d.message, "ok");
                        loadSectionContent(true);
                    }
                    row.css("position", "auto");
                    row.children().last().remove();
                },
                error: function (e) {
                    AddResultMessage(d.msg || d.message || "Error desconocido.", "err");
                    row.css("position", "auto");
                    row.children().last().remove();
                }
            });

        } else {
            row.css("position", "auto");
            row.children().last().remove();
        }
    });
}

function deletePublication(publicationId, row, isMain) {

    if (isMain) {
        SimpleAlert("err", "Imposible eliminar", "No es posible eliminar una publicación principal.");
        return;
    }

    row.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");

    SimpleDialog("warn", "Borrar publicación", "Se borrará la publicación. ¿Desea continuar?", function (approve, element) {
        if (approve) {
            $.ajax({
                type: "POST",
                url: "../PublicationAdm/Delete",
                data: "publicationId=" + publicationId,
                dataType: "json",
                success: function (datos) {
                    if (datos.error) {
                        AddResultMessage(datos.msg, "err");
                    } else {
                        AddResultMessage(datos.msg, "ok");
                        loadSectionContent(true);
                    }
                    row.css("position", "auto");
                    row.children().last().remove();
                },
                error: function (error) {
                    AddResultMessage(error.message || "Error desconocido.", "err");
                    row.css("position", "auto");
                    row.children().last().remove();
                }
            });
        } else {
            row.css("position", "auto");
            row.children().last().remove();
        }
    });
}

function BlockInteraction(block, excludeButton) {
    let to_toggle = $("button:not('#" + excludeButton + "')");

    if (block) {
        to_toggle.prop("disabled", true);
        $("#sectionContentTable").find(".icon").addClass("grey");
    } else {
        to_toggle.prop("disabled", false);
        $("#sectionContentTable").find(".icon").removeClass("grey");
    }
}