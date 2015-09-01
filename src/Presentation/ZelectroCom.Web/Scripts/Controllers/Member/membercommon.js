AddAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
};

GetAntiForgeryToken = function() {
    return $('[name=__RequestVerificationToken]').val();
}