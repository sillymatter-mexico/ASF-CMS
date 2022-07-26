$(document).ready(function() {

    var publicationsTable = $("#publicationList").DataTable({
        ajax: {
            url: "../PublicationAdm/GetAllPublications?u=" + $("#hUname").val(),
            dataSrc: "rows"
        },
        rowId: "id",
        columns: addEnable ? [...asfCms.publication.colDefs, asfCms.publication.deleteColDef] : asfCms.publication.colDefs,
        order:[[1, "desc"]],
        lengthChange: false,
        pageLength: 100,
        lengthMenu: [[100, 200, 500, -1], [100, 200, 500, "-Todos-"]],
        buttons: addEnable ?
            [{
                text: "<i class=\"plus icon\"></i>Agregar publicación",
                action: function (e, dt, node, config) {
                    AddContentDialog(
                        "publication",
                        addPublicationDialogLabels,
                        "navigate"
                    );
                }
            }] : [],
        language: {
            url: "/view/lib/DataTables/i18n/es_mx.json"
        }
    });

    $("#publicationList").on("init.dt", function () {
        console.log("Publication table inited");

        $(publicationsTable.table().container()).find('div.eight.column:eq(0)').append(publicationsTable.buttons().container());

        $("#publicationList").on("click", "td.publicationMap", function () {
            let cell = publicationsTable.cell(this);
            let rowId = publicationsTable.row(cell.index().row).id();
            let tr = $(publicationsTable.rows(cell.index().row).nodes());

            tr.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");

            switchMapEnable(rowId, function (err, data) {
                if (err) {
                    AddResultMessage(err.message || err.msg, "err");
                } else {
                    cell.data(data.modifiedValue).draw();
                    AddResultMessage(data.msg, "ok");
                }
                tr.css("position", "auto");
                tr.children().last().remove();
            });
        });

        $("#publicationList").on("click", "td.publicationDelete", function () {
            let cell = publicationsTable.cell(this);
            let isMain = publicationsTable.row(cell.index().row).data().cell[0];

            if (isMain) {
                SimpleAlert("err", "Imposible eliminar.", "No se puede eliminar una publicación principal.");
                return;
            }


            SimpleDialog("warn", "Eliminar publicación", "Se borrará la publicación. ¿Desea continuar?", function (result, element) {
                if (!result)
                    return;
                let rowId = publicationsTable.row(cell.index().row).id();
                let tr = $(publicationsTable.rows(cell.index().row).nodes());

                tr.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");

                deletePublication(rowId, function (err, data) {
                    if (err || data.error) {
                        AddResultMessage(err.message || data.msg, "err");
                    } else {
                        publicationsTable.row(cell.index().row).remove().draw();
                        AddResultMessage(data.msg, "ok");
                    }
                    tr.css("position", "auto");
                    tr.children().last().remove();
                });
            });
        });
    });
});

function deletePublication(publicationId, callback) {
    $.ajax({
        type: "POST",
        url: "../PublicationAdm/Delete",
        data: "publicationId=" + publicationId,
        success: function (datos) {
            if (datos.error)
                callback(datos);
            else
                callback(null, datos);
        },
        error: function (err) {
            callback({
                error: true,
                msg: err.message || "Error desconocido",
                details: err
            });
        }
    });
}
function switchMapEnable(publicationId, callback) {
    $.ajax({
        type: "POST",
        url: "SwitchMapExclude",
        data: "publicationId=" + publicationId,
        dataType: "json",
        success: function (datos) {
            if (datos.error)
                callback(datos)
            else
                callback(null, datos);
        },
        error: function (err) {
            callback({ error: true, msg: err.message || "Error desconocido", details: err });
        }
    });
}