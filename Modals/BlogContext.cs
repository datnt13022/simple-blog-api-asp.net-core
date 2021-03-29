using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog_API.Modals
{
    public class BlogContext : DbContext
    {
        
        public BlogContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}