require([
    'jquery',
    'bforms-namespace',
    'bforms-grid',
    'bootstrap',
    'membercommon'
], function ($) {
    var sectionsGrid = function (options) {
        this.options = $.extend(true, {}, options);
        this.init();
    };

    sectionsGrid.prototype.init = function () {
        this.$grid = $('#grid');
        this.initGrid();
    };

    sectionsGrid.prototype.initGrid = function () {
        this.$grid.bsGrid({
            pagerUrl: this.options.pagerUrl,
            rowActions: [
            {
                btnSelector: '.js-btn_delete',
                url: this.options.deleteUrl,
                init: $.proxy(this._deleteHandler, this),
                context: this
            }]
        });
        this.$grid.on('mouseover', '.grid_row', $.proxy(this._onMouseOver, this));
        this.$grid.on('mouseleave', '.grid_row', $.proxy(this._onMouseLeave, this));
        this.$grid.on('click', '.grid_row', $.proxy(this._onRowClick, this));
        this.$grid.on('click', '.actionCol', $.proxy(this._onActionColClick, this));
    };

    sectionsGrid.prototype._onActionColClick = function (e) {
        // do not open page when clicking "remove" action
        e.stopPropagation();
        e.preventDefault();
    }

    sectionsGrid.prototype._onRowClick = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title')) {
            window.location.href = this.options.editUrl + "/" + $row.data('objid');
        }
    };

    sectionsGrid.prototype._onMouseOver = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title')) {
            $row.addClass('selected');
            $row.css('cursor', 'pointer');
        }
    };

    sectionsGrid.prototype._onMouseLeave = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title')) {
            $row.removeClass('selected');
            $row.css('cursor', 'default');
        }
    };

    //#region DeleteHandler
    sectionsGrid.prototype._deleteHandler = function (options, $row, context) {

        var $btn = $row.find(options.btnSelector);

        $btn.bsInlineQuestion({
            question: "Вы уверены?",
            placement: 'auto',
            buttons: [{
                text: 'Да',
                cssClass: 'btn-primary bs-confirm',
                callback: $.proxy(function () {
                    var data = [];
                    data.push({
                        Id: $row.data('objid')
                    });

                    this._ajaxDelete($row, data, options.url, function () {

                        context._getPage(true);

                    }, function (response) {

                        if ($(options.btnSelector).is('.js-inlineAction')) {

                            context._pagerAjaxError(response, $row);

                        } else {

                            context._rowActionAjaxError(response, $row);

                        }
                    });

                    $btn.bsInlineQuestion('toggle');
                }, this)
            },
                {
                    text: 'Нет',
                    cssClass: 'btn-theme bs-cancel',
                    callback: function (e) {
                        $btn.bsInlineQuestion('toggle');
                    }
                }]
        });
    };

    sectionsGrid.prototype._ajaxDelete = function ($html, data, url, success, error) {
        var ajaxOptions = {
            name: '|delete|' + data,
            url: url,
            data: data,
            headers: { 'RequestVerificationToken': GetAntiForgeryToken() },
            context: this,
            success: success,
            error: error,
            loadingElement: $html,
            loadingClass: 'loading'
        };
        $.bforms.ajax(ajaxOptions);
    };
    //#endregion

    $(document).ready(function () {
        var page = new sectionsGrid(window.requireConfig.pageOptions.index);
    });
});