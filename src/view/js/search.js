var searchableContent=null;
var selectedItem=null;
$().ready(function() {
    $("#btnSearch").click(function(){
        location.href="../Default/SearchResult?q="+$("#textToSearch").val();
    });
    
    $("#textToSearch").focus(obtainContent);
    $("#textToSearch").keypress(function(){selectedItem=null});
    $("#textToSearch").autocomplete(searchableContent, {
        minChars: 3,
        width: 270,
        matchContains: true,
        autoFill: false,
        max:40,
        selectFirst:false,
        formatItem: function(row, i, max) {
            return row.content+"<br/><small><i style='color:#800000'>"+row.clase+"</i></small>";
        },
        formatMatch: function(row, i, max) {
            return row.content;
        },
        formatResult: function(row) {
            return row.content;
        }
    }).result(function(e, item) {
        if(item.sublcass=="link")
            window.open(item.link, '_blank');
        else
        window.open(item.link, '_self');
    });
});

function obtainContent()
{
    if(searchableContent!=null)
        return true;
    
     $.ajax({
        url: "../Default/GetJsonSearchableContent",
        type: "POST",
        async:true,
        success: function(datos) {
        searchableContent =  eval("(" + datos + ")");
            //alert(searchableContent.toSource());
            $("#textToSearch").setOptions({data: searchableContent})
        }
        });
}

