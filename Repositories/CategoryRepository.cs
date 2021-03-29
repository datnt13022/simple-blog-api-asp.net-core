using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_API.Modals;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogContext _context;
        public CategoryRepository(BlogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> Get()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> Get(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> Create(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async void Delete(string tag)
        {
            var categoryDelete = _context.Categories.Find(tag);
           if (categoryDelete != null)
           {
               _context.Categories.Remove(categoryDelete);
               _context.SaveChanges();
           }
                       
        }

     
    }
}