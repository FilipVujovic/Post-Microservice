using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.BlockMockRepository
{
    public interface IBlockMockRepository
    {
        List<int> GetBlockedUsersForUser(int UserId);

        public bool CheckIfBlocked(int BlockingUserId, int BlockedUserId);
    }
}
