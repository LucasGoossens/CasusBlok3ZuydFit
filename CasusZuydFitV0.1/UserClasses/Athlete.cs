using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasusZuydFitV0._1.ActivityClasses;
using static CasusZuydFitV0._1.DAL.DAL;

namespace CasusZuydFitV0._1.UserClasses
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
        public List<Workout> GetAllWorkouts()
        {
            WorkoutDAL allWorkoutsDal = new WorkoutDAL();
            return allWorkoutsDal.GetAllWorkoutsByAthleteId(UserId);
        }

        static public List<Athlete> GetAllAthletes()
        {
            AthleteDAL allAthletesDal = new AthleteDAL();
            allAthletesDal.GetAthlets();
            return allAthletesDal.athletes;
        }

        public void addActivity(Activity activity)
        {
            ActivityList.Add(activity);
        }
        public void removeActivity(int id)
        {
            Activity activityToRemove = ActivityList.FirstOrDefault(activity => activity.ActivityId == id);
            if (activityToRemove != null)
            {
                ActivityList.Remove(activityToRemove);
            }
        }




    }
}
