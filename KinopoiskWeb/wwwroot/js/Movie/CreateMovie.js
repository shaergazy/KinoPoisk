$(document).ready(function () {

    $('#countrySelect').select2({
        ajax: {
            url: Urls.Movie.GetCountries,
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.id,
                            text: item.name,
                            flag: item.flagLink 
                        };
                    })
                };
            }
        },
        templateResult: formatCountry,
        templateSelection: formatCountrySelection
    });

    $('#genreSelect').select2({
        ajax: {
            url: Urls.Movie.GetGenres,
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.id,
                            text: item.name
                        };
                    })
                };
            }
        }
    });

    $('#addActorModal').on('shown.bs.modal', function () {
        $('#actorSelect').select2({
            ajax: {
                url: Urls.Movie.GetPeople,  // Замените на ваш URL для получения данных
                dataType: 'json',
                processResults: function (data) {
                    return {
                        results: $.map(data, function (item) {
                            return {
                                id: item.id,
                                text: item.firstName + ' ' + item.lastName
                            };
                        })
                    };
                }
            },
            dropdownParent: $('#addActorModal') 
        });
    });

    function formatCountry(country) {
        if (!country.id) {
            return country.text;
        }
        var $country = $(
            '<span><img src="' + country.flag + '" class="img-flag" style="width:20px;height:15px;margin-right:10px;"/>' + country.text + '</span>'
        );
        return $country;
    }

    function formatCountrySelection(country) {
        if (!country.id) {
            return country.text;
        }
        return country.text;
    }

    $('#directorSelect').select2({
        ajax: {
            url: Urls.Movie.GetPeople,
            dataType: 'json',
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            id: item.id,
                            text: item.firstName + ' ' + item.lastName
                        };
                    })
                };
            }
        }
    });



    // Show only name after selected
    $('#countrySelect').on('select2:select', function (e) {
        var data = e.params.data;
        var $selected = $(
            '<span>' + data.text + '</span>'
        );
        $('#select2-countrySelect-container').html($selected);
    });

    // Toastr
    if (successMessage) {
        toastr.success(successMessage);
    }
    if (errorMessage) {
        toastr.error(errorMessage);
    }
});