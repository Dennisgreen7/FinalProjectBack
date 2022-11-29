using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public partial class User
    {
        public User()
        {
            Borrowings = new HashSet<Borrowing>();
        }
        public int UsersId { get; set; }
        public string UsersFirstName { get; set; } = null!;
        public string UsersLastName { get; set; } = null!;
        public string UsersUserName { get; set; } = null!;
        public string UsersEmail { get; set; } = null!;
        public string UsersRole { get; set; } = null!;
        public byte[]? UsersPasswordHash { get; set; }
        public byte[]? UsersPasswordSalt { get; set; }
        public string? UsersRefreshToken { get; set; }
        public DateTime? UsersTokenCreated { get; set; }
        public DateTime? UsersTokenExpires { get; set; }

        public virtual ICollection<Borrowing> Borrowings { get; set; }
    }
}
