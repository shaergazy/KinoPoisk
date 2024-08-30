// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// wwwroot/js/localization.js

function loadTranslations(language) {
    return $.getJSON(`/js/locals/texts/${language}.json`)
        .done(function (data) {
            localStorage.setItem('translations', JSON.stringify(data));
        });
}

function getTranslation(key) {
    const translations = JSON.parse(localStorage.getItem('translations'));

    // Разбиваем ключ по точке, чтобы получить массив ключей
    const keys = key.split('.');

    // Итерируемся по ключам, чтобы спуститься в иерархии объекта
    let result = translations;
    for (const k of keys) {
        if (result[k]) {
            result = result[k];
        } else {
            return key; // Если ключ не найден, возвращаем сам ключ
        }
    }

    return result;
}

