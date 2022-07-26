$().ready(function() {
    $('#preloadTabs').tabs();
    $("#entityList").jqGrid({
        datatype: "json",
        url: '../Auditoria/GetPreloadInsertableEntities?year=' + $("#year").val(),
        colNames: ['Clave', 'Dependencia', 'Tipo', 'Siglas', 'Nombre'],
        colModel: [
            { name: 'EntityKey', index: 'EntityKey', width: 80, sortable: false },
            { name: 'DepKey', index: 'DepKey', width: 80, sortable: false },
            { name: 'TypeKey', index: 'TypeKey', width: 80, sortable: false},
            { name: 'ShortName', index: 'ShortName', width: 100,sortable: false },
            { name: 'Name', index: 'Name', align: 'left', width: 300,sortable: false },
        ],
        height: 'auto',
        rowNum: 250,
        caption: "Entidades"
    });
     $("#sectorList").jqGrid({
            datatype: "json",
            url: '../Auditoria/GetPreloadInsertableSectors?year=' + $("#year").val(),
            colNames: ['clave', 'Nombre'],
            colModel: [
                { name: 'AsfKey', index: 'AsfKey', width: 80, sortable: false },
                { name: 'Name', index: 'Name', width: 200, sortable: false },
            ],
            height: 'auto',
            rowNum: 280,
            caption: "Sectores"
        });
     $("#groupList").jqGrid({
            datatype: "json",
            url: '../Auditoria/GetPreloadInsertableGroups?year=' + $("#year").val(),
            colNames: ['Clave', 'Nombre'],
            colModel: [
                { name: 'Code', index: 'Code', width: 80, sortable: false },
                { name: 'Name', index: 'Name', width: 200, sortable: false },
            ],
            height: 'auto',
            rowNum: 280,
            caption: "Sectores"
        });

    $("#completeAuditList").jqGrid({
        datatype: "json",
        url: '../Auditoria/GetPreloadCompleteAuditList?year=' + $("#year").val(),
        colNames: ['Tipo', 'Clave', 'Numero', 'Titulo', 'Entidad', 'Archivo'],
        colModel: [
            { name: 'TypeName', index: 'TypeName', width: 150, sortable: false },
            { name: 'SectorName', index: 'SectorName', width: 100, sortable: false },
            { name: 'Number', index: 'Number', width: 80,sortable: false },
            { name: 'Title', index: 'Title', width: 350,sortable: false },
            { name: 'EntityName', index: 'EntityName', width: 250,sortable: false },
            { name: 'File', index: 'File', align: 'right', width: 200,sortable: false }
        ],
        height: 'auto',
        rowNum: 2000,
        caption: "Auditorias"
    });
    $("#orphanAuditList").jqGrid({
        datatype: "json",
        url: '../Auditoria/GetPreloadOrphanAuditList?year=' + $("#year").val(),
        colNames: ['Tipo', 'Clave', 'Numero', 'Titulo', 'Entidad', 'Archivo'],
        colModel: [
            { name: 'TypeName', index: 'TypeName', width: 100, sortable: false },
            { name: 'SectorName', index: 'SectorName', width: 80, sortable: false },
            { name: 'Number', index: 'Number', width: 80,sortable: false },
            { name: 'Title', index: 'Title', width: 300,sortable: false },
            { name: 'EntityName', index: 'EntityName', width: 300,sortable: false },
            { name: 'File', index: 'File', align: 'right', width: 300 ,sortable: false   }
        ],
        height: 'auto',
        rowNum: 250,
        caption: "Auditorias"
    });
    $("#orphanFileList").jqGrid({
        datatype: "json",
        url: '../Auditoria/GetPreladOrphanFileList?year=' + $("#year").val(),
        colNames: ['Clave', 'Archivo', 'Procedencia'],
        colModel: [
            { name: 'SectorName', index: 'SectorName', width: 150, sortable: false },
            { name: 'File', index: 'File', width: 300, sortable: false },
            { name: 'Title', index: 'Title', width: 400,sortable: false }
        ],
        height: 'auto',
        width: 'auto',
        rowNum: 250,
        caption: "Entidades"
    });
    $("#btnBack").click(function() {
        $.ajax({
            type: "POST",
            async: false,
            url: "Cancel",
            data: 'year=' + $("#year").val(),
            success: function(datos) {

                history.back(1);
            }
        });
    });
    $("#btnGo").click(function() {
        var res = confirm("Se realizará la carga. Los archivos sin auditoria serán ignorados");
        if (!res)
            return;
        $.ajax({
            type: "POST",
            async: false,
            url: "Insert",
            data: 'year=' + $("#year").val(),
            success: function(datos) {
                alert(datos);
                window.location = "../AuditReport/List";
            }
        });
    });

});
