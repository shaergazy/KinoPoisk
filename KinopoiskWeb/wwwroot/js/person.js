$(document).ready(function () {

    $('#PersonTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Person.GetAll,
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
        },
        "columns": [
            { "name": "Id", "data": "id", "visible": false },
            { "name": "FirstName", "data": "firstName" },
            { "name": "LastName", "data": "lastName" },
            { "name": "BirthDate", "data": "birthDate" },
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return `
                    <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editPersonModal" data-id="${row.id}">Edit</button>
                    <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deletePersonModal" data-id="${row.id}" data-name="${row.firstName}">Delete</button>
                `;
                },
                "sortable": false
            }
        ],
        "order": [[0, "desc"]]
    });
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

    $('#deletePersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#PersonToDelete_Id').val(id);
        modal.find('.modal-body #personNameToDelete').text(name);
    });
});