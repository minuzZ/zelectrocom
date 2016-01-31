require([
    'jquery',
    'bforms-namespace',
    'bforms-grid',
    'bootstrap',
    'membercommon'
], function () {
    var pubReqGrid = function (options) {
        this.options = $.extend(true, {}, options);
        this.init();
    };

    pubReqGrid.prototype.init = function () {
        this.$grid = $('#grid');
        this.initGrid();
    };

    pubReqGrid.prototype.initGrid = function() {
        this.$grid.bsGrid({
            pagerUrl: this.options.pagerUrl
        });
        this.$grid.on('mouseover', '.grid_row', $.proxy(this._onMouseOver, this));
        this.$grid.on('mouseleave', '.grid_row', $.proxy(this._onMouseLeave, this));
        this.$grid.on('click', '.grid_row', $.proxy(this._onRowClick, this));
    };

    pubReqGrid.prototype._onRowClick = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title') && !$row.hasClass('bs-noResultsRow')) {
            window.location.href = this.options.editUrl + "/" + $row.data('objid');
        }
    };

    pubReqGrid.prototype._onMouseOver = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title') && !$row.hasClass('bs-noResultsRow')) {
            $row.addClass('selected');
            $row.css('cursor', 'pointer');
        }
    };

    pubReqGrid.prototype._onMouseLeave = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title') && !$row.hasClass('bs-noResultsRow')) {
            $row.removeClass('selected');
            $row.css('cursor', 'default');
        }
    };
    //#endregion

    $(document).ready(function () {
        var page = new pubReqGrid(window.requireConfig.pageOptions.index);
    });
});