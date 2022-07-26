function initFilesModal(elem, id) {

    elem.modal({
        closable: false,
        onHide: function () {
            if (filesModified.length > 0) {
                elem.find(".content").dimmer("show");
                SimpleDialog("warn", "Modificaciones sin guardar", "Existen modificaciones a los títulos de los archivos que no se han guardado. Al cerrar estos cambios se peredrán. ¿Desea continuar?", function (r, ele) {
                    if (r) {
                        filesModified = [];
                        elem.modal("hide all");
                    }
                    elem.find(".content").dimmer("hide");
                });
                return false;
            } else {
                return true;
            }
        },
        onHidden: function () {
            elem.find("#filesList tr").removeClass("left marked yellow red");
        }
    });

    let filesTable = elem.find("#filesList").DataTable({
        ajax: {
            url: "../File/ListByPublication?publicationId=" + id,
            dataSrc: "rows"
        },
        rowId: "id",
        columns: [
            {
                data: "cell.0", title: "", name: "isMain", orderable: false, className: "collapsing main", render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='" + (data ? "yellow" : "grey") + " star " + (data ? "" : "outline ") + "icon' style=\"cursor: pointer\"></i>" : data;
                }
            },
            {
                data: "cell.1", title: "Nombre", name: "filename", className: "collapsing" },
            { data: "cell.2", title: "MIME", name: "mime", className: "collapsing" },
            {
                data: "cell.3", title: "Título", name: "title", render: function (data, type, row, meta) {
                    return type == "display" ? "<div class='ui fluid input'><input type='text' id='fileInput_" + row.id + "' value='" + data + "'></div>" : data;
                }
            },
            {
                data: "none", title: "", name: "save", orderable: false, className: "collapsing save", render(data, type, row, meta) {
                    return type == "display" ? "<button class='ui icon button'><i class='save icon'></i></button>" : 0;
                }
            }
        ],
        autoWidth: false,
        aaSorting: [],
        searching: false,
        paging: false,
        info: false,
        language: {
            emptyTable: "<i class=\"big ban icon\"></i><br />No hay archivos"
        },
        drawCallback: function () {
            $(this).find("td.main").unbind("click");
            $(this).find("td.main").click(function () {
                let idx = filesTable.cell($(this)).index().row;
                let fileId = filesTable.row(idx).id();
                toggleMain(fileId, id, function (err, data) {
                    if (!err) {
                        filesTable.ajax.reload();
                    } else {
                        console.error(err);
                    }
                });
            });
            $(this).find("td input").unbind("blur");
            $(this).find("td input").blur(function () {
                let idx = filesTable.cell($(this).parent().parent()).index().row;
                let id = filesTable.row(idx).id();
                let value = $(this).val().trim();
                let dataValue = filesTable.row(idx).data().cell[3];
                if (!filesModified.includes(id) && value != dataValue) {
                    filesModified.push(id);
                    let row = $(this).parent().parent().parent();
                    row.removeClass("red green").addClass("left marked yellow");
                }
            });
            $(this).find("td.save button").unbind("click");
            $(this).find("td.save button").click(function () {
                let row = $(this).parent().parent();
                let value = row.find("input").val().trim();
                let id = filesTable.row(row).data().id;
                row.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");
                updateFileTitle(id, value, function (err, data) {
                    if (!row.hasClass("marked"))
                        row.addClass("left marked");
                    if (!err) {
                        row.removeClass("yellow red").addClass("green");
                        filesModified.splice(filesModified.indexOf(id), 1);
                    } else {
                        row.remove("yellow green").addClass("red");
                    }
                    row.css("position", "auto");
                    row.children().last().remove();
                });
            });
        }
    });
}

function resetFilesModal(elem) {
    elem.find("#filesList").DataTable().ajax.reload();
}

function toggleMain(fileId, publicationId, callback) {
    $.ajax({
        type: "POST",
        url: "../File/ToggleMain",
        data: "fileId=" + fileId + "&publicationId=" + publicationId,
        dataType: "json",
        success: function (d) {
            if (d.error)
                callback(d);
            else
                callback(null, d);
        },
        error: function (e) {
            callback({ error: true, msg: e.msg || e.message || "Error desconocido.", details: e });
        }
    });
}

function updateFileTitle(fileId, title, callback) {
    $.ajax({
        type: "POST",
        url: "../File/UpdateTitle",
        data: "title=" + title + "&fileId=" + fileId,
        dataType: "json",
        success: function (d) {
            if (d.error)
                callback(d);
            else
                callback(null, d);
        },
        error: function (e) {
            callback({ error: true, msg: e.msg || e.message || "Error desconocido.", details: e });
        }
    });
}