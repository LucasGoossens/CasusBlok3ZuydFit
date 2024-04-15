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

        public Event(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, string eventLocation, int eventPatricipantLimit,List<Athlete> eventParticipants )
        : base(activityId, activityName, activityDuration, startingTime, trainer, activityDescription)
        {
            EventLocation = eventLocation;
            EventParticipants = eventParticipants;
            EventPatricipantLimit = eventPatricipantLimit;
        }

        public Event(string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, string eventLocation, int eventPatricipantLimit, List<Athlete> eventParticipants)
      : base(activityName, activityDuration, startingTime, trainer, activityDescription)
        {
            EventLocation = eventLocation;
            EventParticipants = eventParticipants;
            EventPatricipantLimit = eventPatricipantLimit;
        }
        
         public Event(string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, List<Equipment> equipments, string eventLocation, int eventPatricipantLimit)
        {
            ActivityName = activityName;
            ActivityDurationMinutes = activityDuration;
            ActivityStartingTime = startingTime;
            Trainer = trainer;
            ActivityDescription = activityDescription;
            Equipments = equipments;
            EventLocation = eventLocation;
            EventPatricipantLimit = eventPatricipantLimit;
        }

        static public List<Event> GetEvents()
        {
            DAL.EventDAL eventDal = new DAL.EventDAL();
            eventDal.GetEvents();
            return eventDal.events;
        }
        public void AddEvent()
        {
            DAL.EventDAL eventDal = new DAL.EventDAL();
            eventDal.CreateEvent(this);
        }
    }
}
