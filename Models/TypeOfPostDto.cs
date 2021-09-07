using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    public class TypeOfPostDto
    {
        public Guid TypeOfPostId { get; set; }

        public Guid Type { get; set; }
    }
}
