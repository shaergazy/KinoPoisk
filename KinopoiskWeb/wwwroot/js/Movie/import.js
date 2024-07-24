$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $('#search-button-title').on('click', function () {
        var title = $('#title').val();
        var year = $('#year').val();
        var plot = $('#plot').val();

        var data = { title: title, year: year, plot: plot };

        $.ajax({
            url: '/Movies/Import?handler=Search',
            method: 'GET',
            data: data,
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                populateResults(response);
            }
        });
    });

    $('#reset-button-title').on('click', function () {
        $('#search-form-title')[0].reset();
        $('#search-results').empty();
    });

    $('#search-button-id').on('click', function () {
        var imdbId = $('#imdbId').val();
        var year = $('#year-id').val();
        var plot = $('#plot-id').val();

        var data = { imdbId: imdbId, year: year, plot: plot };

        $.ajax({
            url: '/Movies/Import?handler=SearchById',
            method: 'GET',
            data: data,
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                populateResults(response);
            }
        });
    });

    $('#reset-button-id').on('click', function () {
        $('#search-form-id')[0].reset();
        $('#search-results').empty();
    });

    function populateResults(response) {
        var resultsDiv = $('#search-results');
        resultsDiv.empty();

        if (response.success && response.movies.searchResults.length > 0) {
            var table = $('<table>').addClass('table');
            table.append('<thead><tr><th>IMDB Id</th><th>Title</th><th>Year</th><th>Poster</th><th>Action</th></tr></thead>');
            var tbody = $('<tbody>');

            response.movies.searchResults.forEach(function (movie) {
                var row = $('<tr>');
                row.append($('<td>').text(movie.imdbId));
                row.append($('<td>').text(movie.title));
                row.append($('<td>').text(movie.year));
                row.append($('<td>').html('<img src="' + movie.poster + '" alt="' + movie.title + '" class="img-thumbnail" width="100" />'));
                row.append($('<td>').append('<button class="btn btn-success import-button" data-imdbid="' + movie.imdbId + '">Import</button>'));
                tbody.append(row);
            });

            table.append(tbody);
            resultsDiv.append(table);

            $('.import-button').on('click', function () {
                var imdbId = $(this).data('imdbid');
                $.ajax({
                    url: '/Movies/Import?handler=Import',
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ imdbId: imdbId }),
                    headers: { 'RequestVerificationToken': token },
                    success: function (response) {
                        if (response.success) {
                            alert('Movie imported successfully!');
                        } else {
                            alert('Error importing movie: ' + response.error);
                        }
                    }
                });
            });
        } else {
            resultsDiv.append($('<p>').text('No movies found.'));
        }
    }
});
