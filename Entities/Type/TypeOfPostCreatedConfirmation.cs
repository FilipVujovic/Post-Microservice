using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities.Type
{
    public class TypeOfPostCreatedConfirmation
    {

        public Guid TypeOfPostId { get; set; }

        public string Type { get; set; }
    }
}
