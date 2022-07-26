var dataTree="";

$(document).ready(function(){
    dataTree=files;
     makeTree();
});
function makeTree()
{
    $("#treesito").jstree({
        "json_data": { data: dataTree
        },
        "themes": {
            "theme": "apple",
            "dots": true,
            "icons": false
        },
        "plugins": ["themes", "json_data", "ui"]
    }).bind("select_node.jstree", function(e, data) {
        var path = data.rslt.obj.data("path");
        if (path != "") {
            $("#objectFile").attr("src", path);

            $("#linkDocumento").attr("href", path);
        }

    });

}