const DATE_FORMAT = 'DD/MM/YYYY';

$(document).ready(function () {
    loadTranslations(currentCulture);
    $('#PersonTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Person.GetAll,
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
                "name": "FirstName",
                "data": function (row) {
                    const englishFirstName = row.translations.$values.find(t => t.languageCode === 0 && t.fieldType === 3);
                    const russianFirstName = row.translations.$values.find(t => t.languageCode === 1 && t.fieldType === 3);

                    return `
                        <div>
                            <strong>EN:</strong> ${englishFirstName ? englishFirstName.value : 'N/A'}
                            <br/>
                            <strong>RU:</strong> ${russianFirstName ? russianFirstName.value : 'N/A'}
                        </div>`;
                }
            },
            {
                "name": "LastName",
                "data": function (row) {
                    const englishLastName = row.translations.$values.find(t => t.languageCode === 0 && t.fieldType === 4);
                    const russianLastName = row.translations.$values.find(t => t.languageCode === 1 && t.fieldType === 4);

                    return `
                        <div>
                            <strong>EN:</strong> ${englishLastName ? englishLastName.value : 'N/A'}
                            <br/>
                            <strong>RU:</strong> ${russianLastName ? russianLastName.value : 'N/A'}
                        </div>`;
                }
            },
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
                        <button class="btn btn-secondary" data-bs-toggle="modal" data-bs-target="#editPersonModal" data-id="${row.id}">${getTranslation('dataTable.edit')}</button>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deletePersonModal" data-id="${row.id}" data-name="${row.translations.$values[0].value}">${getTranslation('dataTable.delete')}</button>
                    `;
                },
                "sortable": false
            }
        ],
        "order": [[0, "desc"]]
    });

    // Открытие модального окна для редактирования
    $('#editPersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var personId = button.data('id');

        $.ajax({
            url: Urls.Person.GetById + `&id=${personId}`,
            method: 'GET',
            success: function (data) {
                $('#editPersonId').val(data.id ? data.id : 4);

                const englishFirstName = data.translations.$values.find(t => t.languageCode === 0 && t.fieldType === 3);
                const russianFirstName = data.translations.$values.find(t => t.languageCode === 1 && t.fieldType === 3);

                const englishLastName = data.translations.$values.find(t => t.languageCode === 0 && t.fieldType === 4);
                const russianLastName = data.translations.$values.find(t => t.languageCode === 1 && t.fieldType === 4);

                $('#editPersonFirstName').val(englishFirstName ? englishFirstName.value : '');
                $('#editPersonFirstNameRu').val(russianFirstName ? russianFirstName.value : '');

                $('#enFirstNameId').val(englishFirstName ? englishFirstName.id : '');
                $('#ruFirstNameId').val(russianFirstName ? russianFirstName.id : '');

                $('#editPersonLastName').val(englishLastName ? englishLastName.value : '');
                $('#editPersonLastNameRu').val(russianLastName ? russianLastName.value : '');

                $('#enLastNameId').val(englishLastName ? englishLastName.id : '');
                $('#ruLastNameId').val(russianLastName ? russianLastName.id : '');

                var birthDate = moment(data.birthDate);
                $('#editPersonBirthDate').val(birthDate.format(DATE_FORMAT));
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });

    // Открытие модального окна для удаления
    $('#deletePersonModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#PersonToDelete_Id').val(id);
        modal.find('.modal-body #personNameToDelete').text(name);
    });

    if (successMessage) {
        toastr.success(getTranslation('notification.success'));
    }
    if (errorMessage) {
        toastr.error(getTranslation('notification.error'));
    }
});
