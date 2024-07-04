$(document).ready(function () {
    $('#editPersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var PersonId = button.data('id');

        $.ajax({
            url: Urls.Person.GetById + `&id=${PersonId}`,
            method: 'GET',
            success: function (data) {
                $('#editPersonId').val(data.id);
                $('#editPersonFirstName').val(data.firstName);
                $('#editPersonLastName').val(data.lastName);
                var birthDate = new Date(data.birthDate);
                var formattedDate = ('0' + (birthDate.getMonth() + 1)).slice(-2) + '/' +
                    ('0' + birthDate.getDate()).slice(-2) + '/' +
                    birthDate.getFullYear();
                $('#editPersonBirthDate').val(formattedDate);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });
});
