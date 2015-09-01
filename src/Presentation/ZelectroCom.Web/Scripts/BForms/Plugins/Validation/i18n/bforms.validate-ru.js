(function (factory) {
    if (typeof define === "function" && define.amd) {
        define('validate-ru', ['jquery', 'bforms-validate'], factory);
    } else {
        factory(window.jQuery);
    }
}(function (jQuery) {
    jQuery.extend(jQuery.validator.messages, {
        required: "Поле не может быть пустым.",
        mandatory: 'Это обязательное поел.',
        remote: "Пожалуйста, исправьте это поле.",
        email: "Пожалуйста, введите корректный email.",
        url: "Пожалуйста, введите корректную ссылку.",
        date: "Пожалуйста, введите корректную дату.",
        dateISO: "Пожалуйста, введите корректную дату (ISO).",
        number: "Пожалуйста, введите число.",
        digits: "Пожалуйста, введите только цифры.",
        creditcard: "Пожалуйста, введите корректный номер кредитной карты.",
        equalTo: "Пожалуйста, введите значение повторно.",
        maxlength: $.validator.format("Пожалуйста, введите не более {0} символов."),
        minlength: $.validator.format("Пожалуйста, введите не менее {0} символов."),
        rangelength: $.validator.format("Пожалуйста, введите зачение длиной между {0} и {1} символ(ов)."),
        range: $.validator.format("Пожалуйста, введите зачение между {0} и {1}."),
        max: $.validator.format("Пожалуйста, введите зачение меньшее или равное {0}."),
        min: $.validator.format("Пожалуйста, введите зачение большее или равное {0}.")
    });
}));