function makeTree(dataTree, id)
{
    $("#treesito_" + id).jstree({
        "json_data" : { data:dataTree
        },  
        "themes" : {
            "dots" : true,
            "icons" : false
        },
        "plugins" : [ "themes", "json_data","ui"]
    }).bind("select_node.jstree", function (e, data) { 
        var path = data.rslt.obj.data("path");
        var rid = $(e.target).attr("id").split("_")[1];
        if (path != "")
            showFile(path, rid);
        else
            hideFile(rid);
    });
    hideFile(id);
}
function showFile(path, rid)
{
    $("#objectFile_" + rid).attr("src",path);
    $("#linkDocumento_" + rid).attr("href",path);
    $("#objectFile_" + rid).show();
    $("#linkDocumento_" + rid).show();
}
function hideFile(rid)
{
    $("#objectFile_" + rid).attr("src","");
    $("#linkDocumento_" + rid).attr("href","");
    $("#objectFile_" + rid).hide();
    $("#linkDocumento_" + rid).hide();
}
