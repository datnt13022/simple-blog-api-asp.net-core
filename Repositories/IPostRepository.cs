using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_API.Modals;

namespace Blog_API.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> Get();
        Task<IEnumerable<Post>> GetByTag(string tag);
        Task<Post> Get(int id);
        Task<Post> Create(Post post);
        Task Update(Post post);
        Task Delete(int id);
    }
}