using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    class User
    {
        public string UserName { get; set; }
        public string Test { get; set; }
        public User(string userName, string test)
        {
            UserName = userName;
            Test = test;

        }
    }
}
