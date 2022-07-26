window.asfCms = {
    publication: {}
};

// -- Semantic form rules

$.fn.form.settings.rules.latin1encoding = function (value) {
    return !validateCharset(value).hasDoubles;
}

$.fn.form.settings.rules.dropdownEmpty = function (value) { //Deprecated
    if (value instanceof jQuery)
        value = value.val();

    if (typeof value == "number")
        return value >= 0;
    if (typeof value == "string")
        return value.trim().length > 0;
    
    return false;
}

// -- DataTables

// ---- Defaults

if ($.fn.dataTable) {
    $.extend(true, $.fn.dataTable.defaults, {
        lengthChange: false,
        pageLength: 50,
        lengthMenu: [[100, 200, 500, -1], [100, 200, 500, "-Todos-"]],
        responsive: {
            breakpoints: [
                { name: "desktop", width: 1200 },
                { name: "tablet", width: 991 },
                { name: "mobile", width: 768 }
            ]
        },
    });
}

// ---- Publications Column definitions

window.asfCms.publication.colRenderers = {
    statusRenderer: function (data, type, row, meta) {
        if (type != "display")
            return data;
        let icon = data.isMain ? "star" : "circle", color;
        switch (data.status) {
            case "1":
                color = "green";
                break;
            case "2":
                color = "grey";
                break;
            case "3":
                color = "orange";
                break;
            case "4":
            default:
                color = "red";
                break;
        }

        return "<i class='small " + color + " " + icon + " icon'></i>";
    },
    editRenderer: function (data, type, row, meta) {
        return type == "display" ? "<a href='../PublicationAdm/Edit?id=" + data.id + "'><i class='small edit icon'></i></a>" : 0;
    },
    mapExcludeRenderer: function (data, type, row, meta) {
        return type == "display" ? "<i class='small " + (data ? "yellow " : "green ") + "circle icon' style='cursor: pointer;'></i>" : data;
    },
    deleteRenderer: function (data, type, row, meta) {
        return type == "display" ? "<i class='small red trash icon' style='cursor: pointer;'></i>" : 0;
    }
};

window.asfCms.publication.simpleColDefs = [
    {
        data: function (row, type, set, meta) {
            return { isMain: row.cell[0], status: row.cell[1] };
        },
        render: asfCms.publication.colRenderers.statusRenderer,
        name: "status", orderable: false, className: "publicationStatus collapsing", responsivePriority: 1
    },
    { data: "cell.2", title: "Título", name: "title", responsivePriority: 1 },
    {
        data: null, name: "publicationEdit", orderable: false, className: "publicationEdit collapsing",
        render: asfCms.publication.colRenderers.editRenderer,
        responsivePriority: 1
    }
];

window.asfCms.publication.colDefs = [
    {
        data: function (row, type, set, meta) {
            return { isMain: row.cell[0], status: row.cell[1] };
        },
        orderData: [0, 1],   
        render: asfCms.publication.colRenderers.statusRenderer,
        name: "status", orderable: true, className: "publicationStatus collapsing", responsivePriority: 1
    },
    { data: "cell.2", title: "Título", name: "title", responsivePriority: 1 },
    { data: "cell.3", title: "Sección", name: "section", responsivePriority: 2 },
    {
        data: null, name: "publicationEdit", orderable: false, className: "publicationEdit collapsing",
        render: asfCms.publication.colRenderers.editRenderer,
        responsivePriority: 1
    },
    {
        data: "cell.5", name: "publicationMap", orderable: false, className: "publicationMap collapsing",
        render: asfCms.publication.colRenderers.mapExcludeRenderer,
        responsivePriority: 1
    }
];

window.asfCms.publication.deleteColDef = {
    data: null, name: "publicationDelete", orderable: false, className: "publicationDelete collapsing",
    render: asfCms.publication.colRenderers.deleteRenderer,
    responsivePriority: 1
};

// -- Semantic html generators

function SectionTreeToDropdown(tree, selectedSectionId, currentSectionId, includeNone, multiple) { //TODO: chenge to base call
    return SectionTreeToDropdownBase(tree, selectedSectionId, currentSectionId, includeNone, multiple, true, "parentSectionId");
}

function SectionTreeToDropdownBase(tree, selectedSectionId, currentSectionId, includeNone, multiple, searchable, inputId) {
    selectedSectionId = selectedSectionId || "";
    currentSectionId = currentSectionId || 0;
    includeNone = (includeNone === true || includeNone === false) ? includeNone : true;
    multiple = (multiple === true || multiple === false) ? multiple : false;
    searchable = (searchable === true || searchable === false) ? searchable : false;
    inputId = inputId || "sectionId";

    let renderNode = function (node, depth) {
        let depthIcons = ""
        for (let i = 0; i < depth; i++)
            depthIcons += "<i class=\"icon\"></i>";
        return "<div class=\"item\" data-value=\"" + node.id + "\">" + depthIcons + "<i class=\"" + (node.type == "section" ? "book" : "file alternate") + " icon\"></i>" + node.title + "</div>";
    };

    let exploreNodeChildren = function (node, container, depth) {
        depth = (depth === undefined || depth === null) ? 0 : depth;

        for (let key in node.children) {
            let child = node.children[key];
            if (child.id == currentSectionId)
                continue;
            let element = $(renderNode(child, depth));

            if (child.type == "section" && child.children.length > 0) {
                exploreNodeChildren(child, container, depth + 1);
            }

            container.append(element);
        }
    }

    let menu = $("<div></div>").addClass("menu");
    if (includeNone)
        menu.append("<div class=\"item\" data-value=\"0\">- Ninguno -</div>");

    exploreNodeChildren(tree, menu, 1);

    let dropdownClasses = multiple ? "fluid multiple " : "";
    dropdownClasses = searchable ? dropdownClasses + "search " : dropdownClasses;
    let dropdown = $("<div></div>")
        .addClass("ui " + dropdownClasses + "selection dropdown")
        .append("<input type=\"hidden\" id=\"" + inputId + "\" name=\"" + inputId + "\" value=\"" + selectedSectionId + "\" />")
        .append("<div class=\"default text\">Seleccione la seccion superior</div>")
        .append("<i class=\"dropdown icon\"></i>")
        .append(menu)
        .dropdown();

    return dropdown;
}

function SectionTreeToList(tree, maxDepth) {
    maxDepth = (maxDepth === undefined || typeof maxDepth != "number") ? Number.MAX_VALUE : maxDepth;

    let renderNode = function (node) {
        return "<div class=\"item\"><i class=\"" + (node.type == "section" ? "book" : "file alternate") + " icon\"></i><div class=\"content\"><div class=\"header\">" + node.title + " <i class=\"blue edit icon\"></i></div></div></div>";
    };

    let exploreNodeChildren = function (node, container, depth) {

        for (let key in node.children) {
            let child = node.children[key];
            let element = $(renderNode(child));

            if (child.type == "section" && child.children.length > 0 && depth < maxDepth) {
                element.find(".content").last().append("<div class=\"list\"></div>");
                let list = element.find(".list");
                exploreNodeChildren(child, list, depth + 1);
            }

            container.append(element);
        }
    }

    let mainList = $("<div></div>").addClass("ui divided list");

    exploreNodeChildren(tree, mainList, 0);

    return mainList;
}

function SectionTreeToTable(tree, maxDepth, canDelete, callbacks) {
    if (typeof maxDepth === "object") {
        callbacks = maxDepth;
        maxDepth = undefined;
    }

    let nop = function nop() { };
    callbacks = callbacks || { publicationDelete: nop, sectionDelete: nop };
    callbacks.publicationDelete = callbacks.publicationDelete || nop;
    callbacks.sectionDelete = callbacks.sectionDelete || nop;
    maxDepth = (maxDepth === undefined || typeof maxDepth != "number") ? Number.MAX_VALUE : maxDepth;

    let renderNode = function (node) {
        let row = $("<tr data-value=\"" + node.id + "\" data-main=\"" + node.isMain + "\"></tr>");
        row.append("<td><i class=\"" + (node.type == "section" ? "book" : "file alternate") + " icon\"></i></td >");
        row.append("<td>" + node.title + "</td>");
        row.append("<td class=\"collapsing\"><a href=\"" + (node.type == "section" ? "../SectionAdm/Edit?sectionId=" : "../PublicationAdm/Edit?id=") + node.id + "\"><i class=\"blue edit icon\"></i></a></td>");

        if (canDelete) {
            let deleteCell = $("<td class=\"collapsing\"><a href=\"#\"><i class=\"red trash icon\"></i></a></td>");
            deleteCell.find(".icon").click({ callbacks: callbacks, type: node.type }, function (evt) {
                let row = $(this);
                do {
                    row = row.parent();
                } while (!row.is("tr"));

                let id = parseInt(row.data("value"));
                let isMain = Boolean(row.data("main"));

                if (evt.data.type === "section")
                    evt.data.callbacks.sectionDelete(id, row, isMain);
                else
                    evt.data.callbacks.publicationDelete(id, row, isMain);
            });

            row.append(deleteCell);
        }
        row.append();
        return row;
    };

    let exploreNodeChildren = function (node, container, depth) {

        for (let key in node.children) {
            let child = node.children[key];
            let element = renderNode(child);

            if (child.type == "section" && child.children.length > 0 && depth < maxDepth) {
                exploreNodeChildren(child, container, depth + 1);
            }

            container.append(element);
        }
    }

    let head = $("<thead></thead>").append("<tr><th class=\"collapsing\"></th><th>Título</th><th class=\"collapsing\" colspan=\"2\"></th></tr>"),
        body = $("<tbody></tbody>");

    let table = $("<table id=\"sectionContentTable\"></table>").addClass("ui celled unstackable table").append(head).append(body);

    exploreNodeChildren(tree, body, 0);

    return table;
}

//--

function validateCharset(text) {
    var res = {
        hasDoubles: false,
        details: []
    };

    for (var i = 0, r = 0, c = 0; i < text.length; i++) {
        var code = text.charCodeAt(i);
        if (code > 255) {
            if (!res.hasDoubles)
                res.hasDoubles = true;
            res.details.push({
                char: text[i],
                code: code,
                row: r,
                col: c
            });
        }

        if (text[i] == '\n') {
            r++;
            c = 0;
        }
        c++;
    }

    return res;
}