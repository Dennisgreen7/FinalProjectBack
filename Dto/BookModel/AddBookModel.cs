namespace Dto.BookModel
{
    public class AddBookModel
    {
        public string BookName { get; set; } = null!;
        public int AuthorId { get; set; }
        public int BookPublishedYear { get; set; }
        public int GenreId { get; set; }
        public string BookLanguage { get; set; } = null!;
        public int BookNumOfPages { get; set; }
        public int BookCopys { get; set; }
        public string? BookAbout { get; set; }
    }
}
