$(document).ready(function () {
    var maxLength = 100; // Максимальная длина сокращенного текста

    $('.description').each(function () {
        var description = $(this);
        var fullText = description.text().trim();
        var movieId = description.attr('id') ? description.attr('id').split('-')[1] : null;

        if (movieId && fullText.length > maxLength) {
            var truncatedText = fullText.substring(0, maxLength) + '...';
            description.html(truncatedText + ' <span class="more" id="more-' + movieId + '">more</span>');

            $('#more-' + movieId).click(function () {
                if ($(this).text() === 'more') {
                    description.html(fullText + ' <span class="less" id="less-' + movieId + '">less</span>');
                }

                $('#less-' + movieId).click(function () {
                    description.html(truncatedText + ' <span class="more" id="more-' + movieId + '">more</span>');
                });
            });
        }
    });
});
