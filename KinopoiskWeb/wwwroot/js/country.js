$(document).ready(function () {
    loadTranslations(currentCulture);
    $('#CountryTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Country.GetAll,
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
            { "name": "ShortName", "data": "shortName" },
            {
                "name": "FlagLink",
                "data": "flagLink",
                "render": function (data, type, row, meta) {
                    return `<img src="${data}" alt="${getTranslation('table.flag')}" style="width: 50px; height: auto;" />`;
                }
            },
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return `
                        <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editCountryModal" data-id="${row.id}">${getTranslation('dataTable.edit')}</button>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteCountryModal" data-id="${row.id}" data-name="${row.translations.$values[0].value}">${getTranslation('dataTable.delete')}</button>
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
                $('#editCountryShortName').val(data.shortName);
                $('#editCountryFlagLink').val(data.flagLink);

                const englishTranslation = data.translations.$values.find(t => t.languageCode === 0);
                const russianTranslation = data.translations.$values.find(t => t.languageCode === 1);

                $('#editCountryEnglishName').val(englishTranslation ? englishTranslation.value : '');
                $('#editCountryRussianName').val(russianTranslation ? russianTranslation.value : '');
            },
            error: function (error) {
                console.error(getTranslation('error.search'), error);
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
        modal.find('.modal-footer .btn-danger').text(getTranslation('button.delete'));
    });

    if (successMessage) {
        toastr.success(getTranslation('notification.success'));
    }
    if (errorMessage) {
        toastr.error(getTranslation('notification.error'));
    }
});
