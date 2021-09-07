using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.FollowingMockRepository
{
    public interface IFollowingMockRepository
    {
        public bool CheckFollowing(int follower, int followedUser);
    }
}
