﻿function initNewsModal(elem, id) {

    jQuery_1_12_4("#pubNewContent").tinymce({
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

    elem.modal({
        closable: false,
        onApprove: function () {
            elem.find(".content").dimmer("show");
            elem.find(".approve").addClass("loading");
            elem.find("#publicationNewsForm").form("validate form");
            return false;
        }
    });

    elem.find("#publicationNewsForm").form({
        fields: {
            pubNewTTL: {
                identifier: "pubNewTTL",
                rules: [
                    { type: "empty", prompt: "Favor de indicar la duración de la noticia." },
                    { type: "integer[0...]", prompt: "La duración debe ser un numero entero positivo." }
                ]
            },
            pubNewContent: {
                identifier: "pubNewContent",
                rules: [
                    { type: "latin1encoding", prompt: "El texto contiene caracteres inválidos." },
                    { type: "maxLength[125000]", prompt: "El texto es demasiado largo." }
                ]
            }
        },
        onSuccess: function (e) {
            let data = elem.find("#publicationNewsForm").serializeArray().reduce(function (d, v) {
                if (d == null || d == undefined)
                    d = {};
                if (d.name && d.value) {
                    let t = {};
                    t[d.name] = d.value;
                    d = t;
                }
                d[v.name] = v.value;
                return d;
            });
            data.publicationId = id;
            data.pubNewContent = tinyMCE.getInstanceById("pubNewContent").getContent();

            updatePublicationNews(data, function (err, data) {
                elem.find(".content").dimmer("hide");
                elem.find(".approve").removeClass("loading");
                if (!err) {
                    AddResultMessage(data.msg, "ok");
                } else {
                    AddResultMessage(err.msg, "err");
                }
                elem.modal("hide");
            });
        },
        onFailure: function (e) {
            console.log("No validado");
            elem.find(".content").dimmer("hide");
            elem.find(".approve").removeClass("loading");
        }
    });

}

function resetNewsModal(elem) {

}

function updatePublicationNews(data, callback) {
    $.ajax({
        type: "POST",
        async: false,
        url: "../PublicationAdm/UpdateNews",
        data: $.param(data),
        dataType: "json",
        success: function (data) {
            if (!data.error)
                callback(null, data);
            else
                callback(data);
        },
        error: function (err) {
            callback({ error: true, msg: err.msg || err.message || "Error desconocido.", details: err });
        }
    });
}