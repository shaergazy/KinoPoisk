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

    $('#deleteGenreModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#GenreToDelete_Id').val(id);
        modal.find('.modal-body #genreNameToDelete').text(name);
    });
});