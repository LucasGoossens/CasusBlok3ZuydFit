using CasusZuydFitV0._1.ActivityClasses;
using CasusZuydFitV0._1.RemainingClasses;
using CasusZuydFitV0._1.UserClasses;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using static CasusZuydFitV0._1.DAL.DAL;

namespace CasusZuydFitV0._1.Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // inlogsysteem
            User loggedInUser = new Athlete("NOUSERLOGGEDIN", "NOUSERLOGGEDIN", "NOUSERLOGGEDIN");
            bool loggedIn = false;
            while (!loggedIn)
            {
                Console.WriteLine("        ZUYDFIT        ");
                Console.WriteLine("=======================");
                Console.WriteLine("Select an Option:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Create an Account");
                Console.Write(">");

                string option = Console.ReadLine();
                Console.Clear();
                switch (option)
                {
                    case "1":
                        loggedIn = Login();
                        break;
                    case "2":
                        CreateNewUser();
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
            // einde inlogsysteem

            bool running = true;
            while (running)
            {
                // hoofdmenu
                Console.WriteLine("        ZUYDFIT        ");
                Console.WriteLine("=======================");
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. View Profile");
                Console.WriteLine("2. View All Activities");
                if (loggedInUser is Trainer)  // check type user
                {
                    Console.WriteLine("3. View All Athletes (Trainers only)");
                }
                Console.WriteLine("Enter your option (or 'exit' to close): ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ManageProfile(loggedInUser);
                        break;
                    case "2":
                        Console.Clear();
                        DisplayAllActivities(loggedInUser);
                        break;
                    case "3":
                        if (loggedInUser is Trainer)
                        {
                            DisplayAllAthletes();  // alleen voor trainers
                        }
                        else
                        {
                            Console.WriteLine("Access denied.");
                        }
                        break;
                    case "exit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }          
            // atleten worden weergegeven
            void DisplayAllAthletes()
            {
                Console.Clear();
                                
                List<Athlete> allAthletes = Athlete.GetAllAthletes();
                Console.WriteLine($"Total number of Athletes: {allAthletes.Count()}");
                Console.WriteLine("-----------------------");                

                Console.WriteLine("List of Athletes:");
                Console.WriteLine("-----------------------");
                foreach (var athlete in allAthletes)
                {
                    if (athlete is Athlete)
                    {
                        Console.WriteLine($"Athlete ID: {athlete.UserId}, Name: {athlete.UserName}, Email: {athlete.UserEmail}");
                        Console.WriteLine("-----------------------");
                    }
                }

                Console.WriteLine("Enter the name or ID of the athlete to search:");
                string searchedAthlete = Console.ReadLine();
                Console.Clear();

                Athlete foundAthlete = null;
                try
                {
                    foreach (var athlete in allAthletes)
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
                        Console.Clear();
                        Console.WriteLine("Athlete found:");
                        Console.WriteLine($"Athlete ID: {foundAthlete.UserId}, Name: {foundAthlete.UserName}, Email: {foundAthlete.UserEmail}");
                        Console.WriteLine("-----------------------");
                        int optionChoice;
                        do
                        {
                            Console.WriteLine("1. View all workouts for this athlete");
                            Console.WriteLine("2. Create a new workout for this athlete");
                            optionChoice = Convert.ToInt32(Console.ReadLine());
                            if (optionChoice != 1 && optionChoice != 2)
                            {
                                Console.WriteLine("Invalid input.");
                            }
                        } while (optionChoice != 1 && optionChoice != 2);

                        switch (optionChoice)
                        {
                            case 1:
                                DisplayFoundAthleteWorkouts(foundAthlete);
                                break;
                            case 2:
                                CreateNewWorkout(foundAthlete);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Athlete not found.");
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Error: Input cannot be empty.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Invalid input. Please enter a valid number.");
                }
            }
            // user aanmaken inlogsysteem
            void CreateNewUser()
            {
                Console.Clear();
                Console.WriteLine("Enter username:");
                string UserName = Console.ReadLine();
                Console.WriteLine("Enter user email:");
                string UserEmail = Console.ReadLine();
                string password = "1";
                string confirmedPassword = "2";

                while (password != confirmedPassword)
                {
                    Console.Write("Enter new password: ");
                    password = Console.ReadLine();
                    Console.Write("Confirm password: ");
                    confirmedPassword = Console.ReadLine();
                    if (password != confirmedPassword)
                    {
                        Console.WriteLine("Invalid password confirmation. Try again");
                    }
                }

                int UserType = 0;

                do
                {
                    Console.WriteLine("Enter the user type:");
                    Console.WriteLine("1. Athlete");
                    Console.WriteLine("2. Trainer");
                    UserType = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    if (UserType == 2)
                    {
                        // Hardcoded password validation for creating a trainer (123)
                        string ValidationPassword = "123";
                        string InputValidationPassword = "1";

                        try
                        {
                            while (ValidationPassword != InputValidationPassword)
                            {
                                Console.WriteLine("Enter the validation password: ");
                                InputValidationPassword = Console.ReadLine();

                                if (ValidationPassword != InputValidationPassword)
                                {
                                    Console.WriteLine("Invalid validation password. Try again.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while verifying the validation password: " + ex.Message);
                        }
                    }

                } while (UserType < 1 || UserType > 2);
                Console.Clear();
                User user;

                switch (UserType)
                {
                    case 1:
                        user = new Athlete(UserName, UserEmail, password, new List<Activity>());
                        user.CreateNewUser();
                        break;
                    case 2:
                        user = new Trainer(UserName, UserEmail, password, new List<Activity>());
                        user.CreateNewUser();
                        break;
                }

                Console.WriteLine("New user " + UserName + " created successfully.");
            }
         
            // nieuwe workout aanmaken voor een atleet
            void CreateNewWorkout(Athlete newWorkOutAthlete)
            {
                Console.Clear();

                Console.WriteLine("      New Workout");
                Console.WriteLine("-----------------------");

                try
                {
                    Console.WriteLine("New workout name:");
                    string newWorkoutName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newWorkoutName))
                    {
                        throw new ArgumentException("The workout name cannot be empty.");
                    }

                    Console.WriteLine("Duration of the workout in minutes:");
                    int newWorkoutDuration = Convert.ToInt32(Console.ReadLine());

                    if (newWorkoutDuration <= 0)
                    {
                        throw new ArgumentException("The duration of the workout must be a positive number.");
                    }

                    Console.WriteLine("Start time of the workout:");
                    string newWorkoutStartingTime = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newWorkoutStartingTime))
                    {
                        throw new ArgumentException("The start time of the workout cannot be empty.");
                    }

                    Console.WriteLine("Description of the workout:");
                    string newWorkoutDescription = Console.ReadLine();
                    Console.Clear();

                    Trainer newWorkOutTrainer = (Trainer)loggedInUser;

                    Workout newWorkout = new Workout(newWorkoutName, newWorkoutDuration, newWorkoutStartingTime, newWorkOutTrainer, newWorkoutDescription, newWorkOutAthlete);
                    newWorkout.CreateNewWorkout();
                    int workoutIdToAddToExercise = newWorkout.ActivityId;

                    LogFeedback newLogFeedback = new LogFeedback(newWorkOutTrainer.UserId, newWorkOutAthlete.UserId, newWorkout.ActivityId);
                    newLogFeedback.CreateInitialLog();

                    int addExerciseOption = 1;
                    while (addExerciseOption == 1)
                    {
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("1. Add a New Exercise to Workout");
                        Console.WriteLine("2. Save Workout");

                        addExerciseOption = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        switch (addExerciseOption)
                        {
                            case 1:
                                Console.WriteLine("-----------------------");
                                Exercise testExercise = Exercise.CreateNewExercise(workoutIdToAddToExercise); // This is a Program.method
                                testExercise.CreateExercise();              // This is an object.method
                                break;
                            case 2:
                                addExerciseOption = 2;
                                break;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Invalid input for workout duration.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while processing the workout: " + ex.Message);
                }
            }

            // alle workouts van een atleet worden weergegeven
            void DisplayFoundAthleteWorkouts(Athlete foundAthlete)
            {
                try
                {
                    List<Workout> allWorkoutsFromFoundAthlete = foundAthlete.GetAllWorkouts();

                    if (allWorkoutsFromFoundAthlete.Count < 1)
                    {
                        Console.WriteLine("No workouts registered.");
                        return;
                    }
                    Console.Clear();
                    Console.WriteLine("Workouts of " + foundAthlete.UserName + ":");
                    foreach (Workout workout in allWorkoutsFromFoundAthlete)
                    {
                        Console.WriteLine("-----------------------");
                        Console.WriteLine($"Workout ID: {workout.ActivityId}, Workout Name: {workout.ActivityName}");
                    }
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Select Workout ID");

                    int selectedWorkoutId;
                    if (!int.TryParse(Console.ReadLine(), out selectedWorkoutId))
                    {
                        Console.WriteLine("Invalid input for workout ID. Please enter a valid number.");
                        return;
                    }

                    Workout workoutToAddFeedback = allWorkoutsFromFoundAthlete.Find(workout => workout.ActivityId == selectedWorkoutId);
                    if (workoutToAddFeedback == null)
                    {
                        Console.WriteLine("No workout found with the specified ID.");
                        return;
                    }

                    TrainerGivesFeedbackOnWorkout(foundAthlete, workoutToAddFeedback, selectedWorkoutId);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while displaying workouts: " + ex.Message);
                }
            }

            // acount van de gebruiker beheren
            void ManageProfile(User user)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine($"Profile:");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine($"Name: {user.UserName}");
                    Console.WriteLine($"Email: {user.UserEmail}");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("1. Edit Profile");
                    Console.WriteLine("2. Delete Profile");
                    Console.WriteLine("3. Main Menu");
                    int profileChoice;
                    if (!int.TryParse(Console.ReadLine(), out profileChoice) || profileChoice < 1 || profileChoice > 3)
                    {
                        Console.WriteLine("Invalid choice. Restart and enter a valid option.");
                        return;
                    }
                    Console.Clear();
                    switch (profileChoice)
                    {
                        case 1:
                            Console.WriteLine("Enter a new username:");
                            string newUserName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newUserName))
                            {
                                throw new ArgumentException("Username cannot be empty.");
                            }

                            Console.Clear();
                            while (User.GetUsers().Any(existingUser => existingUser.UserName == newUserName && newUserName != user.UserName))
                            {
                                Console.WriteLine("This username is already in use, please enter a new one.");
                                newUserName = Console.ReadLine() ?? string.Empty;
                                Console.Clear();
                            }

                            Console.WriteLine("Enter a new email address:");
                            string newUserEmail = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newUserEmail))
                            {
                                throw new ArgumentException("Email address cannot be empty.");
                            }

                            Console.WriteLine("Enter a new password:");
                            string newUserPassword = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newUserPassword))
                            {
                                throw new ArgumentException("Password cannot be empty.");
                            }

                            Console.Clear();
                            user.UpdateUser(newUserName, newUserEmail, newUserPassword);
                            Console.WriteLine("Profile updated.");
                            break;
                        case 2:
                            Console.WriteLine("Are you sure you want to delete your profile?");
                            Console.WriteLine("1. Yes");
                            Console.WriteLine("2. No");
                            int deleteProfileChoice;
                            if (!int.TryParse(Console.ReadLine(), out deleteProfileChoice) || (deleteProfileChoice != 1 && deleteProfileChoice != 2))
                            {
                                Console.WriteLine("Invalid choice. Restart and enter a valid option.");
                                return;
                            }
                            Console.Clear();
                            if (deleteProfileChoice == 1)
                            {
                                user.DeleteUser();
                                Console.WriteLine("Profile deleted.");
                                Console.ReadLine();
                                Environment.Exit(0);
                            }
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while managing the profile: " + ex.Message);
                }
            }

            // feedback geven op een workout door een trainer op een atleet
            void TrainerGivesFeedbackOnWorkout(User user, Workout activityToAddFeedback, int idReceiver)
            {
                List<LogFeedback> workoutLogFeedback = new List<LogFeedback>();
                try
                {
                    Console.Clear();
                    Activity activityToGiveFeedbackOn = activityToAddFeedback;
                    
                    if (activityToGiveFeedbackOn == null)
                    {
                        Console.WriteLine("A non-existing workout was given");
                        return;
                    }
                    else if (activityToGiveFeedbackOn.Trainer.UserId == loggedInUser.UserId)
                    {
                        Workout WorkoutToGiveFeedbackOn = activityToGiveFeedbackOn as Workout;                     

                        List<LogFeedback> workoutLogFeedbackToFilter = LogFeedback.GetFeedback();

                        foreach(LogFeedback log in workoutLogFeedbackToFilter)
                        {
                            if (log.FeedbackActivityId == activityToGiveFeedbackOn.ActivityId)
                            {
                                workoutLogFeedback.Add(log as LogFeedback);
                            }
                        }
                        
                        if(workoutLogFeedback.Count < 1)
                        {
                            Console.WriteLine("This Athlete has not logged this workout yet.");
                            return;
                        }
                        Console.WriteLine("Choose workout date to give feedback on.");

                        for (int i = 0; i < workoutLogFeedback.Count(); i++)
                        {
                            Console.WriteLine($"{i}: " + workoutLogFeedback[i].ActivityDate);
                        }
                        
                        try
                        {
                            int workoutDateOption = Convert.ToInt32(Console.ReadLine());                           
                            Console.WriteLine("Workout date: " + workoutLogFeedback[workoutDateOption].ActivityDate);
                            Console.WriteLine("Results: " + workoutLogFeedback[workoutDateOption].FeedbackInfo);
                            Console.WriteLine("Enter feedback on this workout: ");
                            string FeedbackUpdate = $"\n\nFeedback from {activityToGiveFeedbackOn.Trainer.UserName}: " + Console.ReadLine();
                            Console.Clear();
                            workoutLogFeedback[workoutDateOption].UpdateFeedback(FeedbackUpdate);
                            
                        }
                        catch
                        {
                            Console.WriteLine("Invalid input given. 1");
                        }

                    }
                    else
                    {
                        Console.WriteLine("You are not the trainer of this activity.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input given. 2");
                }
            }

            //  feedback geven op een event door een trainer op een atleet
            void TrainerGivesFeedbackOnEvent(User user, Event activityToAddFeedback)
            {
                LogFeedback logFeedback = null;
                try
                {
                    Console.Clear();
                    Activity activityToGiveFeedbackOn = activityToAddFeedback;
                    if (activityToGiveFeedbackOn == null)
                    {
                        Console.WriteLine("A non-existing event was given");
                        return;
                    }
                    else if (activityToGiveFeedbackOn.Trainer.UserId == loggedInUser.UserId)
                    {

                        Console.WriteLine("All users that are signed up for this event: ");
                        foreach (Athlete athlete in activityToAddFeedback.EventParticipants)
                        {
                            Console.WriteLine($"User ID: {athlete.UserId}, Name: {athlete.UserName}");
                        }
                        Console.WriteLine("Pick a UserID for the user you want to give feedback on");
                        int pickedUserId = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        User? userToGiveFeedbackOn = User.GetUsers().FirstOrDefault(user => user.UserId == pickedUserId);
                        logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == activityToAddFeedback.ActivityId && feedback.FeedbackAthleteId == pickedUserId);


                        
                        Console.WriteLine($"Activity Name: {activityToGiveFeedbackOn.ActivityName}, Athlete: {userToGiveFeedbackOn.UserName}" + (logFeedback != null ? $", Current Feedback: {logFeedback.FeedbackInfo}" : "."));

                        Console.WriteLine("Enter the feedback you want to give: ");
                        string NewFeedback = Console.ReadLine();
                        Console.Clear();
                        logFeedback.UpdateFeedback(NewFeedback);
                        Console.WriteLine("Feedback updated. press enter to go back to the main menu");
                        Console.ReadLine();
                        Console.Clear();


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

            // event aanmaken voor een trainer
            void CreateEvent(User user)
            {
                try
                {
                    Console.WriteLine("Enter event details:");

                    // Gather event details from the user
                    Console.Write("Event name: ");
                    string eventName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(eventName))
                    {
                        throw new ArgumentException("Event name cannot be empty.");
                    }

                    Console.Write("Duration (minutes): ");
                    if (!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)
                    {
                        throw new ArgumentException("Duration must be a positive integer.");
                    }

                    Console.Write("Start time (YYYY-MM-DD HH:MM): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startingTime))
                    {
                        throw new ArgumentException("Invalid date format. Use YYYY-MM-DD HH:MM.");
                    }

                    Console.Write("Description: ");
                    string description = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(description))
                    {
                        throw new ArgumentException("Description cannot be empty.");
                    }

                    Console.Write("Location: ");
                    string location = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        throw new ArgumentException("Location cannot be empty.");
                    }

                    Console.Write("Participant limit: ");
                    if (!int.TryParse(Console.ReadLine(), out int participantLimit) || participantLimit <= 0)
                    {
                        throw new ArgumentException("Participant limit must be a positive integer.");
                    }
                    // Methods gerelateerd aan Equipments zijn functioneel, maar niet geimplementeerd.
                    List<Equipment> equipments = GatherEquipments();

                    // Create Event object
                    Event newEvent = new Event(eventName, duration, startingTime.ToString("MM/dd/yyyy HH:mm:ss"), new Trainer { UserId = user.UserId }, description, equipments, location, participantLimit);

                    newEvent.AddEvent();
                    Console.Clear();
                    Console.WriteLine("Event successfully created!");

                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while creating the event: " + ex.Message);
                }
            }
            // Methods gerelateerd aan Equipments zijn functioneel, maar niet geimplementeerd.
            static List<Equipment> GatherEquipments()
            {
                try
                {
                    Console.WriteLine("Enter equipment details:");
                    Console.Write("Number of equipments (Equipments not yet realized in application, enter 0): ");
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
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while gathering equipment details: " + ex.Message);
                    return new List<Equipment>();
                }
            }
            // login systeem functie
            bool Login()
            {
                try
                {
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(username))
                    {
                        throw new ArgumentException("Username cannot be empty.");
                    }

                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        throw new ArgumentException("Password cannot be empty.");
                    }

                    (string, string) loginInfo = (username, password);

                    User user = User.GetUsers().Find(user => (user.UserName, user.UserPassword) == loginInfo);

                    if (user != null)
                    {
                        Console.Clear();
                        Console.WriteLine("Login successful!");
                        loggedInUser = user;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password.");
                        return false;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while logging in: " + ex.Message);
                    return false;
                }
            }

            // alle activity mogelijkheden weergeven
            void DisplayAllActivities(User user)
            {
                try
                {
                    Console.WriteLine("Which activities would you like to see?");
                    Console.WriteLine("1: Events (Group Activity)");
                    Console.WriteLine("2: Workouts (Individual Activity)");                    
                    if (user is Trainer)
                    {
                        Console.WriteLine("3: Create New Event");
                    }
                    else if (user is Athlete)
                    {
                        Console.WriteLine("3: Register for Event");
                        Console.WriteLine("4: Unregister from Event");
                    }

                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            if (user is Athlete)
                            {
                                Event.DisplayAllEventsUser(loggedInUser);
                            }
                            else
                            {
                                Event.ShowEventsFromTrainer(loggedInUser);
                                Console.WriteLine("Enter Event ID to view details: ");
                                Console.WriteLine("To return enter 0."); 
                                int IdEventToreceivefeedback = Convert.ToInt32(Console.ReadLine());
                                Console.Clear();
                                if (IdEventToreceivefeedback != 0)
                                {
                                    Console.WriteLine("1. Edit event.");
                                    Console.WriteLine("2. Give feedback. ");
                                    string editOption = Console.ReadLine();
                                    Event eventToAddFeedback = Event.GetEvents().Find(eventItem => eventItem.ActivityId == IdEventToreceivefeedback);
                                    if (editOption == "1")
                                    {
                                        eventToAddFeedback.EditEvent();
                                    }
                                    else if (editOption == "2")
                                    {                                        
                                        TrainerGivesFeedbackOnEvent(loggedInUser, eventToAddFeedback);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid option.");
                                        return;
                                    }
                                }
                            }
                            break;
                        case "2":
                            
                            if (user is Trainer)
                            {
                                Console.WriteLine("Workouts can be found in the View All Athletes menu.");
                                break;
                            }
                            Workout.DisplayAllWorkouts(loggedInUser.UserId);
                            break;
                        case "3":
                            if (user is Trainer)
                            {
                                CreateEvent(user);
                            }
                            else if (user is Athlete)
                            {
                                ShowEvents();
                                RegisterForEvent(user);
                            }
                            break;
                        case "4":
                            if (user is Athlete)
                            {
                                Console.Clear();
                                RemoveRegistration(user);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice.");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while displaying activities: " + ex.Message);
                }
            }

            // alle events weergeven
            void ShowEvents()
            {
                foreach (Event events in Event.GetEvents())
                {
                    events.ShowEvent();
                    Console.WriteLine("-----------------------");
                }
            }
            // athleet registreren voor een event
            void RegisterForEvent(User user)
            {
                try
                {
                    Console.WriteLine("Enter the ID of the event you want to register for: ");
                    int registerID;
                    while (!int.TryParse(Console.ReadLine(), out registerID))
                    {
                        Console.WriteLine("Invalid ID. Enter a whole number.");
                        Console.WriteLine("Enter the ID of the event you want to register for: ");
                    }

                    Event? eventToRegister = Event.GetEvents().FirstOrDefault(registerEvent => registerEvent.ActivityId == registerID);
                    Console.Clear();
                    if (eventToRegister == null)
                    {
                        Console.WriteLine("The entered ID does not match any event. Press enter to return to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else if (eventToRegister.EventParticipants.Exists(a => a.UserId == user.UserId))
                    {
                        Console.WriteLine("You are already registered for this event. Press enter to return to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else if (eventToRegister.EventParticipants.Count < eventToRegister.EventPatricipantLimit)
                    {
                        string emptyString = "";
                        LogFeedback newLogFeedback = new LogFeedback(eventToRegister.Trainer.UserId, user.UserId, eventToRegister.ActivityId, emptyString, emptyString);
                        newLogFeedback.CreateLog();
                        Athlete athlete = (Athlete)user;
                        athlete.AddActivity(eventToRegister);
                        Console.WriteLine("You are registered for the event. Press enter to return to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("The event is full, you cannot register. Press enter to return to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while registering for the event: " + ex.Message);
                }
            }

            // Methods gerelateerd aan Equipments zijn functioneel, maar niet geimplementeerd.
            void DeleteEquipment()
            {
                try
                {
                    Console.WriteLine("Enter the ID of the equipment you want to delete: ");
                    int equipmentId;
                    while (!int.TryParse(Console.ReadLine(), out equipmentId))
                    {
                        Console.WriteLine("Invalid ID. Enter a whole number.");
                        Console.WriteLine("Enter the ID of the equipment you want to delete: ");
                    }

                    Equipment equipmentToDelete = Equipment.GetEquipment().FirstOrDefault(equipment => equipment.EquipmentId == equipmentId);
                    if (equipmentToDelete == null)
                    {
                        Console.WriteLine("The entered ID does not match any equipment.");
                    }
                    else
                    {
                        Console.WriteLine($"Are you sure you want to delete {equipmentToDelete.EquipmentName}? Make sure to inform all trainers about this.");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        int deleteChoice;
                        while (!int.TryParse(Console.ReadLine(), out deleteChoice) || (deleteChoice != 1 && deleteChoice != 2))
                        {
                            Console.WriteLine("Invalid choice. Enter 1 or 2.");
                            Console.WriteLine("1. Yes");
                            Console.WriteLine("2. No");
                        }
                        if (deleteChoice == 1)
                        {
                            equipmentToDelete.DeleteEquipment();
                            Console.WriteLine("Equipment successfully deleted.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while deleting the equipment: " + ex.Message);
                }
            }
            // athleet verwijderen van een event
            void RemoveRegistration(User user)
            {
                try
                {
                    Athlete athlete = (Athlete)user;
                    bool foundEvent = false;
                    foreach (Activity activity in athlete.ActivityList)
                    {
                        if (activity is Event)
                        {
                            Event events = (Event)activity;
                            Console.WriteLine($"Event ID: {events.ActivityId}");
                            Console.WriteLine($"Event Name: {events.ActivityName}");
                            Console.WriteLine($"Event Location: {events.EventLocation}");
                            Console.WriteLine($"Event Duration: {events.ActivityDurationMinutes}");
                            Console.WriteLine($"Event Starting Time: {events.ActivityStartingTime}");
                            Console.WriteLine($"Event Description: {events.ActivityDescription}");
                            Console.WriteLine();
                            foundEvent = true;
                        }
                    }

                    if (!foundEvent)
                    {
                        Console.WriteLine("You are not registered for any event.");
                        return;
                    }

                    Console.WriteLine("Enter the ID of the event you want to unregister from: ");
                    int unregisterID;
                    while (!int.TryParse(Console.ReadLine(), out unregisterID))
                    {
                        Console.WriteLine("Invalid ID. Enter a whole number.");
                        Console.WriteLine("Enter the ID of the event you want to unregister from: ");
                    }
                    Console.Clear();

                    Event? eventToUnregister = Event.GetEvents().FirstOrDefault(unregisterEvent => unregisterEvent.ActivityId == unregisterID);
                    if (eventToUnregister == null)
                    {
                        Console.WriteLine("The entered ID does not match any event. Press enter to return to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        LogFeedback logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == eventToUnregister.ActivityId && feedback.FeedbackAthleteId == user.UserId);
                        if (logFeedback == null)
                        {
                            Console.WriteLine("You are not registered for this event. Press enter to return to the menu");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            logFeedback.DeleteFeedback();
                            Console.WriteLine("You are unregistered from the event. Press enter to return to the menu");
                            
                            athlete.RemoveActivity(unregisterID);
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while unregistering from the event: " + ex.Message);
                }
            }
        }
    }
}

