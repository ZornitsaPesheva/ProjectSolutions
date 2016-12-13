using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Solutions.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public string AutorId { get; set; }

        public int ChapterId { get; set; }

        public ICollection<Chapter> Chapters { get; set; }

    }
}