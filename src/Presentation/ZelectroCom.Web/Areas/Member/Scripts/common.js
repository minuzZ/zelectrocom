AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
};