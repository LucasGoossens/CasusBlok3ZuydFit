using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    internal class Exercise : Activity
    {
        public Dictionary<string, string> ExerciseLog { get; set; }
        public Athlete ExerciseParticipant { get; set; }

        public Exercise(int activityId, string activityName, int activityDuration, string startingTime, Trainer trainer, string activityDescription, List<Equipment> equipents, Dictionary<string, string> exerciseLog, Athlete exerciseParticipant)
            : base(activityId, activityName, activityDuration, startingTime, trainer, activityDescription, equipents)
        {
            ExerciseLog = exerciseLog;
            ExerciseParticipant = exerciseParticipant;
        }
    }
}
