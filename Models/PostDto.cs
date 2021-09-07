using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    public class PostDto
    {
        public Guid PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostDescription { get; set; }

        public string City { get; set; }

        public int UserId { get; set; }

        

    }
}
