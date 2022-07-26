function SimpleDialog(type, title, description, callback) {
    let color, icon;
    switch (type) {
        case "warn":
        case "warning":
            color = "yellow";
            icon = "exclamation triangle";
            break;
        case "err":
        case "error":
            color = "red";
            icon = "times triangle";
            break;
        case "ok":
        case "log":
        default:
            color = "green";
            icon = "check triangle";
            break;
    }

    let panel = $("#modalSimpleDialog");
    panel.find(".header").text(title);
    panel.find(".content i").addClass(color + " " + icon);
    panel.find(".content span").text(description);
    panel.modal({
        allowMultiple: true,
        keyboardShortcuts: false,
        closable: false,
        onApprove: function (ele) {
            callback(true, ele);
        },
        onDeny: function (ele) {
            callback(false, ele)
        }
    });
    panel.modal("show");
}

function SimpleAlert(type, title, description) {
    let color, icon;
    switch (type) {
        case "warn":
        case "warning":
            color = "yellow";
            icon = "exclamation triangle";
            break;
        case "err":
        case "error":
            color = "red";
            icon = "times triangle";
            break;
        case "ok":
        case "log":
        default:
            color = "green";
            icon = "check triangle";
            break;
    }

    let panel = $("#modalSimpleAlert");
    panel.find(".header").text(title);
    panel.find(".content i").addClass(color + " " + icon);
    panel.find(".content span").text(description);
    panel.modal({
        allowMultiple: true,
        keyboardShortcuts: false,
        closable: false
    });
    panel.modal("show");
}