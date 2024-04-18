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

            User loggedInUser = new Athlete("NOUSERLOGGEDIN", "NOUSERLOGGEDIN", "NOUSERLOGGEDIN");
            bool loggedIn = false;
            while (!loggedIn)
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
                        ManageProfile(loggedInUser);
                        break;
                    case "2":
                        DisplayAllActivities();
                        break;
                    case "3":
                        //DisplayAllUsers();
                        DisplayAllAthletes();
                        break;
                    case "exit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }


            void DisplayAllUsers()
            {
                Console.Clear();

                List<User> allUsers = User.GetUsers();
                Console.WriteLine("Total Users: ");
                Console.WriteLine(allUsers.Count());


                Console.WriteLine("List of Users:");
                foreach (var user in allUsers)
                {
                    Console.WriteLine($"User ID: {user.UserId}, Name: {user.UserName}, Email: {user.UserEmail}");
                }

            }

            void DisplayAllAthletes()
            {
                Console.Clear();

                List<User> allUsers = User.GetUsers();
                Console.WriteLine("Totaal aantal gebruikers: ");
                Console.WriteLine(allUsers.Count());

                Console.WriteLine("Lijst met atleten:");
                foreach (var athlete in allUsers)
                {
                    if (athlete is Athlete)
                    {
                        Console.WriteLine($"Atleet ID: {athlete.UserId}, Naam: {athlete.UserName}, E-mail: {athlete.UserEmail}");
                    }
                }

                Console.WriteLine("Voer de naam of ID van de atleet in om te zoeken:");
                string searchedAthlete = Console.ReadLine();

                Athlete foundAthlete = null;
                try
                {
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
                        Console.WriteLine("Atleet gevonden:");
                        Console.WriteLine($"Atleet ID: {foundAthlete.UserId}, Naam: {foundAthlete.UserName}, E-mail: {foundAthlete.UserEmail}");
                        int optionChoice;
                        do
                        {
                            Console.WriteLine("1. Bekijk alle trainingen voor deze atleet");
                            Console.WriteLine("2. Maak een nieuwe training voor deze atleet");
                            optionChoice = Convert.ToInt32(Console.ReadLine());
                            if (optionChoice != 1 && optionChoice != 2)
                            {
                                Console.WriteLine("Ongeldige invoer.");
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
                        Console.WriteLine("Atleet niet gevonden.");
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Fout: De invoer kan niet leeg zijn.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Fout: Ongeldige invoer. Voer alstublieft een geldig nummer in.");
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
                Console.WriteLine("Voer gebruikersnaam in:");
                string UserName = Console.ReadLine();
                Console.WriteLine("Voer gebruikerse-mail in:");
                string UserEmail = Console.ReadLine();
                string password = "1";
                string confirmedPassword = "2";

                while (password != confirmedPassword)
                {
                    Console.Write("Voer nieuw wachtwoord in: ");
                    password = Console.ReadLine();
                    Console.Write("Bevestig wachtwoord: ");
                    confirmedPassword = Console.ReadLine();
                    if (password != confirmedPassword)
                    {
                        Console.WriteLine("Ongeldige wachtwoordbevestiging. Probeer opnieuw");
                    }
                }

                int UserType = 0;

                // Uiteindelijk zijn er andere schermen voor verschillende gebruikers aanmaken,
                // dit is om te testen
                do
                {
                    Console.WriteLine("Voer het gebruikerstype in:");
                    Console.WriteLine("1. Atleet");
                    Console.WriteLine("2. Trainer");
                    UserType = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    if (UserType == 2)
                    {
                        // Hardcoded validatie wachtwoord voor het maken van een trainer
                        string ValidationPassword = "123";
                        string InputValidationPassword = "1";

                        try
                        {
                            while (ValidationPassword != InputValidationPassword)
                            {
                                Console.WriteLine("Voer het validatiewachtwoord in: ");
                                InputValidationPassword = Console.ReadLine();

                                if (ValidationPassword != InputValidationPassword)
                                {
                                    Console.WriteLine("Ongeldig validatiewachtwoord. Probeer opnieuw.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Er is een fout opgetreden bij het verifiëren van het validatiewachtwoord: " + ex.Message);
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

                Console.WriteLine("Nieuwe gebruiker " + UserName + " succesvol aangemaakt.");
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

            void CreateNewWorkout(Athlete newWorkOutAthlete)
            {
                Console.Clear();

                Console.WriteLine("      Nieuwe training");
                Console.WriteLine("-----------------------");

                try
                {
                    Console.WriteLine("Nieuwe training naam:");
                    string newWorkoutName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newWorkoutName))
                    {
                        throw new ArgumentException("De trainingsnaam mag niet leeg zijn.");
                    }

                    Console.WriteLine("Duur van de training in minuten:");
                    int newWorkoutDuration = Convert.ToInt32(Console.ReadLine());

                    if (newWorkoutDuration <= 0)
                    {
                        throw new ArgumentException("De duur van de training moet een positief getal zijn.");
                    }

                    Console.WriteLine("Starttijd van de training:");
                    string newWorkoutStartingTime = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newWorkoutStartingTime))
                    {
                        throw new ArgumentException("De starttijd van de training mag niet leeg zijn.");
                    }

                    Console.WriteLine("Beschrijving van de training:");
                    string newWorkoutDescription = Console.ReadLine();

                    Trainer newWorkOutTrainer = (Trainer)loggedInUser;

                    Workout newWorkout = new Workout(newWorkoutName, newWorkoutDuration, newWorkoutStartingTime, newWorkOutTrainer, newWorkoutDescription, newWorkOutAthlete);
                    newWorkout.CreateNewWorkout();
                    int workoutIdToAddToExercise = newWorkout.ActivityId;

                    LogFeedback newLogFeedback = new LogFeedback(newWorkOutTrainer.UserId, newWorkOutAthlete.UserId, newWorkout.ActivityId);
                    newLogFeedback.CreateLog();

                    int addExerciseOption = 1;
                    while (addExerciseOption == 1)
                    {
                        Console.WriteLine("-----------------------");
                        Console.WriteLine("1. Nieuwe Oefening toevoegen aan Training");
                        Console.WriteLine("2. Training opslaan");

                        addExerciseOption = Convert.ToInt32(Console.ReadLine());

                        switch (addExerciseOption)
                        {
                            case 1:
                                Console.WriteLine("-----------------------");
                                Exercise testExercise = Exercise.CreateNewExercise(workoutIdToAddToExercise); // dit is een Program.method
                                testExercise.CreateExercise();              // dit is een object.method
                                break;
                            case 2:
                                addExerciseOption = 2;
                                break;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Fout: " + ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Fout: Ongeldige invoer voor duur van de training.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Er is een fout opgetreden bij het verwerken van de training: " + ex.Message);
                }
            }



            void DisplayAllWorkouts()
            {
                try
                {
                    Athlete currentAthlete = Athlete.GetAllAthletes().Find(athlete => athlete.UserId == loggedInUser.UserId);

                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Alle Trainingen:\n");
                    Console.WriteLine("-----------------------");
                    int workoutNumber = 0;
                    foreach (Workout workout in Workout.GetWorkouts())
                    {
                        workoutNumber++;
                        if (currentAthlete.ActivityList.Contains(workout))
                        {
                            Console.WriteLine($"{workoutNumber}. Naam training: {workout.ActivityName}");
                            Console.WriteLine($"   Duur (minuten): {workout.ActivityDurationMinutes}");
                            Console.WriteLine($"   Trainer: {workout.Trainer.UserName}");
                            Console.WriteLine($"   Beschrijving: {workout.ActivityDescription}");
                            Console.WriteLine("---------------------------------------------------------");
                        }
                    }

                    Console.WriteLine("\nVoer het nummer van de training in om de details en oefeningen te bekijken:");
                    if (!int.TryParse(Console.ReadLine(), out int selectedNumber) || selectedNumber < 1 || selectedNumber > Workout.GetWorkouts().Count)
                    {
                        Console.WriteLine("Ongeldige selectie. Start opnieuw en voer een geldig trainingsnummer in.");
                        return;
                    }

                    selectedNumber--;
                    Workout selectedWorkout = Workout.GetWorkouts()[selectedNumber];

                    Console.WriteLine($"\nGeselecteerde training: {selectedWorkout.ActivityName}");
                    Console.WriteLine($"Duur (minuten): {selectedWorkout.ActivityDurationMinutes}");
                    Console.WriteLine($"Starttijd: {selectedWorkout.ActivityStartingTime}");
                    Console.WriteLine($"Trainer: {selectedWorkout.Trainer.UserName}");
                    Console.WriteLine($"Beschrijving: {selectedWorkout.ActivityDescription}");

                    Console.WriteLine("\nOefeningen:");
                    if (selectedWorkout.WorkoutExercises != null && selectedWorkout.WorkoutExercises.Count > 0)
                    {
                        foreach (Exercise exercise in selectedWorkout.WorkoutExercises)
                        {
                            Console.WriteLine($"- Oefening Naam: {exercise.ExerciseName}");
                            Console.WriteLine($"  Beschrijving: {exercise.ExerciseDescription}");
                            Console.WriteLine($"  Resultaat: {exercise.ExerciseResult}");
                        }
                        LogFeedback.CheckFeedback(currentAthlete, selectedWorkout);
                    }
                    else
                    {
                        Console.WriteLine("Deze training heeft geen oefeningen vermeld.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Er is een fout opgetreden bij het weergeven van de trainingen: " + ex.Message);
                }
            }


            void DisplayFoundAthleteWorkouts(Athlete foundAthlete)
            {
                try
                {
                    List<Workout> allWorkoutsFromFoundAthlete = foundAthlete.GetAllWorkouts();

                    if (allWorkoutsFromFoundAthlete.Count < 1)
                    {
                        Console.WriteLine("Geen trainingen geregistreerd.");
                        return;
                    }

                    Console.WriteLine("Trainingen van " + foundAthlete.UserName + ":");
                    foreach (Workout workout in allWorkoutsFromFoundAthlete)
                    {
                        Console.WriteLine("-----------------------");
                        Console.WriteLine($"Training ID: {workout.ActivityId} Training naam: {workout.ActivityName}");
                    }
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Selecteer Training ID");

                    int selectedWorkoutId;
                    if (!int.TryParse(Console.ReadLine(), out selectedWorkoutId))
                    {
                        Console.WriteLine("Ongeldige invoer voor training ID. Voer alstublieft een geldig nummer in.");
                        return;
                    }

                    Workout workoutToAddFeedback = allWorkoutsFromFoundAthlete.Find(workout => workout.ActivityId == selectedWorkoutId);
                    if (workoutToAddFeedback == null)
                    {
                        Console.WriteLine("Geen training gevonden met het opgegeven ID.");
                        return;
                    }

                    TrainerGivesFeedback(foundAthlete, workoutToAddFeedback, selectedWorkoutId);
                    // Of ga verder met het maken van het menu
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Er is een fout opgetreden bij het weergeven van de trainingen: " + ex.Message);
                }
            }


            void ManageProfile(User user)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine($"Profiel:");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine($"Naam: {user.UserName}");
                    Console.WriteLine($"E-mail: {user.UserEmail}");
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("1. Profiel bewerken");
                    Console.WriteLine("2. Profiel verwijderen");
                    Console.WriteLine("3. Hoofdmenu");
                    int profileChoice;
                    if (!int.TryParse(Console.ReadLine(), out profileChoice) || profileChoice < 1 || profileChoice > 3)
                    {
                        Console.WriteLine("Ongeldige keuze. Start opnieuw en voer een geldige optie in.");
                        return;
                    }
                    Console.Clear();
                    switch (profileChoice)
                    {
                        case 1:
                            Console.WriteLine("Voer een nieuwe gebruikersnaam in:");
                            string newUserName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newUserName))
                            {
                                throw new ArgumentException("Gebruikersnaam mag niet leeg zijn.");
                            }

                            Console.Clear();
                            while (User.GetUsers().Any(existingUser => existingUser.UserName == newUserName && newUserName != user.UserName))
                            {
                                Console.WriteLine("Deze gebruikersnaam is al in gebruik, voer een nieuwe in.");
                                newUserName = Console.ReadLine() ?? string.Empty;
                                Console.Clear();
                            }

                            Console.WriteLine("Voer een nieuwe e-mailadres in:");
                            string newUserEmail = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newUserEmail))
                            {
                                throw new ArgumentException("E-mailadres mag niet leeg zijn.");
                            }

                            Console.WriteLine("Voer een nieuw wachtwoord in:");
                            string newUserPassword = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newUserPassword))
                            {
                                throw new ArgumentException("Wachtwoord mag niet leeg zijn.");
                            }

                            Console.Clear();
                            user.UpdateUser(newUserName, newUserEmail, newUserPassword);
                            Console.WriteLine("Profiel bijgewerkt.");
                            Console.ReadLine();
                            Environment.Exit(0);
                            break;
                        case 2:
                            Console.WriteLine("Weet je zeker dat je je profiel wilt verwijderen?");
                            Console.WriteLine("1. Ja");
                            Console.WriteLine("2. Nee");
                            int deleteProfileChoice;
                            if (!int.TryParse(Console.ReadLine(), out deleteProfileChoice) || (deleteProfileChoice != 1 && deleteProfileChoice != 2))
                            {
                                Console.WriteLine("Ongeldige keuze. Start opnieuw en voer een geldige optie in.");
                                return;
                            }
                            Console.Clear();
                            if (deleteProfileChoice == 1)
                            {
                                user.DeleteUser();
                                Console.WriteLine("Profiel verwijderd.");
                                Console.ReadLine();
                                Environment.Exit(0);
                            }
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Fout: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Er is een fout opgetreden bij het beheren van het profiel: " + ex.Message);
                }
            }



            void TrainerGivesFeedback(User user, Workout activityToAddFeedback, int idReceiver)
            {
                LogFeedback logFeedback = null;
                try
                {
                    //Console.WriteLine($"All activities where {user.UserName} is the trainer: ");
                    //foreach (Activity activity in Activity.GetActivities())
                    //{
                    //    if (activity.Trainer.UserId == user.UserId)
                    //    {
                    //        Console.WriteLine($"Activity ID: {activity.ActivityId}, Name: {activity.ActivityName}");
                    //    }

                    //Console.WriteLine("Enter the ID of the activity you want to give feedback on: ");
                    //int activityId = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Activity activityToGiveFeedbackOn = activityToAddFeedback;
                    if (activityToGiveFeedbackOn == null)
                    {
                        Console.WriteLine("A non-existing workout was given"); // dit hoeft niet 
                        return;
                    }
                    else if (activityToGiveFeedbackOn.Trainer.UserId == loggedInUser.UserId)
                    {
                        if (activityToGiveFeedbackOn is Event) // werkt nu alleen nog voor workout 
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
                            logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == eventToGiveFeedback.ActivityId && feedback.FeedbackAthleteId == pickedUserId);
                        }
                        else
                        {
                            Workout WorkoutToGiveFeedbackOn = activityToGiveFeedbackOn as Workout;
                            logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == WorkoutToGiveFeedbackOn.ActivityId && feedback.FeedbackAthleteId == idReceiver);
                        }
                        Console.Clear();
                        Console.WriteLine($"Activity Name: {activityToGiveFeedbackOn.ActivityName}" + (logFeedback != null ? $" Current Feedback: {logFeedback.FeedbackInfo}" : "."));

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




           

            bool Login()
            {


                Console.Write("Enter username: ");
                string username = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                (string, string) loginInfo = (username, password);

                User user = User.GetUsers().Find(user => (user.UserName, user.UserPassword) == loginInfo);

                if (user != null)
                {
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

            void CreateAccount()
            {
                Console.Write("Enter new username: ");
                string username = Console.ReadLine();
                Console.Write("Enter new email: ");
                string email = Console.ReadLine();
                string password = "1";
                string confirmedPassword = "2";

                while (password != confirmedPassword)
                {
                    Console.Write("Enter new password: ");
                    password = Console.ReadLine();
                    Console.Write("confirm password: ");
                    confirmedPassword = Console.ReadLine();
                    if (password != confirmedPassword)
                    {
                        Console.WriteLine("Invalid password confirmation. Try again");
                    }
                }

                Athlete newAthlete = new Athlete(username, email, password);
                newAthlete.CreateNewUser();

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
                        Event.DisplayAllEvents(loggedInUser);
                        break;
                    case "2":
                        Workout.DisplayAllWorkouts(loggedInUser.UserId);
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please enter 1 or 2.");
                        break;
                }
            }
            void ShowEvents()
            {
                foreach (Event events in Event.GetEvents())
                {
                    events.ShowEvent();
                }
            }


            void RegisterForEvent(User user)
            {

                Console.WriteLine("Enter the ID of the event you want to register for: ");
                int registerID = int.Parse(Console.ReadLine());
                Console.Clear();
                Event? eventToRegister = Event.GetEvents().FirstOrDefault(registerEvent => registerEvent.ActivityId == registerID);
                if (eventToRegister == null)
                {
                    Console.WriteLine("The entered ID does not match any event. press enter to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();
                }

                else if (eventToRegister.EventParticipants.Exists(a => a.UserId == user.UserId))
                {
                    Console.WriteLine("You are already registered for this event. press enter to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (eventToRegister.EventParticipants.Count < eventToRegister.EventPatricipantLimit)
                {
                    LogFeedback newLogFeedback = new LogFeedback(eventToRegister.Trainer.UserId, user.UserId, eventToRegister
                        .ActivityId);
                    newLogFeedback.CreateLog();
                    Console.Clear();
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

            void RemoveRegistration(User user)
            {
                Athlete athlete = (Athlete)user;
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
                    }
                }

                Console.WriteLine("Enter the ID of the event you want to unregister for: ");
                int unregisterID = int.Parse(Console.ReadLine());
                Console.Clear();
                Event? eventToUnregister = Event.GetEvents().FirstOrDefault(unregisterEvent => unregisterEvent.ActivityId == unregisterID);
                if (eventToUnregister == null)
                {
                    Console.WriteLine("The entered ID does not match any event. press enter to go back to the menu");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    LogFeedback logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == eventToUnregister.ActivityId && feedback.FeedbackAthleteId == user.UserId);
                    if (logFeedback == null)
                    {
                        Console.WriteLine("You are not registered for this event. press enter to go back to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    else
                    {
                        logFeedback.DeleteFeedback();
                        Console.WriteLine("You have been unregistered for the event. press enter to go back to the menu");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }



        }
    }
}

