const DATE_FORMAT = 'DD/MM/YYYY';

$(document).ready(function () {
    loadTranslations(currentCulture).done(function () {

    function toggleDescription(description, movieId) {
        const fullText = description.data('fullText') || description.text().trim();
        description.data('fullText', fullText); 

        if (fullText.length > maxLength) {
            const truncatedText = fullText.substring(0, maxLength) + '...';
            description.html(truncatedText + ' <span class="more" id="more-' + movieId + '">' + getTranslation('button.more') + '</span>');
        }
    }

    $('.description').each(function () {
        const description = $(this);
        const movieId = description.attr('id') ? description.attr('id').split('-')[1] : null;
        if (movieId) {
            toggleDescription(description, movieId);
        }
    });

    $('#moviesTable').on('click', '.more', function () {
        const description = $(this).parent();
        const movieId = $(this).attr('id').split('-')[1];
        const fullText = description.data('fullText');

        description.html(fullText + ' <span class="less" id="less-' + movieId + '">' + getTranslation('button.less') + '</span>');
    });

    $('#moviesTable').on('click', '.less', function () {
        const description = $(this).parent();
        const movieId = $(this).attr('id').split('-')[1];
        toggleDescription(description, movieId);
    });

    function updateStars(ratingElement) {
        const rating = ratingElement.data('rating');
        ratingElement.find('span').each(function () {
            const starValue = $(this).data('value');
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

    const token = $('input[name="__RequestVerificationToken"]').val();

    $('.rating span').on('click', function () {
        const movieId = $(this).closest('.rating').data('movie-id');
        const ratingValue = $(this).data('value');

        const formData = {
            MovieId: movieId,
            StarCount: ratingValue
        };

        $.ajax({
            url: Urls.Movie.Rate + `/${movieId}`,
            type: 'POST',
            headers: {
                "RequestVerificationToken": token
            },
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    toastr.success(getTranslation('notification.rating_success'));
                    updateStars($(this).closest('.rating').data('rating', ratingValue));
                } else if (response.redirect) {
                    window.location.href = response.redirect;
                }
            },
            error: function (xhr, status, error) {
                toastr.error(getTranslation('error.rating') + ': ' + error);
            }
        });
    });

    $('.rating span').hover(
        function () {
            const hoverValue = $(this).data('value');
            $(this).prevAll().addBack().css('color', '#f5b301');
        },
        function () {
            updateStars($(this).closest('.rating'));
        }
    );

    const movieId = $('.rating').data('movie-id');

    const table = $('#commentsTable').DataTable({
        "processing": true,
        "serverSide": true,
        "searching": false,
        "paging": true,
        "info": false,
        "ajax": {
            "url": Urls.Movie.LoadComments + `/${movieId}`,
            "type": "POST",
            "data": function (d) {
                d.movieId = movieId;
            }
        },
        "scrollY": "200px",
        "scrollCollapse": true,
        "deferRender": true,
        "language": {
            "url": `/js/locals/datatables/${currentCulture}.json`
        },
        "columns": [
            { "data": "userName" },
            { "data": "text" },
            { "data": "date", "render": function (data) { return moment(data).format(DATE_FORMAT); } }
        ]
    });

    $('#add-comment-form').on('submit', function (event) {
        event.preventDefault();
        const comment = $('#comment').val();

        if (comment.trim() === '') {
            toastr.error(getTranslation('error.empty_comment'));
            return;
        }

        const formData = {
            MovieId: movieId,
            CommentText: comment
        };

        $.ajax({
            url: Urls.Movie.AddComment + `/${movieId}`,
            type: 'POST',
            headers: {
                "RequestVerificationToken": token
            },
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    $('#comments-list').append(`<li>${comment}</li>`);
                    $('#comment').val('');
                    toastr.success(getTranslation('notification.comment_added'));
                } else if (response.redirect) {
                    window.location.href = response.redirect;
                }
            },
            error: function (xhr, status, error) {
                toastr.error(getTranslation('error.adding_comment') + ': ' + error);
            }
        });
    });
    });
});
