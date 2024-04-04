using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    internal abstract class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }


        public User(int userId, string userName, string userEmail, string userPassword)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            UserPassword = userPassword;
        }
    }
}
