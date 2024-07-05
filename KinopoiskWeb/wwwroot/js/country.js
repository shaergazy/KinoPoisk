$(document).ready(function () {

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