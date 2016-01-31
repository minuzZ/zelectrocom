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
        this.$draftForm.on('click', '.js-postDraftBtn', $.proxy(this.showModal, this));
        this.$draftForm.on('click', '.js-returnBtn', $.proxy(this.onReturn, this));
        this.$draftForm.on('click', '.js-submitBtn', $.proxy(this.onSubmit, this));
        this.$draftForm.on('click', '.js-proposeChangesBtn', $.proxy(this.onProposeChanges, this));
        this.$draftForm.on('click', '#okPostBtn', $.proxy(this.onPostDraft, this));
    };

    draftPage.prototype.baseSaveAction = function (e, targetUrl, onSuccessFunc) {
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
                url: targetUrl,
                data: draftData,
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

    draftPage.prototype.onSaveDraft = function (e) {
        this.baseSaveAction(e, this.options.saveDraftUrl);
    };

    draftPage.prototype.onPostDraft = function (e) {
        this.baseSaveAction(e, this.options.postUrl, $.proxy(this.redirectOnPost, this));
    };

    draftPage.prototype.redirectOnPost = function () {
        window.location.href = this.options.successUrl;
    };

    draftPage.prototype.showModal = function (e) {
        e.preventDefault();
        $("#modalBox").modal('show');
    };

    draftPage.prototype.onReturn = function (e) {
        this.baseSaveAction(e, this.options.returnUrl);
    };

    draftPage.prototype.onSubmit = function (e) {
        this.baseSaveAction(e, this.options.submitUrl);
    };

    draftPage.prototype.onProposeChanges = function (e) {
        this.baseSaveAction(e, this.options.changesUrl);
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
                removeDialogTabs: 'link:upload;link:advanced;image:Upload;image:advanced;',
                on: {
                    instanceReady: function () {
                        this.dataProcessor.htmlFilter.addRules({
                            elements: {
                                img: function (el) {
                                    // Add some class.
                                    el.addClass('z-img-responsive');
                                }
                            }
                        });
                    }
                }
            }
        );
    });
})