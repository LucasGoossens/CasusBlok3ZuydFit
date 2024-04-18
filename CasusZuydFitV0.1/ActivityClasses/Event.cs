using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasusZuydFitV0._1.UserClasses;
using static CasusZuydFitV0._1.DAL.DAL;


namespace CasusZuydFitV0._1.ActivityClasses
{
    public class Event : Activity
    {
        public List<Athlete> EventParticipants { get; set; }
        public string EventLocation { get; set; }
        public int EventPatricipantLimit { get; set; }


        public Event(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, string eventLocation, int eventPatricipantLimit, List<Athlete> eventParticipants)
        : base(activityId, activityName, activityDuration, startingTime, trainer, activityDescription)
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

        public void ShowEvent()
        {
            Console.WriteLine($"Event ID: {ActivityId}");
            Console.WriteLine($"Event Name: {ActivityName}");
            Console.WriteLine($"Event Location: {EventLocation}");
            Console.WriteLine($"Event Duration: {ActivityDurationMinutes}");
            Console.WriteLine($"Event Starting Time: {ActivityStartingTime}");
            Console.WriteLine($"Event Description: {ActivityDescription}");
            Console.WriteLine();
        }

        static public List<Event> GetEvents()
        {
            EventDAL eventDal = new EventDAL();
            eventDal.GetEvents();
            return eventDal.events;
        }
        public void AddEvent()
        {
            EventDAL eventDal = new EventDAL();
            eventDal.CreateEvent(this);
        }

        public static void DisplayAllEvents(User user)
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine("Which events do you want to see?");

            Console.WriteLine("1. The events I am signed up for");
            Console.WriteLine("2. All events");
            string choiceString = Console.ReadLine();
            int eventChoice;
            try
            {
                eventChoice = int.Parse(choiceString);
            }
            catch (FormatException)
            {
                Console.WriteLine("The entered choice is not valid.");
                return;
            }

            Console.Clear();

            switch (eventChoice)
            {
                case 1:
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("These are the events you are signed up for:");
                    foreach (var eventItem in GetEvents())
                    {
                        if (eventItem.EventParticipants.Exists(a => a.UserId == user.UserId))
                        {
                            Console.WriteLine($"Event ID: {eventItem.ActivityId}, Name: {eventItem.ActivityName}, Location: {eventItem.EventLocation}");
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("These are all the events:");
                    foreach (var eventItem in GetEvents())
                    {
                        Console.WriteLine($"Event ID: {eventItem.ActivityId}, Name: {eventItem.ActivityName}, Location: {eventItem.EventLocation}");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice entered.");
                    break;
            }
        }


    }
}
