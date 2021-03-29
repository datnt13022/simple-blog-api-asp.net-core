using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_API.Modals;

namespace Blog_API.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> Get();
        Task<Category> Create(Category category);
        Task Update(Category category);
        void Delete(string tag);
    }
}