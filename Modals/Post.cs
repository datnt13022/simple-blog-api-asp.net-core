using System;
using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modals
{
    public class Post
    {   
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime publishedAt { get; set; }
        public bool isPublished { get; set; }
        public string tag { get; set; }
    }
}