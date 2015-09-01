require([
        'jquery',
        'bootstrap',
        'bforms-initUI',
        'bforms-ajax',
        'bforms-validate-unobtrusive',
        'bforms-extensions',
        'membercommon',
        'ckeditor'
], function ($) {
    var draftPage = function (options) {
        this.options = $.extend(true, {}, options);
    };

    draftPage.prototype.init = function () {
        this.$draftForm = $('.js-draftForm');

        //apply BForms plugins
        this.$draftForm.bsInitUI(this.options.styleInputs);
        this.addHandlers();
    };

    draftPage.prototype.addHandlers = function () {
        this.$draftForm.on('click', '.js-saveDraftBtn', $.proxy(this.onSaveDraft, this));
        $('#previewBtn').on('click', null, $.proxy(this.onPreviewDraft, this));
    };

    draftPage.prototype.onSaveDraft = function (e) {
        e.stopPropagation();
        e.preventDefault();
        var $target = $(e.currentTarget);

        //set Text from CKeditor
        for (instance in CKEDITOR.instances)
            CKEDITOR.instances[instance].updateElement();

        $.validator.unobtrusive.parse(this.$draftForm);
        var validatedForm = this.$draftForm.validate();

        if (this.$draftForm.valid()) {

            var draftData = this.$draftForm.parseForm();

            $target.prop('disabled', "disabled");

            $.bforms.ajax({
                url: this.options.saveDraftUrl,
                data: draftData,
                headers: { 'RequestVerificationToken': GetAntiForgeryToken() },
                success: $.proxy(function () {
                    $target.removeProp('disabled');
                    $("#success-alert").alert();
                    $("#success-alert").fadeTo(2000, 500).slideUp(500, function () {
                        $("#success-alert").hide();
                    });
                }, this),
                validationError: function(response) {
                    if (response != null && response.Errors != null) {
                        validatedForm.showErrors(response.Errors, true);
                    }
                    $target.removeProp('disabled');
                }
            });
        }
    };

    draftPage.prototype.onPreviewDraft = function (e) {
        e.stopPropagation();
        e.preventDefault();

        //set Text from CKeditor
        for (instance in CKEDITOR.instances)
            CKEDITOR.instances[instance].updateElement();

        $.validator.unobtrusive.parse(this.$draftForm);
        var validatedForm = this.$draftForm.validate();

        if (this.$draftForm.valid()) {

            var draftData = this.$draftForm.parseForm();
            alert(this.options.previewUrl);
            $.bforms.ajax({
                url: this.options.previewUrl,
                data: draftData,
                headers: { 'RequestVerificationToken': GetAntiForgeryToken() },
                success: $.proxy(function () {
                    alert("OK");
                }, this),
                validationError: function (response) {
                    if (response != null && response.Errors != null) {
                        validatedForm.showErrors(response.Errors, true);
                    }
                }
            });
        }
    };

    $(document).ready(function () {

        $("#success-alert").hide();
        $("[data-hide]").on("click", function () {
            $(this).closest("." + $(this).attr("data-hide")).hide();
        });

        var ctrl = new draftPage(window.requireConfig.pageOptions.index);
        ctrl.init();

        //set draft id to find article content folder
        var roxyFileman = '../../../Scripts/fileman/index.html?articleId=' + $('#Id').val();
        CKEDITOR.replace("articleEditor",
            {
                filebrowserBrowseUrl: roxyFileman,
                filebrowserImageBrowseUrl: roxyFileman + '&type=image',
                removeDialogTabs: 'link:upload;link:advanced;image:Upload;image:advanced;'
            }
        );
    });
})