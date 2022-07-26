$(document).ready(function () {
    if(typeof msgTo ===  "undefined")
        msgTo = "";
    hideMailMeesage();
    $("#msgPanel").hide();

    $("#msgCharsMax").text(mailMaxChars);

    $(".msg").click(function() {
        $("#msgPanel").toggle("blind", showMailPanel);
    });
    $("#msgCloseInfoBtn").click(function() {
        $("#msgPanel").toggle("blind", showMailPanel);
    });

    $("#msgContentInput").keyup(function() {
        let content = $("#msgContentInput").val();
        content = typeof content == "string" ? content : "";
        if (content.length > mailMaxChars)
            $("#msgContentInput").addClass("fieldError");
        else
            $("#msgContentInput").removeClass("fieldError");
        
        $("#msgCharsCur").text(content.length);
    });

    $("#msgSendBtn").click(function() {
        let msgFrom = $("#msgFromInput").val();
        let msgContent = sanitize($("#msgContentInput").val()).trim();
        let valid = true;

        //Validation
        if (!isEmail(msgFrom)) {
            $("#msgFromInput").addClass("fieldError");
            valid = false;
        } else {
            $("#msgFromInput").removeClass("fieldError");
        }
        if (msgContent.length <= 0 || msgContent.length > mailMaxChars) {
            $("#msgContentInput").addClass("fieldError");
            valid = false;
        } else {
            $("#msgContentInput").removeClass("fieldError");
        }
        
        if (!valid)
            return;

        $("#msgFromInput, #msgContentInput").removeClass("fieldError");

        showMailMessage("Enviando correo");

        $.ajax({
            url: "/Ajax/SendMail",
            method: "POST",
            data: { messageFormFrom: msgFrom, messageFormMessage: msgContent, messageTo: msgTo },
            success: function (r) {
                let result = $.parseJSON(r);
                if (result.result == "true" || result.result == true) {
                    showMailMessage("Correo enviado")
                } else {
                    showMailMessage("Error al enviar correo")
                    console.log("Error::", r);
                }
            },
            error: function(e) {
                showMailMessage("Error al enviar correo, intente nuevamente más tarde.");
                console.log("Error::", e);
            }

        });
    });
    $("#msgCancelBtn").click(function() {
        $("#msgPanel").toggle("blind", showMailPanel);
    });
});

function showMailPanel() {
    hideMailMeesage();
    $("#msgFromInput,#msgContentInput").val("").removeClass("fieldError");
}

function showMailMessage(message) {
    $("#msgMessage").html(message);
    $("#msgInfo").show();
}

function hideMailMeesage() {
    $("#msgInfo").hide();
}

function isEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function sanitize(text) {
    return text.replace(/<(|\/|[^>\/bi]|\/[^>bi]|[^\/>][^>]+|\/[^>][^>]+)>/g, '');
}