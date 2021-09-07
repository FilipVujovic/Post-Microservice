using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    public class BlockMockDto
    {


        public Guid BlockId { get; set; }

        public int BlockingUserId { get; set; }

        public int BlockedUserId { get; set; }
    }
}
