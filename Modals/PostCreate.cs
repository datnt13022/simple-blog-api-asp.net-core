using System;

namespace Blog_API.Modals
{
    public class PostCreate
    {
        public string title { get; set; }
        public string content { get; set; }
        public DateTime publishedAt { get; set; }
        public bool isPublished { get; set; }
        public string tag { get; set; }
    }
}