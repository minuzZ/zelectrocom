require([
    'jquery',
    'bforms-namespace',
    'bforms-grid',
    'bootstrap',
    'membercommon'
], function ($) {
    var oldMediaGrid = function (options) {
        this.options = $.extend(true, {}, options);
        this.init();
    };

    oldMediaGrid.prototype.init = function () {
        this.$grid = $('#grid');
        this.initGrid();
    };

    oldMediaGrid.prototype.initGrid = function () {
        this.$grid.bsGrid({
            pagerUrl: this.options.pagerUrl
        });
    };

    $(document).ready(function () {
        var page = new oldMediaGrid(window.requireConfig.pageOptions.index);
    });
});