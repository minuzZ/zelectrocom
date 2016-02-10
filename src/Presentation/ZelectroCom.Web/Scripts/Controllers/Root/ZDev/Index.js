define([
        'jquery',
        'bootstrap',
        'ias',
        'masonry',
        'imagesLoaded',
        'jquery.bridget'
], function ($, bs, InfiniteScroll, Masonry, ImagesLoaded, bridget) {

    initialize : function a() {
        bridget('masonry', Masonry);
        bridget('imagesLoaded', ImagesLoaded);
    }

    ImagesLoaded('.masonry-container', function () {
        var msnry = new Masonry('.masonry-container', {
            columnWidth: '.grid-sizer',
            itemSelector: '.item',
            percentPosition: true
        });
        var ias = jQuery.ias({
            container: '.masonry-container',
            item: '.item',
            pagination: '#pagination',
            next: '.next'
        });
        ias.on('render', function (items) {
            $(items).css({ opacity: 0 });
        });

        ias.on('rendered', function (items) {
            msnry.appended(items);
        });
    });

});