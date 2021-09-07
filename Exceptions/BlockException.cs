using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    public class BlockException : Exception
    {

        public BlockException(string message)
            : base(message)
        {

        }
        public BlockException(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
