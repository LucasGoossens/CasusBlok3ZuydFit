using System.Data.SqlClient;
using CasusZuydFitV0._1.ActivityClasses;
using CasusZuydFitV0._1.RemainingClasses;
using CasusZuydFitV0._1.UserClasses;
using static CasusZuydFitV0._1.DAL.DAL;
namespace CasusZuydFitV0._1.DAL
{
    public class DAL
    {
        //bij de dbConstring dient bij data source de naam van de server te worden ingevuld. De rest van de eigenschappen blijven hetzelfde.
        private static readonly string dbConString = "Data Source=LUCAS; Initial Catalog=ZuydFitFinal; Integrated Security=True; MultipleActiveResultSets=True";
        // Dal voor User klasse
        public class UserDAL
        {
            //lege lijst met users
            public List<User> users = new List<User>();

            // methode om user een specefieke op te halen uit de lijst
            public User GetUser(string username, string password)
            {
                return users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && u.UserPassword == password);
            }
            // De methode GetUser haalt alle gebruiker op en plaatst ze in de lijst users

            public void GetUsers()
            {
                users.Clear();
                try
                {
                    ActivityDAL activityDAL = new ActivityDAL();
                    activityDAL.GetActivities();
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        // op dit moment worden alle users opgehaald uit de database
                        string query = "Select * from [User]";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Users ophalen uit database
                                    int userId = reader.GetInt32(0);
                                    string userName = reader.GetString(1);
                                    string userEmail = reader.GetString(2);
                                    string userPassword = reader.GetString(3);
                                    int userType = reader.GetInt32(4);

                                    if (userType == 1) // User is sporter
                                    {
                                        Athlete user = new Athlete(userId, userName, userEmail, userPassword, new List<Activity>());
                                        users.Add(user);
                                    }
                                    else if (userType == 2) // User is trainer
                                    {
                                        Trainer user = new Trainer(userId, userName, userEmail, userPassword, new List<Activity>());
                                        users.Add(user);
                                    }
                                }
                            }
                        }
                        // activiteitenlijsten van users worden gevuld
                        query = "Select AthleteId, ActivityId from LogFeedback";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int athleteId = reader.GetInt32(0);
                                    int activityId = reader.GetInt32(1);
                                    Activity activity = activityDAL.activities.Find(x => x.ActivityId == activityId);
                                    Athlete athlete = users.Find(x => x.UserId == athleteId) as Athlete;
                                    athlete.ActivityList.Add(activity);
                                }
                            }
                            foreach (Activity activity in activityDAL.activities)
                            {
                                Trainer trainer = users.Find(x => x.UserId == activity.Trainer.UserId) as Trainer;
                                trainer.ActivityList.Add(activity);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }


            // Een nieuwe user wordt in de database geplaatst
            public void CreateNewUser(User user)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();

                        string query = "INSERT INTO [User](UserName, UserEmail, UserPassword, UserType) VALUES(@UserName, @UserEmail, @UserPassword, @UserType);";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {

                            dbCommand.Parameters.AddWithValue("@UserName", user.UserName);
                            dbCommand.Parameters.AddWithValue("@UserEmail", user.UserEmail);
                            dbCommand.Parameters.AddWithValue("@UserPassword", user.UserPassword);

                            if (user is Athlete)
                            {
                                dbCommand.Parameters.AddWithValue("@UserType", 1);
                            }
                            else if (user is Trainer)
                            {
                                dbCommand.Parameters.AddWithValue("@UserType", 2);
                            }


                            dbCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }

            }
            // Een user wordt verwijderd uit de database
            public void DeleteUser(User user)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "DELETE FROM [User] WHERE UserId = @UserId;";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {
                            dbCommand.Parameters.AddWithValue("@UserId", user.UserId);
                            dbCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het verwijderen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }
            // gegevens van een user worden bijgewerkt in de database
            public void UpdateUser(User user)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "UPDATE [User] SET UserName = @UserName, UserEmail = @UserEmail, UserPassword = @UserPassword WHERE UserId = @UserId;";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {
                            dbCommand.Parameters.AddWithValue("@UserId", user.UserId);
                            dbCommand.Parameters.AddWithValue("@UserName", user.UserName);
                            dbCommand.Parameters.AddWithValue("@UserEmail", user.UserEmail);
                            dbCommand.Parameters.AddWithValue("@UserPassword", user.UserPassword);

                            dbCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het bijwerken van de klanten in de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }

        }
        // dal voor de activity klasse
        public class ActivityDAL
        {
            // lege lijst met activiteiten
            public List<Activity> activities = new List<Activity>();
            // lijst met activiteiten wordt gevuld
            public void GetActivities()

            {
                activities.Clear();

                // trainers worden opgehaald
                TrainerDAL getTrainerDal = new TrainerDAL();
                getTrainerDal.GetTrainers();
                // atleten worden opgehaald
                AthleteDAL getAthleteDal = new AthleteDAL();
                getAthleteDal.GetAthlets();
                // equipment wordt opgehaald, met huidige implementatie niet nodig
                EquipmentDAL getEquipmentDal = new EquipmentDAL();
                getEquipmentDal.GetEquipment();
                // exercises worden opgehaald
                ExerciseDAL getExerciseDal = new ExerciseDAL();
                getExerciseDal.GetExercises();

                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        // alle activiteiten worden opgehaald.
                        string query = "Select * from [Activity]";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // standaard activiteiten worden opgehaald
                                while (reader.Read())
                                {
                                    int activityId = reader.GetInt32(0);
                                    string activityName = reader.GetString(1);
                                    int activityDuration = reader.GetInt32(2);
                                    string activityStartingTime = reader.GetString(3);
                                    string activityDescription = reader.GetString(4);

                                    int activityTrainerId = reader.GetInt32(5);
                                    // trainer wordt gekoppeld aan activiteit
                                    Trainer activityTrainer = getTrainerDal.trainers.Find(trainer => trainer.UserId == activityTrainerId);

                                    string activityType = reader.GetString(6);

                                    if (activityType == "event") // activiteit is een event
                                    {
                                        string eventLocation = reader.GetString(7);
                                        int eventParticipantsLimit = reader.GetInt32(8);
                                        // lijst met atleten van het event wordt gevuld
                                        List<Athlete> eventAthletes = new List<Athlete>();
                                        string activityQuery = $"Select AthleteId from LogFeedback where ActivityId = {activityId}";
                                        using (SqlCommand athleteCommand = new SqlCommand(activityQuery, connection))
                                        {
                                            using (SqlDataReader athleteReader = athleteCommand.ExecuteReader())
                                            {
                                                while (athleteReader.Read())
                                                {
                                                    int athleteId = athleteReader.GetInt32(0);
                                                    Athlete athlete = getAthleteDal.athletes.Find(a => a.UserId == athleteId);
                                                    if (athlete != null)
                                                    {
                                                        eventAthletes.Add(athlete);
                                                    }
                                                }
                                            }
                                            Event eventToAdd = new(activityId, activityName, activityDuration, activityStartingTime, activityTrainer, activityDescription, eventLocation, eventParticipantsLimit, eventAthletes);
                                            activities.Add(eventToAdd);
                                        }

                                    }
                                    else if (activityType == "workout") // activiteit is een workout
                                    {
                                        // atleet van de workout wordt gekoppeld
                                        string athleteQuery = $"Select AthleteId from LogFeedback where ActivityId = {activityId}";
                                        using (SqlCommand athleteCommand = new SqlCommand(athleteQuery, connection))
                                        {
                                            object executeScalarResult = athleteCommand.ExecuteScalar();
                                            int athleteId = Convert.ToInt32(executeScalarResult);
                                            Athlete athlete = getAthleteDal.athletes.Find(a => a.UserId == athleteId);
                                            if (athlete != null && !activities.Any(activity => activity.ActivityId == activityId)) 
                                                // controleert of activity nog niet bestaat
                                                // " !activities.Any(activity => activity.ActivityId == activityId)" 
                                                // dit moet waarschijnlijk overal waar activities/workouts opgehaald worden dmv LogFeedback query,
                                                // dit voorkomt dat je activities dubbel ophaalt.

                                            {
                                                Workout workoutToAdd = new Workout(activityId, activityName, activityDuration, activityStartingTime, activityTrainer, activityDescription, athlete);
                                                // exercises worden gekoppeld aan de workout
                                                foreach (Exercise exercise in getExerciseDal.Exercises)
                                                {
                                                    if (exercise.WorkoutId == activityId)
                                                    {
                                                        workoutToAdd.WorkoutExercises.Add(exercise);
                                                    }
                                                }

                                                activities.Add(workoutToAdd);

                                            }
                                        }

                                    }
                                }
                            }
                        }
                        // equipment wordt opgehaald en gekoppeld aan de juiste activities, met huidige implementatie niet nodig
                        string activityEquipmentQuery = "Select * from [ActivityEquipment]";
                        using (SqlCommand activityEquipmentCommand = new SqlCommand(activityEquipmentQuery, connection))
                        {
                            using (SqlDataReader reader = activityEquipmentCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int activityId = reader.GetInt32(1);
                                    int equipmentId = reader.GetInt32(2);
                                    Activity activity = activities.Find(a => a.ActivityId == activityId);
                                    Equipment equipment = getEquipmentDal.equipments.Find(e => e.EquipmentId == equipmentId);
                                    activity.Equipments.Add(equipment);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgetreden bij het ophalen van activiteiten uit de database. Neem contact op met de klantenservice: {ex.Message}");

                }
            }

        }
        // da; voor de atleet klasse
        public class AthleteDAL
        {
            // lege lijst met atleten
            public List<Athlete> athletes = new List<Athlete>();
            public void GetAthlets()
            //Atleten worden opgehaald maar lijsten worden nog niet gevuld
            {
                athletes.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "Select * from [User] Where UserType = 1";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Users ophalen uit database
                                    int userId = reader.GetInt32(0);
                                    string userName = reader.GetString(1);
                                    string userEmail = reader.GetString(2);
                                    string userPassword = reader.GetString(3);

                                    Athlete user = new Athlete(userId, userName, userEmail, userPassword, new List<Activity>());
                                    athletes.Add(user);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de gebruikers uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }


        }
        // klasse voor equipment
        public class EquipmentDAL
        {
            // lege lijst met equipment
            public List<Equipment> equipments = new List<Equipment>();
            // equipment wordt opgehaald
            public void GetEquipment()
            {
                equipments.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "Select * from Equipment";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Users ophalen uit database
                                    int equipmentId = reader.GetInt32(0);
                                    string equipmentName = reader.GetString(1);
                                    string equipmentDescription = reader.GetString(2);
                                    bool equipmentAvailability = reader.GetBoolean(3);

                                    Equipment equipment = new Equipment(equipmentId, equipmentName, equipmentDescription, equipmentAvailability);
                                    equipments.Add(equipment);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }
            // equipment wordt toegevoegd aan de database (nog niet geimplementeerd in program)
            public void CreateEquipment(Equipment equipment)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [Equipment](EquipmentName, EquipmentDescription, EquipmentAvailability) VALUES(@EquipmentName, @EquipmentDescription, @EquipmentAvailability); SELECT SCOPE_IDENTITY();";

                        SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName);
                        dbCommand.Parameters.AddWithValue("@EquipmentDescription", equipment.EquipmentDescription);
                        dbCommand.Parameters.AddWithValue("@EquipmentAvailability", equipment.EquipmentAvailability);

 
                        int insertedEquipmentId = Convert.ToInt32(dbCommand.ExecuteScalar());

                        // nieuwe equipmentId wordt geprint
                        Console.WriteLine($"New EquipmentId: {insertedEquipmentId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }

            // equipment wordt bijgewerkt in de database (nog niet geimplementeerd in program)
            public void UpdateEquipment(Equipment equipment)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "UPDATE [Equipment] SET EquipmentName = @EquipmentName, EquipmentDescription = @EquipmentDescription, EquipmentAvailability = @EquipmentAvailability WHERE EquipmentId = @EquipmentId;";

                        SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                        dbCommand.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName);
                        dbCommand.Parameters.AddWithValue("@EquipmentDescription", equipment.EquipmentDescription);
                        dbCommand.Parameters.AddWithValue("@EquipmentAvailability", equipment.EquipmentAvailability);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de informatie uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }
            // equipment wordt verwijderd uit de database (nog niet geimplementeerd in flow van program)
            public void DeleteEquipment(Equipment equipment)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "Delete [Equipment] WHERE EquipmentId = @EquipmentId;";

                        SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de informatie uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }
        }

        // klasse voor event
        public class EventDAL
        {
            // lege lijst met events
            public List<Event> events = new List<Event>(); // the type event is TYPE = 'event' in DB!
            // events worden opgehaald uit de database
            public void GetEvents()
            {
                // Clear existing events
                events.Clear();

                try
                {
                    // trainers worden opgehaald
                    TrainerDAL trainerDAL = new TrainerDAL();
                    trainerDAL.GetTrainers();
                    // equipment wordt opgehaald (niet nodig met huidige implementatie)
                    EquipmentDAL equipmentDAL = new EquipmentDAL();
                    equipmentDAL.GetEquipment();
                    // atleten worden opgehaald
                    AthleteDAL athleteDAL = new AthleteDAL();
                    athleteDAL.GetAthlets();


                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();

                        // events worden opgehaald uit de database
                        string query = "SELECT * FROM [Activity] WHERE Type = 'event';";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int activityId = reader.GetInt32(0);
                                    string activityName = reader.GetString(1);
                                    int activityDuration = reader.GetInt32(2);
                                    string activityStartingTime = reader.GetString(3);
                                    string activityDescription = reader.GetString(4);
                                    int activityTrainerId = reader.GetInt32(5);


                                    Trainer activityTrainer = trainerDAL.trainers.Find(trainer => trainer.UserId == activityTrainerId);

                                    string eventLocation = reader.GetString(7);
                                    int eventParticipantsLimit = reader.GetInt32(8);

                                    List<Athlete> eventAthletes = new List<Athlete>();
                                    // adding athletes to the event
                                    string activityQuery = $"Select AthleteId from LogFeedback where ActivityId = {activityId}";
                                    using (SqlCommand athleteCommand = new SqlCommand(activityQuery, connection))
                                    {
                                        using (SqlDataReader athleteReader = athleteCommand.ExecuteReader())
                                        {

                                            while (athleteReader.Read())
                                            {
                                                int athleteId = athleteReader.GetInt32(0);

                                                Athlete athlete = athleteDAL.athletes.Find(a => a.UserId == athleteId);
                                                if (athlete != null)
                                                {
                                                    eventAthletes.Add(athlete);
                                                }
                                            }
                                        }

                                        Event eventToAdd = new Event(activityId, activityName, activityDuration, activityStartingTime, activityTrainer, activityDescription, eventLocation, eventParticipantsLimit, eventAthletes);
                                        events.Add(eventToAdd);
                                    }
                                }
                            }
                        }

                        // equipment wordt opgehaald en gekoppeld aan de juiste events, met huidige implementatie niet nodig
                        string activityEquipmentQuery = "Select * from [ActivityEquipment]";
                        using (SqlCommand activityEquipmentCommand = new SqlCommand(activityEquipmentQuery, connection))
                        {
                            using (SqlDataReader reader = activityEquipmentCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int activityId = reader.GetInt32(1);
                                    int equipmentId = reader.GetInt32(2);
                                    Event eventToEdit = events.Find(a => a.ActivityId == activityId);
                                    Equipment equipment = equipmentDAL.equipments.Find(e => e.EquipmentId == equipmentId);
                                    eventToEdit.Equipments.Add(equipment);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving events from the database. Please contact customer service: {ex.Message}");
                }
            }

            // event wordt toegevoegd aan de database
            public void CreateEvent(Event newEvent)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();

                        // event wordt toegevoegd aan de Activity tabel in de database
                        string insertQuery = "INSERT INTO [Activity] (ActivityName, ActivityDuration, ActivityStartingTime, ActivityDescription, TrainerId, EventLocation, EventParticipantLimit, Type) " +
                                            "VALUES (@Name, @Duration, @StartingTime, @Description, @TrainerId, @Location, @ParticipantsLimit, @Type); " +
                                            "SELECT SCOPE_IDENTITY();";

                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {

                            command.Parameters.AddWithValue("@Name", newEvent.ActivityName);
                            command.Parameters.AddWithValue("@Duration", newEvent.ActivityDurationMinutes);
                            command.Parameters.AddWithValue("@StartingTime", newEvent.ActivityStartingTime);
                            command.Parameters.AddWithValue("@Description", newEvent.ActivityDescription);
                            command.Parameters.AddWithValue("@TrainerId", newEvent.Trainer.UserId);
                            command.Parameters.AddWithValue("@Location", newEvent.EventLocation);
                            command.Parameters.AddWithValue("@ParticipantsLimit", newEvent.EventPatricipantLimit);
                            command.Parameters.AddWithValue("@Type", "event");

                            int newEventId = Convert.ToInt32(command.ExecuteScalar());



                            // equipment wordt toegevoegd aan het event
                            foreach (Equipment equipment in newEvent.Equipments)
                            {
                                string insertEquipmentQuery = "INSERT INTO ActivityEquipment (ActivityId, EquipmentId) " +
                                                            "VALUES (@ActivityId, @EquipmentId);";

                                using (SqlCommand equipmentCommand = new SqlCommand(insertEquipmentQuery, connection))
                                {

                                    equipmentCommand.Parameters.AddWithValue("@ActivityId", newEventId);
                                    equipmentCommand.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);

                                    equipmentCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while creating the event in the database. Please contact customer service: {ex.Message}");
                }
            }

            // event wordt bijgewerkt in de database
            public void UpdateEvent(Event updatedEvent)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();

                        string updateQuery = "UPDATE [Activity] SET ActivityName = @ActivityName, ActivityDuration = @ActivityDuration, ActivityStartingTime = @ActivityStartingTime, " +
                                            "ActivityDescription = @ActivityDescription, EventLocation = @EventLocation, " +
                                            "EventParticipantLimit = @EventParticipantLimit WHERE ActivityId = @ActivityId;";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ActivityId", updatedEvent.ActivityId);
                            command.Parameters.AddWithValue("@ActivityName", updatedEvent.ActivityName);
                            command.Parameters.AddWithValue("@ActivityDuration", updatedEvent.ActivityDurationMinutes);
                            command.Parameters.AddWithValue("@ActivityStartingTime", updatedEvent.ActivityStartingTime);
                            command.Parameters.AddWithValue("@ActivityDescription", updatedEvent.ActivityDescription);
                            command.Parameters.AddWithValue("@EventLocation", updatedEvent.EventLocation);
                            command.Parameters.AddWithValue("@EventParticipantLimit", updatedEvent.EventPatricipantLimit);                            


                            command.ExecuteNonQuery();


                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while updating the event in the database. Please contact customer service: {ex.Message}");
                }
            }


            // event wordt verwijderd uit de database
            public void DeleteEvent(int eventId)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM [Activity] WHERE ActivityId = @ActivityId;";

                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ActivityId", eventId);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the event from the database. Please contact customer service: {ex.Message}");
                }
            }


        }

        // exercise klasse
        public class ExerciseDAL
        {
            // lege lijst met exercises
            public List<Exercise> Exercises = new List<Exercise>();

            public void GetExercises()
            {
                Exercises.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        // alle exercises worden opgehaald uit database
                        string query = "SELECT * FROM [Exercise];";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int exerciseId = reader.GetInt32(0);
                                    string exerciseName = reader.GetString(1);
                                    int workoutId = reader.GetInt32(2);
                                    string excersiceResult = reader.GetString(3);
                                    string exerciseDescription = reader.GetString(4);


                                    Exercise exercise = new Exercise(exerciseId, exerciseName, excersiceResult, exerciseDescription, workoutId);
                                    Exercises.Add(exercise);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }

            // specifieke exercises worden opgehaald uit de database door middel van workoutId
            public List<Exercise> GetAllExercisesInWorkoutWithId(int workoutId)
            {
                List<Exercise> Exercises = new List<Exercise>();
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = $"SELECT * FROM [Exercise] WHERE workoutId = {workoutId}";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int exerciseId = reader.GetInt32(0);
                                    string exerciseName = reader.GetString(1);
                                    //int workoutId = reader.GetInt32(2); niet nodig, heb je al
                                    string excersiceResult = reader.GetString(3);
                                    string exerciseDescription = reader.GetString(4);

                                    Exercise exercise = new Exercise(exerciseId, exerciseName, excersiceResult, exerciseDescription, workoutId);
                                    Exercises.Add(exercise);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while creating a new exercise in the database. Please contact Customer Service: {ex.Message}");
                }

                return Exercises;

            }

            //nieuwe exercise wordt toegevoegd aan de database
            public void CreateNewExercise(Exercise exercise)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [Exercise](ExerciseName, ExerciseDescription, ExerciseResult, WorkoutId) VALUES(@ExerciseName, @ExerciseDescription, @ExerciseResult, @WorkoutId);";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {
                            dbCommand.Parameters.AddWithValue("@ExerciseName", exercise.ExerciseName);
                            dbCommand.Parameters.AddWithValue("@ExerciseDescription", exercise.ExerciseDescription);
                            dbCommand.Parameters.AddWithValue("@ExerciseResult", exercise.ExerciseResult);
                            dbCommand.Parameters.AddWithValue("@WorkoutId", exercise.WorkoutId);

                            dbCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while creating a new exercise in the database. Please contact Customer Service: {ex.Message}");
                }
            }
            // exercise wordt bijgewerkt in de database, nog niet geimplementeerd in program en klasse
            public void UpdateExercise(Exercise exercise)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "UPDATE [Exercise] SET ExerciseName = @ExerciseName, ExerciseDescription = @ExerciseDescription, ExerciseResult = @ExerciseResult WHERE ExerciseId = @ExerciseId;";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {

                            dbCommand.Parameters.AddWithValue("@ExerciseId", exercise.ExerciseId);
                            dbCommand.Parameters.AddWithValue("@ExerciseName", exercise.ExerciseName);
                            dbCommand.Parameters.AddWithValue("@ExerciseDescription", exercise.ExerciseDescription);
                            dbCommand.Parameters.AddWithValue("@ExerciseResult", exercise.ExerciseResult);

                            dbCommand.ExecuteNonQuery();
                        };


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while updating the exercise in the database. Please contact customer service. Error: {ex.Message}");
                }
            }
            // exercise wordt verwijderd uit de database, nog niet geimplementeerd in program en klasse
            public void DeleteExercise(int exerciseId)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "DELETE FROM [Exercise] WHERE ExerciseId = @ExerciseId;";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {

                            dbCommand.Parameters.AddWithValue("@ExerciseId", exerciseId);

                            dbCommand.ExecuteNonQuery();
                        }


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the exercise from the database. Please contact customer service. Error: {ex.Message}");
                }
            }

        }
        // dal voor workout klasse
        public class WorkoutDAL
        {
            // lege lijst met workouts
            public List<Workout> workouts = new List<Workout>();
            // workouts worden opgehaald uit de database
            public void GetWorkouts()
            {
                workouts.Clear();
                // trainers worden opgehaald
                TrainerDAL getTrainerDal = new TrainerDAL();
                getTrainerDal.GetTrainers();
                // atleten worden opgehaald
                AthleteDAL getAthleteDal = new AthleteDAL();
                getAthleteDal.GetAthlets();
                // equipment wordt opgehaald, met huidige implementatie niet nodig
                EquipmentDAL getEquipmentDal = new EquipmentDAL();
                getEquipmentDal.GetEquipment();
                // exercises worden opgehaald
                ExerciseDAL getExerciseDal = new ExerciseDAL();
                getExerciseDal.GetExercises();

                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        // workouts worden opgehaald
                        string query = "Select * from [Activity] where Type = 'workout'";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int activityId = reader.GetInt32(0);
                                    string activityName = reader.GetString(1);
                                    int activityDuration = reader.GetInt32(2);
                                    string activityStartingTime = reader.GetString(3);
                                    string activityDescription = reader.GetString(4);

                                    int activityTrainerId = reader.GetInt32(5);
                                    Trainer activityTrainer = getTrainerDal.trainers.Find(trainer => trainer.UserId == activityTrainerId);


                                    // atleet van de workout wordt gekoppeld

                                    string athleteQuery = $"Select AthleteId from LogFeedback where ActivityId = {activityId}";
                                    using (SqlCommand athleteCommand = new SqlCommand(athleteQuery, connection))
                                    {
                                        object executeScalarResult = athleteCommand.ExecuteScalar();
                                        int athleteId = Convert.ToInt32(executeScalarResult);
                                        Athlete athlete = getAthleteDal.athletes.Find(a => a.UserId == athleteId);
                                        if (athlete != null && !workouts.Any(activity => activity.ActivityId == activityId))
                                        {
                                            Workout workoutToAdd = new Workout(activityId, activityName, activityDuration, activityStartingTime, activityTrainer, activityDescription, athlete);
                                            workouts.Add(workoutToAdd); 

                                            foreach (Exercise exercise in getExerciseDal.Exercises)
                                            {
                                                if (exercise.WorkoutId == activityId)
                                                {
                                                    workoutToAdd.WorkoutExercises.Add(exercise);
                                                }
                                            }
                                        }
                                    }


                                }
                            }
                        }
                        // equipment wordt opgehaald en gekoppeld aan de juiste workouts, met huidige implementatie niet nodig
                        string activityEquipmentQuery = "Select * from [ActivityEquipment]";
                        using (SqlCommand activityEquipmentCommand = new SqlCommand(activityEquipmentQuery, connection))
                        {
                            using (SqlDataReader reader = activityEquipmentCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int activityId = reader.GetInt32(1);
                                    int equipmentId = reader.GetInt32(2);
                                    Activity activity = workouts.Find(a => a.ActivityId == activityId);
                                    Equipment equipment = getEquipmentDal.equipments.Find(e => e.EquipmentId == equipmentId);
                                    activity.Equipments.Add(equipment);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgetreden bij het ophalen van activiteiten uit de database. Neem contact op met de klantenservice: {ex.Message}");

                }


            }


            // workout wordt toegevoegd aan de database
            public int CreateNewWorkout(Workout workout)
            {
                int insertedId = -1;

                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [Activity](ActivityName, ActivityDuration, ActivityStartingTime, ActivityDescription, TrainerId, Type) VALUES(@ActivityName, @ActivityDuration, @ActivityStartingTime, @ActivityDescription, @TrainerId, @Type); SELECT SCOPE_IDENTITY();";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {
                            dbCommand.Parameters.AddWithValue("@ActivityName", workout.ActivityName);
                            dbCommand.Parameters.AddWithValue("@ActivityDuration", workout.ActivityDurationMinutes);
                            dbCommand.Parameters.AddWithValue("@ActivityStartingTime", workout.ActivityStartingTime);
                            dbCommand.Parameters.AddWithValue("@ActivityDescription", workout.ActivityDescription);
                            dbCommand.Parameters.AddWithValue("@TrainerId", workout.Trainer.UserId);
                            dbCommand.Parameters.AddWithValue("@Type", "workout");

                            object result = dbCommand.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                insertedId = Convert.ToInt32(result);
                            }
                            else
                            {
                                // Log error if the result is null
                                Console.WriteLine("Failed to get inserted ID from the database.");
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Log SQL exceptions
                    Console.WriteLine($"SQL Exception occurred: {sqlEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while saving the workout to the database. Please contact Customer Service: {ex.Message}");
                }

                return insertedId;
            }
            
            // workout van specifieke atleet wordt opgehaald uit de database
            public List<Workout> GetAllWorkoutsByAthleteId(int athleteId)
            {
                TrainerDAL allTrainers = new TrainerDAL();
                allTrainers.GetTrainers();

                List<Workout> WorkoutsToReturn = new List<Workout>();

                string query = $"SELECT Activity.ActivityId, ActivityName, ActivityDuration, ActivityStartingTime, ActivityDescription, Activity.TrainerId FROM LogFeedback INNER JOIN Activity ON LogFeedback.ActivityId = Activity.ActivityId WHERE Activity.Type = 'workout' AND LogFeedback.AthleteId = {athleteId}";
                using (SqlConnection connection = new SqlConnection(dbConString))
                {
                    connection.Open();
                    using (SqlCommand athleteCommand = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = athleteCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int activityId = reader.GetInt32(0);
                                if (WorkoutsToReturn.Any(activity => activity.ActivityId == activityId)){
                                    continue;
                                }
                                string activityName = reader.GetString(1);
                                int activityDuration = reader.GetInt32(2);
                                string activityStartingTime = reader.GetString(3);
                                string activityDescription = reader.GetString(4);
                                Trainer activityTrainer = allTrainers.trainers.FirstOrDefault(trainer => trainer.UserId == reader.GetInt32(5));
                                // worden geen Equipments meegegeven, niet nodig voor gebruik

                                Workout newWorkout = new Workout(activityId, activityName, activityDuration, activityStartingTime, activityTrainer, activityDescription);

                                WorkoutsToReturn.Add(newWorkout);
                            }
                        }
                    }
                }
                return WorkoutsToReturn;
            }
        }


        // klasse voor logfeedback
        public class LogFeedbackDAL
        {
            public List<LogFeedback> logFeedbacks = new List<LogFeedback>();

            public void GetLogFeedback()
            {
                logFeedbacks.Clear();
                try
                {
                    // users worden opgehaald
                    UserDAL userDal = new UserDAL();
                    userDal.GetUsers();
                    // activities worden opgehaald
                    ActivityDAL activityDal = new ActivityDAL();
                    activityDal.GetActivities();
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        // alle logfeedback wordt opgehaald
                        string query = "Select * from [LogFeedback]";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader.IsDBNull(5)) // Controleert of de FeedbackDate in SQL null is, de entries met geen Date dienen alleen als link van de workout-templates
                                    {
                                        continue;
                                    }
                                    // Users ophalen uit database
                                    int logFeedbackId = reader.GetInt32(0);
                                    int trainerId = reader.GetInt32(1);
                                    int athleteId = reader.GetInt32(2);
                                    int activityId = reader.GetInt32(3);
                                    string feedbackInfo = reader.GetString(4);
                                    string feedbackDate = reader.GetString(5); // deze is null bij alle initialLogs die aangemaakt worden bij het eerst aanmaken van een workout

                                    LogFeedback feedback = new LogFeedback(logFeedbackId, trainerId, athleteId, activityId, feedbackInfo, feedbackDate);
                                    logFeedbacks.Add(feedback);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.StackTrace}");
                }
            }

            /// <summary> CreateInitialLogFeedback 
            /// Deze is omdat LogFeedback in de database gebruikt wordt als link tussen User en Activity,
            /// bij het aanmaken van een nieuwe Workout "template" wordt geen datum meegegeven en geen FeedbackInfo
            /// </summary>

            // logfeedback wordt aangemaakt in de database voor workout "template"
            public void CreateInitialLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [LogFeedback](TrainerId, AthleteId, ActivityId) VALUES(@TrainerId, @AthleteId, @ActivityId);";

                        using SqlCommand dbCommand = new SqlCommand(query, connection);
                        dbCommand.Parameters.AddWithValue("@TrainerId", feedback.FeedbackTrainerId);
                        dbCommand.Parameters.AddWithValue("@AthleteId", feedback.FeedbackAthleteId);
                        dbCommand.Parameters.AddWithValue("@ActivityId", feedback.FeedbackActivityId);
                        
                        
                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het aanmaken van logbook in de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }

            // logfeedback wordt aangemaakt in de database
            public void CreateLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [LogFeedback](TrainerId, AthleteId, ActivityId, FeedbackInfo, ActivityDate) VALUES(@TrainerId, @AthleteId, @ActivityId, @FeedbackInfo, @ActivityDate);";

                        using SqlCommand dbCommand = new SqlCommand(query, connection);
                        dbCommand.Parameters.AddWithValue("@TrainerId", feedback.FeedbackTrainerId);
                        dbCommand.Parameters.AddWithValue("@AthleteId", feedback.FeedbackAthleteId);
                        dbCommand.Parameters.AddWithValue("@ActivityId", feedback.FeedbackActivityId);
                        dbCommand.Parameters.AddWithValue("@FeedbackInfo", feedback.FeedbackInfo);
                        dbCommand.Parameters.AddWithValue("@ActivityDate", feedback.ActivityDate);
                            // hier een soort string van maken die te printen is
                        // hier niet feedbackDate vgm
                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het aanmaken van logbook in de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }

            // logfeedback wordt bijgewerkt in de database
            public void UpdateLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        // controleer of het in database van iedereen "FeedbackId" heet en niet "LogFeedbackId"
                        string query = "UPDATE [LogFeedback] SET FeedbackInfo = FeedbackInfo + @FeedbackInfo WHERE FeedbackId = @FeedbackId;";

                        using SqlCommand dbCommand = new SqlCommand(query, connection);
                        // volgens mij hoeft hier niet alle parameters opnieuw geupdate worden, wordt door beide functionaliteiten deze method gebruiken
                        // alleen FeedbackInfo geupdate. 
                        //dbCommand.Parameters.AddWithValue("@TrainerId", feedback.FeedbackTrainerId);
                        //dbCommand.Parameters.AddWithValue("@AthleteId", feedback.FeedbackAthleteId);
                        //dbCommand.Parameters.AddWithValue("@ActivityId", feedback.FeedbackActivityId);
                        dbCommand.Parameters.AddWithValue("@FeedbackInfo", feedback.FeedbackInfo);
                        dbCommand.Parameters.AddWithValue("@FeedbackId", feedback.FeedbackId);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgetreden bij het bijwerken van de feedback in de database. Neem contact op met de klantenservice. Foutmelding: {ex.Message}");
                }
            }
            // logfeedback wordt verwijderd uit de database
            public void DeleteLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "DELETE FROM [LogFeedback] WHERE FeedbackId = @LogFeedbackId;";

                        using SqlCommand dbCommand = new SqlCommand(query, connection);
                        dbCommand.Parameters.AddWithValue("@LogFeedbackId", feedback.FeedbackId);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgetreden bij het verwijderen van de feedback uit de database. Neem contact op met de klantenservice. Foutmelding: {ex.Message}");
                }
            }

        }
        // klasse voor trainer
        public class TrainerDAL
        {
            // lege lijst met trainers
            public List<Trainer> trainers = new List<Trainer>();
            // trainers worden opgehaald uit de database zonder gevulde lijst
            public void GetTrainers()
            {
                trainers.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(dbConString))
                    {
                        connection.Open();
                        string query = "SELECT * FROM [User] WHERE UserType = 2";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int userId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                    string userName = reader.IsDBNull(1) ? "Unknown" : reader.GetString(1);
                                    string userEmail = reader.IsDBNull(2) ? "No Email" : reader.GetString(2);
                                    string userPassword = reader.IsDBNull(3) ? "No Password" : reader.GetString(3);

                                    Trainer trainer = new Trainer(userId, userName, userEmail, userPassword, new List<Activity>());
                                    trainers.Add(trainer);
                                }
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Log SQL errors for further analysis.
                    Console.WriteLine($"Er is een probleem met de Database. Error: {sqlEx.Message}");
                }
                catch (Exception ex)
                {
                    // General error handling
                    Console.WriteLine($"Er is een onverwacht probleem met het ophalen van gegevens uit de Database. Error: {ex.Message}");
                }
            }
        }

    }
}

