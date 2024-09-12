using DAL.Enums;
using DAL.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IContainer = QuestPDF.Infrastructure.IContainer;
namespace BLL
{
    public class ListMoviePdfDocument : IDocument
    {
        public readonly List<Movie> _movies;
        private readonly string _language = "en";
        public ListMoviePdfDocument(List<Movie> movies, string language)
        {
            _movies = movies;
            _language = language;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Movies Report")
                    .SemiBold().FontSize(24).FontColor(Colors.BlueGrey.Darken4);

                page.Content()
                    .Column(x =>
                    {
                        x.Spacing(10);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Title
                                columns.RelativeColumn(4); // Description
                                columns.RelativeColumn(1.8f); // Released Date
                                columns.RelativeColumn(1.5f); // Duration
                                columns.RelativeColumn(1); // IMDB Rating
                                columns.RelativeColumn(1.2f); // Own Rating
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Title").SemiBold();
                                header.Cell().Text("Description").SemiBold();
                                header.Cell().Text("Released Date").SemiBold();
                                header.Cell().Text("Duration").SemiBold();
                                header.Cell().Text("IMDB Rating").SemiBold();
                                header.Cell().Text("Rating").SemiBold();
                            });

                            foreach (var movie in _movies)
                            {
                                table.Cell().Element(CellStyle).Text(movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Title && x.LanguageCode.ToString() == _language).Value);
                                table.Cell().Element(CellStyle).Text(movie.Translations.FirstOrDefault(x => x.FieldType == TranslatableFieldType.Description && x.LanguageCode.ToString() == _language).Value);
                                table.Cell().Element(CellStyle).Text(movie.ReleasedDate.ToString("dd/MM/yy"));
                                table.Cell().Element(CellStyle).Text($"{movie.Duration} min");
                                table.Cell().Element(CellStyle).Text(movie.IMDBRating.ToString());
                                table.Cell().Element(CellStyle).Text(movie.Rating.ToString());
                            }
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        }

        private IContainer CellStyle(IContainer container)
        {
            return container.Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(5)
            .PaddingHorizontal(10);
        }
    }
}