using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    public class Athlete : User
    {
        public List<Activity> ActivityList { get; set; }

        public Athlete(int userId, string userName, string userEmail, string userPassword, List<Activity> activity) : base(userId, userName, userEmail, userPassword)
        {
            ActivityList = activity;
        }

        public Athlete(string userName, string userEmail, string userPassword, List<Activity> activity) : base(userName, userEmail, userPassword)
        {
            ActivityList = activity;
        }

        public void EventRegisterAthlete(Event eventToRegisterAthlete)
        {
            eventToRegisterAthlete.EventParticipants.Add(this);
            ActivityList.Add(eventToRegisterAthlete);
        }

        public void EventRemoveRegistration()
        {

        }

        public void CreateExercise()
        {

        }

        public void DeleteExercise()
        {

        }



    
    }
}
