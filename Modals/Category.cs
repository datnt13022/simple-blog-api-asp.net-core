using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modals
{
    public class Category
    {
        [Key]
        public string tag { get; set; }
        public string categoryName { get; set; }
    }
}