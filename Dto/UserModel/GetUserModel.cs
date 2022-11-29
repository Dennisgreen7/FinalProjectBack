using System.Text.Json.Serialization;

namespace Dto.UserModel
{
    public class GetUserModel
    {
        public int UsersId { get; set; }
        public string UsersFirstName { get; set; } = null!;
        public string UsersLastName { get; set; } = null!;
        public string UsersUserName { get; set; } = null!;
        public string UsersEmail { get; set; } = null!;
        public string UsersRole { get; set; } = null!;
        public string? UsersPassword { get; set; } = null!;
        public string? ImageSrc { get; set; }
        public byte[]? UsersPasswordHash { get; set; }
        public byte[]? UsersPasswordSalt { get; set; }
        public string? UsersRefreshToken { get; set; }
        public DateTime? UsersTokenCreated { get; set; }
        public DateTime? UsersTokenExpires { get; set; }
    }
}
