$(document).ready(function () {
    $('#moviesTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Movie.GetAll,
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() }
        },
        "columns": [
            { "name": "Id", "data": "id", "visible": false },
            { "name": "Title", "data": "title" },
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
});