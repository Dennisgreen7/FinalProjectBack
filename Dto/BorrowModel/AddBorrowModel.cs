namespace Dto.BorrowModel
{
    public class AddBorrowModel
    {
        public int BorrowingBookId { get; set; }
        public int BorrowingUserId { get; set; }
        public DateTime BorrowingReturnDate { get; set; }
        public DateTime? BorrowingReturnedDate { get; set; }
    }
}
