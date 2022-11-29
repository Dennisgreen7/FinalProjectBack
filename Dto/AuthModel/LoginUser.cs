using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.AuthModel
{
    public class LoginUser
    {
        public string UsersUserName { get; set; } = null!;
        public string UsersPassword { get; set; } = null!;
    }
}
