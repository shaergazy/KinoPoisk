$(document).ready(function () {
    $('#deletePersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#PersonToDelete_Id').val(id);
        modal.find('.modal-body #personNameToDelete').text(name);
    });
});