var resultMessagesContainer;
var resultMessageTemplate;

$(document).ready(function () {
    resultMessagesContainer = $("#resultMessages");
    resultMessagesContainer.hide();
    resultMessageTemplate = resultMessagesContainer.find("div");
    resultMessageTemplate.remove();
});

function AddResultMessage(message, type) {
    ClearResultMessages();

    type = type.toLowerCase();

    var color;
    var icon;

    switch (type) {
        case "warn":
        case "warning":
            color = "yellow";
            icon = "exclamation circle";
            break;
        case "ok":
        case "log":
            color = "green";
            icon = "check circle";
            break;
        case "err":
        case "error":
            color = "red";
            icon = "times circle";
            break;
    }

    var msgPane = resultMessageTemplate.clone();
    msgPane.find("span").text(message);
    msgPane.find("i").addClass(color + " " + icon);
    msgPane.addClass(color);

    resultMessagesContainer.append(msgPane).show();
}

function ClearResultMessages() {
    resultMessagesContainer.empty().hide();
}