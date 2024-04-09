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
        public int EventPatricipantLimit { get; set; }
        
        public Event(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, List<Equipment> equipents, List<Athlete> eventParticipants, string eventLocation, int eventPatricipantLimit)
            : base(activityId, activityName, activityDuration, startingTime, trainer, activityDescription, equipents)
        {
            EventParticipants = eventParticipants;
            EventLocation = eventLocation;
            EventPatricipantLimit = eventPatricipantLimit;
        }
        public Event(string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, List<Equipment> equipents, List<Athlete> eventParticipants, string eventLocation, int eventPatricipantLimit)
           : base(activityName, activityDuration, startingTime, trainer, activityDescription, equipents)
        {
            EventParticipants = eventParticipants;
            EventLocation = eventLocation;
            EventPatricipantLimit = eventPatricipantLimit;
        }

        public Event(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, string eventLocation, int eventPatricipantLimit)
         : base(activityId, activityName, activityDuration, startingTime, trainer, activityDescription)
        {
            EventLocation = eventLocation;
            EventPatricipantLimit = eventPatricipantLimit;
        }

        public Event(string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, string eventLocation, int eventPatricipantLimit)
          : base(activityName, activityDuration, startingTime, trainer, activityDescription)
        {            
            EventLocation = eventLocation;
            EventPatricipantLimit = eventPatricipantLimit;
        }
    }
}
