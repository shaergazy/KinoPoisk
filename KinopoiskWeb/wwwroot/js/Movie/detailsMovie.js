$(document).ready(function () {
    var movieId = $('.rating').data('movie-id');
    var token = $('input[name="__RequestVerificationToken"]').val();

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

    $('.rating-star').on('click', function () {
        var ratingValue = $(this).data('value');
        var formData = {
            MovieId: movieId,
            StarCount: ratingValue
        };

        $.ajax({
            url: `/Movies/Details/${movieId}?handler=Rate`,
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

    var table = $('#commentsTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": `/Movies/Details/${movieId}?handler=LoadComments`,
            "type": "POST",
            "data": function (d) {
                d.movieId = movieId;
                d.length = 10; // Установить количество загружаемых записей
            }
        },
        "scrollY": "400px", // Установить высоту таблицы
        "scrollCollapse": true,
        "paging": false, // Отключить стандартную пагинацию
        "deferRender": true, // Включить отложенный рендеринг
        "columns": [
            { "data": "userName" },
            { "data": "text" },
            { "data": "date", "render": function (data) { return moment(data).format('DD/MM/YYYY'); } }
        ]
    });

    // Функция для подгрузки комментариев при прокрутке вниз
    $('#commentsTable').on('scroll', function () {
        var scrollHeight = this.scrollHeight;
        var scrollTop = this.scrollTop;
        var height = $(this).height();
        if (scrollHeight - scrollTop <= height) {
            table.ajax.reload(null, false); // Перезагрузка данных без сброса пагинации
        }
    });

    $('#add-comment-form').on('submit', function (event) {
        event.preventDefault();
        var comment = $('#comment').val();

        if (comment.trim() === '') {
            toastr.error('Comment cannot be empty.');
            return;
        }

        var formData = {
            MovieId: movieId,
            CommentText: comment
        };

        $.ajax({
            url: `/Movies/Details/${movieId}?handler=AddComment`,
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
                    toastr.success('Comment added successfully.');
                } else if (response.redirect) {
                    window.location.href = response.redirect;
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Error adding comment: ' + error);
            }
        });
    });
});
