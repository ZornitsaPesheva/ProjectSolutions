using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Solutions.Models
{
    public class Chapter
    {
        private ICollection<Post> posts;
        public Chapter()
        {
            this.posts = new HashSet<Post>();
        }

        public Chapter(string title, int courseId)
        {
            this.Title = title;
            this.CourseId = courseId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
               
    }
}