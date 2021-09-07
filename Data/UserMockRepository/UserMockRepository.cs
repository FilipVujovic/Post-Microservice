using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.UserMockRepository
{
    public class UserMockRepository : IUserMockRepository
    {
        public static List<UserMockDto> Users { get; set; } = new List<UserMockDto>();

        public UserMockRepository()
        {
            FillData();
        }

        private static void FillData()
        {
            Users.AddRange(new List<UserMockDto>
            {
                new UserMockDto 
                {
                    UserID = 1,
                    Email = "f.vujovic998@gmail.com",
                    City = "Kikinda"
                },
               new UserMockDto
                {
                    UserID = 2,
                    Email = "peraperic@yahoo.com",
                    City = "Subotica"
                },
                new UserMockDto 
                {
                    UserID = 3,
                    Email = "markolala5@aoe.com",
                    City = "Elemir"
                },
                 new UserMockDto
                {
                    UserID = 4,
                    Email = "teodoram@gmail.com",
                    City = "Nis"
                }
            });
        }

        public List<UserMockDto> GetAllUsers()
        {
            return Users;
        }

        public UserMockDto GetUserByID(int userID)
        {
            return Users.FirstOrDefault(e => e.UserID == userID);
        }
    }
}
