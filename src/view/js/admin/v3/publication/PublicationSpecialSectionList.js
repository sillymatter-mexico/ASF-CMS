$(document).ready(() => {

    $("[id^='publicationList']").each(function (idx, elem) {
        let type = $(elem).attr('id').split('-')[1];
        let sectionId = parseInt($("#sectionId-" + type).val());

        let dataTable = $(elem).DataTable({
            ajax: {
                url: "../SectionSpecialAdm/ListPublicationBySpecial?specialPublicationType=" + type,
                dataSrc: "rows"
            },
            rowId: "id",
            columns: addEnable ? [...asfCms.publication.simpleColDefs, asfCms.publication.deleteColDef] : asfCms.publication.simpleColDefs,
            buttons: addEnable ? [{
                text: "<i class=\"plus icon\"></i>Agregar publicación",
                action: function (e, dt, node, config) {
                    AddContentDialog(
                        "special",
                        addPublicationDialogLabels,
                        "navigate",
                        sectionId,
                        type,
                        function (err, data) {
                            console.log("Regrese");
                        }
                    );
                }
            }] : [],
            initComplete: function () {
                $(dataTable.table().container()).find('div.eight.column:eq(0)').append(dataTable.buttons().container());
            },
            rowCallback: function (row, data, displayNum, displayIdx, dataIdx) {
                $(row).find("td.publicationDelete > i").click(function () {
                    SimpleDialog("warn", "Eliminar publicación especial", "Esta accíón eliminará la publicación seleccionada ¿desea continuar?", function (res, ele) {
                        if (res) {
                            console.log("A eliminar");
                        }
                    });
                });
            }
        });
    });
});

function deletePublication(publicationId) {
    if (!confirm("Se borrara la publicacion. ¿Desea continuar?"))
        return;
    $.ajax({
        type: "POST",
        async: false,
        url: "../PublicationAdm/Delete",
        data: "publicationId=" + publicationId,
        success: function (datos) {
            $("[id^='publicationList']").trigger("reloadGrid");
            alert(datos);
        }
    });
}