$(document).ready(function () {
    loadTranslations(currentCulture).done(function () {

        function getLanguageCode(culture) {
            var cultureToLanguageCode = {
                'en': 0,    
                'ru': 1,              
            };

            return cultureToLanguageCode[culture] || 0;                      
        }

        function getTranslatedValue(translations, currentCulture) {
            var languageCode = getLanguageCode(currentCulture);                
            var translation = translations.find(function (t) {
                return t.languageCode === languageCode;
            });
            return translation ? translation.value : null;
        }

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
                            var translatedName = getTranslatedValue(item.translations.$values, currentCulture);       
                            return {
                                id: item.id,
                                text: translatedName || item.shortName,                      
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
            placeholder: getTranslation('select.country')
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
                            var translatedName = getTranslatedValue(item.translations.$values, currentCulture);       
                            return {
                                id: item.id,
                                text: translatedName || item.name                      
                            };
                        })
                    };
                }
            },
            placeholder: getTranslation('select.genre'),
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
                            results: $.map(data.$values, function (item) {
                                var firstNameTranslation = item.translations.$values.find(t => t.fieldType === 3 && t.languageCode === getLanguageCode(currentCulture));
                                var lastNameTranslation = item.translations.$values.find(t => t.fieldType === 4 && t.languageCode === getLanguageCode(currentCulture));

                                var firstName = firstNameTranslation ? firstNameTranslation.value : '';
                                var lastName = lastNameTranslation ? lastNameTranslation.value : '';

                                return {
                                    id: item.id,
                                    text: firstName + " " + lastName
                                };
                            })
                        };
                    }
                },
                dropdownParent: $('#addActorModal')
            });
        });

        $('#saveActorButton').text(getTranslation('button.save_actor')).click(function () {
            var personId = $('#actorSelect').val();
            var personName = $('#actorSelect option:selected').text();
            var order = $('#actorOrder').val();

            var isActorAlreadyAdded = $('#actorsList .card input[name$="PersonId"]').filter(function () {
                return $(this).val() === personId;
            }).length > 0;

            if (isActorAlreadyAdded) {
                $('#actorSelectError').text(getTranslation('error.actor_already_added'));
                return;
            }

            if (personId && order) {
                var index = $('#actorsList .card').length;

                var actorCard = `<div class="card mt-2" data-index="${index}" style="flex: 1 1 30%;">
                                     <div class="card-body position-relative">
                                         <input name="Movie.Actors[${index}].PersonId" type="hidden" value="${personId}" />
                                         <input name="Movie.Actors[${index}].Order" type="hidden" value="${order}" />
                                         <h5 class="card-title">${personName}</h5>
                                         <p class="card-text">${getTranslation('order')}: ${order}</p> <!-- Локализованный текст "Order" -->
                                         <button type="button" class="btn btn-sm btn-link position-absolute top-0 end-0 delete-actor">X</button>
                                     </div>
                                  </div>`;
                $('#actorsList').append(actorCard);
                $('#addActorModal').modal('hide');
            } else {
                if (!personId) {
                    $('#actorSelectError').text(getTranslation('error.select_actor'));
                }
                if (!order) {
                    $('#actorOrderError').text(getTranslation('error.select_order'));
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
                        results: $.map(data.$values, function (item) {
                            var firstNameTranslation = item.translations.$values.find(t => t.fieldType === 3 && t.languageCode === getLanguageCode(currentCulture));
                            var lastNameTranslation = item.translations.$values.find(t => t.fieldType === 4 && t.languageCode === getLanguageCode(currentCulture));

                            var firstName = firstNameTranslation ? firstNameTranslation.value : '';
                            var lastName = lastNameTranslation ? lastNameTranslation.value : '';

                            return {
                                id: item.id,
                                text: firstName + " " + lastName
                            };
                        })
                    };
                }
            },
            minimumInputLength: 3,
            placeholder: getTranslation('select.director')
        });

        $('#countrySelect').on('select2:select', function (e) {
            var data = e.params.data;
            var $selected = $(
                '<span>' + data.text + '</span>'
            );
            $('#select2-countrySelect-container').html($selected);
        });

        if (successMessage) {
            toastr.success(getTranslation(successMessage));
        }
        if (errorMessage) {
            toastr.error(getTranslation(errorMessage));
        }
    });
});
