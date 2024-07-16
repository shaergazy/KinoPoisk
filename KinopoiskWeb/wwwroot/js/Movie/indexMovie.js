$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    if (!token) {
        console.error("CSRF token not found.");
        return;
    }

    $('#moviesTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: Urls.Movie.GetAll,
            method: 'POST',
            headers: { 'RequestVerificationToken': token }
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
