$(document).ready(function () {

    window.history.pushState({ "html": "Administración de secciones", "pageTitle": "Administración de secciones" }, "", window.location.protocol + "//" + window.location.host + "/SectionAdm/List");

    var sectionTable = $("#sectionList").DataTable({
        ajax: {
            url: "../SectionAdm/GetAllSections?u=" + $("#hUname").val(),
            dataSrc: "rows"
        },
        rowId: "id",
        columns: [
            { data: "cell.0", title: "Titulo", name: "Titulo", responsivePriority: 1 },
            { data: "cell.2", title: "Redirección", name: "Redirección", responsivePriority: 2 },
            {
                data: "cell.3", orderable: false, className: "sectionEdit collapsing", render: function (data, type, row, meta) {
                    return type == "display" ? "<a href='#'><i class='small edit icon'></i></a>" : 0;
                },
                responsivePriority: 1
            },
            {
                data: "cell.4", orderable: false, className: "sectionMap collapsing", render: function (data, type, row, meta) {
                    return type == "display" ? "<a href='#'><i class='small " + (data ? "yellow " : "green ") + "circle icon'></i></a>" : data;
                },
                responsivePriority: 1
            },
            addEnable ? {
                data: "cell.5", orderable: false, className: "sectionDelete collapsing", render: function (data, type, row, meta) {
                    return type == "display" ? "<a href='#'><i class='small red trash icon'></i></a>" : 0;
                },
                responsivePriority: 1
            } : addEnable
        ].filter(Boolean),
        lengthChange:false,
        pageLength: 200,
        lengthMenu: [[100, 200, 500, -1], [100, 200, 500, "-Todos-"]],
        buttons: addEnable ?
            [{
                text: "<i class=\"plus icon\"></i>Agregar sección",
                action: function (e, dt, node, config) {
                    AddContentDialog(
                        "section",
                        addSectionDialogLabels,
                        "navigate"
                    );
                }
            }] : [],
        language: {
            url: "/view/lib/DataTables/i18n/es_mx.json"
        }
    });

    $("#sectionList").on("init.dt", function() {
        console.log("Table fully inited.");

        $(sectionTable.table().container()).find('div.eight.column:eq(0)').append(sectionTable.buttons().container());

        $("#sectionList").on("click", "td.sectionEdit", function () {
            let rowId = sectionTable.row(sectionTable.cell(this).index().row).id();

            window.location.href = "../SectionAdm/Edit?sectionId=" + rowId + "&u=" + $("#hUname").val();
        });
        $("#sectionList").on("click", "td.sectionMap", function () {
            if (!addEnable)
                return;

            let cell = sectionTable.cell(this);
            let rowId = sectionTable.row(cell.index().row).id();

            let tr = $(sectionTable.rows(cell.index().row).nodes());
            
            tr.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");

            switchMapEnable(rowId, function (err, datos) {
                if (err || datos.error) {
                    AddResultMessage(err.message || datos.msg, "err");
                } else {
                    cell.data(datos.modifiedValue).draw();
                    AddResultMessage(datos.msg, "ok");
                }
                tr.css("position", "auto");
                tr.children().last().remove();
            });
        });

        $("#sectionList").on("click", "td.sectionDelete", function () {
            let cell = sectionTable.cell(this);
            let data = cell.row(cell.index().row).data();

            let hasParent = data.cell[6];
            let isMain = data.cell[7];

            if (!hasParent && isMain) {
                SimpleAlert("err", "Imposible eliminar", "No es posible eliminar una sección raiz.");
                return;
            }

            SimpleDialog("warn", "Eliminar sección", "Se borraran todas las publicaciones y subsecciones de la seccion. ¿Desea continuar?", function (result, element) {
                if (!result)
                    return;
                let rowId = sectionTable.row(cell.index().row).id();
                let tr = $(sectionTable.rows(cell.index().row).nodes());

                tr.css("position", "relative").append("<div class=\"ui dimmer active\"><div class=\"ui loader\"></div></div>");

                deleteSection(rowId, function (err, data) {
                    if (err || data.error) {
                        AddResultMessage(err.message || data.msg, "err");
                    } else {
                        sectionTable.row(cell.index().row).remove().draw();
                        AddResultMessage(data.msg, "ok");
                    }
                    tr.css("position", "auto");
                    tr.children().last().remove();
                });
            });
        });
    });
});

function deleteSection(sectionId, callback) {
    $.ajax({
        type: "POST",
        url: "Delete",
        data: "sectionId=" + sectionId,
        dataType: "json",
        success: function (d) {
            callback(null, d);
        },
        error: function (e) {
            callback(e);
        }
    });
}

function switchMapEnable(sectionId, callback) {
    $.ajax({
        type: "POST",
        url: "SwitchMapExclude",
        data: "sectionId=" + sectionId,
        dataType: "json",
        success: function (d) {
            callback(null, d);
        },
        error: function (e) {
            callback(e);
        }
    });
}