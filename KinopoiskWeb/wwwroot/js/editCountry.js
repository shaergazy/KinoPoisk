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
});