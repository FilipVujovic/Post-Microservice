using PostService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.FollowingMockRepository
{
    public class FollowingMockRepository : IFollowingMockRepository
    {

        public static List<FollowingMockDto> FollowingInstances { get; set; } = new List<FollowingMockDto>();

        public void FillData()
        {
            FollowingMockDto followInstance = new FollowingMockDto(); //Korisnika 1 prati korisnik 3
            followInstance.FollowedId = 1;
            followInstance.FollowerId = 3;
            FollowingInstances.Add(followInstance);

            FollowingMockDto followInstance1 = new FollowingMockDto(); //Korisnika 2 prati korisnik 3
            followInstance1.FollowedId = 2;
            followInstance1.FollowerId = 3;
            FollowingInstances.Add(followInstance1);

            FollowingMockDto followInstance2 = new FollowingMockDto(); //Korisnika 3 prati korisnik 1
            followInstance2.FollowedId = 3;
            followInstance2.FollowerId = 1;
            FollowingInstances.Add(followInstance2);
        }

        public FollowingMockRepository()
        {
            FillData();
        }
        public bool CheckFollowing(int follower, int followedUser)
        {
            Console.WriteLine("Follower - " + follower);
            Console.WriteLine("FollowedUser - " + followedUser);
            foreach (var followInstance in FollowingInstances)
            {
                Console.WriteLine("Follower - " + followInstance.FollowerId);
                Console.WriteLine("Followed - " + followInstance.FollowedId);
                if (followInstance.FollowerId == follower && followInstance.FollowedId == followedUser)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
