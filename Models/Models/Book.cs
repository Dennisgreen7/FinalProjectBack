using System;
using System.Collections.Generic;

namespace Models.Models
{
    public partial class Book
    {
        public Book()
        {
            Borrowings = new HashSet<Borrowing>();
        }

        public int BookId { get; set; }
        public string BookName { get; set; } = null!;
        public int AuthorId { get; set; }
        public int BookPublishedYear { get; set; }
        public int GenreId { get; set; }
        public string BookLanguage { get; set; } = null!;
        public int BookNumOfPages { get; set; }
        public int BookCopys { get; set; }
        public string? BookAbout { get; set; }
        public virtual Author Author { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual ICollection<Borrowing> Borrowings { get; set; }
    }
}
