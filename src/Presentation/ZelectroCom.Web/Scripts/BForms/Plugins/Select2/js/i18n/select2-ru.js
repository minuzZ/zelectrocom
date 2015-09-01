(function(factory) {
    if (typeof define === "function" && define.amd) {
        define('select2-ru', ['jquery', 'select2'], factory);
    } else {
        factory(window.jQuery);
    }
}(function($) {
    $.extend($.fn.select2.defaults, {
        formatNoMatches: function () { return "Совпадений не найдено"; },
        formatInputTooShort: function (input, min) { var n = min - input.length; return "Пожалуйста, введите еще " + n + " символ(ов)"; },
        formatInputTooLong: function (input, max) { var n = input.length - max; return "Пожалуйста, удалите " + n + " символ(ов)"; },
        formatSelectionTooBig: function (limit) { return "Можно выбрать только " + limit + " значений"; },
        formatLoadMore: function (pageNumber) { return "Загружаем..."; },
        formatSearching: function () { return "Поиск..."; },
        formatPlaceholder: function () {
            return "Выбрать";
        },
        formatSaveItem: function (val) {
            return "Добавить " + val;
        },
    });
}));
