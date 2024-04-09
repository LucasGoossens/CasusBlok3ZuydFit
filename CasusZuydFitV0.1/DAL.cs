using System.Data;
using System.Data.SqlClient;
using Microsoft.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CasusZuydFitV0._1
{
    public class DAL
    {
        private static readonly string dbConString = "Server=tcp:gabriellunesu.database.windows.net,1433;Initial Catalog=ZuydFitFinal;Persist Security Info=False;User ID=gabriellunesu;Password=3KmaCBt5nU4qZ4s%xG5@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public class UserDAL
        {
            public List<User> users = new List<User>();
            public void GetUsers()
            //User wordt opgehaald maar lijsten worden nog niet gevuld
            {
                users.Clear();
                try
                {
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

                        SqlCommand dbCommand = new SqlCommand(query, connection);

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
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
                }

            }
        }

        public class ActivityDAL
        {
        }

        public class AthleteDAL
        {
            public List<Athlete> athlets = new List<Athlete>();
            public void GetAthlets()
            //User wordt opgehaald maar lijsten worden nog niet gevuld
            {
                athlets.Clear();
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
                                    athlets.Add(user);
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
                                    int activityDuration = reader.GetInt32(2); // Assuming this is the activity duration
                                    string startingTime = reader.GetString(3); // Assuming this is the starting time
                                  
                                    string activityDescription = reader.GetString(4); 


                                    int trainerId = reader.GetInt32(5); // Assuming TrainerId is stored in the database
                                    Trainer trainer = trainerDAL.trainers.First(x => x.UserId == trainerId); // You need to implement this method to get Trainer details

                                    List<Equipment> equipments = new List<Equipment>(); 

                                    List<Athlete> eventParticipants = new List<Athlete>(); 

                                    string eventLocation = reader.GetString(7);
                                    
                                    int eventParticipantLimit = reader.GetInt32(8);

                                    // Create Event object and add it to the Events list
                                    Event eventItem = new Event(eventId, eventName, activityDuration, startingTime, activityDescription, trainer, equipments, eventParticipants, eventLocation, eventParticipantLimit);
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
                                    // reader.GetString(2) overgeslagen want klopt niet in db 
                                    string exerciseDescription = reader.GetString(3);

                                    Exercise exercise = new Exercise(exerciseId, exerciseName, "test", exerciseDescription);
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

            public void CreateNewExercise(Exercise exercise)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [Exercise](ExerciseName, ExerciseDescription, ExerciseResult) VALUES(@ExerciseName, @ExerciseDescription, @ExerciseResult);";

                        SqlCommand dbCommand = new SqlCommand(query, connection);

                        dbCommand.Parameters.AddWithValue("@ExerciseName", exercise.ExerciseName);
                        dbCommand.Parameters.AddWithValue("@ExerciseDescription", exercise.ExerciseDescription);
                        dbCommand.Parameters.AddWithValue("@ExerciseResult", exercise.ExerciseResult);

                        dbCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
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

                        using (SqlCommand dbCommand = new SqlCommand(query, connection)){

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

                            using (SqlCommand dbCommand = new SqlCommand(query, connection)){

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

            //public void CreateNewWorkout()
            //{
            //    try
            //    {
            //        using (SqlConnection connection = new SqlConnection(DAL.dbConString))
            //        {
            //            connection.Open();
            //            string query = "INSERT INTO [Workout](Workout, ExerciseDescription, ExerciseResult) VALUES(@ExerciseName, @ExerciseDescription, @ExerciseResult);";

            //            SqlCommand dbCommand = new SqlCommand(query, connection);

            //            dbCommand.Parameters.AddWithValue("@ExerciseName", exercise.ExerciseName);
            //            dbCommand.Parameters.AddWithValue("@ExerciseDescription", exercise.ExerciseDescription);
            //            dbCommand.Parameters.AddWithValue("@ExerciseResult", exercise.ExerciseResult);

            //            dbCommand.ExecuteNonQuery();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database. Neem contact op met de Klantenservice + {ex.Message}");
            //    }

            //}

            public void AddExerciseToWorkout(Exercise exercise)
            {

            }
        }

        public class LogFeedbackDAL
        {
            public void GetLogFeedback()
            {
                // kan pas gedaan worden wanneer get activty's werkt?
            }

            public void CreateLogFeedback(LogFeedback feedback)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "INSERT INTO [LogFeedback](TrainerId, AthleteId, ActivityId, FeedbackInfo) VALUES(@TrainerId, @AthleteId, @ActivityId, @FeedbackInfo);";

                        SqlCommand dbCommand = new SqlCommand(query, connection);

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

                        SqlCommand dbCommand = new SqlCommand(query, connection);

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

                        SqlCommand dbCommand = new SqlCommand(query, connection);
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
