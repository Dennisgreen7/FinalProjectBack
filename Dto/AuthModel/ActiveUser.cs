using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.AuthModel
{
    public class ActiveUser
    {
        public string UsersId { get; set; }
        public string UsersFullName { get; set; } = null!;
        public string UsersRole { get; set; } = null!;
    }
}
