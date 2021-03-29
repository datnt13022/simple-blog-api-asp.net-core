using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_API.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogContext _context;

        public UserRepository(BlogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public User Create(CreateUserModal user)
        {
            if (string.IsNullOrWhiteSpace(user.password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.username == user.username))
                throw new AppException("Username "+ user.username + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.password, out passwordHash, out passwordSalt);
            User newUser = new User();

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;
            newUser.fullname = user.fullname;
            newUser.username = user.username;
            
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
        
      
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", " ");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            var user = _context.Users.SingleOrDefault(x => x.username == username);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null; 
            return user;
        }

        public void Update(int id, UserUpdate user)
        {
            var userUpdate = _context.Users.Find(id);
            if (userUpdate == null)
                throw new AppException("User not found");
            //if user change user name
            if (!string.IsNullOrWhiteSpace(user.username) && user.username != userUpdate.username)
            {
                
                if (_context.Users.Any(x => x.username == user.username))
                    throw new AppException("Username " + user.username + " is already taken");

                userUpdate.username = user.username;
            }
            //if user change fullname
            if (!string.IsNullOrWhiteSpace(user.fullname))
            {
                userUpdate.fullname = user.fullname;
            }
            //if user change password
            if (!string.IsNullOrWhiteSpace(user.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(user.password, out passwordHash, out passwordSalt);

                userUpdate.PasswordHash = passwordHash;
                userUpdate.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(userUpdate);
            _context.SaveChanges();
        }

        

       
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User getbyIID(int userID)
        {
            return _context.Users.Find(userID);
        }
    }
}