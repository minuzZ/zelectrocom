require([
        'jquery',
        'bootstrap',
        'membercommon',
        'bforms-namespace',
        'bforms-panel',
        'bforms-fileupload'
], function () {
    $('.bs-userInfo').bsPanel({
        name: 'userInfo',
        editSuccessHandler: function (e, data) {
            $('.js-fullName').text(data.Profile.Firstname + " " + data.Profile.Lastname);
            $('.js-descr').text(data.Profile.Description);
        }
    });
});