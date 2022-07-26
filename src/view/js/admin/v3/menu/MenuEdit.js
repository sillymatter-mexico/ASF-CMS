var labelTemplate;

$.ajax({
    url: "../SectionAdm/GetTree",
    dataType: "json",
    success: function (d) {
        if (!d.error) {
            $("div[id$='AddSection']").each((idx, elem) => {
                let menuKey = GetMenuKey($(elem).attr("id"), "AddSection");
                let dd = SectionTreeToDropdownBase(d.data, 0, 0, false, false, true, menuKey + "SectionIdInput");
                dd.addClass("fluid");
                let input = $("<div class='ui action fluid input'></div>");
                let btn = $("<button class='ui button'>Agregar</button>");
                btn.click((e) => {
                    let field = $(e.currentTarget).parent().parent();
                    let menuKey = GetMenuKey(field.attr("id"), "AddSection");
                    let sectionId = field.find("#" + menuKey + "SectionIdInput").val();
                    if (sectionId) {
                        let sectionName = field.find(".text").text();
                        console.log("Click! adding -- %s, Id= %d, Name=", menuKey, sectionId, sectionName);

                        let newLabel = labelTemplate.clone();
                        newLabel.find("span").text(sectionName);
                        newLabel.find("input[name=sectionId]").val(sectionId);
                        AddLabelListeners(0, newLabel);

                        $("#" + menuKey + "ItemsContainer").append(newLabel);
                    }
                });
                input.append(dd).append(btn);

                $(elem).append(input);
            });
        } else {
            console.error("Error", d.msg);
        }
    },
    error: function (e) {
        console.error(e);
    }
});

$().ready(function () {
    labelTemplate = $("#labelTemplate").clone();
    $("#labelTemplate").remove();

    $(".ui.label").each(AddLabelListeners);

    $("[id$='Save']").click(function (e) { Save(GetMenuKey(e.currentTarget.id, "Save")); });
});

function Save(menuKey) {
    let menuSegment = $(`#${menuKey}Segment`);
    menuSegment.addClass("loading");

    let menu = {
        key: menuKey,
        cssClass: menuSegment.find(`#${menuKey}CssClass`).val(),
        sections: []
    };

    menuSegment.find(".ui.label").each((idx, label) => {
        menu.sections.push({
            sectionId: $(label).find("input[name=sectionId]").val(), cssClass: $(label).find("input[name=itemCssClass]").val()
        });
    });

    UpdateMenu(menu, function (err, data) {
        menuSegment.removeClass("loading");
        if (err) {
            AddResultMessage(err.msg, "err");
        } else {
            AddResultMessage(data.msg, "ok");
        }
    });
}

function GetMenuKey(id, sufix) {
    return id.substring(0, id.indexOf(sufix));
}

function AddLabelListeners(index, labelElement) {
    $(labelElement).find(".up").click((e) => {
        let label = $(e.currentTarget).parent();
        if (label.siblings(":first") != label) {
            let elem1 = label.prev();
            let elem2 = label;
            elem2.after(elem1);
        }
    });
    $(labelElement).find(".down").click((e) => {
        let label = $(e.currentTarget).parent();
        if (label.siblings(":last") != label) {
            let elem1 = label.next();
            let elem2 = label;
            elem2.before(elem1);
        }
    });
    $(labelElement).find(".close").click((e) => {
        $(e.currentTarget).parent().remove();
    });
}

function UpdateMenu(menu, callback) {
    $.ajax({
        type: "POST",
        url: "Save",
        data: menu,
        dataType: "json",
        success: function (data) {
            if (!data.error) {
                callback(null, data);
            } else {
                callback(data);
            }
        },
        error: function (error) {
            callback({ error: true, msg: error.message || "Error desconocido", details: error });
        }
    });
}