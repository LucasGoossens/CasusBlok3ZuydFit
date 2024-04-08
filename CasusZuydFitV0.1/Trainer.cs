using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL;

namespace CasusZuydFitV0._1
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
        static public List<Trainer> GetTrainers()
        {
            TrainerDAL Dal = new TrainerDAL();
            Dal.GetTrainers();
            return Dal.trainers;
        }
    }
}
