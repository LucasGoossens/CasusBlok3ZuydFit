using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    public class Exercise : Activity
    {

        public Athlete ExerciseParticipant { get; set; }

        public Exercise(int activityId, string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, List<Equipment> equipments, Athlete exerciseParticipant)
              : base(activityId, activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription, equipments)
        {

            ExerciseParticipant = exerciseParticipant;
        }

        public Exercise(string activityName, int activityDurationMinutes, string activityStartingTime, Trainer trainer, string activityDescription, List<Equipment> equipments, Athlete exerciseParticipant)
           : base(activityName, activityDurationMinutes, activityStartingTime, trainer, activityDescription, equipments)
        {

            ExerciseParticipant = exerciseParticipant;
        }
    }
}
