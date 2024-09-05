$(document).ready(function () {
    loadTranslations(currentCulture);
    $('#GenreTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Genre.GetAll,
            type: 'POST',
            headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
            dataSrc: function (json) {
                return json.data.$values;
            }
        },
        "language": {
            "url": `/js/locals/datatables/${currentCulture}.json`
        },
        "columns": [
            { "name": "Id", "data": "id", "visible": false },
            {
                "name": "Name",
                "data": function (row) {
                    // Ищем переводы для английского и русского языков
                    const englishTranslation = row.translations.$values.find(t => t.languageCode === 0);
                    const russianTranslation = row.translations.$values.find(t => t.languageCode === 1);

                    return `
                        <div>
                            <strong>EN:</strong> ${englishTranslation ? englishTranslation.value : 'N/A'}
                            <br/>
                            <strong>RU:</strong> ${russianTranslation ? russianTranslation.value : 'N/A'}
                        </div>`;
                }
            },
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return `
                        <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editGenreModal" data-id="${row.id}">${getTranslation('dataTable.edit')}</button>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteGenreModal" data-id="${row.id}" data-name="${row.translations.$values[0].value}">${getTranslation('dataTable.delete')}</button>
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

                const englishTranslation = data.translations.$values.find(t => t.languageCode === 0);
                const russianTranslation = data.translations.$values.find(t => t.languageCode === 1);

                $('#editGenreEnglishName').val(englishTranslation ? englishTranslation.value : '');
                $('#editGenreRussianName').val(russianTranslation ? russianTranslation.value : '');
            },
            error: function (error) {
                console.error(getTranslation('error.loading'), error);
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
        toastr.success(getTranslation('notification.success'));
    }
    if (errorMessage) {
        toastr.error(getTranslation('notification.error'));
    }
});
