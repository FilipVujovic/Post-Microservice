using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities.Type
{
    public class TypeOfPost
    {

        [Key]
        [Required]
        public Guid TypeOfPostId { get; set; }

        [StringLength(50)]
        [Required]
        
        public string Type { get; set; }

    }
}
