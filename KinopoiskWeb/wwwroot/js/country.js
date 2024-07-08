$(document).ready(function () {
    $('#CountryTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Country.GetAll,
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
        },
        "columns": [
            { "name": "Id", "data": "id", "visible": false },
            { "name": "Name", "data": "name" },
            { "name": "ShortName", "data": "shortName" },
            {
                "name": "FlagLink",
                "data": "flagLink",
                "render": function (data, type, row, meta) {
                    return `<img src="${data}" alt="Flag" style="width: 50px; height: auto;" />`;
                }
            },
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return `
                        <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editCountryModal" data-id="${row.id}">Edit</button>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteCountryModal" data-id="${row.id}" data-name="${row.name}">Delete</button>
                    `;
                },
                "sortable": false
            }
        ],
        "order": [[0, "desc"]]
    });
    $('#editCountryModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var countryId = button.data('id');

        $.ajax({
            url: Urls.Country.GetById + `&id=${countryId}`,
            method: 'GET',
            success: function (data) {
                $('#editCountryId').val(data.id);
                $('#editCountryName').val(data.name);
                $('#editCountryShortName').val(data.shortName);
                $('#editCountryFlagLink').val(data.flagLink);
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    $('#deleteCountryModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#CountryToDelete_Id').val(id);
        modal.find('.modal-body #countryNameToDelete').text(name);
    });
});