using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    public class Workout : Activity
    {

        public Athlete WorkoutParticipant { get; set; }
        public List<Exercise> WorkoutExercises = new List<Exercise>();

        public Workout(int activityId, string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, List<Equipment> equipments, Athlete exerciseParticipant)
              : base(activityId, activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription, equipments)
        {
            WorkoutParticipant = exerciseParticipant;
        }

        public Workout(string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, List<Equipment> equipments, Athlete exerciseParticipant)
           : base(activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription, equipments)
        {
            WorkoutParticipant = exerciseParticipant;
        }

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
        //public void AddExercise(Exercise exercise)
        //{
        //    WorkoutExercises.Add(exercise);
        //    //DAL.AddExercise(exercise);
        //}

    }
}
