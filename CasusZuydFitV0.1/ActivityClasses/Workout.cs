using System;
using System.Collections.Generic;
using System.Linq;
using CasusZuydFitV0._1.ActivityClasses;
using CasusZuydFitV0._1.UserClasses;
using CasusZuydFitV0._1.DAL;
using CasusZuydFitV0._1.RemainingClasses;
using System.Threading.Channels;
using System.Globalization;


namespace CasusZuydFitV0._1
{
    public class Workout : Activity
    {
        public Athlete WorkoutParticipant { get; set; }
        public List<Exercise> WorkoutExercises = new List<Exercise>();

        public Workout(string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, Athlete exerciseParticipant)
            : base(activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription)
        {
            WorkoutParticipant = exerciseParticipant;
        }

        public Workout(int activityId, string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription)
            : base(activityId, activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription)
        {
            // hmm   
        }

        public Workout(int activityId, string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, Athlete activityAthlete)
            : base(activityId, activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription)
        {
            WorkoutParticipant = activityAthlete;
        }

        public static List<Workout> GetWorkouts()
        {
            DAL.DAL.WorkoutDAL workoutdal = new DAL.DAL.WorkoutDAL();
            workoutdal.GetWorkouts();
            // hier dan GetWorkoutsByAthleteId(this.ActivityParticipant);
            return workoutdal.workouts;
        }

        public void CreateNewWorkout()
        {
            DAL.DAL.WorkoutDAL workoutdal = new DAL.DAL.WorkoutDAL();
            this.ActivityId = workoutdal.CreateNewWorkout(this);
        }

        public static void DisplayAllWorkouts(int loggedInUserid)
        {
            Athlete currentAthlete = Athlete.GetAllAthletes().Find(athlete => athlete.UserId == loggedInUserid);
            Console.Clear();
            Console.WriteLine("-----------------------");
            Console.WriteLine("All Workouts:\n");
            Console.WriteLine("-----------------------");
            DAL.DAL.WorkoutDAL allAthleteWorkouts = new DAL.DAL.WorkoutDAL();
            int workoutNumber = 0;

            foreach (Workout workout in allAthleteWorkouts.GetAllWorkoutsByAthleteId(loggedInUserid))
            {
                workoutNumber++;
                Console.WriteLine($"{workoutNumber}. Workout Name: {workout.ActivityName}");
                Console.WriteLine($"   Duration (minutes): {workout.ActivityDurationMinutes}");
                Console.WriteLine($"   Trainer: {workout.Trainer.UserName}");
                Console.WriteLine($"   Description: {workout.ActivityDescription}");
                Console.WriteLine("---------------------------------------------------------");

            }

            Console.WriteLine("\nEnter the number of the workout to view its details and exercises:");
            if (!int.TryParse(Console.ReadLine(), out int selectedNumber) || selectedNumber < 1 || selectedNumber > GetWorkouts().Count)
            {
                Console.Clear();
                Console.WriteLine("Invalid selection. Please restart and enter a valid workout number.");
                return;
            }

            selectedNumber--;
            Workout selectedWorkout = GetWorkouts()[selectedNumber];
            Console.Clear();
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

                }
                Console.WriteLine("-----------------------");
                Console.WriteLine("1. Log this Workout session.");
                Console.WriteLine("2. View logged Workouts");
                Console.WriteLine("3. Return to main.");
                int option = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                switch (option)
                {
                    case 1:
                        selectedWorkout.LogNewWorkout();
                        break;
                    case 2:
                        selectedWorkout.ViewAllWorkoutSessions();
                        break;
                    case 3:
                        return;
                }
            }
            else
            {
                Console.WriteLine("This workout has no exercises listed.");
            }
        }

        void ViewAllWorkoutSessions()
        {
            List<LogFeedback> workoutLogFeedback = new List<LogFeedback>();
            try
            {
                Console.Clear();

                List<LogFeedback> workoutLogFeedbackToFilter = LogFeedback.GetFeedback();

                foreach (LogFeedback log in workoutLogFeedbackToFilter)
                {
                    if (log.FeedbackActivityId == this.ActivityId)
                    {
                        workoutLogFeedback.Add(log as LogFeedback);
                    }
                }

                if (workoutLogFeedback.Count < 1)
                {
                    Console.WriteLine("You have not logged this workout yet.");
                    return;
                }
                Console.WriteLine("Choose workout date.");
                for (int i = 0; i < workoutLogFeedback.Count(); i++)
                {
                    Console.WriteLine($"{i}: " + workoutLogFeedback[i].ActivityDate);
                }
                //4
                try
                {
                    int workoutDateOption = Convert.ToInt32(Console.ReadLine());
                    //5
                    Console.WriteLine("Workout date: " + workoutLogFeedback[workoutDateOption].ActivityDate);
                    Console.WriteLine("Results: " + workoutLogFeedback[workoutDateOption].FeedbackInfo);

                }
                catch
                {
                    Console.WriteLine("Invalid input given. ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while gathering workout session details: " + ex.Message);

            }
        }
        void LogNewWorkout()
        {
            string feedbackResult = "";
            string workoutDate;
            bool isValidDate = false;

            do
            {
                Console.WriteLine("Enter Workout date (YYYY/MM/DD)");
                workoutDate = Console.ReadLine();

                if (workoutDate.Length != 10)
                {
                    Console.WriteLine("Enter a valid date in the format YYYY/MM/DD.");
                    continue;
                }
                // Controleert of geldige DateTime formaat is ingevoerd
                if (!DateTime.TryParseExact(workoutDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Enter a valid date in the format YYYY/MM/DD.");
                }
                else
                {
                    isValidDate = true;
                }
            }
            while (!isValidDate);

            foreach (Exercise exercise in this.WorkoutExercises)
            {
                Console.WriteLine($"- Exercise Name: {exercise.ExerciseName}");
                Console.WriteLine("What was the result for this exercise?");
                string exerciseResult = Console.ReadLine();
                feedbackResult += $"\nExercise Name: {exercise.ExerciseName}\nResult: {exerciseResult}";
                Console.Clear();
            }
            LogFeedback newLogFeedback = new LogFeedback(this.Trainer.UserId, this.WorkoutParticipant.UserId, this.ActivityId, feedbackResult, workoutDate);
            newLogFeedback.CreateLog();
        }

    }
}