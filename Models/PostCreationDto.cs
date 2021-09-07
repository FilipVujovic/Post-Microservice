using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Models
{
    /// <summary>
    /// Predstavlja model kreiranja posta.
    /// </summary>
    public class PostCreationDto
    {
        /// <summary>
        /// Naslov posta
        /// </summary>
        
        [MaxLength(15)]
        [Required(ErrorMessage = "Post title is required!")]
        public string PostTitle { get; set; }

        /// <summary>
        /// Opis posta
        /// </summary>
        [MaxLength(255)]
        [Required(ErrorMessage = "Post description is required!")]
        public string PostDescription { get; set; }

        /// <summary>
        /// Grad na koji se post odnosi
        /// </summary>
        [Required(ErrorMessage = "Post city is required!")]
        public string City { get; set; }

        /// <summary>
        /// Id korisnika koji postavlja post
        /// </summary>
        [Required(ErrorMessage = "User Id is required!")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Type Id is required!")]
        public string Type { get; set; }
    }
}
