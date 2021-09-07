using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.AuthorizationMock
{
    public interface IAuthorizationMockService
    {
        public bool AuthorizeToken(string token);
    }
}
