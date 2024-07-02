$(document).ready(function () {
    $('#editPersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var firstName = button.data('firstname');
        var lastName = button.data('lastname');
        var birthDate = button.data('birthdate');
        var modal = $(this);
        modal.find('.modal-body input#EditedPerson_Id').val(id);
        modal.find('.modal-body input#EditedPerson_FirstName').val(firstName);
        modal.find('.modal-body input#EditedPerson_LastName').val(lastName);
        modal.find('.modal-body input#EditedPerson_BirthDate').val(birthDate.split('T')[0]);
    });
});