require([
        'jquery',
        'bootstrap',
        'bforms-initUI',
        'bforms-ajax',
        'bforms-validate-unobtrusive',
        'bforms-extensions',
        'membercommon'
], function ($) {
    var zItemPage = function (options) {
        this.options = $.extend(true, {}, options);
    };

    zItemPage.prototype.init = function () {
        this.$zItemForm = $('.js-zItemForm');
        //apply BForms plugins
        this.$zItemForm.bsInitUI(this.options.styleInputs);
        this.addHandlers();
    };

    zItemPage.prototype.addHandlers = function () {
        this.$zItemForm.on('click', '.js-saveZItemBtn', $.proxy(this.onSaveItem, this));
    };

    zItemPage.prototype.baseSaveAction = function (e, targetUrl, onSuccessFunc) {
        e.stopPropagation();
        e.preventDefault();

        var $target = $(e.currentTarget);

        $.validator.unobtrusive.parse(this.$zItemForm);
        var validatedForm = this.$zItemForm.validate();
        if (this.$zItemForm.valid()) {
            
            var zItemData = this.$zItemForm.parseForm();
            $target.prop('disabled', "disabled");

            $.bforms.ajax({
                url: targetUrl,
                data: zItemData,
                headers: { 'RequestVerificationToken': GetAntiForgeryToken() },
                success: $.proxy(function () {
                    if (typeof onSuccessFunc === 'function') {
                        onSuccessFunc();
                    } else {
                        $target.removeProp('disabled');
                        $("#success-alert").alert();
                        $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
                            $("#success-alert").hide();
                        });
                    }
                }, this),
                validationError: function (response) {
                    if (response != null && response.Errors != null) {
                        validatedForm.showErrors(response.Errors, true);
                    }
                    $target.removeProp('disabled');
                }
            });
        }
    }

    zItemPage.prototype.onSaveItem = function (e) {
        this.baseSaveAction(e, this.options.saveZItemUrl, $.proxy(this.redirectOnPost, this));
    };

    zItemPage.prototype.redirectOnPost = function () {
        window.location.href = this.options.successUrl;
    };

    $(document).ready(function () {
        $("#success-alert").hide();
        var ctrl = new zItemPage(window.requireConfig.pageOptions.index);
        ctrl.init();
    });
})