namespace Models.Models
{
    public partial class Borrowing
    {
        public int BorrowingId { get; set; }
        public int BorrowingBookId { get; set; }
        public int BorrowingUserId { get; set; }
        public DateTime BorrowingDate { get; set; }
        public DateTime BorrowingReturnDate { get; set; }
        public DateTime? BorrowingReturnedDate { get; set; }

        public virtual Book BorrowingBook { get; set; } = null!;
        public virtual User BorrowingUser { get; set; } = null!;
    }
}
