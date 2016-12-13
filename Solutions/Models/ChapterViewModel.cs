using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Solutions.Models
{
    public class ChapterViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public int CourseId { get; set; }
        
        public ICollection<Course> Courses { get; set; }
    }
}