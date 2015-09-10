require([
        'jquery',
        'bootstrap',
        'bforms-initUI',
        'bforms-validate-unobtrusive',
        'bforms-extensions',
        'membercommon'
], function ($) {
    var sectionPage = function (options) {
        this.options = $.extend(true, {}, options);
    };

    sectionPage.prototype.init = function () {
        this.$sectionForm = $('.js-sectionForm');

        //apply BForms plugins
        this.$sectionForm.bsInitUI(this.options.styleInputs);
    };

    $(document).ready(function () {
        var ctrl = new sectionPage(window.requireConfig.pageOptions.index);
        ctrl.init();
    });
})