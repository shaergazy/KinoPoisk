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

    // Обновление звезд для всех элементов рейтинга
    $('.rating').each(function () {
        updateStars($(this));
    });

    var token = $('input[name="__RequestVerificationToken"]').val();

    $('.rating span').on('click', function () {
        var movieId = $(this).closest('.rating').data('movie-id');
        var ratingValue = $(this).data('value');

        var formData = {
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
                    toastr.success('Rating submitted successfully!');
                    updateStars($(this).closest('.rating').data('rating', ratingValue));
                } else if (response.redirect) {
                    window.location.href = response.redirect;
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Error submitting rating: ' + error);
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

    var movieId = $('.rating').data('movie-id');

    var table = $('#commentsTable').DataTable({
        "processing": true,
        "serverSide": true,
        "searching": false, // Убрать строку поиска
        "paging": false, // Отключить стандартную пагинацию
        "info": false, // Убрать нижний раздел
        "ajax": {
            "url": Urls.Movie.LoadComments + `/${movieId}`,
            "type": "POST",
            "data": function (d) {
                d.movieId = movieId;
                d.length = 10; // Установить количество загружаемых записей
            }
        },

        "scrollY": "400px", 
        "scrollCollapse": true,
        "deferRender": true,
        "columns": [
            { "data": "userName" },
            { "data": "text" },
            { "data": "date", "render": function (data) { return moment(data).format('DD/MM/YYYY'); } }
        ]

    });

    // Функция для подгрузки комментариев при прокрутке вниз
    $('#commentsTable_wrapper .dataTables_scrollBody').on('scroll', function () {
        var scrollHeight = this.scrollHeight;
        var scrollTop = this.scrollTop;
        var height = $(this).height();
        if (scrollHeight - scrollTop <= height + 100) {
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
