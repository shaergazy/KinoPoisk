const DATE_FORMAT = 'DD/MM/YYYY';

$(document).ready(function () {
    // Initialize DataTable with server-side processing
    $('#PersonTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Person.GetAll,
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
        },
        "language": {
            "url": `/js/locals/datatables/${currentCulture}.json` // Load DataTables localization file based on the current culture
        },
        "columns": [
            { "name": "Id", "data": "id", "visible": false },
            { "name": "FirstName", "data": "firstName" },
            { "name": "LastName", "data": "lastName" },
            {
                "name": "BirthDate", "data": "birthDate",
                "render": function (data, type, row) {
                    return moment(data).format(DATE_FORMAT);
                }
            },
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return `
                    <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editPersonModal" data-id="${row.id}">${getTranslation('button.edit')}</button>
                    <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deletePersonModal" data-id="${row.id}" data-name="${row.firstName}">${getTranslation('button.delete')}</button>
                    `;
                },
                "sortable": false
            }
        ],
        "order": [[0, "desc"]]
    });

    // Handle the opening of the Edit Person modal
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
                var birthDate = moment(data.birthDate);
                var formattedDate = birthDate.format(DATE_FORMAT);
                $('#editPersonBirthDate').val(formattedDate);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    // Handle the opening of the Delete Person modal
    $('#deletePersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#PersonToDelete_Id').val(id);
        modal.find('.modal-body #personNameToDelete').text(name);
    });
});
