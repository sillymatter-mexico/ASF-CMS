let currentMenu = null;

$(document).ready(function() {
    $('.tabs').find('[id^=tab]')
    .mouseenter(function (e) {
        if (currentMenu)
            currentMenu.hide();

        let parent = $(e.currentTarget);
        let id = typeof parent.attr('id') == 'string' ? parent.attr('id') : parent[0].id;
        let sufix = id.split('-')[1];
        currentMenu = $('#menu-' + sufix)
        .show()
        .position({ my: 'center top', of: parent, at: 'center center' });
    })
    .mouseleave(function (e) {
        let under = $(document.elementFromPoint(e.clientX, e.clientY));
        if (currentMenu != null && under.attr('id') != currentMenu.attr('id') && !$.contains(currentMenu[0], under[0])) {
            currentMenu.hide();
            currentMenu = null;
        }
    });
    $('.tabs').find('[id^=menu]')
    .mouseleave(function(e) {
        let parent = $(e.currentTarget);
        let id = typeof parent.attr('id') == 'string' ? parent.attr('id') : parent[0].id;
        let sufix = id.split('-')[1];
        let under = $(document.elementFromPoint(e.clientX, e.clientY));
        let section = $('#tab-' + sufix);

        if (currentMenu != null && under.attr('id') != section.attr('id') && !$.contains(section[0], under[0])) {
            currentMenu.hide();
            currentMenu = null;
        }
    });
});