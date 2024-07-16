$(document).ready(function () {
    var maxLength = 100; // Максимальная длина сокращенного текста

    function toggleDescription(description, movieId) {
        var fullText = description.data('fullText') || description.text().trim();
        description.data('fullText', fullText); // Сохраняем полный текст в data атрибуте

        if (fullText.length > maxLength) {
            var truncatedText = fullText.substring(0, maxLength) + '...';
            description.html(truncatedText + ' <span class="more" id="more-' + movieId + '">more</span>');
        }
    }

    $('.description').each(function () {
        var description = $(this);
        var movieId = description.attr('id') ? description.attr('id').split('-')[1] : null;
        if (movieId) {
            toggleDescription(description, movieId);
        }
    });

    $('#moviesTable').on('click', '.more', function () {
        var description = $(this).parent();
        var movieId = $(this).attr('id').split('-')[1];
        var fullText = description.data('fullText');

        description.html(fullText + ' <span class="less" id="less-' + movieId + '">less</span>');
    });

    $('#moviesTable').on('click', '.less', function () {
        var description = $(this).parent();
        var movieId = $(this).attr('id').split('-')[1];
        toggleDescription(description, movieId);
    });

    // Добавление рейтинга
    $('.rating span').on('click', function () {
        const rating = $(this).data('value');
        const movieId = $(this).parent().attr('id').split('-')[1];
        addRating(movieId, rating);

        // Подсветка выбранных звезд
        $(this).siblings().addBack().css('color', '#000');
        $(this).prevAll().addBack().css('color', 'gold');
    });
});

function addRating(movieId, rating) {
    // Логика добавления рейтинга (например, отправка на сервер)
    console.log(`Рейтинг фильма ${movieId} составляет ${rating} звезд`);
}

function addComment(movieId) {
    const commentInput = $(`#comment-input-${movieId}`);
    const commentList = $(`#comment-list-${movieId}`);
    const comment = commentInput.val();

    if (comment) {
        commentList.append(`<li>${comment}</li>`);
        commentInput.val('');
    }
}
