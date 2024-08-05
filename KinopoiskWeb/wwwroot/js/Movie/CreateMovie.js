$(document).ready(function () {

    $('#countrySelect').select2({
        ajax: {
            url: Urls.Country.GetCountries,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                if (!params.term || params.term.length < 3) {
                    return null; 
                } else {
                    return {
                        searchTerm: params.term 
                    };
                }
            },
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
        allowClear: true,
        templateResult: formatCountry,
        templateSelection: formatCountrySelection,
        minimumInputLength: 3, 
    });

    $('#genreSelect').select2({
        ajax: {
            url: Urls.Genre.GetGenres,
            dataType: 'json',
            delay: 250, 
            data: function (params) {
                return {
                    searchTerm: params.term 
                };
            },
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
        },
        placeholder: 'Select genre',
        allowClear: true
    });


    $('#addActorModal').on('shown.bs.modal', function () {
        $('#actorSelect').select2({
            ajax: {
                url: Urls.Person.GetActors,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    if (!params.term || params.term.length < 3) {
                        return null;
                    } else {
                        return {
                            searchTerm: params.term
                        };
                    }
                },
                processResults: function (data) {
                    return {
                        results: $.map(data, function (item) {
                            return {
                                id: item.id,
                                text: item.firstName + " " + item.lastName
                            };
                        })
                    };
                },
                minimumInputLength: 3, 
            },
            dropdownParent: $('#addActorModal') 
        });
    });

    $('#saveActorButton').click(function () {
        var personId = $('#actorSelect').val();
        var personName = $('#actorSelect option:selected').text();
        var order = $('#actorOrder').val();


        var isActorAlreadyAdded = $('#actorsList .card input[name$="PersonId"]').filter(function () {
            return $(this).val() === personId;
        }).length > 0;

        if (isActorAlreadyAdded) {
            $('#actorSelectError').text('Actor already added.');
            return;
        }

        if (personId && order) {
            var index = $('#actorsList .card').length;

            var actorCard = `<div class="card mt-2" data-index="${index}" style="flex: 1 1 30%;">
                                 <div class="card-body position-relative">
                                     <input name="Movie.Actors[${index}].PersonId" type="hidden" value="${personId}" />
                                     <input name="Movie.Actors[${index}].Order" type="hidden" value="${order}" />
                                     <h5 class="card-title">${personName}</h5>
                                     <p class="card-text">Order: ${order}</p>
                                     <button type="button" class="btn btn-sm btn-link position-absolute top-0 end-0 delete-actor">X</button>
                                 </div>
                              </div>`;
            $('#actorsList').append(actorCard);
            $('#addActorModal').modal('hide');
        } else {
            if (!personId) {
                $('#actorSelectError').text('Select actor');
            }
            if (!order) {
                $('#actorOrderError').text('Select order');
            }
        }
    });

    $(document).on('click', '.delete-actor', function () {
        var card = $(this).closest('.card');
        card.remove();


        $('#actorsList .card').each(function (index) {
            $(this).find('input[name^="Movie.Actors"]').each(function () {
                var name = $(this).attr('name');
                var newName = name.replace(/\[.*?\]/, `[${index}]`);
                $(this).attr('name', newName);
            });
        });
    });


    $('#actorsList .card').each(function (index) {
        $(this).find('input[name^="Actors"]').each(function () {
            var name = $(this).attr('name');
            var newName = name.replace(/\[.*?\]/, `[${index}]`);
            $(this).attr('name', newName);
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
            url: Urls.Person.GetDirectors,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                if (!params.term || params.term.length < 3) {
                    return null;
                } else {
                    return {
                        searchTerm: params.term
                    };
                }
            },
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
        minimumInputLength: 3, 
    });

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