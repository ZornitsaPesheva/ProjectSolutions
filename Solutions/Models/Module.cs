using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Solutions.Models
{
    public class Module
    {
        private ICollection<Course> courses;

        public Module()
        {
            this.courses = new HashSet<Course>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
      //  [Index(IsUnique == true)]
        [StringLength(255)]
        public string Name { get; set; }

        public int Priority { get; set; }

        public virtual ICollection<Course> Courses { get; set; }


    }
}