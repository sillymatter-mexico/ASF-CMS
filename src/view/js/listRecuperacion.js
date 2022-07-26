$(document).ready(function(){
$("#lista").css('display', 'block');
$("#item").css('display', 'none');
});

function showRecuperacion(id)
{
    $("#lista").css('display', 'none');
    $("#item").css('display', 'block');
    $("#item").html('');
    $.ajax({
        type: "POST",
        async: true,
        url: "../Recuperacion/FriendlyShow",
        data: "id="+id,
        success: function(datos) {
        $("#item").html(datos);
        }
    });

}
function showList(id)
{
    $("#item").css('display', 'none');
    $("#item").html('');
    $("#lista").css('display', 'block');
/*    $.ajax({
        type: "POST",
        async: false,
        url: "../Recuperacion/FriendlyShow",
        data: "id="+id,
        success: function(datos) {
        $("#item").html(datos);
        }
    });
*/
}
