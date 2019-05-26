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

        public Type BaHuUserAchievementGroup { get; }

        public Type BaHuUserAskedForReward { get; }

        public Type BaHuUserCompletedAchievements { get; }
        
        public Type BaHuUserCompletedSubTask { get; }
        
        public Type BaHuUserAskedForSubTask { get; }


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
            BaHuUserAchievementGroup = userAchievementGroup;
            BaHuUserCompletedAchievements = userCompletedAchievement;
            BaHuUserAskedForReward = userAskedReward;
            BaHuNotification = notification;
            BaHuSubTask = subTask;
            BaHuUserCompletedSubTask = userCompletedSubTask;
            BaHuUserAskedForSubTask = userAskedForSubTask;
        }
    }
}