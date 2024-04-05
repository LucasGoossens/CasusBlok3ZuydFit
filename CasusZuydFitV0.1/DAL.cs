using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
namespace CasusZuydFitV0._1
{
    public class DAL
    {
        private static readonly string dbConString = "Server=tcp:gabriellunesu.database.windows.net,1433;Initial Catalog=ZuydFit;Persist Security Info=False;User ID=gabriellunesu;Password=3KmaCBt5nU4qZ4s%xG5@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public class UserDAL
        {
            public List<User> users = new List<User>();
            public void GetUsers()
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(DAL.dbConString))
                    {
                        connection.Open();
                        string query = "Select * from [Users]";
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
                                    else if (userType == 3) // user is Eventorganisator
                                    {
                                        Eventorganisor user = new Eventorganisor(userId, userName, userEmail, userPassword, new List<Event>());
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
        }

        public class ActivityDAL
        {
        }

        public class AthleteDAL
        {
        }

        public class EquipmentDAL
        {
        }

        public class EventDAL
        {
        }

        public class FeedbackDAL
        {
        }

        public class TrainerDAL
        {
        }

        public class EventorganisorDAL
        {
        }
    }
}
