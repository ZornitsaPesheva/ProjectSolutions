using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Solutions.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(225)]
        public string Name { get; set; }

        public int ModuleId { get; set; }
        public ICollection<Course> Courses { get; set; }
        public List<Module> Modules { get; internal set; }
    }
}