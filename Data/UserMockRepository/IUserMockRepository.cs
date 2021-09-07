using PostService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.UserMockRepository
{
    public interface IUserMockRepository
    {
        List<UserMockDto> GetAllUsers();
        UserMockDto GetUserByID(int userID);
    }
}
