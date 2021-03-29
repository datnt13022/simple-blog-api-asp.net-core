using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_API.Modals;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repositories
{
    public class PostRepository:IPostRepository
    {
        private readonly BlogContext _context;
        public PostRepository(BlogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Post>> Get()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByTag(string tag)
        {
             var post= _context.Posts.Where(x => x.tag == tag);
             
             return post;
        }

        public async Task<Post> Get(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post> Create(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task Update(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var postDelete = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(postDelete);
            await _context.SaveChangesAsync();
        }
    }
}