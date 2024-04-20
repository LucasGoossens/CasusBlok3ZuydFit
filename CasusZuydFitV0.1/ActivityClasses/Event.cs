using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CasusZuydFitV0._1.RemainingClasses;
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
            Console.WriteLine($"Name: {ActivityName}");
            Console.WriteLine($"Location: {EventLocation}");
            Console.WriteLine($"Duration: {ActivityDurationMinutes}");
            Console.WriteLine($"Starting Time: {ActivityStartingTime}");
            Console.WriteLine($"Description: {ActivityDescription}");
            Console.WriteLine($"Participant Limit: {EventParticipants.Count}/{EventPatricipantLimit}");
            Console.WriteLine();
        }

        public void EditEvent()
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine($"Event ID: {ActivityId}");
            Console.WriteLine($"1. Name: {ActivityName}");
            Console.WriteLine($"2. Location: {EventLocation}");
            Console.WriteLine($"3. Duration: {ActivityDurationMinutes}");
            Console.WriteLine($"4. Starting Time: {ActivityStartingTime}");
            Console.WriteLine($"5. Description: {ActivityDescription}");
            Console.WriteLine($"6. Participant Limit: {EventParticipants.Count}/{EventPatricipantLimit}");
            Console.WriteLine("-----------------------");
            Console.WriteLine("0. Delete this event.");

            Console.WriteLine("Choose property to edit.");
            int propertyToEdit = Convert.ToInt32(Console.ReadLine());

            switch (propertyToEdit)
            {
                case 0:
                    Console.WriteLine("Are you sure you want to delete this event? (Press 0 to delete or any other key to cancel).");
                    if (Console.ReadLine() == "0")
                    {
                        EventDAL eventDal = new EventDAL();
                        eventDal.DeleteEvent(this.ActivityId);
                        Console.WriteLine("Event deleted.");                        
                        return;
                    }
                    else
                    {
                        this.EditEvent();
                        return;
                    }                    
                case 1:
                    Console.WriteLine("Enter new Event Name:");
                    ActivityName = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Enter new Event Location:");
                    EventLocation = Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("Enter new Event Duration:");
                    ActivityDurationMinutes = Convert.ToInt32(Console.ReadLine());
                    break;
                case 4:
                    Console.Write("Enter new Event Starting Time:(YYYY-MM-DD HH:MM): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startingTime))
                    {
                        Console.WriteLine("Invalid date format. Use YYYY-MM-DD HH:MM.");
                        this.EditEvent();
                        return;
                    }                    
                    ActivityStartingTime = Console.ReadLine();
                    break;
                case 5:
                    Console.WriteLine("Enter new Event Description:");
                    ActivityDescription = Console.ReadLine();
                    break;
                case 6:
                    Console.WriteLine("Enter Participant Limit:");
                    try
                    {
                        EventPatricipantLimit = Convert.ToInt32(Console.ReadLine());
                        if(EventPatricipantLimit < EventParticipants.Count)
                        {
                            Console.WriteLine("Can't make the participant limit lower than the amount of currently registered participants. Remove participants from event first.");
                            this.EditEvent();
                            return;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid entry.");
                        this.EditEvent();
                        return;
                    }
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid option.");                    
                    this.EditEvent();
                    return;
            }
            while (true)
            {
                Console.WriteLine("Press 1 to continue edit or any other key to exit.");
                string continueEditOption = Console.ReadLine();

                if (continueEditOption == "1")
                {
                    Console.Clear();
                    this.EditEvent();
                }
                else
                {
                    this.UpdateEvent();
                    break; 
                }
            }
        }

        public void UpdateEvent()
        {
            EventDAL eventDal = new EventDAL();
            eventDal.UpdateEvent(this);

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

        public static void DisplayAllEventsUser(User user)
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
                            LogFeedback.CheckFeedback(user, eventItem);
                            eventItem.ShowEvent();
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("These are all the events:");
                    foreach (var eventItem in GetEvents())
                    {
                        eventItem.ShowEvent();
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice entered.");
                    break;
            }
        }

        public static void ShowEventsFromTrainer(User trainer)
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine("These are the events you are hosting:");
            if(GetEvents().Count < 1)
            {
                Console.WriteLine("You are currently not hosting any events.");
                return;
            }
            foreach (var eventItem in GetEvents())
            {
                if (eventItem.Trainer.UserId == trainer.UserId)
                {
                    Console.WriteLine($"Number of Event Participants: {eventItem.EventParticipants.Count}");
                    eventItem.ShowEvent();
                }
            }
        }


    }
}
