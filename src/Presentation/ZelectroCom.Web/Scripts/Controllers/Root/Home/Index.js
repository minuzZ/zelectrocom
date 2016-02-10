require([
        'jquery',
        'bootstrap',
        'ias',
        'spoiler',
        'highlightjs',
        'scrollup'
], function ($) {
    $('#newArticles').addClass('active');

    var ias = jQuery.ias({
        container: '#posts',
        item: '.post',
        pagination: '#pagination',
        next: '.next'
    });

    ias.extension(new IASSpinnerExtension());
    ias.extension(new IASTriggerExtension({
        offset: 100,
        htmlPrev: '<div class="ias-trigger ias-trigger-prev" style="text-align: center; cursor: pointer;"><a class="btn z-btn-primary" style="margin-top: 20px;">Загрузить предыдущие статьи</a></div>'
    }));
    ias.extension(new IASPagingExtension());
    ias.extension(new IASHistoryExtension({}));
    ias.extension(new IASNoneLeftExtension({ text: "Ой.. Это всё :)" }));

    $.scrollUp({
        scrollName: 'scrollUp', // Element ID
        scrollText: '<span class="glyphicon glyphicon-chevron-up"></span>'
    });

    $('#scrollUp').addClass('hidden-xs hidden-sm');
})