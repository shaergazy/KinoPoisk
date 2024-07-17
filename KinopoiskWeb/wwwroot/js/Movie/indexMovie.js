$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (!token) {
        console.error("CSRF token not found.");
        return;
    }

    // Initialize Select2 for dynamic data loading
    function initializeSelect2(elementId, url, placeholder) {
        $('#' + elementId).select2({
            ajax: {
                url: url,
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
            placeholder: placeholder,
            allowClear: true
        });
    }

    // Initialize Select2 elements
    initializeSelect2('titleFilter', Urls.Movie.GetTitles, 'Select title');
    initializeSelect2('countryFilter', Urls.Country.GetCountries, 'Select country');
    initializeSelect2('actorFilter', Urls.Person.GetPeople, 'Select actor');
    initializeSelect2('directorFilter', Urls.Person.GetDirectors, 'Select director');

    // Initialize DataTable
    var table = $('#moviesTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Movie.GetAll,
            method: 'POST',
            headers: { 'RequestVerificationToken': token },
            data: function (d) {
                d.title = $('#titleFilter').val();
                d.releasedDate = $('#releasedDateFilter').val();
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
                    return `<a href="/Movies/Details/${row.id}" class="btn btn-primary">Details</a>`;
                }
            }
        ],
        "order": [[0, "desc"]]
    });

    // Reload table when filter changes
    $('.filter-input').on('change', function () {
        table.ajax.reload();
    });

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
