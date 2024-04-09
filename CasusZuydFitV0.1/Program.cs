using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using static CasusZuydFitV0._1.DAL;

namespace CasusZuydFitV0._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=======================");
                Console.WriteLine("Kies optie:");
                Console.WriteLine("-----------------------");
                Console.WriteLine("1: Display all users");
                Console.WriteLine("2: Create new user");
                Console.WriteLine("-----------------------");
                Console.WriteLine("3. Create new Exercise");
                Console.WriteLine("4. Display all Exercises");
                Console.WriteLine("5. Create new Workout");


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
                        CreateNewExercise();
                        break;
                    case 4:
                        DisplayAllExercises();
                        break;
                    case 5:
                        //CreateNewWorkout();
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

            void CreateNewExercise()
            {
                Console.Clear();
                Console.WriteLine("Enter Exercise Name:");
                string ExerciseName = Console.ReadLine();
                Console.WriteLine("Enter Exercise description:");
                string ExerciseDescription = Console.ReadLine();

                Exercise testExercise = new Exercise(ExerciseName, "exerciseresult", ExerciseDescription);
                testExercise.CreateExercise();
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
                    Console.WriteLine("Als je een terugkerende gebruiker bent, voer je username in.");
                    Console.WriteLine("Als je een nieuwe gebruiker bent, voer -1 in");
                    string inputUserName = Console.ReadLine() ?? string.Empty;
                    Console.Clear();
                    if (inputUserName != "-1")
                    {
                        Console.WriteLine("Voer je wachtwoord in: ");
                        string inputPassword = Console.ReadLine() ?? string.Empty;
                        Console.Clear();
                        // Verwijzing naar class/dal nodig
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