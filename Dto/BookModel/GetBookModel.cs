using Models.Models;
using System.Text.Json.Serialization;
namespace Dto.BookModel
{
    public class GetBookModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = null!;
        public int AuthorId { get; set; }
        public int BookPublishedYear { get; set; }
        public int GenreId { get; set; }
        public string BookLanguage { get; set; } = null!;
        public int BookNumOfPages { get; set; }
        public int BookCopys { get; set; }
        public string? ImageSrc { get; set; }
        public string? BookAbout { get; set; }
        [JsonIgnore]
        public Author Author { get; set; } = null!;
        [JsonIgnore]
        public Genre Genre { get; set; } = null!;
        public string AuthorName { get => Author.AuthorName; }
        public string GenreName { get => Genre.GenreName; }

    }
}
