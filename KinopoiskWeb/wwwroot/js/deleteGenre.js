$(document).ready(function () {
    $('#deleteGenreModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#GenreToDelete_Id').val(id);
        modal.find('.modal-body #genreNameToDelete').text(name);
    });
});