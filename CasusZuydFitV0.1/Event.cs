using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CasusZuydFitV0._1
{
    public class Event : Activity
    {
        public List<Athlete> EventParticipants { get; set; }
        public string EventLocation { get; set; }


        Event(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, List<Equipment> equipents, List<Athlete> eventParticipants, string eventLocation)
            : base(activityId, activityName, activityDuration, startingTime, trainer, activityDescription, equipents)
        {
            EventParticipants = eventParticipants;
            EventLocation = eventLocation;

        }
    }
}
