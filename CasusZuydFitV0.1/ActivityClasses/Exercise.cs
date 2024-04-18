using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL.DAL;

namespace CasusZuydFitV0._1.ActivityClasses
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseResult { get; set; }
        public int ExerciseSets { get; set; } // deze weg voorlopig
        public int ExerciseReps { get; set; } // deze weg voorlopig
        public int ExerciseWeight { get; set; } // deze weg voorlopig
        public string ExerciseDescription { get; set; }

        public int WorkoutId { get; set; }

        public Exercise(int exerciseId, string exerciseName, string exerciseResult, /*int ExerciseSets, int ExerciseReps, int ExerciseWeight, */string exerciseDescription, int workoutId)
        {
            ExerciseId = exerciseId;
            ExerciseName = exerciseName;
            ExerciseResult = exerciseResult;
            //ExerciseSets = ExerciseSets; hoofdletters nog veranderen hier en in parameters
            //ExerciseReps = ExerciseReps;
            //ExerciseWeight = ExerciseWeight;
            ExerciseDescription = exerciseDescription;
            WorkoutId = workoutId;
        }

        public Exercise(string exerciseName, string exerciseResult,/* int ExerciseSets, int ExerciseReps, int ExerciseWeight, */string exerciseDescription, int workoutId)
        {
            ExerciseName = exerciseName;
            ExerciseResult = exerciseResult;
            //ExerciseSets = ExerciseSets;
            //ExerciseReps = ExerciseReps;
            //ExerciseWeight = ExerciseWeight;
            ExerciseDescription = exerciseDescription;
            WorkoutId = workoutId;
        }

        public static Exercise CreateNewExercise(int workoutId)
        {
            Console.Clear();
            Console.WriteLine("Enter Exercise Name:");
            string exerciseName = Console.ReadLine();
            Console.WriteLine("Enter Exercise Description:");
            string exerciseDescription = Console.ReadLine();

            return new Exercise(exerciseName, "exerciseresult", exerciseDescription, workoutId);
        }


        static public List<Exercise> GetExercises()
        {
            ExerciseDAL dal = new ExerciseDAL();
            dal.GetExercises();
            return dal.Exercises;
        }

        public void CreateExercise()
        {
            ExerciseDAL exerciseDAL = new ExerciseDAL();
            exerciseDAL.CreateNewExercise(this);
        }

    }
}
