$(document).ready(function () {
    $('#editGenreModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var genreId = button.data('id');

        $.ajax({
            url: Urls.Genre.GetById + `&id=${genreId}`,
            method: 'GET',
            success: function (data) {
                $('#editGenreId').val(data.id);
                $('#editGenreName').val(data.name);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });
});