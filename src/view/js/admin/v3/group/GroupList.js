$.ajax({
    url: "../SectionAdm/GetTree",
    dataType: "json",
    success: function (d) {
        if (!d.error) {
            let dd = SectionTreeToDropdown(d.data, 0, 0, false, true);
            $("#groupSectionsModalPanel > .content").append(dd);
        } else {
            console.error("Error", d.msg);
        }
    },
    error: function (e) {
        console.error(e);
    }
});

$(document).ready(function () {
    let addEnable = true;

    $("#groupAddModalPanel").modal({
        closable: false,
        onApprove: function () {
            $("#groupAddModalPanel .approve").addClass("loading");
            $("#groupAddModalPanel .deny").addClass("disabled");
            $("#groupAddModalPanel .content").dimmer("show");

            let name = $("#groupAddModalPanel input[name='groupName']").val();

            addGroup(name, function (err, data) {
                if (err) {
                    AddResultMessage(err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    groupTable.ajax.reload();
                }
                $("#groupAddModalPanel .approve").removeClass("loading");
                $("#groupAddModalPanel .deny").removeClass("disabled");
                $("#groupAddModalPanel .content").dimmer("hide");
                $("#groupAddModalPanel").modal("hide");
            });

            return false;
        }
    });

    $("#groupSectionsModalPanel").modal({
        closable: false,
        onApprove: function () {
            $("#groupSectionsModalPanel .approve").addClass("loading");
            $("#groupSectionsModalPanel .deny").addClass("disabled");
            $("#groupSectionsModalPanel .content").dimmer("show");

            let data = {
                group_id: $("#groupSectionsModalPanel input[name='group_id']").val(),
                sections: $("#groupSectionsModalPanel input[name='parentSectionId']").val()
            };

            setSections(data, function (err, data) {
                if (err) {
                    AddResultMessage(err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    groupTable.ajax.reload();
                }
                $("#groupSectionsModalPanel .approve").removeClass("loading");
                $("#groupSectionsModalPanel .deny").removeClass("disabled");
                $("#groupSectionsModalPanel .content").dimmer("hide");
                $("#groupSectionsModalPanel").modal("hide");
            });

            return false;
        }
    });

    $("#groupUsersModalPanel").find(".ui.dropdown").dropdown();

    $("#groupUsersModalPanel").modal({
        closable: false,
        onApprove: function () {
            $("#groupUsersModalPanel .approve").addClass("loading");
            $("#groupUsersModalPanel .deny").addClass("disabled");
            $("#groupUsersModalPanel .content").dimmer("show");

            let data = {
                group_id: $("#groupUsersModalPanel input[name='group_id']").val(),
                groupUsers: $("#groupUsersModalPanel [name='users']").val()
            };

            setUsers(data, function (err, data) {
                if (err) {
                    AddResultMessage(err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    groupTable.ajax.reload();
                }
                $("#groupUsersModalPanel .approve").removeClass("loading");
                $("#groupUsersModalPanel .deny").removeClass("disabled");
                $("#groupUsersModalPanel .content").dimmer("hide");
                $("#groupUsersModalPanel").modal("hide");
            });

            return false;
        }
    });

    let groupTable = $("#groupList").DataTable({
        ajax: {
            url: "../Group/GetAllGroups",
            dataSrc: "rows"
        },
        rowId: "id",
        columns: [
            { data: "cell.0", title: "Grupo", name: "name", responsivePriority: 1 },
            { data: "cell.1", title: "# Secciones", name: "sectionNumber", className: "collapsing", responsivePriority: 5 },
            { data: "cell.2", title: "# Usuarios", name: " userNumber", className: "collapsing", responsivePriority: 5 },
            {
                data: "none", title: "", name: "groupEdit", className: "collapsing groupEdit", orderable: false, render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='blue edit icon' style='cursor: pointer'></i>" : 0;
                },
                responsivePriority: 1
            },
            {
                data: "none", title: "", name: "groupUsersEdit", className: "collapsing userEdit", orderable: false, render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='users icon' style='cursor: pointer'></i>" : 0;
                },
                responsivePriority: 1
            },
            {
                data: "none", title: "", name: "groupDelete", className: "collapsing groupDelete", orderable: false, render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='red trash icon' style='cursor: pointer'></i>" : 0;
                },
                responsivePriority: 1
            }
        ],
        buttons: addEnable ? [{
            text: "<i class=\"plus icon\"></i> Agregar grupo",
            action: function (e, dt, node, config) {
                $("#groupAddModalPanel input").val("");
                $("#groupAddModalPanel").modal("show");
            }
        }] : [],
        initComplete: function () {
            $(groupTable.table().container()).find('div.eight.column:eq(0)').append(groupTable.buttons().container());
        },
        rowCallback: function (row, data, displayNum, displayIndex, dataIndex) {
            $(row).find("i.trash").click(function () {
                $(row).dimmer("show");

                let groupId = groupTable.row(row).id();

                SimpleDialog("warn", "Borrar grupo", "El grupo será borrado ¿Desea continuar?", function (result, e) {
                    if (result) {
                        deleteGroup(groupId, function (err, data) {
                            if (!err) {
                                groupTable.ajax.reload();
                                AddResultMessage(data.msg, "ok");
                            } else {
                                AddResultMessage(err.msg);
                            }
                            $(row).dimmer("hide");
                        });
                    }
                });

            });
            $(row).find("i.users").click(function () {
                let rowIdx = groupTable.cell($(this).parent()).index().row;
                let groupId = groupTable.row(rowIdx).id();

                $("#groupUsersModalPanel").find("[name='group_id']").val(groupId);

                $.ajax({
                    type: "GET",
                    url: "../Group/GetUsersInGroup?id=" + groupId,
                    dataType: "json",
                    success: function (d) {
                        let userIds = d.rows.map(function (obj) { return obj.id; });

                        $("#groupUsersModalPanel").find(".ui.dropdown").dropdown("clear").dropdown("set selected", userIds);
                        $("#groupUsersModalPanel").modal("show");
                    },
                    error: function (e) {
                        console.error(e)
                    }
                });
                
            });
            $(row).find("i.edit").click(function () {
                let rowIdx = groupTable.cell($(this).parent()).index().row;
                let groupId = groupTable.row(rowIdx).id();

                $("#groupSectionsModalPanel").find("[name='group_id']").val(groupId);

                $.ajax({
                    type: "GET",
                    url: "../Group/GetSectionsInGroup?id=" + groupId,
                    dataType: "json",
                    success: function (d) {
                        let sectionIds = d.rows.map(function (row) { return row.id; });

                        $("#groupSectionsModalPanel").find(".ui.dropdown")
                            .dropdown("clear")
                            .dropdown("set selected", sectionIds);
                        
                        $("#groupSectionsModalPanel").modal("show");
                    },
                    error: function (e) {
                        console.error(e);
                    }
                });

            })
        }
    });
});

function addGroup(name, callback) {
    $.ajax({
        type: "POST",
        url: "Insert",
        data: "name=" + name,
        dataType: "json",
        success: function (d) {
            console.log(d);
            if (d.error)
                callback(d);
            else
                callback(null, d);
        },
        error: function (e) {
            console.error(e);
            callback({ error: true, msg: e.msg || e.message || "Error desconocido.", details: e });
        }
    });
}

function deleteGroup(groupId, callback) {
    $.ajax({
        type: "GET",
        url: "../Group/Delete",
        data: "groupId=" + groupId,
        dataType: "json",
        success: function (d) {
            console.log(d);
            if (d.error)
                callback(d);
            else
                callback(null, d);
        },
        error: function (e) {
            console.error(e);
            callback({ error: true, msg: e.msg || e.message || "Error desconocido.", details: e });
        }
    });
}

function addUserToGroup(username, id)
{
        $.ajax({
            type: "GET",
            async: false,
            url: "AddUser",
            data: "id="+id+"&username="+username,
            success: function(datos) {
                 alert(datos);
                 $("#userList").trigger("reloadGrid");
            }
        });
}
function deleteUserFromGroup(username, id)
{
        $.ajax({
            type: "GET",
            async: false,
            url: "DeleteUser",
            data: "id="+id+"&username="+username,
            success: function(datos) {
                 alert(datos);
                 $("#userList").trigger("reloadGrid");
            }
        });
}
function addSectionToGroup(id)
{
    var select = $('#sectionSelect option:selected').clone();
    if (select.html() == null)
        return;
    var exists=$('#groupSelect option[value='+select.val()+']').clone();
    if(exists.html() != null)
    {   
        return;
    }
    $.ajax({
        type: "GET",
        async: false,
        url: "AddSection",
        data: "group_id="+id+"&section_id="+select.val(),
        success: function(datos) {
             alert(datos);
            select.html(select.html().replace(/&nbsp;/g, ''));
            select.html(select.html().replace('-', ''));
            select.appendTo($('#groupSelect'));
        }
    });
}
function deleteSectionFromGroup(id)
{
    var select = $('#groupSelect option:selected').clone();
    if (select.html() == null)
        return;
    $.ajax({
        type: "GET",
        async: false,
        url: "DeleteSection",
        data: "group_id="+id+"&section_id="+select.val(),
        success: function(datos) {
                alert(datos);
               
            $("#groupSelect option:selected").remove();
        }
    });

}

function setSections(data, callback) {
    $.ajax({
        type: "POST",
        url: "SetSections",
        data: $.param(data),
        dataType: "json",
        success: function (d) {
            console.log(d);
            if (d.error)
                callback(d);
            else
                callback(null, d);
        },
        error: function (e) {
            console.error(e);
            callback({ error: true, msg: e.msg || e.message || "Error desconocido.", details: e});
        }
    });
}

function setUsers(data, callback) {
    $.ajax({
        type: "POST",
        url: "SetUsers",
        data: $.param(data),
        dataType: "json",
        success: function (d) {
            console.log(d);
            if (d.error)
                callback(d);
            else
                callback(null, d);
        },
        error: function (e) {
            console.error(e);
            callback({ error: true, msg: e.msg || e.message || "Error desconocido.", details: e });
        }
    });
}