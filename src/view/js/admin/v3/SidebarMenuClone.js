$(window).ready(function () {

    $("#btnUserMenu").popup({
        popup: $("#userMenuPopup"),
        on: "click",
        position: "bottom center"
    });

    $(".left.menu .item").each(function (idx, item) {
        let c = $(item).clone();

        let m = $(c).find(".menu, .dropdown");
        if (m.length) {
            m.remove();
            $(c).addClass("header");
        }

        $(".sidebar").append(c);
    });

    $("#btnSidebar").click(function () {
        $(".ui.sidebar").sidebar("toggle");
    });
});