var dataTree="";

$(document).ready(function() {
    $("#guardar").click(function() {
        var resultado = valida();
        if (!resultado.valido) {
            alert(resultado.mensaje);
            return;
        }
        jQuery.jstree._reference("#treesito").deselect_all();
        var json = jQuery.jstree._reference("#treesito").get_json();
        var jsonString = JSON.stringify(json);
        $("#files").val(jsonString);
        $.ajax({
            type: "POST",
            async: true,
            url: "Save",
            data: $("#frmRecuperaciones").serialize(),
            success: function(datos) {
                var response = jQuery.parseJSON(datos);
                alert(response.Message);
                $("#id").val(response.Data);

            }
        });


    });

    dataTree = files;

    $("#procesar").click(function() {
        if (dataTree != '' && !confirm("Se perderán todos los cambios"))
            return;

        $.ajax({
            type: "POST",
            async: true,
            url: "getRecuperacionData",
            data: "directoryPath=" + $("#directoryPath").val(),
            success: function(datos) {
                dataTree = jQuery.parseJSON(datos);
                hideFile();
                makeTree();

            }
        });
    });
    makeTree();
    hideFile();

});
function showFile(path)
{
    $("#objectFile").attr("src",path);
    $("#linkDocumento").attr("href",path);
    $("#objectFile").show();
    $("#linkDocumento").show();

}
function hideFile()
{
    $("#objectFile").attr("src","");
    $("#linkDocumento").attr("href","");
    $("#objectFile").hide();
    $("#linkDocumento").hide();
}
function makeTree()
{
    $("#treesito").jstree({
        "json_data" : { data:dataTree
        },  
        "themes" : {
            "dots" : true,
            "icons" : false
        },
        "crrm" : {
            "move" : {
                "check_move" : function (m) {
                    var p = this._get_parent(m.o);
                    if(!p) return false;
                    p = p == -1 ? this.get_container() : p;
                    if(p === m.np) return true;
                    if(p[0] && m.np[0] && p[0] === m.np[0]) return true;
                    return false;
                }
            }
        },
        "hotkeys":{
            "del":function(){ 
                var clases=this._get_node().attr("class");
                if(clases.indexOf("invisible")>-1)
                  clases=clases.replace("invisible","visible");
                else
                    clases=clases.replace("visible","invisible");
                this._get_node().attr("class",clases);
                this.deselect_all ();
            }
        },
        "dnd" : {
            "drop_target" : false,
            "drag_target" : false
        },
        "plugins" : [ "themes", "json_data","dnd","crrm","ui","hotkeys"]
    }).bind("select_node.jstree", function (e, data) { 
        var path=data.rslt.obj.data("path");
        if(path!="")
            showFile(path);
        else 
            hideFile();
        
    });

}

function selectFolder(field)
{
    mcFileManager.browse({ fields: field, rootpath: '~/pags/Recupera1', relative_urls: true, exclude_file_pattern: '/^\./' });
}
function valida() {
    var resultado = { valido: true, mensaje: "" };
    if ($.trim($("#title").val()) == "") {
        $("#title").focus();
        resultado.valido = false;
        resultado.mensaje = "Falta especificar el titulo";
    }
    return resultado;
}