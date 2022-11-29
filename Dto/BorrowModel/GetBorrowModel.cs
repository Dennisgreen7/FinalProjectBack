using Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dto.BorrowModel
{
    public class GetBorrowModel
    {
        public int BorrowingId { get; set; }
        public int BorrowingBookId { get; set; }
        public int BorrowingUserId { get; set; }
        public DateTime BorrowingDate { get; set; }
        public DateTime BorrowingReturnDate { get; set; }
        public DateTime? BorrowingReturnedDate { get; set; }
        public int DaysLeft { get=> (BorrowingReturnDate - BorrowingDate).Days;}
        [JsonIgnore]
        public Book BorrowingBook { get; set; } = null!;
        [JsonIgnore]
        public User BorrowingUser { get; set; } = null!;
        public string UserName { get => BorrowingUser.UsersFirstName + " " + BorrowingUser.UsersLastName; }
        public string BookName { get => BorrowingBook.BookName; }
    }
}
