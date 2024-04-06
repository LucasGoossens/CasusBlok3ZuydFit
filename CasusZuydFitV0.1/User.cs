﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL;

namespace CasusZuydFitV0._1
{
    public abstract class User
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

        public User(string userName, string userEmail, string userPassword)
        {
            UserName = userName;
            UserEmail = userEmail;
            UserPassword = userPassword;
        }

        // dit misschien weg dan aangezien t al in DAL gebeurd?
        static void GetUsers()
        {
            UserDAL Dal = new UserDAL();
            Dal.GetUsers();
        }

        public void CreateNewUser()
        {
            UserDAL Dal = new UserDAL();
            Dal.CreateNewUser(this);
        }
    }
}
