$(document).ready(function () {

    var newsTable = $("#newsList").DataTable({
        ajax: {
            url: "../NewsAdm/GetAllNews?u=" + $("#hUname").val(),
            dataSrc: "rows"
        },
        rowId: "id",
        columns: [
            { data: "cell.0", title: "Título", name: "title", responsivePriority: 1 },
            { data: "cell.1", title: "Título de sección", name: "sectionTitle", responsivePriority: 1 },
            {
                data: function (row, type, set, meta) {
                    return row.cell[2];
                }, title: "", name: "includeIn", responsivePriority: 2, className: "include", render: function (data, type, row, meta) {
                    let tooltip = "<div>Sección: <i class='" + (row.cell[3] ? "green " : "grey ") + "circle check icon'></i><div class='ui divider'></div>Publicación: <i class='" + (row.cell[2] ? "green " : "grey ") + "circle check icon'></i></div>";
                    return type == "display" ? "<i class='" + (data ? "green " : "grey ") + "eye outline" + (data ? "" : " slash") + " icon' data-html=\"" + tooltip + "\"></i>" : data;
                }
            },
            {
                data: "cell.4", title: "", name: "ttl", responsivePriority: 2, render: function (data, type, row, meta) {
                    let c = "black";
                    if (data < 5)
                        c = "yellow";
                    if (data < 3)
                        c = "orange";
                    if (data == 0)
                        c = "red";
                    return type == "display" ? "<span class='ui " + c + " text'>" + data + "</span>" : data;
                }
            },
            {
                data: "cell.5", title: "", name: "pin", responsivePriority: 2, render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='" + (data ? "green " : "grey ") + "thumbtack icon'></i>" : data;
                }
            },
            {
                data: "cell.6", title: "", name: "hasContent", responsivePriority: 2, render: function (data, type, row, meta) {
                    return type == "display" ? "<i class='" + (data ? "green " : "grey ") + "newspaper icon'></i>" : data;
                }
            },
            {
                data: null, title: "", name: "edit", responsivePriority: 1, orderable: false, render: function (data, type, row, meta) {
                    return type == "display" ? "<a href='../PublicationAdm/Edit?id=" + row.id + "&from=news'><i class='blue edit icon'></i></a>" : row.id;
                }
            },
        ],
        initComplete: function () {
            let searchVisible = function (ele) {
                let visible = $(ele).is(":checked");
                newsTable.columns(2).search(visible ? true : "").draw();
            };
            let checkbox = $("<div class='ui toggle checkbox'><input type='checkbox' id='visibles' name='visibles' checked='false'><label>Visibles</label></div>").checkbox({
                onChange: function () {
                    searchVisible(this)
                }
            });
            searchVisible(checkbox.find("input"));
            $(newsTable.table().container()).find('div.eight.column:eq(0)').addClass("middle aligned").append(checkbox);
        },
        drawCallback: function () {
            $("#newsList").find("td.include > i").popup();
        }
    });

    $("#visibles").click(function () {
        if (this.checked)
        {
            $("#newsList").jqGrid('setGridParam', { url: '../NewsAdm/GetAllVisibleNews?u=' + $("#hUname").val() }).trigger("reloadGrid");
        }
        else {
            $("#newsList").jqGrid('setGridParam', { url: '../NewsAdm/GetAllNews?u=' + $("#hUname").val() }).trigger("reloadGrid");
        }
    });

});