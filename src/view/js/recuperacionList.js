$(document).ready(function() {
    $("#recuperacionList").jqGrid({
        datatype: "json",
        url: '../Recuperacion/GetAllActive',
        colNames: ['','Id','Titulo','Fecha de creacion',''],
        colModel: [
            { name: 'Active', index: 'Active', width: 15, sortable: false, formatter: c => c },
            { name: 'Id', index: 'Id', width: 17, sortable: false },
            { name: 'Title', index: 'Title', width: 300, sortable: true},
            { name: 'CreationDate', index: 'CreationDate', width: 190, sortable: true},
            { name: 'cmd', index: 'cmd', width: 16, sortable: false, formatter: c => c }
        ],
         ondblClickRow: function (rowid) {
           window.location.href = "../Recuperacion/Edit?Id=" + rowid;
        },
        height: 'auto',
        rowNum: 1000,
        caption: "Informes"
    });
    $('#newRecuperacionPopUp').dialog({
        autoOpen: false,
        width: 600,
        height: 'auto',
        modal: true,
        title: "Nueva Seccion de Recuperacion",
        buttons: {
            "Aceptar": function() {
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "Insert",
                    data:$("#frmNew").serialize(),
                    success: function(datos) {
                        alert(datos);
                       $(this).dialog("close");
                       $("#recuperacionList").trigger("reloadGrid");

                    }
                });
               
            },
            "Cancelar": function() {
               $(this).dialog("close");
            }
        }

    });
    $("#btnNewRecuperacion").click(function(){
        $.ajax({
            type: "POST",
            async: false,
            url: "New",
            success: function(datos) {
                $("#newRecuperacionPopUp").html(datos);
                $("#title").blur(function() {
                    $("#permalink").val($.trim($("#title").val()).replace(/ /g, '_'));
                });
                $("#newRecuperacionPopUp").dialog('open');
            }
        });
    });
});
function eliminaRecuperacion(id)
{
    if(!confirm("¿Desea eliminar este informe de recuperación?"))
        return;
    $.ajax({
        type: "POST",
        async: false,
        url: "../Recuperacion/Delete",
        data:"id="+id,
        success: function(datos) {
            alert(datos);
            $("#recuperacionList").trigger("reloadGrid");
        }
    });
}


