using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.BorrowModel
{
    public class ClientBorrow
    {
        public int BorrowingBookId { get; set; }
        public int BorrowingUserId { get; set; }
    }
}
