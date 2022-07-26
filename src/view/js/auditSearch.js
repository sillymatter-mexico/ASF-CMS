var filters;

$().ready(function() {
    $("#tipo").append("<option value=\"\">--</option>");
    $("#anio").append("<option value=\"\">--</option>");
    $("#sector").append("<option value=\"\">--</option>");


    performSearch("");
    $("#btnSearchAudit").click(function() {
        performSearch("");
    });
    $("#tipo").change(function() {
        performSearch("tipo")
    });
    $("#anio").change(function() {

        performSearch("anio")
    });
    $("#sector").change(function() {
        performSearch("sector")
    });

    $("#grupoFuncionalId").change(function() {
        performSearch("grupo");
    });


});

function performSearch(field) {
    $.ajax({
        type: "POST",
        async: true,
        url: "../Auditoria/PerformSearch",
        cache: false,
        data: $("#frmSearch").serialize(),
        success: function(datos) {
            filters = eval("(" + datos + ")");
            llenaTipos(field,  $("#tipo").val());
            llenaAnios(field, $("#anio").val());
            llenaSectores(field, $("#sector").val());
            llenaGrupos(field, $("#grupoFuncionalId").val());
            if (field != "titulo")
                cargaTitulos();
            if (field != "ente")
                cargaEntes();
            makeTable();
        }
    });
    }
    function cargaEntes() {
//        alert(filters.entes.length);
        $("#nombreEnte").unautocomplete();
        $("#nombreEnte").autocomplete(filters.entes, {
            minChars: 0,
            width: 270,
            matchContains: true,
            autoFill: false,
            max: 40,
            formatItem: function(row, i, max) {
                return row.value;
            },
            formatMatch: function(row, i, max) {
                return row.value;
            },
            formatResult: function(row) {
                return row.value;
            }
    }).result(function(e, item) {
        $("#ente").val(item.key);
        performSearch("ente");
    });

     }
    function cargaTitulos()
    {
        $("#titulo").unautocomplete();
        $("#titulo").autocomplete(filters.titulos, {
            minChars: 0,
            width: 400,
            matchContains: true,
            autoFill: false,
            max: 40,
            formatItem: function(row, i, max) {
                return row.key;
            },
            formatMatch: function(row, i, max) {
                return row.key;
            },
            formatResult: function(row) {
                return row.key;
            }
        }).result(function(e, item) {
        performSearch("titulo");

        });
    }
    
    function makeTable() {
        $("tbody tr",'#auditList').remove();
        
        $.each(filters.results, function(i, item) {
        $('#auditList > tbody:last').append('<tr><td>' + item.Year + '</td>'+'<td>'+item.TypeName+'</td>'+
        '<td>' + item.Sector + '</td><td>' + item.EntityName + '</td><td>' + item.Number + '</td>'+
        '<td><a href="'+item.File+item.Page+'" target="_blank">' + item.Title + '</a></td></tr>');
    });
}
function llenaTipos(field, valor) {
//    alert("tipo " + valor);
    if (field == "tipo" && valor != "")
        return;
    $("option", "#tipo").remove();
    $("#tipo").append("<option value=\"\">--</option>");
    $.each(filters.tipos, function(i, item) {
    if (item.key == valor)
        $("#tipo").append("<option value=\"" + item.key + "\" selected >" + item.value + "</option>");
    else
            $("#tipo").append("<option value=\"" + item.key + "\"  >" + item.value + "</option>");
    });
}
function llenaAnios(field, valor) {
     if(valor>=2010)
        $(".grupoFuncional").show();
     else 
        $(".grupoFuncional").hide();
    if (field == "anio" && valor != "")
        return;
    $("option", "#anio").remove();
    $("#anio").append("<option value=\"\">--</option>");
    $.each(filters.anios, function(i, item) {
    if(item.key==valor)
        $("#anio").append("<option value=\"" + item.key + "\" selected>" + item.value + "</option>");
    else
        $("#anio").append("<option value=\"" + item.key + "\" >" + item.value + "</option>");

});
}
function llenaSectores(field, valor) {
   // alert("sector " + valor);
    if (field == "sector" && valor != "")
        return;
    $("option", "#sector").remove();
    $("#sector").append("<option value=\"\">--</option>");
    $.each(filters.sectores, function(i, item) {
    if (item.key == valor)
        $("#sector").append("<option value=\"" + item.key + "\" selected>" + item.value + "</option>");
    else
        $("#sector").append("<option value=\"" + item.key + "\">" + item.value + "</option>");
});
}
function llenaGrupos(field, valor) {
    //alert("grupo " + valor);
    if (field == "grupo" && valor != "")
        return;
    $("option", "#grupoFuncionalId").remove();
    $("#grupoFuncionalId").append("<option value=\"\">--</option>");
    $.each(filters.grupos, function(i, item) {
    if (item.key == valor)
        $("#grupoFuncionalId").append("<option value=\"" + item.key + "\" selected>" + item.value + "</option>");
    else
        $("#grupoFuncionalId").append("<option value=\"" + item.key + "\">" + item.value + "</option>");
});
}
