$(document).ready(function () {
    $(".linkAccessCapture").click(function (e) {

        let origin = e.target.closest(`a`);

        if(origin) {

            let data = new FormData();
            data.append("accessUrl", origin.href);
            navigator.sendBeacon("/Ajax/IncreaseLinkCounter", data);
        }
    });
});