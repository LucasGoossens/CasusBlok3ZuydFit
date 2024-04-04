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
        public int ActivityDuration { get; set; }
        public string StartingTime { get; set; }
        public Trainer Trainer { get; set; }
        public string ActivityDescription { get; set; }
        public List<Equipment> Equipments { get; set; }

        public Activity(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, List<Equipment> equipments)
        {
            ActivityId = activityId;
            ActivityName = activityName;
            ActivityDuration = activityDuration;
            StartingTime = startingTime;
            Trainer = trainer;
            ActivityDescription = activityDescription;
            Equipments = equipments;
        }
    }
}
