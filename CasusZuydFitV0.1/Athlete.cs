using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL;

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

        public Athlete(string userName, string userEmail, string userPassword) : base(userName, userEmail, userPassword)
        {
            
        }
        
        
        public void EventRegisterAthlete(Event eventToRegisterAthlete)
        {
            eventToRegisterAthlete.EventParticipants.Add(this);
            ActivityList.Add(eventToRegisterAthlete);
        }



        public List<Workout> GetAllWorkouts()
        {
            WorkoutDAL allWorkoutsDal = new WorkoutDAL();
            return allWorkoutsDal.GetAllWorkoutsByAthleteId(this.UserId);
        }

        static public List<Athlete> GetAllAthletes()
        {
            AthleteDAL allAthletesDal = new AthleteDAL();
            allAthletesDal.GetAthlets();
            return allAthletesDal.athletes;
        }



    
    }
}
