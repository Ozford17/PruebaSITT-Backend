using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Models.Identity
{
    public  class UserData
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
         
        public string UserName { get; set; }
        public string Email { get; set; }
        

        public UserData(string userId, string fullName , string userName, string email )
        {
            UserId = userId;
            FullName = fullName;
            UserName = userName;
            Email = email;
             
        }
    }
}
