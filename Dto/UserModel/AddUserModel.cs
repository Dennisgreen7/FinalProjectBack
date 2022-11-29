using System.Text.Json.Serialization;

namespace Dto.UserModel
{
    public class AddUserModel
    {
        public int? UsersId { get; set; }
        public string UsersFirstName { get; set; } = null!;
        public string UsersLastName { get; set; } = null!;
        public string UsersUserName { get; set; } = null!;
        public string UsersEmail { get; set; } = null!;
        public string UsersRole { get; set; } = null!;
        public string? UsersPassword { get; set; } = null!;
    }
}
