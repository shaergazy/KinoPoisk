$(document).ready(function () {
    var maxLength = 1000;
    loadTranslations(currentCulture);
    function toggleDescription(description, movieId) {
        var fullText = description.data('fullText') || description.text().trim();
        description.data('fullText', fullText);

        if (fullText.length > maxLength) {
            var truncatedText = fullText.substring(0, maxLength) + '...';
            description.html(truncatedText + ` <span class="more" id="more-${movieId}">${getTranslation('dataTable.more')}</span>`);
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

        description.html(fullText + ` <span class="less" id="less-${movieId}">${getTranslation('dataTable.less')}</span>`);
    });

    $('#moviesTable').on('click', '.less', function () {
        var description = $(this).parent();
        var movieId = $(this).attr('id').split('-')[1];
        toggleDescription(description, movieId);
    });
    
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
    console.log(`${getTranslation('rating.rating_for_movie')} ${movieId} ${getTranslation('rating.is')} ${rating} ${getTranslation('rating.stars')}`);
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

$('.rating').each(function () {
    updateStars($(this));
});

var token = $('input[name="__RequestVerificationToken"]').val();

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
                alert(getTranslation('notification.rating_success'));
                location.reload(); 
            } else if (response.redirect) {
                window.location.href = response.redirect;
            } else {
                alert(getTranslation('error.rating_failed'));
            }
        },
        error: function (xhr, status, error) {
            alert(getTranslation('error.rating_submission') + error);
        }
    });
});

$('.rating span').hover(
    function () {
        var hoverValue = $(this).data('value');
        $(this).prevAll().addBack().css('color', '#f5b301');
    },
    function () {
        updateStars($(this).closest('.rating'));
    }
);
