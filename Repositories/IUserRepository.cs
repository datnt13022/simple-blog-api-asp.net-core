using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_API.Modals;

namespace Blog_API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(int userID);
        User  Create(CreateUserModal user);
        Task<User> Authenticate(string username , string password);
        void Update(int id,UserUpdate user);
        void Delete(int userID);
        User getbyIID(int userID);
    }
}