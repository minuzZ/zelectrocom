require([
        'jquery',
        'bootstrap',
        'jscroll',
        'spoiler',
        'highlightjs'
], function ($) {
    $(function () {
        $('#postslist').jscroll({
            loadingHtml: '<img src="/Content/img/ajax-loader.gif" />'
        });
    });
})