using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities
{
    public class Post
    {
        [Required]
        [Key]
        public Guid PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostDescription { get; set; }

        public string City { get; set; }

        public int UserId { get; set; }

        public DateTime PostPublishingDateTime { get; set; }

        public DateTime LastModified { get; set; }

        [ForeignKey("PostTypeId")]
        public Guid PostTypeId { get; set; }
    }
}
