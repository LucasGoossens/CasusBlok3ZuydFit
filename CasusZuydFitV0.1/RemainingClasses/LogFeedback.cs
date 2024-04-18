﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CasusZuydFitV0._1.ActivityClasses;
using CasusZuydFitV0._1.UserClasses;
using static CasusZuydFitV0._1.DAL.DAL;

namespace CasusZuydFitV0._1.RemainingClasses
{
    public class LogFeedback
    {
        public int FeedbackId { get; set; }
        public int FeedbackTrainerId { get; set; }
        public int FeedbackAthleteId { get; set; }
        public int FeedbackActivityId { get; set; }
        public string? FeedbackInfo { get; set; }

        public LogFeedback(int feedbackId, int trainerId, int athleteId, int activityId, string feedbackInfo)
        {
            FeedbackId = feedbackId;
            FeedbackTrainerId = trainerId;
            FeedbackAthleteId = athleteId;
            FeedbackActivityId = activityId;
            FeedbackInfo = feedbackInfo;
        }
        public LogFeedback(int trainerId, int athleteId, int activityId, string feedbackInfo)
        {            
            FeedbackTrainerId = trainerId;
            FeedbackAthleteId = athleteId;
            FeedbackActivityId = activityId;
            FeedbackInfo = feedbackInfo;
        }
        public LogFeedback(int trainerId, int athleteId, int activityId)
        {
            FeedbackTrainerId = trainerId;
            FeedbackAthleteId = athleteId;
            FeedbackActivityId = activityId;
        }

        //public void AddTrainer(Trainer trainer)
        //{
        //    Trainer = trainer;
        //}

        //public void AddRecipient(Athlete user) // dit klopt niet in class diagram
        //{
        //    Athlete = user;
        //}

        //public void AddActivity(Activity activity)
        //{
        //    Activity = activity;

        //}

        public void ShowFeedback()
        {

        }

        public static List<LogFeedback> GetFeedback()
        {
            LogFeedbackDAL Dal = new LogFeedbackDAL();
            Dal.GetLogFeedback();
            return Dal.logFeedbacks;
        }

        public void CreateLog()
        {
            LogFeedbackDAL Dal = new LogFeedbackDAL();
            Dal.CreateLogFeedback(this);
        }

        public void UpdateFeedback(string newFeedback)
        {
            FeedbackInfo = newFeedback;
            LogFeedbackDAL Dal = new LogFeedbackDAL();
            Dal.UpdateLogFeedback(this);
        }
        public void DeleteFeedback()
        {
            LogFeedbackDAL Dal = new LogFeedbackDAL();
            Dal.DeleteLogFeedback(this);
        }

         public static void CheckFeedback(User user, Activity activity)
            {
                LogFeedback? logFeedback = LogFeedback.GetFeedback().FirstOrDefault(feedback => feedback.FeedbackActivityId == activity.ActivityId && feedback.FeedbackAthleteId == user.UserId);
                if (logFeedback.FeedbackInfo == "")
                { Console.WriteLine("No feedback given yet."); }
                else if (logFeedback != null)
                {
                    Console.WriteLine($"Feedback for {activity.ActivityName}: {logFeedback.FeedbackInfo}");
                }
                else { Console.WriteLine("Something went wrong, Please contact the Servicedesk"); }

            }
    }
}