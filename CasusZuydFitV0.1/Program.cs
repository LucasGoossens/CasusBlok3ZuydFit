﻿using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using static CasusZuydFitV0._1.DAL;

namespace CasusZuydFitV0._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DeleteEquipment();
            // Example usage
            Program program = new Program();

            //User user = new Athlete(2, "test", "test", "test", new System.Collections.Generic.List<Activity>());

            while (true)
            {
                Console.WriteLine("ZuydFit");
                Console.WriteLine("Select Option:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Create Account");
                Console.Write(">");

                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        CreateAccount();
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
            


            User user = new Trainer(2, "testTrainer", "testTrainer", "testTrainer", new System.Collections.Generic.List<Activity>());
            bool running = true;
            while (running)
            {
                Console.WriteLine("        ZUYDFIT        ");
                Console.WriteLine("=======================");
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. View Profile");
                Console.WriteLine("2. View All Activities");
                Console.WriteLine("3. View All Athletes (Trainers only!)");
                Console.WriteLine("Enter option (or 'exit' to close): ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        //ManageProfile(user);
                        break;
                    case "2":
                        //DisplayAllActivities(user);
                        break;
                    case "3":
                        DisplayAllUsers();
                        break;
                    case "exit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }

            //User user = new Athlete(1, "test", "test", "test", new System.Collections.Generic.List<Activity>());


            while (true)
            {
                Console.WriteLine("=======================");
                Console.WriteLine("Kies optie:");
                Console.WriteLine("-----------------------");
                Console.WriteLine("1: Display all users");
                Console.WriteLine("2: Create new user");
                Console.WriteLine("-----------------------");
                //Console.WriteLine("3. Create new Exercise");
                //Console.WriteLine("4. Display all Exercises");
                Console.WriteLine("3. Create new Workout");
                Console.WriteLine("4. Show all events");
                Console.WriteLine("5. Show all workouts");
                Console.WriteLine("6. Manage profile");
                Console.WriteLine("7. Create Event");

                // Trainer functies
                Console.WriteLine("7. Trainer gives feedback");

                int option;

                try
                {
                    option = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option.");
                    continue;
                }


                switch (option)
                {
                    case 1:
                        DisplayAllUsers();
                        break;
                    case 2:
                        CreateNewUser();
                        break;
                    case 3:
                        CreateNewWorkout();
                        break;
                    case 4:
                        DisplayAllEvents(user);
                        break;
                    case 5:
                        DisplayAllWorkouts();
                        break;
                    case 6:
                        ManageProfile(user);
                        break;
                    case 7:
                        TrainerGivesFeedback(user);
                        break;
                    case 8:
                        CreateEvent(user);
                        break;
                }
            }

            void DisplayAllUsers()
            {
                Console.Clear();


                Console.WriteLine("Total Users: ");
                Console.WriteLine(User.GetUsers().Count());


                Console.WriteLine("List of Users:");
                foreach (var user in User.GetUsers())
                {
                    Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}, Email: {user.UserEmail}");
                }

            }

            void DisplayAllAthletes()
            {
                Console.Clear();

                AthleteDAL athleteDAL = new AthleteDAL();
                athleteDAL.GetAthlets();

                List<User> allUsers = User.GetUsers();
                Console.WriteLine("Total Users: ");
                Console.WriteLine(allUsers.Count());

                Console.WriteLine("List of Athletes:");
                foreach (var athlete in allUsers)
                {
                    Console.WriteLine($"Athlete ID: {athlete.UserId}, Name: {athlete.UserName}, Email: {athlete.UserEmail}");
                }

                Console.WriteLine("Enter the name or ID of the athlete to search for:");
                string searchedAthlete = Console.ReadLine();

                Athlete foundAthlete = null;
                foreach (var athlete in allUsers)
                {
                    if (athlete.UserName.Equals(searchedAthlete, StringComparison.OrdinalIgnoreCase) ||
                        athlete.UserId.ToString() == searchedAthlete)
                    {
                        foundAthlete = (Athlete)athlete;
                        break;
                    }
                }

                if (foundAthlete != null)
                {
                    Console.WriteLine("Athlete found:");
                    Console.WriteLine($"Athlete ID: {foundAthlete.UserId}, Name: {foundAthlete.UserName}, Email: {foundAthlete.UserEmail}");
                }
                else
                {
                    Console.WriteLine("Athlete not found.");
                }
            }


            User FindUser(string searchTerm)
            {
                int.TryParse(searchTerm, out int id);
                var user = User.GetUsers().FirstOrDefault(u => u.UserName.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) || u.UserId == id);
                return user;
            }


            void CreateNewUser()
            {
                Console.Clear();
                Console.WriteLine("Enter user name:");
                string UserName = Console.ReadLine();
                Console.WriteLine("Enter user email:");
                string UserEmail = Console.ReadLine();
                Console.WriteLine("Enter user password:");
                string UserPassword = Console.ReadLine();

                int UserType = 0;

                // uiteindelijk zijn er andere schermen voor verschillende gebruikers aanmaken,
                // dit is om te testen
                do
                {
                    Console.WriteLine("Enter user type:");
                    Console.WriteLine("1. Athlete");
                    Console.WriteLine("2. Trainer");
                    UserType = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                } while (UserType < 1 || UserType > 2);

                User user;

                switch (UserType)
                {
                    case 1:
                        user = new Athlete(UserName, UserEmail, UserPassword, new List<Activity>());
                        user.CreateNewUser();
                        break;
                    case 2:
                        user = new Trainer(UserName, UserEmail, UserPassword, new List<Activity>());
                        user.CreateNewUser();
                        break;
                }

                Console.WriteLine("New user " + UserName + "succesfully created.");
            }

            Exercise CreateNewExercise(int workoutId) // dit moet waarschijnlijk in Exercise.cs
            {
                Console.Clear();
                Console.WriteLine("Enter Exercise Name:");
                string ExerciseName = Console.ReadLine();
                Console.WriteLine("Enter Exercise description:");
                string ExerciseDescription = Console.ReadLine();

                Exercise testExercise = new Exercise(ExerciseName, "exerciseresult", ExerciseDescription, workoutId);
                return testExercise;
            }

            void DisplayAllExercises()
            {
                Console.Clear();

                Console.WriteLine("Total Exercises: ");
                Console.WriteLine(Exercise.GetExercises().Count());


                Console.WriteLine("List of Exercises:");
                foreach (var exercise in Exercise.GetExercises())
                {
                    Console.WriteLine($"Exercise ID: {exercise.ExerciseId}, Name: {exercise.ExerciseName}, Description: {exercise.ExerciseDescription}");
                }
            }

            void CreateNewWorkout()
            {
                Console.Clear();

                Console.WriteLine("      New workout");
                Console.WriteLine("-----------------------");
                Console.WriteLine("New Workout Name:");
                string newWorkoutName = Console.ReadLine();
                Console.WriteLine("Workout duration in minutes:");
                int newWorkoutDuration = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Workout starting time:");
                string newWorkoutStartingTime = Console.ReadLine();
                Console.WriteLine("Workout description:");
                string newWorkoutDescription = Console.ReadLine();
                Console.WriteLine("Workout Athlete ID:"); // dit is foutgevoelig voor nu, uiteindelijk misschien eerst lijst van alle Athlete Id's geven
                int newAthleteParticipantId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Workout Trainer ID:"); // dit is foutgevoelig voor nu, uiteindelijk misschien eerst lijst van alle Trainer Id's geven
                int newTrainerParticipantId = Convert.ToInt32(Console.ReadLine());

                List<User> allUsers = User.GetUsers();

                Athlete newWorkOutAthlete = (Athlete)allUsers.FirstOrDefault(athlete => athlete.UserId == newAthleteParticipantId);
                Trainer newWorkOutTrainer = (Trainer)allUsers.FirstOrDefault(trainer => trainer.UserId == newTrainerParticipantId);

                Workout newWorkout = new Workout(newWorkoutName, newWorkoutDuration, newWorkoutStartingTime, newWorkOutTrainer, newWorkoutDescription, newWorkOutAthlete);
                newWorkout.CreateNewWorkout();
                int workoutIdToAddToExercise = newWorkout.ActivityId;

                LogFeedback newLogFeedback = new LogFeedback(newWorkOutTrainer.UserId, newWorkOutAthlete.UserId, newWorkout.ActivityId);
                newLogFeedback.CreateFeedback();

                int addExerciseOption = 1;
                while (addExerciseOption == 1)
                {
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("1. Add new Exercise to Workout");
                    Console.WriteLine("2. Save Workout");

                    addExerciseOption = Convert.ToInt32(Console.ReadLine());

                    switch (addExerciseOption)
                    {
                        case 1:
                            Console.WriteLine("-----------------------");
                            Exercise testExercise = CreateNewExercise(workoutIdToAddToExercise); // dit is een Program.method
                            testExercise.CreateExercise();              // dit is een object.method
                            break;
                        case 2:
                            addExerciseOption = 2;
                            break;
                    }
                }

            }

            void DisplayAllEvents(User user)
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
                        foreach (var eventItem in Event.GetEvents())
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
                        foreach (var eventItem in Event.GetEvents())
                        {
                            Console.WriteLine($"Event ID: {eventItem.ActivityId}, Name: {eventItem.ActivityName}, Location: {eventItem.EventLocation}");

                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice entered.");
                        break;
                }
            }

            void DisplayAllWorkouts()
            {
                WorkoutDAL workoutDAL = new WorkoutDAL();
                workoutDAL.GetWorkouts();

                AthleteDAL athleteDAL = new AthleteDAL();
                athleteDAL.GetAthlets();

                int MockAthleteId = 6; // dit dan uiteindelijk de ingelogde athlete
                Athlete currentAthlete = athleteDAL.athletes.Find(athlete => athlete.UserId == MockAthleteId);

                Console.WriteLine("-----------------------");
                Console.WriteLine("All Workouts:\n");
                Console.WriteLine("-----------------------");
                int workoutNumber = 1;
                foreach (Workout workout in workoutDAL.workouts)
                {
                    workoutNumber++;
                    if (currentAthlete.ActivityList.Contains(workout))
                    {
                        Console.WriteLine($"{workoutNumber}. Workout Name: {workout.ActivityName}");
                        Console.WriteLine($"   Duration (minutes): {workout.ActivityDurationMinutes}");
                        Console.WriteLine($"   Trainer: {workout.Trainer.UserName}");
                        Console.WriteLine($"   Description: {workout.ActivityDescription}");
                        Console.WriteLine("---------------------------------------------------------");
                    }
                }

                Console.WriteLine("\nEnter the number of the workout to view its details and exercises:");
                if (!int.TryParse(Console.ReadLine(), out int selectedNumber) || selectedNumber < 1 || selectedNumber > workoutDAL.workouts.Count)
                {
                    Console.WriteLine("Invalid selection. Please restart and enter a valid workout number.");
                    return;
                }

                selectedNumber--;
                Workout selectedWorkout = workoutDAL.workouts[selectedNumber];

                Console.WriteLine($"\nSelected Workout: {selectedWorkout.ActivityName}");
                Console.WriteLine($"Duration (minutes): {selectedWorkout.ActivityDurationMinutes}");
                Console.WriteLine($"Starting Time: {selectedWorkout.ActivityStartingTime}");
                Console.WriteLine($"Trainer: {selectedWorkout.Trainer.UserName}");
                Console.WriteLine($"Description: {selectedWorkout.ActivityDescription}");

                Console.WriteLine("\nExercises:");
                if (selectedWorkout.WorkoutExercises != null && selectedWorkout.WorkoutExercises.Count > 0)
                {
                    foreach (Exercise exercise in selectedWorkout.WorkoutExercises)
                    {
                        Console.WriteLine($"- Exercise Name: {exercise.ExerciseName}");
                        Console.WriteLine($"  Description: {exercise.ExerciseDescription}");
                        Console.WriteLine($"  Result: {exercise.ExerciseResult}");
                    }
                }
                else
                {
                    Console.WriteLine("This workout has no exercises listed.");
                }


            }

            void ManageProfile(User user)
            {


                Console.Clear();
                Console.WriteLine($"Profile:");
                Console.WriteLine("-----------------------");
                Console.WriteLine($"Name: {user.UserName}");
                Console.WriteLine($"Email: {user.UserEmail}");
                Console.WriteLine("-----------------------");
                Console.WriteLine("1. Edit profile");
                Console.WriteLine("2. Delete profiel");
                Console.WriteLine("3. Main menu");
                int profileChoice = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (profileChoice)
                {
                    case 1:
                        Console.WriteLine("enter a new username:");
                        string newUserName = Console.ReadLine();
                        Console.Clear();
                        while (User.GetUsers().Any(existingUser => existingUser.UserName == newUserName && newUserName != user.UserName))
                        {
                            Console.WriteLine("This username is already taken, please enter a new one.");
                            newUserName = Console.ReadLine() ?? string.Empty;
                            Console.Clear();
                        }
                        Console.WriteLine("enter a new email:");
                        string newUserEmail = Console.ReadLine();
                        Console.WriteLine("enter a new password:");
                        string newUserPassword = Console.ReadLine();
                        Console.Clear();
                        user.UpdateUser(newUserName, newUserEmail, newUserPassword);
                        Console.WriteLine("Profile updated.");
                        Console.WriteLine();
                        break;
                    case 2:
                        Console.WriteLine("Are you sure you wanna delete your profile?");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        int deleteProfileChoice = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        if (deleteProfileChoice == 1)
                        {
                            user.DeleteUser();
                            Console.WriteLine("Profile deleted.");
                            Console.WriteLine();
                        }
                        break;
                }
            }


            void TrainerGivesFeedback(User user)
            {
                try
                {
                    LogFeedback logFeedback = null;
                    Console.WriteLine($"All activities where {user.UserName} is the trainer: ");
                    foreach (Activity activity in Activity.GetActivities())
                    {
                        if (activity.Trainer.UserId == user.UserId)
                        {
                            Console.WriteLine($"Activity ID: {activity.ActivityId}, Name: {activity.ActivityName}");
                        }
                    }
                    Console.WriteLine("Enter the ID of the activity you want to give feedback on: ");
                    int activityId = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Activity activityToGiveFeedbackOn = Activity.GetActivities().FirstOrDefault(activity => activity.ActivityId == activityId);
                    if (activityToGiveFeedbackOn == null)
                    {
                        Console.WriteLine("A non-existing ID was given");
                        return;
                    }
                    else if (activityToGiveFeedbackOn.Trainer.UserId == user.UserId)
                    {
                        if (activityToGiveFeedbackOn is Event)
                        {
                            Event eventToGiveFeedback = activityToGiveFeedbackOn as Event;
                            Console.WriteLine("All users that are signed up for this event: ");
                            foreach (Athlete athlete in eventToGiveFeedback.EventParticipants)
                            {
                                Console.WriteLine($"User ID: {athlete.UserId}, Name: {athlete.UserName}");
                            }
                            Console.WriteLine("Pick a UserID for the user you wanna give feedback on");
                            int pickedUserId = Convert.ToInt32(Console.ReadLine());
                            User? userToGiveFeedbackOn = User.GetUsers().FirstOrDefault(user => user.UserId == pickedUserId);
                            logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == activityId && feedback.FeedbackAthleteId == pickedUserId);
                        }
                        else
                        {
                            Workout WorkoutToGiveFeedbackOn = activityToGiveFeedbackOn as Workout;
                            logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == activityId && feedback.FeedbackAthleteId == WorkoutToGiveFeedbackOn.WorkoutParticipant.UserId);
                        }
                        Console.Clear();
                        Console.WriteLine($"Activity Name: {activityToGiveFeedbackOn.ActivityName} Current Feedaback: {logFeedback.FeedbackInfo}");
                        Console.WriteLine("Enter the feedback you want to give: ");
                        string NewFeedback = Console.ReadLine();
                        logFeedback.UpdateFeedback(NewFeedback);

                    }
                    else
                    {
                        Console.WriteLine("You are not the trainer of this activity.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input given.");
                }
            }


            void CreateEvent(User user)
            {
                Console.WriteLine("Enter event details:");

                // Gather event details from the user
                Console.Write("Event Name: ");
                string eventName = Console.ReadLine();

                Console.Write("Duration (minutes): ");
                int duration;
                while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
                {
                    Console.WriteLine("Duration must be a positive integer.");
                    Console.Write("Duration (minutes): ");
                }

                Console.Write("Starting Time (YYYY-MM-DD HH:MM): ");
                DateTime startingTime;
                while (!DateTime.TryParse(Console.ReadLine(), out startingTime))
                {
                    Console.WriteLine("Invalid date format. Please use YYYY-MM-DD HH:MM.");
                    Console.Write("Starting Time (YYYY-MM-DD HH:MM): ");
                }

                Console.Write("Description: ");
                string description = Console.ReadLine();

                int trainerId = user.UserId;

                Console.Write("Location: ");
                string location = Console.ReadLine();

                Console.Write("Participant Limit: ");
                int participantLimit;
                while (!int.TryParse(Console.ReadLine(), out participantLimit) || participantLimit <= 0)
                {
                    Console.WriteLine("Participant Limit must be a positive integer.");
                    Console.Write("Participant Limit: ");
                }

                List<Equipment> equipments = GatherEquipments();

                // Create Event object
                Event newEvent = new Event(eventName, duration, startingTime.ToString("MM/dd/yyyy HH:mm:ss"), new Trainer { UserId = trainerId }, description, equipments, location, participantLimit);

                newEvent.AddEvent();
                Console.WriteLine("Event created successfully!");
            }

            static List<Equipment> GatherEquipments()
            {
                Console.WriteLine("Enter equipment details:");
                Console.Write("Number of equipments: ");
                int count;
                while (!int.TryParse(Console.ReadLine(), out count) || count < 0)
                {
                    Console.WriteLine("Number of equipments must be a positive integer.");
                    Console.Write("Number of equipments: ");
                }

                List<Equipment> equipments = new List<Equipment>();
                for (int i = 0; i < count; i++)
                {
                    Console.Write($"Equipment {i + 1} ID: ");
                    int equipmentId;
                    while (!int.TryParse(Console.ReadLine(), out equipmentId) || equipmentId <= 0)
                    {
                        Console.WriteLine("Equipment ID must be a positive integer.");
                        Console.Write($"Equipment {i + 1} ID: ");
                    }

                    equipments.Add(new Equipment { EquipmentId = equipmentId });
                }

                return equipments;
            }




            void CheckFeedback(User user, Activity activity)
            {
                LogFeedback? logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == activity.ActivityId && feedback.FeedbackAthleteId == user.UserId);
                if (logFeedback.FeedbackInfo == "")
                { Console.WriteLine("No feedback given yet."); }
                else if (logFeedback != null)
                {
                    Console.WriteLine($"Feedback for {activity.ActivityName}: {logFeedback.FeedbackInfo}");
                }
                else { Console.WriteLine("Something went wrong, Please contact the Servicedesk"); }

            }

            User Login(UserDAL dal)
            {
                UserDAL UserDAL = new UserDAL();
                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                User user = dal.GetUser(username, password);
                if (user != null)
                {
                    Console.WriteLine("Login successful!");
                    return user;
                }
                else
                {
                    Console.WriteLine("Invalid username or password.");
                    return null;
                }
            }

            void CreateAccount(UserDAL dal)
            {
                
                Console.Write("Enter new username: ");
                string username = Console.ReadLine();
                Console.Write("Enter new email: ");
                string email = Console.ReadLine();
                Console.Write("Enter new password: ");
                string password = Console.ReadLine();

                Athlete newAthlete = new Athlete(username, email, password);
                dal.CreateNewUser(newAthlete);

                Console.WriteLine("Account created successfully!");
            }


            void DisplayAllActivities()
            {
                Console.WriteLine("What Activities do you want to see?");
                Console.WriteLine("1: Events (Group activity)");
                Console.WriteLine("2: Workouts (Solo activity)");
                Console.Write("Please enter your choice (1 or 2): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllEvents(user);
                        break;
                    case "2":
                        DisplayAllWorkouts();
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please enter 1 or 2.");
                        break;
                }
            }
            void RegisterForEvent(User user)
            {
                foreach (Event events in Event.GetEvents())
                {
                    Console.WriteLine($"Event ID: {events.ActivityId}");
                    Console.WriteLine($"Event Name: {events.ActivityName}");
                    Console.WriteLine($"Event Location: {events.EventLocation}");
                    Console.WriteLine($"Event Duration: {events.ActivityDurationMinutes}");
                    Console.WriteLine($"Event Starting Time: {events.ActivityStartingTime}");
                    Console.WriteLine($"Event Description: {events.ActivityDescription}");
                    Console.WriteLine();
                }

                Console.WriteLine("Enter the ID of the event you want to register for: ");
                int registerID = Int32.Parse(Console.ReadLine());
                Console.Clear();
                Event? eventToRegister = Event.GetEvents().FirstOrDefault(registerEvent => registerEvent.ActivityId == registerID);
                if (eventToRegister == null)
                {
                    Console.WriteLine("The entered ID does not match any event. press enter to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();
                }

                else if (eventToRegister.EventParticipants.Count < eventToRegister.EventPatricipantLimit)
                {
                    LogFeedback newLogFeedback = new LogFeedback(eventToRegister.Trainer.UserId, user.UserId, eventToRegister
                        .ActivityId);

                    newLogFeedback.CreateFeedback();
                    Console.WriteLine("You have been registered for the event. press enter to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("The event is full, you cannot register. press enter to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();


                }
            }
            void DeleteEquipment()
            {
                try
                {
                    Console.WriteLine("Enter the ID of the equipment you want to delete: ");
                    int equipmentId = Convert.ToInt32(Console.ReadLine());
                    Equipment equipmentToDelete = Equipment.GetEquipment().FirstOrDefault(equipment => equipment.EquipmentId == equipmentId);
                    if (equipmentToDelete == null)
                    {
                        Console.WriteLine("The entered ID does not match any equipment.");
                    }
                    else
                    {
                        Console.WriteLine($"Are you sure you want to delete {equipmentToDelete.EquipmentName}?, Make sure you inform all trainers about this.");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        int deleteChoice = Convert.ToInt32(Console.ReadLine());
                        if (deleteChoice == 1)
                        {
                            equipmentToDelete.DeleteEquipment();
                            Console.WriteLine("Equipment deleted successfully.");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input given.");

                }
            }

            User AuthenticateUser()
            {
                UserDAL dal = new UserDAL();
                while (true)
                {
                    Console.WriteLine("ZuydFit");
                    Console.WriteLine("Select Option:");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Create Account");
                    Console.Write("> ");

                    string option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            User user = Login(dal);
                            if (user != null)
                            {
                                return user;
                            }
                            break;
                        case "2":
                            CreateAccount(dal);
                            break;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                    }
                }
            }

        }
    }
}


// Om users uit db te printen te testen.

//// Console.WriteLine("Welkom bij de ZuydFit Apllicatie");
//// Console.WriteLine("Kies een UserId om in te loggen");
//// int gebruikerKeuze = int.Parse(Console.ReadLine() ?? string.Empty);

//// Instantiate DAL to access user data
//DAL dal = new DAL();

//// Instantiate UserDAL to access user data
//DAL.UserDAL userDAL = new DAL.UserDAL();

//// Get all users from the database
//userDAL.GetUsers();

//Console.WriteLine (userDAL.users.Count());

//// Display user information
//Console.WriteLine("List of Users:");
//foreach (var user in userDAL.users)
//{
//    Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}, Email: {user.UserEmail}");
//}


/*
            // Inlog systeem begoonnen niet af
            bool loginSucceeded = false;
            while (!loginSucceeded)
            {
                try
                {
                    Console.WriteLine("If you already have a account, Enter your username");
                    Console.WriteLine("If you don't have a account, enter -1");
                    string inputUserName = Console.ReadLine() ?? string.Empty;
                    Console.Clear();
                    if (inputUserName != "-1")
                    {
                        Console.WriteLine("Enter your password: ");
                        string inputPassword = Console.ReadLine() ?? string.Empty;
                        Console.Clear();
                        User user = 
                    }
                    else
                    {
                        Console.WriteLine("Voer je niewew gebruikersnaam in: ");
                        string username = Console.ReadLine() ?? string.Empty;
                        //verwijzing naar dal nodig
                        Console.Clear();
                        Console.WriteLine("Kies een wachtwoord: ");
                        string password = Console.ReadLine() ?? string.Empty;
                        Console.Clear();
                        Console.WriteLine("Voer je email in: ");
                        string email = Console.ReadLine() ?? string.Empty;
                        Console.Clear();
                        int parttimeUserId = 0;
                        int keuzeRol = 0;
                        while (keuzeRol < 1 || keuzeRol > 3)
                        {
                            Console.WriteLine("Kies een rol: ");
                            Console.WriteLine("1. Sporter");
                            Console.WriteLine("2. Trainer");
                            Console.WriteLine("3. Eventorganiser");
                            keuzeRol = int.Parse(Console.ReadLine() ?? string.Empty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine($"Er is een foute invoerwaarde ingevoerd + {ex.Message}");
                }
            }
*/