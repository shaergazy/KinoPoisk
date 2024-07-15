$(document).ready(function () {
    $('#GenreTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Genre.GetAll,
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
        },
        "columns": [
            { "name": "Id", "data": "id", "visible": false },
            { "name": "Name", "data": "name" },
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return `
                        <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editGenreModal" data-id="${row.id}">Edit</button>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteGenreModal" data-id="${row.id}" data-name="${row.name}">Delete</button>
                    `;
                },
                "sortable": false
            }
        ],
        "order": [[0, "desc"]]
    });

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
    if (successMessage) {
        toastr.success(successMessage);
    }
    if (errorMessage) {
        toastr.error(errorMessage);
    }
});