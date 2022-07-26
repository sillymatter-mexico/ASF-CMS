$(document).ready(function () {
    let = itemTemplate = $("#modificationItem");
    itemTemplate.remove();

    $("#filterBtn").click(function (e) {
        e.preventDefault();
        modificationSearch();
    });

    modificationSearch();
});

function modificationSearch() {
    let formArray = $("#modificationSearchForm").serializeArray();
    let form = "?";
    for (let idx = 0; idx < formArray.length; idx++) {
        if (formArray[idx].value != "") {
            if (form[form.length - 1] != "?")
                form += "&"
            if (formArray[idx].name == "period") {
                let date = new Date();
                form += "searchStart=" + date.toISOString() + "&";
                switch (formArray[idx].value) {
                    case "year":
                        date.setFullYear(date.getFullYear() - 1);
                        break;
                    case "month":
                        date.setMonth(date.getMonth() - 1);
                        break;
                    case "week":
                        date.setDate(date.getDate() - 7);
                    case "day":
                        date.setDate(date.getDate() - 1);
                        break;
                }
                form += "searchEnd=" + date.toISOString();
            } else {
                form += formArray[idx].name + "=" + formArray[idx].value;
            }
        }
    }

    $.ajax({
        dataType: "json",
        url: "../ModificationLogAdm/Entries" + form,
        success: function (entries) {
            $("#modificationList").empty();
            if (entries.length == 0)
                $("#modificationNoResults").show();
            else
                $("#modificationNoResults").hide();

            for (let idx = 0; idx < entries.length; idx++) {
                let entry = entries[idx];
                let item = itemTemplate.clone();

                item.find(".icons > .icon").first().addClass(getTargetIconClass(entry.TargetType));
                item.find(".icons > .corner").addClass(getModificationIconClass(entry.Type));
                item.find(".header").text(entry.Created);
                item.find(".message").text(entry.Message);
                item.find(".extra").text(entry.Details).hide();
                
                if (entry.Details !== null && entry.Details !== "") {
                    item.find(".description > .plus").click(toggleExtra)
                } else {
                    item.find(".description > .plus").hide();
                }

                $("#modificationList").append(item);
            }
        },
        error: function (err) {
            console.error(err);
        }
    });
}

function toggleExtra(e) {
    $(e.currentTarget).parent().parent().find(".extra").toggle();
    if ($(e.currentTarget).hasClass("plus"))
        $(e.currentTarget).removeClass("plus").addClass("minus");
    else
        $(e.currentTarget).removeClass("minus").addClass("plus");
}

function getTargetIconClass(target) {
    switch (target) {
        case "SECTION":
            return "book";
        case "PUBLICATION":
            return "file alternate";
        default:
            return "file outline";
    }
}

function getModificationIconClass(modification) {
    switch (modification) {
        case "MODIFY":
            return "save blue";
        case "CREATE":
            return "plus circle green";
        case "DELETE":
            return "minus circle red";
        case "RESTORE":
            return "history blue";
        default:
            return "edit";
    }
}