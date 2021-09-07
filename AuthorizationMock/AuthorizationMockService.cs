using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.AuthorizationMock
{
    public class AuthorizationMockService : IAuthorizationMockService
    {
        private readonly IConfiguration configuration;

        public AuthorizationMockService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool AuthorizeToken(string token)
        {
            if(token == null)
            {
                return false;
            }
            if (!token.StartsWith("Bearer"))
            {
                return false;
            }
            if(token.Substring(token.IndexOf("Bearer") + 7) != configuration.GetValue<string>("Token:SecretToken"))
            {
                return false;
            }
            return true;
        }
    }
}
