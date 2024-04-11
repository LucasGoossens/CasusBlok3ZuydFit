using System.Data;
using System.Data.SqlClient;
using Microsoft.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CasusZuydFitV0._1
{
    public class DAL
    {
        private static readonly string dbConString = "Server=tcp:gabriellunesu.database.windows.net,1433;Initial Catalog=ZuydFitFinal;Persist Security Info=False;User ID=gabriellunesu;Password=3KmaCBt5nU4qZ4s%xG5@;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public class UserDAL
        {
            public List<User> users = new List<User>();
            public void GetUsers()
            {
                users.Clear();
                try
                {
                    ActivityDAL activityDAL = new ActivityDAL();
                    activityDAL.GetActivities();
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
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
                        // users bestaan maar lijsten zijn nog leeg
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

            // nu gebruiken we UserDAL om alle soorten Users aan te maken in SQL,
            // mogelijk dit dan opsplitsen zodat alle subclass-specific dingen apart worden uitgevoerd bij het aanmaken
            public void CreateNewUser(User user)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
            public void DeleteUser(User user)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
            public void UpdateUser(User user)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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

        public class ActivityDAL
        {
            public List<Activity> activities = new List<Activity>();
            public void GetActivities()

            {
                activities.Clear();

                TrainerDAL getTrainerDal = new TrainerDAL();
                getTrainerDal.GetTrainers();

                AthleteDAL getAthleteDal = new AthleteDAL();
                getAthleteDal.GetAthlets();

                EquipmentDAL getEquipmentDal = new EquipmentDAL();
                getEquipmentDal.GetEquipment();

                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "Select * from [Activity]";
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
                                    Trainer activityTrainer = (Trainer)getTrainerDal.trainers.Find(trainer => trainer.UserId == activityTrainerId);

                                    string activityType = reader.GetString(6);

                                    if (activityType == "event")
                                    {
                                        string eventLocation = reader.GetString(7);
                                        int eventParticipantsLimit = Convert.ToInt32(reader.GetString(8));

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
                                    else if (activityType == "workout")
                                    {
                                        string athleteQuery = $"Select AthleteId from LogFeedback where ActivityId = {activityId}";
                                        using (SqlCommand athleteCommand = new SqlCommand(athleteQuery, connection))
                                        {
                                            object executeScalarResult = athleteCommand.ExecuteScalar();
                                            int athleteId = Convert.ToInt32(executeScalarResult);
                                            Athlete athlete = getAthleteDal.athletes.Find(a => a.UserId == athleteId);
                                            if (athlete != null)
                                            {                                                
                                                Workout workoutToAdd = new Workout(activityId, activityName, activityDuration, activityStartingTime, activityTrainer, activityDescription, athlete);
                                                activities.Add(workoutToAdd);
                                            }
                                        }
                                    }
                                }
                            }
                        }
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

        public class AthleteDAL
        {
            public List<Athlete> athletes = new List<Athlete>();
            public void GetAthlets()
            //User wordt opgehaald maar lijsten worden nog niet gevuld
            {
                athletes.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }


        }

        public class EquipmentDAL
        {
            public List<Equipment> equipments = new List<Equipment>();
            public void GetEquipment()
            {
                equipments.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
            public void CreateEquipment(Equipment equipment)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [Equipment](EquipmentName, EquipmentDescription, EquipmentAvailability) VALUES(@EquipmentName, @EquipmentDescription, @EquipmentAvailability);";

                        SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName);
                        dbCommand.Parameters.AddWithValue("@EquipmentDescription", equipment.EquipmentDescription);
                        dbCommand.Parameters.AddWithValue("@EquipmentAvailability", equipment.EquipmentAvailability);


                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }


            public void UpdateEquipment(Equipment equipment)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }

            public void DeleteEquipment(Equipment equipment)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }
        }

        public class EventDAL
        {
            public List<Event> events = new List<Event>(); // the type event is TYPE = 'event' in DB!

            public void GetEvents()
            {
                events.Clear();
                try
                {
                    TrainerDAL trainerDAL = new TrainerDAL();
                    trainerDAL.GetTrainers();

                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "SELECT * FROM [Activity] WHERE Type = 'event';";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int eventId = reader.GetInt32(0);
                                    string eventName = reader.GetString(1);
                                    int activityDuration = reader.GetInt32(2);
                                    string startingTime = reader.GetString(3);

                                    string activityDescription = reader.GetString(4);


                                    int trainerId = reader.GetInt32(5);
                                    Trainer trainer = trainerDAL.trainers.First(x => x.UserId == trainerId);

                                    List<Equipment> equipments = new List<Equipment>(); // deze moet nog gevuld worden

                                    List<Athlete> eventParticipants = new List<Athlete>(); // deze moet nog gevuld worden

                                    string eventLocation = reader.GetString(7);

                                    int eventParticipantLimit = reader.GetInt32(8);

                                    // Create Event object and add it to the Events list
                                    Event eventItem = new Event(eventId, eventName, activityDuration, startingTime, trainer, activityDescription, equipments, eventParticipants, eventLocation, eventParticipantLimit);
                                    events.Add(eventItem);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while retrieving events from the database. Please contact customer service. Error: {ex.Message}");
                }
            }
        }

        public class ExerciseDAL
        {
            public List<Exercise> Exercises = new List<Exercise>();

            public void GetExercises()
            {
                Exercises.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
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

            //The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Exercise_Activity". The conflict occurred in database "ZuydFitFinal", table "dbo.Activity", column 'ActivityId'.
            public void CreateNewExercise(Exercise exercise)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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

            public void UpdateExercise(Exercise exercise)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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

            public void DeleteExercise(int exerciseId)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "DELETE FROM [Exercise] WHERE ExerciseId = @ExerciseId;";

                        using (SqlCommand dbCommand = new SqlCommand(query, connection))
                        {

                            dbCommand.Parameters.AddWithValue("@ExerciseId", exerciseId);

                            dbCommand.ExecuteNonQuery();
                        };


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the exercise from the database. Please contact customer service. Error: {ex.Message}");
                }
            }

            public void AddWorkoutIdToExercise(Workout workout)
            {

            }

        }

        public class WorkoutDAL
        {

            public void GetWorkouts()
            {

            }

            public int CreateNewWorkout(Workout workout)
            {
                int insertedId = -1;

                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while saving the workout to the database. Please contact Customer Service: {ex.Message}");
                }

                return insertedId;
            }

            public void AddExerciseToWorkout(Exercise exercise)
            {

            }
        }

        public class LogFeedbackDAL
        {
            public List<LogFeedback> logFeedbacks = new List<LogFeedback>();

            public void GetLogFeedback()
            {
                logFeedbacks.Clear();
                try
                {
                    UserDAL userDal = new UserDAL();
                    userDal.GetUsers();
                    ActivityDAL activityDal = new ActivityDAL();
                    activityDal.GetActivities();
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "Select * from [LogFeedback]";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Users ophalen uit database
                                    int logFeedbackId = reader.GetInt32(0);
                                    int trainerId = reader.GetInt32(1);
                                    int athleteId = reader.GetInt32(2);
                                    int activityId = reader.GetInt32(3);
                                    string feedbackInfo = reader.GetString(4);


                                    Trainer trainer = userDal.users.Find(x => x.UserId == trainerId) as Trainer;
                                    Athlete athlete = userDal.users.Find(x => x.UserId == athleteId) as Athlete;


                                    Activity activity = activityDal.activities.Find(x => x.ActivityId == activityId);

                                    LogFeedback feedback = new LogFeedback(logFeedbackId, trainer, athlete, activity, feedbackInfo);
                                    logFeedbacks.Add(feedback);
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


            public void CreateLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [LogFeedback](TrainerId, AthleteId, ActivityId, FeedbackInfo) VALUES(@TrainerId, @AthleteId, @ActivityId, @FeedbackInfo);";

                        using SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@TrainerId", feedback.Trainer.UserId);
                        dbCommand.Parameters.AddWithValue("@AthleteId", feedback.Athlete.UserId);
                        dbCommand.Parameters.AddWithValue("@ActivityId", feedback.Activity.ActivityId);
                        dbCommand.Parameters.AddWithValue("@FeedbackInfo", feedback.FeedbackInfo);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }
            }


            public void UpdateLogFeedback(LogFeedback feedback) // dit klopt niet?
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "UPDATE [LogFeedback] SET TrainerId = @TrainerId, AthleteId = @AthleteId, ActivityId = @ActivityId, FeedbackInfo = @FeedbackInfo WHERE LogFeedbackId = @LogFeedbackId;";

                        using SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@TrainerId", feedback.Trainer.UserId);
                        dbCommand.Parameters.AddWithValue("@AthleteId", feedback.Athlete.UserId);
                        dbCommand.Parameters.AddWithValue("@ActivityId", feedback.Activity.ActivityId);
                        dbCommand.Parameters.AddWithValue("@FeedbackInfo", feedback.FeedbackInfo);
                        dbCommand.Parameters.AddWithValue("@LogFeedbackId", feedback.FeedbackId);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgetreden bij het bijwerken van de feedback in de database. Neem contact op met de klantenservice. Foutmelding: {ex.Message}");
                }
            }

            public void DeleteLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "DELETE FROM [LogFeedback] WHERE LogFeedbackId = @LogFeedbackId;";

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

        public class TrainerDAL
        {
            public List<Trainer> trainers = new List<Trainer>();
            public void GetTrainers()
            //Trainer wordt opgehaald maar lijsten worden nog niet gevuld
            {
                trainers.Clear();
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "Select * from [User] Where UserType = 2";
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

                                    Trainer user = new Trainer(userId, userName, userEmail, userPassword, new List<Activity>());
                                    trainers.Add(user);
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
        }
    }
}

