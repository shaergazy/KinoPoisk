$(document).ready(function () {
    function updateStars(rating) {
        $('.rating-star').each(function () {
            var starValue = $(this).data('value');
            if (starValue <= rating) {
                $(this).css('color', '#f5b301');
            } else {
                $(this).css('color', '');
            }
        });
    }

    var initialRating = $('.rating').data('rating');
    updateStars(initialRating);

    var token = $('input[name="__RequestVerificationToken"]').val();

    $('.rating-star').on('click', function () {
        var movieId = $('.rating').data('movie-id'); // Получаем идентификатор фильма из атрибута data-movie-id
        var ratingValue = $(this).data('value');

        var formData = {
            MovieId: movieId,
            StarCount: ratingValue
        };

        $.ajax({
            url: `/Movies/Details/${movieId}?handler=Rate`, // Формируем URL с использованием movieId
            type: 'POST',
            headers: {
                "RequestVerificationToken": token
            },
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    toastr.success('Rating submitted successfully!');
                    updateStars(ratingValue);
                } else if (response.redirect) {
                    window.location.href = response.redirect;
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Error submitting rating: ' + error);
            }
        });
    });

    $('.rating-star').hover(
        function () {
            var hoverValue = $(this).data('value');
            updateStars(hoverValue);
        },
        function () {
            updateStars(initialRating);
        }
    );
});
