using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    public class PostModificationDto
    {
        [Required(ErrorMessage = "Post Id is required!")]
        public Guid PostId { get; set; }
        [Required(ErrorMessage = "Post title is required!")]
        public string PostTitle { get; set; }
        [Required(ErrorMessage = "Post description is required!")]
        public string PostDescription { get; set; }
        [Required(ErrorMessage = "Post city is required!")]
        public string City { get; set; }

    }
}
