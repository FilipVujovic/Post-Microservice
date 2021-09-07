using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models.Mocks
{
    public class FollowingMockDto
    {
        public int FollowerId { get; set; }

        public int FollowedId { get; set; }
    }
}
