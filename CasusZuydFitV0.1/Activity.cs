using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL;

namespace CasusZuydFitV0._1
{
    public abstract class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int ActivityDurationMinutes { get; set; }
        public string ActivityStartingTime { get; set; }
        public Trainer Trainer { get; set; }
        public string ActivityDescription { get; set; }
        public List<Equipment> Equipments { get; set; } = new List<Equipment>();

        public Activity()
        {
            
        }

        public Activity(int activityId, string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, List<Equipment> equipments)
        {
            ActivityId = activityId;
            ActivityName = activityName;
            ActivityDurationMinutes = activityDurationMinutes;
            ActivityStartingTime = activityStartingTime;
            Trainer = trainer;
            ActivityDescription = activityDescription;
            Equipments = equipments;
        }

        public Activity(string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, List<Equipment> equipments)
        {
            ActivityName = activityName;
            ActivityDurationMinutes = activityDurationMinutes;
            ActivityStartingTime = activityStartingTime;
            Trainer = trainer;
            ActivityDescription = activityDescription;
            Equipments = equipments;
        }

        public Activity(int activityId, string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription)
        {
            ActivityId = activityId;
            ActivityName = activityName;
            ActivityDurationMinutes = activityDurationMinutes;
            ActivityStartingTime = activityStartingTime;
            Trainer = trainer;
            ActivityDescription = activityDescription;
        }

        public Activity(string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription)
        {
            ActivityName = activityName;
            ActivityDurationMinutes = activityDurationMinutes;
            ActivityStartingTime = activityStartingTime;
            Trainer = trainer;
            ActivityDescription = activityDescription;
            
        }
        public static List<Activity> GetActivities()
        {
            ActivityDAL activityDal = new ActivityDAL();
            activityDal.GetActivities();
            return activityDal.activities;
        }
    }
}
