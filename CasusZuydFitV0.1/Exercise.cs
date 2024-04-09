using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasusZuydFitV0._1.DAL;

namespace CasusZuydFitV0._1
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

        public Exercise(int ExerciseId, string ExerciseName, string ExerciseResult, /*int ExerciseSets, int ExerciseReps, int ExerciseWeight, */string ExerciseDescription)
        {
            ExerciseId = ExerciseId;
            ExerciseName = ExerciseName;
            ExerciseResult = ExerciseResult;
            //ExerciseSets = ExerciseSets;
            //ExerciseReps = ExerciseReps;
            //ExerciseWeight = ExerciseWeight;
            ExerciseDescription = ExerciseDescription;
        }

        public Exercise(string ExerciseName, string ExerciseResult,/* int ExerciseSets, int ExerciseReps, int ExerciseWeight, */string ExerciseDescription)
        {
            ExerciseName = ExerciseName;
            ExerciseResult = ExerciseResult;
            //ExerciseSets = ExerciseSets;
            //ExerciseReps = ExerciseReps;
            //ExerciseWeight = ExerciseWeight;
            ExerciseDescription = ExerciseDescription;
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
