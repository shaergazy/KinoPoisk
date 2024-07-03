$(document).ready(function () {
    $('#deleteCountryModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var id = button.data('id');
        var name = button.data('name');
        var modal = $(this);
        modal.find('.modal-body input#CountryToDelete_Id').val(id);
        modal.find('.modal-body #countryNameToDelete').text(name);
    });
});