require([
    'jquery',
    'bforms-namespace',
    'bforms-grid',
    'bforms-toolbar',
    'bootstrap',
    'membercommon'
], function () {
    var publicationsGrid = function (options) {
        this.options = $.extend(true, {}, options);
        this.init();
    };

    publicationsGrid.prototype.init = function () {
        this.$grid = $('#grid');
        this.$toolbar = $('#toolbar');

        this.initGrid();
        this.initToolbar();
    };

    publicationsGrid.prototype.initGrid = function() {
        this.$grid.bsGrid({
            $toolbar: this.$toolbar,
            pagerUrl: this.options.pagerUrl,
            countUrl: this.options.countUrl,
            //#region filterButtons
            filterButtons: [{
                btnSelector: '.js-actives',
                filter: function ($el) {
                    return $el.data('active') == 'True';
                }
            }, {
                btnSelector: '.js-inactives',
                filter: function ($el) {
                    return $el.data('active') != 'True';
                },
            }],
            //#endregion
            detailsUrl: this.options.getRowsUrl,
        });


        this.$grid.on('mouseover', '.grid_row', $.proxy(this._onMouseOver, this));
        this.$grid.on('mouseleave', '.grid_row', $.proxy(this._onMouseLeave, this));
        this.$grid.on('click', '.grid_row', $.proxy(this._onRowClick, this));
    };

    publicationsGrid.prototype._onRowClick = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title') && !$row.hasClass('bs-noResultsRow')) {
            window.location.href = this.options.editUrl + "/" + $row.data('objid');
        }
    };

    publicationsGrid.prototype._onMouseOver = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title') && !$row.hasClass('bs-noResultsRow')) {
            $row.addClass('selected');
            $row.css('cursor', 'pointer');
        }
    };

    publicationsGrid.prototype._onMouseLeave = function (e) {
        var $row = $(e.currentTarget);
        if (!$row.hasClass('title') && !$row.hasClass('bs-noResultsRow')) {
            $row.removeClass('selected');
            $row.css('cursor', 'default');
        }
    };

    publicationsGrid.prototype.initToolbar = function () {

        // on init
        this.$toolbar.bsToolbar({
            uniqueName: 'usersToolbar',
            controlsOptions: {
                focusFirst: false
            },
            //customControlsOptions: {
            //    AdvancedSearch: {
            //        focusFirst: true
            //    }
            //},
            subscribers: [this.$grid]/*,
            autoInitControls: false,
            //initialize default controls manually
            controls: [
                $.bforms.toolbar.defaults.advancedsearch,
                $.bforms.toolbar.controls.yourCustomControl
            ]*/
        });



        //// after init
        //this.$toolbar.bsToolbar('controls', [$.bforms.toolbar.controls.yourCustomControl]);

        //// Step 1: get advanced search plugin from toolbar defaults namespace
        //var advancedsearch = new $.bforms.toolbar.defaults.advancedsearch(this.$toolbar);

        //// Step 2: update button settings
        //advancedsearch.setcontrol('search', {
        //    handler: $.proxy(function () {
        //        console.log('custom');
        //        var widget = $('#toolbar').data('bformsBsToolbar');
        //        for (var i = 0; i < widget.subscribers.length; i++) {
        //            widget.subscribers[i].bsGrid('search', data);
        //        }
        //    }, this)
        //});

        //// Step 3: add control to toolbar
        //this.$toolbar.bsToolbar('controls', [advancedSearch]);

    };

    $(document).ready(function () {
        var page = new publicationsGrid(window.requireConfig.pageOptions.index);
    });
});