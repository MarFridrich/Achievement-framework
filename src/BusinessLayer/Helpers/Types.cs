using System;

namespace BusinessLayer.Helpers
{
    public class Types
    {
        public Type BaHuAchievement { get; }

        public Type BaHuAchievementGroup { get; }

        public Type BaHuExtensibleUser { get; }

        public Type BaHuNotification { get; }

        public Type BaHuReward { get; }

        public Type BaHuRole { get; }

        public Type BaHuSubTask { get; }

        public Type BaHUserAchievementGroup { get; }

        public Type BaHUserAskedForReward { get; }

        public Type BaHUserCompletedAchievements { get; }
        
        public Type BaHUserCompletedSubTask { get; }
        
        public Type BaHUserAskedForSubTask { get; }


        public Types(Type role, Type achievement, Type achievementGroup, Type reward, Type userAchievementGroup,
            Type userCompletedAchievement,
            Type userAskedReward,
            Type notification,
            Type subTask,
            Type userAskedForSubTask,
            Type userCompletedSubTask)
        {
            BaHuRole = role;
            BaHuAchievement = achievement;
            BaHuAchievementGroup = achievementGroup;
            BaHuReward = reward;
            BaHUserAchievementGroup = userAchievementGroup;
            BaHUserCompletedAchievements = userCompletedAchievement;
            BaHUserAskedForReward = userAskedReward;
            BaHuNotification = notification;
            BaHuSubTask = subTask;
            BaHUserCompletedSubTask = userCompletedSubTask;
            BaHUserAskedForSubTask = userAskedForSubTask;
        }
    }
}