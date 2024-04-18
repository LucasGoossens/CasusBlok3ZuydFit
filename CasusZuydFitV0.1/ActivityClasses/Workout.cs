using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasusZuydFitV0._1.UserClasses;
using static CasusZuydFitV0._1.DAL.DAL;

namespace CasusZuydFitV0._1.ActivityClasses
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
            WorkoutDAL workoutdal = new WorkoutDAL();
            workoutdal.GetWorkouts();
            return workoutdal.workouts;
        }

        public void CreateNewWorkout()
        {
            WorkoutDAL workoutdal = new WorkoutDAL();
            ActivityId = workoutdal.CreateNewWorkout(this);
        }
        //public void AddExercise(Exercise exercise)
        //{
        //    WorkoutExercises.Add(exercise);
        //    //DAL.AddExercise(exercise);
        //}

    }
}
