using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.BlockMockRepository
{
    public class BlockMockRepository : IBlockMockRepository
    {
        public List<BlockMockDto> Blocked { get; set; } = new List<BlockMockDto>();

        public BlockMockRepository()
        {
            FillData();
        }
        private void FillData()
        {
            BlockMockDto BlockInstance = new BlockMockDto();
            BlockInstance.BlockId = Guid.NewGuid();
            BlockInstance.BlockingUserId = 1;
            BlockInstance.BlockedUserId = 2;
            Blocked.Add(BlockInstance);
        }

        public bool CheckIfBlocked(int BlockingUserId, int BlockedUserId)
        {
            foreach(var block in Blocked)
            {
                if(block.BlockedUserId == BlockingUserId && block.BlockingUserId == BlockedUserId)
                {
                    return true;
                } else if (block.BlockingUserId == BlockingUserId && block.BlockedUserId == BlockedUserId)
                {
                    return true;
                }
            }
            return false;
        }

        public List<int> GetBlockedUsersForUser(int UserId)
        {
            List<int> blockedUsers = new List<int>();
            foreach (var block in Blocked)
            {
                if (block.BlockedUserId == UserId) {   //Korisnik je nas blokirao
                    blockedUsers.Add(block.BlockingUserId);
                } else if(block.BlockingUserId == UserId) // Mi smo blokirali korisnika
                {
                    blockedUsers.Add(block.BlockedUserId);
                }
            }
            return blockedUsers;
        }
    }
}
