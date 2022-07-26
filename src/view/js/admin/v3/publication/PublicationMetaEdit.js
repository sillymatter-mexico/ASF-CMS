function initMetaModal(elem, data, id) {
    elem.modal({
        closable: false,
        onApprove: function () {
            elem.find(".approve").addClass("loading");
            elem.find(".deny").prop("disabled", true);
            elem.find(".content").dimmer({ closable: false, variation: "inverted" }).dimmer("show");

            let rowDataObj = elem.find("#metaList").DataTable().rows().data();
            let rowDataArr = [];
            rowDataObj.each(function (val, index) { rowDataArr[index] = val; });

            updateMeta({ meta: JSON.stringify(rowDataArr), id: id }, function (err, data) {
                if (err) {
                    AddResultMessage(err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    metaTags = rowDataArr; //TODO: Find a better way
                }
                elem.find(".approve").removeClass("loading");
                elem.find(".deny").prop("disabled", false);
                elem.find(".content").dimmer("hide");
                elem.modal("hide");
            });
            return false;
        }
    });

    elem.find("#metaForm").form({
        fields: {
            metaType: {
                identifier: "metaType",
                rules: [
                    { type: "empty", prompt: "Seleccione un tipo de meta-tag." }
                ]
            },
            metaValue: {
                identifier: "metaValue",
                rules: [
                    { type: "empty", prompt: "Escriba el valor para el meta-tag." },
                    { type: "maxLength[300]", prompt: "El valor no puede tener más de {ruleValue} caracteres." }
                ]
            }
        },
        onSuccess: function (e, fields) {
            e.preventDefault();
            metaTable.rows.add([ { Type: fields.metaType, Value: fields.metaValue, Preview: "" } ]).draw();
        }
    });

    let metaTable = elem.find("#metaList").DataTable({
        data: data,
        columns: [
            { data: "Type", title: "Tipo", name: "type", className: "collapsing" },
            { data: "Value", title: "Valor", name: "value" },
            {
                data: "none", title: "", name: "delete", className: "collapsing delete", render: function (data, type, row, meta) {
                    return type == "display" ? "<button class=\"ui icon button\"><i class=\"small red trash icon\"></i></button>" : 0;
                }
            }
        ],
        searching: false,
        ordering: false,
        paging: false,
        info: false,
        language: {
            emptyTable: "<i class= \"big ban icon\"></i><br />No hay meta tags"
        },
        drawCallback: function () {
            $(this).find("td.delete button").click(function () {
                let idx = metaTable.cell($(this).parent()).index().row;
                metaTable.row(idx).remove().draw();
            });
        }
    });
}

function resetMetaModal(elem, data) {
    elem.find("#metaForm").removeClass("error").form("reset");
    let table = elem.find("#metaList").DataTable();
    table.clear();
    table.rows.add(data).draw();
}

function updateMeta(data, callback) {
    callback(null, true);
    $.ajax({
        type: "POST",
        url: "../PublicationAdm/UpdateMeta",
        data: $.param(data),
        dataType: "json",
        success: function (data) {
            if (data.error)
                callback(data);
            else
                callback(null, data)
        },
        error: function (error) {
            callback({ error: true, msg: error.message || "Error desconocido.", details: error });
        }
    });
}