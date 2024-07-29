$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();

    // Поиск по заголовку
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
                console.log("Response from Search by Title:", response);
                populateResults(response);
            },
            error: function (xhr, status, error) {
                console.error("Error in Search by Title request:", status, error);
                toastr.error('Error in Search by Title request: ' + error);
            }
        });
    });

    // Сброс формы поиска по заголовку
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
                console.log("Response from Search by ID:", response);
                populateResultsById(response);
            },
            error: function (xhr, status, error) {
                console.error("Error in Search by ID request:", status, error);
                toastr.error('Error in Search by ID request: ' + error);
            }
        });
    });

    // Сброс формы поиска по IMDB Id
    $('#reset-button-id').on('click', function () {
        $('#search-form-id')[0].reset();
        $('#search-results').empty();
    });

    // Функция для заполнения результатов поиска
    function populateResults(response) {
        var resultsDiv = $('#search-results');
        resultsDiv.empty();

        if (response && response.success && response.movies && response.movies.searchResults && response.movies.searchResults.length > 0) {
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

            // Обработчик для кнопки "Import"
            $('.import-button').on('click', function () {
                var imdbId = $(this).data('imdbid'); // Получаем значение атрибута data-imdbid
                $.ajax({
                    url: Urls.Movie.Import,
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ ImdbId: imdbId }), // Оборачиваем imdbId в JSON
                    dataType: 'json',
                    headers: { 'RequestVerificationToken': token },
                    success: function (response) {
                        if (response.success) {
                            toastr.success('Movie imported successfully!');
                        } else {
                            toastr.error('Error importing movie: ' + response.error);
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error('Error importing movie: ' + error);
                    }
                });
            });
        } else {
            resultsDiv.append($('<p>').text('No movies found.'));
            toastr.info('No movies found.');
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
                            <p class="card-text"><strong>Year:</strong> ${movie.year}</p>
                            <p class="card-text"><strong>Rated:</strong> ${movie.rated}</p>
                            <p class="card-text"><strong>Released:</strong> ${movie.released}</p>
                            <p class="card-text"><strong>Runtime:</strong> ${movie.runtime}</p>
                            <p class="card-text"><strong>Genre:</strong> ${movie.genre}</p>
                            <p class="card-text"><strong>Director:</strong> ${movie.director}</p>
                            <p class="card-text"><strong>Writer:</strong> ${movie.writer}</p>
                            <p class="card-text"><strong>Actors:</strong> ${movie.actors}</p>
                            <p class="card-text"><strong>Plot:</strong> ${movie.plot}</p>
                            <p class="card-text"><strong>Language:</strong> ${movie.language}</p>
                            <p class="card-text"><strong>Country:</strong> ${movie.country}</p>
                            <p class="card-text"><strong>IMDB Rating:</strong> ${movie.imdbRating}</p>
                            <button class="btn btn-success import-button" data-imdbid="${movie.imdbId}">Import</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
            resultsDiv.append(details);

            // Обработчик для кнопки "Import"
            $('.import-button').on('click', function () {
                var imdbId = $(this).data('imdbid'); // Получаем значение атрибута data-imdbid
                $.ajax({
                    url: Urls.Movie.Import,
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ ImdbId: imdbId }), // Оборачиваем imdbId в JSON
                    dataType: 'json',
                    headers: { 'RequestVerificationToken': token },
                    success: function (response) {
                        if (response.success) {
                            toastr.success('Movie imported successfully!');
                        } else {
                            toastr.error('Error importing movie: ' + response.error);
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error('Error importing movie: ' + error);
                    }
                });
            });
        } else {
            resultsDiv.append($('<p>').text('No movie found.'));
            toastr.info('No movie found.');
        }
    }
});
