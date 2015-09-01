require([
    'jquery',
    'bforms-namespace',
    'bforms-grid',
    'bootstrap',
    'membercommon'
], function () {
    var draftsGrid = function (options) {
        this.options = $.extend(true, {}, options);
        this.init();
    };

    draftsGrid.prototype.init = function () {
        this.$grid = $('#grid');
        this.initGrid();
    };

    draftsGrid.prototype.initGrid = function() {
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
        $('.actionCol').click(function (event) {
            // do not open draft when clicking "remove" action
            event.stopPropagation();
        });
    };

    draftsGrid.prototype._onRowClick = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title')) {
            window.location.href = this.options.draftUrl + "/" + $row.data('objid');
        }
    };

    draftsGrid.prototype._onMouseOver = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title')) {
            $row.addClass('selected');
            $row.css('cursor', 'pointer');
        }
    };

    draftsGrid.prototype._onMouseLeave = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title')) {
            $row.removeClass('selected');
            $row.css('cursor', 'default');
        }
    };

    //#region DeleteHandler
    draftsGrid.prototype._deleteHandler = function (options, $row, context) {

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

    draftsGrid.prototype._ajaxDelete = function ($html, data, url, success, error) {
        var ajaxOptions = {
            name: '|delete|' + data,
            url: url,
            data: data,
            headers: { 'RequestVerificationToken' : GetAntiForgeryToken() },
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
        var page = new draftsGrid(window.requireConfig.pageOptions.index);
    });
});