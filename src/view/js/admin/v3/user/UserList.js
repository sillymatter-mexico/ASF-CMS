$(document).ready(function () {

    $(".ui .checkbox").checkbox();
    $(".ui .dropdown").dropdown();
    
    let usersTable = $("#userList").DataTable({
        ajax: {
            url: "../User/GetAllUsers",
            dataSrc: "rows"
        },
        rowId: "id",
        columns: [
            {
                data: "cell.2", title: "", name: "active", className: "collapsing", render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='small " + (data ? "green" : "grey") + " circle icon'></i>" : data;
                }
            },
            { data: "cell.0", title: "Nombre de usuario", name: "username" },
            { data: "cell.1", title: "Tipo", name: "Type", className: "collapsing" },
            {
                data: "none", title: "", name: "edit", orderable: false, className: "userEdit collapsing", render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='small blue edit icon' style='cursor: pointer'></i>" : 0; //"<a href='../User/Edit?username='" + row.id + "><i class='small blue edit icon'></i></a>" : 0;
                }
            }
        ],
        buttons: [{
            text: "<i class=\"plus icon\"></i>Agregar usuario",
            action: function (e, dt, node, config) {
                showUserModal("insert");
            }
        }],
        initComplete: function () {
            $(usersTable.table().container()).find('div.eight.column:eq(0)').append(usersTable.buttons().container());
        },
        rowCallback: function (row, data, displayNum, displayIndex, dataIndex) {
            $(row).find(".userEdit > i").click(function () {
                showUserModal("update", {
                    username: data.cell[0],
                    active: data.cell[2],
                    type: data.cell[1]
                });
            });
        }
    });

    $("#frmNewUsr").form({
        fields: {
            username: {
                identifier: "username",
                rules: [
                    { type: "empty", prompt: "Ingrese un nombre de usuario." },
                    { type: "latin1encoding", prompt: "El nombre de usuario contiene caracteres inválidos." },
                    { type: "minLength[3]", prompt: "El nombre de usuario debe tener al menos {ruleValue} caractéres de longitud." },
                    { type: "maxLength[20]", prompt: "El nombre de usuario debe tener un máximo de {ruleValue} caracteres de longitud." }
                ]
            },
            type: {
                identifier: "type",
                rules: [
                    { type: "empty", prompt: "Seleccione el perfil de usuario." }
                ]
            },
            password: {
                identifier: "password",
                depends: "requirePassword",
                rules: [
                    { type: "empty", prompt: "Ingrese una contraseña." },
                    { type: "latin1encoding", prompt: "La contraseña contiene caracteres inválidos." },
                    { type: "minLength[3]", prompt: "La contraseña debe tener al menos {ruleValue} caractéres de longitud." },
                    { type: "maxLength[20]", prompt: "La contraseña debe tener un máximo de {ruleValue} caracteres de longitud." }
                ]
            },
            passwordConfirm: {
                identifier: "passwordConfirm",
                depends: "password",
                rules: [
                    { type: "match[password]", prompt: "La contraseña no coincide con la confirmación." }
                ]
            }
        },
        onSuccess: function (e) {
            let panel = $("#userModalPanel");
            let action = panel.data("action");
            saveUserAction(action, function (err, data) {
                if (err) {
                    AddResultMessage(err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    usersTable.ajax.reload();
                }
                panel.find(".approve").removeClass("loading");
                panel.find(".deny").prop("disabled", false);
                panel.find(".content").dimmer("hide");
                panel.modal("hide");
            });
        },
        onFailure: function (e) {
            let panel = $("#userModalPanel");
            panel.find(".approve").removeClass("loading");
            panel.find(".deny").prop("disabled", false);
            panel.find(".content").dimmer("hide");
        },
        debug: {
            debug: true,
            form: "usuario"
        }

    });
});

function showUserModal(action, data) {
    switch (action) {
        case "update":
        case "edit":
            $("#frmNewUsr").form("set values", data);
            $("#requirePassword").val("");
            $("#oldUsername").val(data.username);
            $("#userModalPanel").data("action", "Update");
            break;
        case "insert":
        case "new":
        case "add":
            $("#frmNewUsr").form("clear");
            $("#requirePassword").val("yes");
            $("#userModalPanel").data("action", "Insert");
            break;
        default:
            break;
    }
    $("#userModalPanel").modal({
        closable: false,
        onApprove: function () {
            let panel = $("#userModalPanel");
            panel.find(".approve").addClass("loading");
            panel.find(".deny").prop("disabled", true);
            panel.find(".content").dimmer("show");
            panel.find("#frmNewUsr").form("validate form");
            return false;
        }
    });
    $("#userModalPanel").modal("show");
}

function saveUserAction(action, callback) {
    $.ajax({
        type: "POST",
        url: "../User/" + action,
        data: $("#frmNewUsr").serialize(),
        dataType: "json",
        success: function (d) {
            if (!d.error)
                callback(null, d);
            else
                callback(d);
        },
        error: function (e) {
            callback({ error: true, msg: e.msg || e.message || "Error desconocido", details: e });
        }
    });
}