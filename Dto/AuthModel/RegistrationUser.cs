using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.AuthModel
{
    public class RegistrationUser
    {
        public string UsersFirstName { get; set; } = null!;
        public string UsersLastName { get; set; } = null!;
        public string UsersUserName { get; set; } = null!;
        public string UsersEmail { get; set; } = null!;
        public string UsersPassword { get; set; } = null!;
        public string? UsersRole { get; set; } = null!;
    }
}
