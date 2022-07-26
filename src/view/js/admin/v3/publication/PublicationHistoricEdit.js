function initHistoricModal(elem, elem2, id) {
    elem.find(".menu .item").tab();

    elem.modal({
        closable: false,
        allowMultiple: true
    });

    elem2.modal({
        closable: false,
        allowMultiple: true,
        onApprove: function () {
            elem2.find(".content").dimmer();
            elem2.find(".approve").addClass("loading");

            restoreHistoric(elem2.find("input"), function (err, data) {
                if (!err) {
                    window.location.reload(true);
                } else {
                    elem2.find(".content").dimmer("hide").prepend("<div>" + err.msg + "</div>");
                    elem2.find(".approve").removeClass("loading");
                }
            });
            return false;
        }
    });

    let historicTable = elem.find("#historicList").DataTable({
        ajax: {
            url: '../PublicationAdm/GetHistoricPublications?Id=' + id,
            dataSrc: "rows"
        },
        rowId: "id",
        columns: [
            { data: "cell.0", title: "Fecha", name: "date" },
            { data: "cell.1", title: "Título", name: "title" },
            {
                data: "none", title: "", name: "recover", className: "collapsing restore", render: function (data, type, row, meta) {
                    return type == "display" ? "<button class=\"ui icon button\"><i class=\"history icon\"></i></button>" : 0;
                }
            }
        ],
        autoWidth: false,
        ordering: false,
        searching: false,
        paging: false,
        info: false,
        select: {
            info: false,
            style: "single",
            className: "left marked blue"
        },
        rowCallback: function (row, data, displayNum, displayIdx, dataIdx) {
            $(row).find("td.restore > button").click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                elem2.find("#historicPublicationId").val(historicTable.row(row).id());
                elem2.modal("show");
            });
        },
        language: {
            emptyTable: "<i class=\"big ban icon\"></i><br />No hay históricos"
        }
    });

    elem2.find(".checkbox").on("change", function () {
        let fields = elem2.find("input:checked");
        if (fields.length > 0) {
            elem2.find(".approve").removeClass("disabled");
        } else {
            elem2.find(".approve").addClass("disabled");
        }
    });

    historicTable.on("select", function (e, dt, type, indexes) {
        if (indexes.length > 0) {
            console.log("indice de datos: " + indexes[0]);
            let data = historicTable.row(indexes[0]).data();
            $("#historicContent").removeClass("center aligned").html(data.cell[2]);
            $("#historicNews").removeClass("center aligned").html(data.cell[3]);
        }
    });

    historicTable.on("deselect", function (e, dt, type, indexes) {
        $("#historicContent").addClass("center aligned").html("<i class=\"big ban icon\"></i><br />No hay contenido");
        $("#historicNews").addClass("center aligned").html("<i class=\"big ban icon\"></i><br />No hay contenido");
    });
}

function resetHistoricModal(elem, elem2, id) {
    $("#historicContent").addClass("center aligned").html("<i class=\"big ban icon\"></i><br />No hay contenido");
    $("#historicNews").addClass("center aligned").html("<i class=\"big ban icon\"></i><br />No hay contenido");
    let table = elem.find("#historicList").DataTable();
    table.rows({ selected: true }).deselect();
}

function restoreHistoric(fields, callback) {
    $.ajax({
        type: "POST",
        url: "../PublicationAdm/RestoreHistoricPublication",
        data: $(fields).serialize(),
        dataType: "json",
        success: function (data) {
            if (!data.error) {
                callback(null, data);
            } else {
                callback(e);
            }
        },
        error: function (e) {
            callback({ error: true, msg: e.message || e.msg || "Error desconocido", details: e });
        }
    });
}