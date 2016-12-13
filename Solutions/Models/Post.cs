using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Solutions.Models
{
    public class Post
    {
        public Post()
        {

        }

        public Post(string authorId, string title, string link, int chapterId)
        {
            this.AuthorId = authorId;
            this.Title = title;
            this.Link = link;
            this.ChapterId = chapterId;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }
        
        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Chapter")]
        public int ChapterId { get; set; }
        public virtual Chapter Chapter { get; set; }

    }
}