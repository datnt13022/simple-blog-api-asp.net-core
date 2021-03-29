using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modals
{
    public class User 
    {
        [Key]
        public int userID { get; set; }
        public string username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string fullname { get; set; }

    }
}