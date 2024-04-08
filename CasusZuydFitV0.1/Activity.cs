using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int ActivityDurationMinutes { get; set; }
        public string ActivityStartingTime { get; set; }
        public Trainer Trainer { get; set; }
        public string ActivityDescription { get; set; }
        public List<Equipment> Equipments { get; set; }

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
    }
}
