using System;
using System.Collections.Generic;
using System.Linq;

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
            DAL.WorkoutDAL workoutdal = new DAL.WorkoutDAL();
            workoutdal.GetWorkouts();
            return workoutdal.workouts;
        }

        public void CreateNewWorkout()
        {
            DAL.WorkoutDAL workoutdal = new DAL.WorkoutDAL();
            this.ActivityId = workoutdal.CreateNewWorkout(this);
        }

        public static void DisplayAllWorkouts(int loggedInUserid)
        {

            Athlete currentAthlete = Athlete.GetAllAthletes().Find(athlete => athlete.UserId == loggedInUserid);


            Console.WriteLine("-----------------------");
            Console.WriteLine("All Workouts:\n");
            Console.WriteLine("-----------------------");
            int workoutNumber = 0;
            foreach (Workout workout in GetWorkouts())
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
                Console.WriteLine("Invalid selection. Please restart and enter a valid workout number.");
                return;
            }

            selectedNumber--;
            Workout selectedWorkout = GetWorkouts()[selectedNumber];

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
                LogFeedback.CheckFeedback(currentAthlete, selectedWorkout); // You need to modify this line accordingly
            }
            else
            {
                Console.WriteLine("This workout has no exercises listed.");
            }
        }
    }
}
