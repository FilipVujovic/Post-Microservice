using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException(string message)
             : base(message)
        {

        }
        public GeneralException(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
