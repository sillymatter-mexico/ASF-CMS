$().ready(function() {
    $("#unpublished").datepicker({ "dateFormat": 'yy-mm-dd' });
    $("#published").datepicker({ "dateFormat": 'yy-mm-dd' });

    $("#btnSave").click(function() {
        $.ajax({
            type: "POST",
            async: false,
            url: "Update",
            data: $("#frmNew").serialize(),
            success: function(datos) {
                alert(datos);
            }
        });


    });
    $("#btnGenerateIndex").click(function() {
        generaIndice();
    });
    $("#btnLoadAudit").click(function() {
        $.ajax({
            type: "POST",
            async: false,
            url: $("#loadAuditForm").attr('action'),
            data: $("#loadAuditForm").serialize(),
            success: function(datos) {
                if (datos == "")
                    window.location = '../Auditoria/PreloadReport?year=' + $("#loadAuditYear").val();
                else
                    alert(datos);
            }
        });

    });
    $("#btnDeleteAudit").click(function() {
        if (!confirm("Si continua se borraran todas las auditorías cargadas en este informe, ¿Desea continuar? "))
            return;
        $.ajax({
            type: "POST",
            async: false,
            url: "../Auditoria/Delete",
            data: "auditReportId=" + $("#auditReportId").val(),
            success: function(datos) {
                alert(datos);
            }
        });

    });

});
function generaIndice() {
    var year = $("#year").val();
    if($("#indexPermalink").val()!='')
        if(!confirm("Se sobreescribirá el índice existente. ¿Desea continuar?"))
            return;
        
    $.ajax({
        type: "POST",
        async: false,
        url: "GetIndex",
        data: "year=" + year,
        success: function(datos) {
            $.ajax({
                type: "POST",
                async: false,
                url: "SaveAutomaticIndex",
                data: $("#frmNew").serialize() + "&content=" + datos,
                success: function(res) {
                    alert("La operacion se realizo con exito");
                    window.location = "../PublicationAdm/Edit?id=" + res;
                }
            });
        }
    });
}
function selectFile(field) {
    mcFileManager.init({
        rootpath: '~/Trans/Informes',
        urlprefix: '..',
        extensions: 'htm,html,pdf,ppt,pptx,doc,docx,xlsx,xls,pps'

    });
    mcFileManager.browse({ fields: field, relative_urls: true });

}
function selectXLS(field) {
    mcFileManager.init({
        rootpath: '~/Trans/Informes',
        urlprefix: '..',
        extensions: 'xlsx,xls'

    });
    mcFileManager.browse({ fields: field, relative_urls: true });

}
function selectFolder(field) {

    mcFileManager.init({
        rootpath : '~/Trans/Informes',
        urlprefix : '..',
        extensions:'htm,html'
        
    });
    mcFileManager.browse({ fields: field, relative_urls: true});
}
