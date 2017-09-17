using System;
using System.ComponentModel.DataAnnotations;

namespace My_Blog.Models
{
    public class Comment
    {
        [Required(ErrorMessage = "Name required")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Comment text required")]
        public string Body { get; set; }
        public DateTimeOffset PubDate { get; set; } = DateTimeOffset.Now;
        public bool IsPublic { get; set; }
        public Guid UniqueId { get; set; }
    }
}
