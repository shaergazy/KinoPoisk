$(document).ready(function () {
    var maxLength = 1000; // Максимальная длина сокращенного текста

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
    $('.rating').each(function () {
        var ratingValue = parseFloat($(this).data('rating'));
        $(this).find('span').each(function (index) {
            if (index < ratingValue) {
                $(this).addClass('active');
            }
        });
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

// Function to update the stars based on the current rating
function updateStars(ratingElement) {
    var rating = ratingElement.data('rating');
    ratingElement.find('span').each(function () {
        var starValue = $(this).data('value');
        if (starValue <= rating) {
            $(this).css('color', '#f5b301');
        } else {
            $(this).css('color', '');
        }
    });
}

// Update stars for all movie ratings
$('.rating').each(function () {
    updateStars($(this));
});

// Get CSRF token
var token = $('input[name="__RequestVerificationToken"]').val();

// Handle star click event
$('.rating span').on('click', function () {
    var movieId = $(this).closest('.rating').attr('id').split('-')[1];
    var ratingValue = $(this).data('value');

    var formData = {
        MovieId: movieId,
        StarCount: ratingValue
    };

    $.ajax({
        url: '@Url.Page("/Movies/Details", new { id = "", handler = "Rate" })',
        type: 'POST',
        headers: {
            "RequestVerificationToken": token
        },
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                alert('Rating submitted successfully!');
                location.reload(); // Reload the page to update the rating
            } else if (response.redirect) {
                window.location.href = response.redirect;
            } else {
                alert('Failed to submit rating.');
            }
        },
        error: function (xhr, status, error) {
            alert('Error submitting rating: ' + error);
        }
    });
});

// Handle star hover event
$('.rating span').hover(
    function () {
        var hoverValue = $(this).data('value');
        $(this).prevAll().addBack().css('color', '#f5b301');
    },
    function () {
        updateStars($(this).closest('.rating'));
    }
);
