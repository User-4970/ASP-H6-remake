using System;
using System.Collections.Generic;

namespace ASP_Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Summary { get; set; }
        public string Content { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }
}
