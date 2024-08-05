$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (!token) {
        console.error("CSRF token not found.");
        return;
    }

    $('#countryFilter').select2({
        ajax: {
            url: Urls.Country.GetCountries,
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
            },
            minimumInputLength: 3
        },
        placeholder: 'Select country',
        allowClear: true
    });

    $('#actorFilter').select2({
        ajax: {
            url: Urls.Person.Get,
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
                            text: item.firstName + " " + item.lastName
                        };
                    })
                };
            },
            minimumInputLength: 3
        },
        placeholder: 'Select actor',
        allowClear: true
    });

    $('#directorFilter').select2({
        ajax: {
            url: Urls.Person.GetDirectors,
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
                            text: item.firstName + " " + item.lastName
                        };
                    })
                };
            },
            minimumInputLength: 3
        },
        placeholder: 'Select director',
        allowClear: true
    });

    // Initialize DataTable
    var table = $('#moviesTable').DataTable({
        "processing": true,
        "serverSide": true,
        "dom": 'Blrtip',
        "ajax": {
            url: Urls.Movie.GetAll,
            method: 'POST',
            headers: { 'RequestVerificationToken': token },
            data: function (d) {
                d.title = $('#titleFilter').val();
                d.year = $('#releasedDateFilter').val();
                d.country = $('#countryFilter').val();
                d.actor = $('#actorFilter').val();
                d.director = $('#directorFilter').val();
            }
        },
        "columns": [
            { "name": 'Id', "data": 'id', "visible": false },
            {
                "name": 'Poster',
                "data": 'poster',
                "render": function (data, type, row, meta) {
                    return data ? `<img src="${data}" alt="Poster" width="50" height="75" />` : '';
                }
            },
            { "name": 'Title', "data": 'title' },
            {
                "name": 'Description',
                "data": 'description',
                "render": function (data, type, row, meta) {
                    var maxLength = 100;
                    if (data.length > maxLength) {
                        var shortText = data.substring(0, maxLength);
                        var fullText = data;
                        return `
                            <span class="short-text">${shortText}...</span>
                            <span class="full-text" style="display: none;">${fullText}</span>
                            <a href="#" class="more-link">more</a>
                        `;
                    } else {
                        return data;
                    }
                }
            },
            { "name": 'ReleasedDate', "data": 'releasedDate', "render": function (data) { return data ? new Date(data).toLocaleDateString() : ''; } },
            { "name": 'Duration', "data": 'duration' },
            { "name": 'IMDBRating', "data": 'imdbRating' },
            {
                "name": 'Actions',
                "data": null,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var actions = `<a href="/Movies/Details/${row.id}" class="btn btn-primary">Details</a>`;
                    if (isAdmin) {
                        actions += ` <a href="/Movies/Update/${row.id}" class="btn btn-secondary">Edit</a>`;
                        actions += ` <button class="btn btn-danger delete-movie" data-id="${row.id}">Delete</button>`;
                    }
                    return actions;
                }
            }
        ],
        "order": [[0, "desc"]],
        "buttons": [
            {
                text: 'Export as PDF',
                action: function () {
                    generateReport('pdf');
                }
            },
            {
                text: 'Export as Excel',
                action: function () {
                    generateReport('excel');
                }
            }
        ],
        "layout": {
            topStart: 'buttons'
        }
    });

    $('#moviesTable').on('click', '.delete-movie', function (e) {
        e.preventDefault();
        var movieId = $(this).data('id');
        var movieName = $(this).closest('tr').find('td').eq(1).text();

        $('#MovieToDelete_Id').val(movieId);
        $('#movieNameToDelete').text(movieName);

        $('#deleteMovieModal').modal('show');
    });


    function generateReport(format) {
        $.ajax({
            url: format === 'pdf' ? Urls.Movie.GetPDF : Urls.Movie.GetExcel,
            type: 'POST',
            headers: { 'RequestVerificationToken': token },
            data: table.ajax.params(),
            xhrFields: {
                responseType: 'blob'
            },
            success: function (data) {
                var blob = new Blob([data], { type: format === 'pdf' ? 'application/pdf' : 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = format === 'pdf' ? `MoviesReport_${Date.now()}.pdf` : `MoviesReport_${Date.now()}.xlsx`;
                link.click();
            }
        });
    }

    // Function to reload table with debounce
    function debounce(func, delay) {
        let debounceTimer;
        return function () {
            const context = this;
            const args = arguments;
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => func.apply(context, args), delay);
        };
    }

    // Reload table when filter changes with debounce
    $('.filter-input').on('input', debounce(function () {
        var titleLength = $('#titleFilter').val().length;
        if (titleLength >= 3 || titleLength === 0) {
            table.ajax.reload();
        }
    }, 500));

    // Show more/less description
    $('#moviesTable').on('click', '.more-link', function (e) {
        e.preventDefault();
        var $link = $(this);
        var $shortText = $link.siblings('.short-text');
        var $fullText = $link.siblings('.full-text');

        if ($fullText.is(':visible')) {
            $shortText.show();
            $fullText.hide();
            $link.text('more');
        } else {
            $shortText.hide();
            $fullText.show();
            $link.text('less');
        }
    });
});
