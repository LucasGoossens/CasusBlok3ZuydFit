namespace CasusZuydFitV0._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welkom bij de ZuydFit Apllicatie");


        }
    }
}

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