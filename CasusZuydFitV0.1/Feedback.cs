using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasusZuydFitV0._1
{
    internal class Feedback
    {
        public int FeedbackId { get; set; }
        public Trainer Trainer { get; set; }
        public Athlete Athlete { get; set; }
        public Activity Activity { get; set; }
        public string FeedbackInfo { get; set; }

        Feedback(int feedbackId, Trainer trainer, Athlete athlete, Activity activity, string feedbackInfo)
        {
            FeedbackId = feedbackId;
            Trainer = trainer;
            Athlete = athlete;
            Activity = activity;
            FeedbackInfo = feedbackInfo;
        }
    }
}
