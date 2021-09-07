using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    public class TypeOfPostCreationConfirmationDto
    {
        public Guid TypeOfPostId { get; set; }

        public string Type { get; set; }
    }
}
