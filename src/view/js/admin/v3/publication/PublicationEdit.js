$(document).ready(function () {

    // -- Init --

    // Checkbox
    $(".ui.checkbox").checkbox();

    // Calendars
    $("#published").calendar({
        type: "datetime",
        endCalendat: $("#unpublished")
    });
    $("#unpublished").calendar({
        type: "datetime",
        startCalendar: $("#published")
    });

    // WYSIWYG Editor
    jQuery_1_12_4("#elm1").tinymce({
        script_url: '../view/js/tinymce/tiny_mce.js',
        width: "100%",
        filemanager_rootpath: '~/uploads/' + $("#permalink").val(),
        imagemanager_rootpath: '~/uploads/' + $("#permalink").val(),
        theme: "advanced",
        plugins: "pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,advlist,imagemanager,filemanager,icon_include",

        theme_advanced_buttons1: "fontselect,fontsizeselect,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,forecolor,backcolor,|,link,unlink,anchor,image,insertfile,icon_include,cleanup,code,help",
        theme_advanced_buttons2: "formatselect,styleselect,cut,copy,paste,pastetext,pasteword,|,undo,redo,|,bullist,numlist,|,outdent,indent,blockquote,|,search,replace,|,styleprops,|,visualchars,nonbreaking",
        theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,iespell,media,advhr,|,ltr,rtl,|,fullscreen,|,attribs,",
        theme_advanced_buttons4: "",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,
        extended_valid_elements: "iframe[src|width|height|name|align|frameborder|allow=|allowfullscreen=|id],style[type],span[displayText|class],video[id|buffered|controls=|height|width|max-width|preload|muted=|loop|src],source[src|type]",
        valid_children: "+body[style]",
        content_css: "../view/css/style_tiny.css",
        template_external_list_url: "lists/template_list.js",
        external_link_list_url: "lists/link_list.js",
        external_image_list_url: "lists/image_list.js",
        media_external_list_url: "lists/media_list.js"
    });

    // Parent dropdown

    if (!isSpecial) {
        $.ajax({
            url: "../SectionAdm/GetTree",
            dataType: "json",
            success: function (d) {
                if (!d.error) {
                    $(".eight.wide:eq(2)").append(SectionTreeToDropdown(d.data, parseInt($("#parentSectionIdOld").val()), 0, false).dropdown().addClass(isMain ? "disabled" : ""));
                    initForm();
                } else {
                    console.error("Error", d.msg);
                }
            },
            error: function (e) {
                console.error(e);
            }
        });
    } else {
        $(".eight.wide:eq(2)").append("<input type=\"hidden\" id=\"parentSectionId\" name=\"parentSectionId\" value=\"" + $("#parentSectionIdOld").val() + "\" />");
        initForm();
    }

    let id = $("#Id").val();
    initNewsModal($("#newsModalPanel"), id);
    initMetaModal($("#metaModalPanel"), metaTags, id);
    initFilesModal($("#filesModalPanel"), id);
    initHistoricModal($("#historicModalPanel"), $("#historicConfirmModalPanel"), id);

    // -- Actions --

    $("#btnSave").click(function () {
        $(this).addClass("loading");
        $("#frmNew").addClass("loading").form("validate form");
    });

    $("#btnNewsEdit").click(function () {
        $("#newsModalPanel").modal("show");
    });

    $("#btnFilesEdit").click(function () {
        resetFilesModal($("#filesModalPanel"));
        $("#filesModalPanel").modal("show");
    });

    $("#btnMetaEdit").click(function () {
        resetMetaModal($("#metaModalPanel"), metaTags);
        $("#metaModalPanel").modal("show");
    });

    $("#btnHistoricEdit").click(function () {
        resetHistoricModal($("#historicModalPanel"), $("#historicConfirmModalPanel"), id);
        $("#historicModalPanel").modal("show");
    });

    let params = (new URL(document.location)).searchParams;
    let from = params.get("from");
    switch (from) {
        case "news":
            $("#newsModalPanel").modal("show");
            break;
    }
});

function initForm() {
    $("#frmNew").form({
        fields: {
            title: {
                identifier: "title",
                rules: [
                    { type: "empty", prompt: "Ingrese el nombre de la publicación." },
                    { type: "latin1encoding", prompt: "El título contiene caracteres inválidos." }
                ]
            },
            parentSectionId: {
                identifier: "parentSectionId",
                rules: [
                    { type: "empty", prompt: "Seleccione la seccion a la que pertenece esta publicación." },
                ]
            },
            Position: {
                identifier: "Position",
                optional: true,
                rules: [
                    { type: "integer[0...]", prompt: "El valor de posición debe ser un numero entero positivo." }
                ]
            },
            CssClass: {
                identifier: "CssClass",
                optional: true,
                rules: [
                    { type: "regExp[^\s*(-?[_a-zA-Z]+[_-a-zA-Z0-9]*\s*)*$]", prompt: "Escriba nombres de clase css válidos separados por espacios." }
                ]
            },
            published: {
                identifier: "published",
                rules: [
                    { type: "empty", prompt: "Selecciones una fecha de publicación válida." }
                ]
            },
            unpublished: {
                identifier: "unpublished",
                rules: [
                    { type: "empty", prompt: "Selecciones una fecha para retirar válida." }
                ]
            },
            elm1: {
                identifier: "elm1",
                rules: [
                    { type: "latin1encoding", prompt: "El texto contiene caracteres inválidos." }
                ]
            }
        },
        onSuccess: function (e) {
            tinymce.triggerSave();
            updatePublication(function (err, data) {
                $("#frmNew,#btnSave").removeClass("loading");
                if (err) {
                    AddResultMessage(err.msg, "err");
                } else {
                    AddResultMessage(data.msg, "ok");
                    //TODO: Ask if return
                }
            });
        },
        onFailure: function (e) {
            $("#frmNew,#btnSave").removeClass("loading");
        }
    });
}

function updatePublication(callback) {
    $.ajax({
        type: "POST",
        url: $("#frmNew").attr("action"),
        data: $("#frmNew").serialize(),
        dataType: "json",
        success: function (data) {
            if (data.error) {
                callback(data);
            } else {
                callback(null, data);
            }
        },
        error: function (error) {
            callback({ error: true, msg: error.message || "Error desconocido", details: error });
        }
    });
}