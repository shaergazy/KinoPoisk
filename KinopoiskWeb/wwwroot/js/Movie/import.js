$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    loadTranslations(currentCulture);

    $('#search-button-title').on('click', function () {
        var title = $('#title').val();
        var year = $('#year').val();
        var plot = $('#plot').val();

        var data = { title: title, year: year, plot: plot };

        $.ajax({
            url: Urls.Movie.Search,
            method: 'GET',
            data: data,
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                console.log(getTranslation('notification.search_success'), response);
                populateResults(response);
            },
            error: function (xhr, status, error) {
                console.error(getTranslation('error.search'), status, error);
                toastr.error(getTranslation('error.search') + ': ' + error);
            }
        });
    });

    $('#reset-button-title').on('click', function () {
        $('#search-form-title')[0].reset();
        $('#search-results').empty();
    });

    // Поиск по IMDB Id
    $('#search-button-id').on('click', function () {
        var imdbId = $('#imdbId').val();
        var year = $('#year-id').val();
        var plot = $('#plot-id').val();

        var data = { imdbId: imdbId, year: year, plot: plot };

        $.ajax({
            url: Urls.Movie.SearchById,
            method: 'GET',
            data: data,
            headers: { 'RequestVerificationToken': token },
            success: function (response) {
                console.log(getTranslation('notification.search_success_by_id'), response);
                populateResultsById(response);
            },
            error: function (xhr, status, error) {
                console.error(getTranslation('error.search_by_id'), status, error);
                toastr.error(getTranslation('error.search_by_id') + ': ' + error);
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

        if (response && response.success && response.movies && response.movies.searchResults && response.movies.searchResults.$values && response.movies.searchResults.$values.length > 0) {
            var table = $('<table>').addClass('table');
            table.append('<thead><tr><th>' + getTranslation('table.imdb_id') + '</th><th>' + getTranslation('table.title') + '</th><th>' + getTranslation('table.year') + '</th><th>' + getTranslation('table.poster') + '</th><th>' + getTranslation('table.action') + '</th></tr></thead>');
            var tbody = $('<tbody>');

            response.movies.searchResults.$values.forEach(function (movie) {
                var row = $('<tr>');
                row.append($('<td>').text(movie.imdbId));
                row.append($('<td>').text(movie.title));
                row.append($('<td>').text(movie.year));
                row.append($('<td>').html('<img src="' + movie.poster + '" alt="' + movie.title + '" class="img-thumbnail" width="100" />'));
                row.append($('<td>').append('<button class="btn btn-success import-button" data-imdbid="' + movie.imdbId + '">' + getTranslation('button.import') + '</button>'));
                tbody.append(row);
            });

            table.append(tbody);
            resultsDiv.append(table);

            $('.import-button').on('click', function () {
                var imdbId = $(this).data('imdbid');
                $.ajax({
                    url: Urls.Movie.Import,
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ ImdbId: imdbId }),
                    dataType: 'json',
                    headers: { 'RequestVerificationToken': token },
                    success: function (response) {
                        if (response.success) {
                            toastr.success(getTranslation('notification.import_success'));
                        } else {
                            toastr.error(getTranslation('error.import') + ': ' + response.error);
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error(getTranslation('error.import') + ': ' + error);
                    }
                });
            });
        } else {
            resultsDiv.append($('<p>').text(getTranslation('result.no_movies')));
            toastr.info(getTranslation('notification.no_movies'));
        }
    }

    function populateResultsById(response) {
        var resultsDiv = $('#search-results');
        resultsDiv.empty();

        if (response && response.success && response.movie) {
            var movie = response.movie;
            var details = `
            <div class="card mb-3" style="max-width: 540px;">
                <div class="row g-0">
                    <div class="col-md-4">
                        <img src="${movie.poster}" class="img-fluid rounded-start" alt="${movie.title}">
                    </div>
                    <div class="col-md-8">
                        <div class="card-body">
                            <h5 class="card-title">${movie.title}</h5>
                            <p class="card-text"><strong>${getTranslation('movie.year')}:</strong> ${movie.year}</p>
                            <p class="card-text"><strong>${getTranslation('movie.rated')}:</strong> ${movie.rated}</p>
                            <p class="card-text"><strong>${getTranslation('movie.released')}:</strong> ${movie.released}</p>
                            <p class="card-text"><strong>${getTranslation('movie.runtime')}:</strong> ${movie.runtime}</p>
                            <p class="card-text"><strong>${getTranslation('movie.genre')}:</strong> ${movie.genre}</p>
                            <p class="card-text"><strong>${getTranslation('movie.director')}:</strong> ${movie.director}</p>
                            <p class="card-text"><strong>${getTranslation('movie.writer')}:</strong> ${movie.writer}</p>
                            <p class="card-text"><strong>${getTranslation('movie.actors')}:</strong> ${movie.actors}</p>
                            <p class="card-text"><strong>${getTranslation('movie.plot')}:</strong> ${movie.plot}</p>
                            <p class="card-text"><strong>${getTranslation('movie.language')}:</strong> ${movie.language}</p>
                            <p class="card-text"><strong>${getTranslation('movie.country')}:</strong> ${movie.country}</p>
                            <p class="card-text"><strong>${getTranslation('movie.imdbRating')}:</strong> ${movie.imdbRating}</p>
                            <button class="btn btn-success import-button" data-imdbid="${movie.imdbId}">${getTranslation('button.import')}</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
            resultsDiv.append(details);

            $('.import-button').on('click', function () {
                var imdbId = $(this).data('imdbid');
                $.ajax({
                    url: Urls.Movie.Import,
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ ImdbId: imdbId }),
                    dataType: 'json',
                    headers: { 'RequestVerificationToken': token },
                    success: function (response) {
                        if (response.success) {
                            toastr.success(getTranslation('notification.import_success'));
                        } else {
                            toastr.error(getTranslation('error.import') + ': ' + response.error);
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error(getTranslation('error.import') + ': ' + error);
                    }
                });
            });
        } else {
            resultsDiv.append($('<p>').text(getTranslation('result.no_movie')));
            toastr.info(getTranslation('notification.no_movie'));
        }
    }
});
