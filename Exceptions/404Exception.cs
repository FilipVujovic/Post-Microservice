using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Exceptions
{
    public class _404Exception : Exception
    {
        public _404Exception(string message)
             : base(message)
        {

        }
        public _404Exception(string message, Exception inner)
             : base(message, inner)
        {

        }
    }
}
