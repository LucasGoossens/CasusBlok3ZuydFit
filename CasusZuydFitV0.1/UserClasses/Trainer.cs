using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasusZuydFitV0._1.ActivityClasses;

namespace CasusZuydFitV0._1.UserClasses
{
    public class Trainer : User
    {
        public List<Activity> ActivityList { get; set; }

        public Trainer(int userId, string userName, string userEmail, string userPassword, List<Activity> activitylist) : base(userId, userName, userEmail, userPassword)
        {
            ActivityList = activitylist;
        }
        public Trainer(string userName, string userEmail, string userPassword, List<Activity> activitylist) : base(userName, userEmail, userPassword)
        {
            ActivityList = activitylist;
        }

        public Trainer()
        {
        }
    }
}
