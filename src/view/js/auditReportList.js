$(document).ready(function() {
    
$("#reportList").jqGrid({
        datatype: "json",
        url: '../AuditReport/GetAllActiveReports',
        colNames: ['','A&ntilde;o','Titulo','Publicar en','Auditorias'],
        colModel: [
            { name: 'IsPublished', index: 'IsPublished', width: 15, sortable: false, formatter: c => c },
            { name: 'Year', index: 'Year', width: 50, sortable: true},
            { name: 'Title', index: 'Title', width: 250, sortable: true},
            { name: 'PublishDate', index: 'PublishDate', width: 80, sortable: true},
            { name: 'LoadedAudits', index: 'LoadedAudits', width: 250, sortable: true},
        ],
         ondblClickRow: function (rowid) {
           window.location.href = "../AuditReport/Edit?auditReportId=" + rowid;
        },
        height: 'auto',
        rowNum: 1000,
        caption: "Informes",
    });
    $('#newReportPopUp').dialog({
        autoOpen: false,
        width: 600,
        height: 'auto',
        modal: true,
        title: "Nuevo Informe de Auditoria",
        buttons: {
            "Aceptar": function() {
               if(!validaNewPopUp())
                   return;
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "Insert",
                    data:$("#frmNew").serialize(),
                    success: function(datos) {
                        alert(datos);
                       $('#newReportPopUp').dialog("close");

                       $("#reportList").trigger("reloadGrid");

                    }
                });
               
            },
            "Cancelar": function() {
               $(this).dialog("close");
            }
        }

    });
    $("#btnNewReport").click(function(){
        $.ajax({
            type: "POST",
            async: false,
            url: "New",
            success: function(datos) {
            
                $("#newReportPopUp").html(datos);
                $("#title").blur(function() {
                    $("#permalink").val($.trim($("#title").val()).replace(/ /g, '_'));
                });
                $("#year").numeric();

                $("#newReportPopUp").dialog('open');
            }
        });
    });
   
});
function validaNewPopUp()
{
    if(jQuery.trim($("#year").val())=='')
    {    
        alert("El año no puede estar vacio");
        $("#year").focus();
        return false;
    }
    if(jQuery.trim($("#title").val())=='')
    {    
        alert("El titulo no puede estar vacio");
        $("#title").focus();
        return false;
    }
    return true;
}



