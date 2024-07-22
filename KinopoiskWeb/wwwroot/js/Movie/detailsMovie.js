$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $('.rating span').on('click', function () {
        var ratingValue = $(this).data('value');
        var movieId = $(this).closest('.rating').attr('id').split('-')[1];

        $.ajax({
            url: '@Url.Page("/Movies/Details", new { id = Model.Movie.Id, handler = "Rate" })',
            type: 'POST',
            contentType: 'application/json',
            headers: {
                "RequestVerificationToken": token
            },
            delay: 250,
            data: function (params) {
                return {
                    starCount: params.starCount
                };
            },
            success: function (response) {
                alert('Rating submitted successfully!');
            },
            error: function (xhr, status, error) {
                alert('Error submitting rating: ' + error);
            }
        });
    });

    $('.rating span').hover(
        function () {
            $(this).prevAll().addBack().css('color', '#f5b301');
        },
        function () {
            $(this).prevAll().addBack().css('color', '');
        }
    );
});