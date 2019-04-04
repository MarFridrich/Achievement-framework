using System;

namespace BusinessLayer.Helpers
{
    public class Types
    {
        public Type FrameworkAchievement { get; }

        public Type FrameworkAchievementGroup { get; }

        public Type FrameworkExtensibleUser { get; }

        public Type FrameworkNotification { get; }

        public Type FrameworkReward { get; }

        public Type FrameworkRole { get; }

        public Type FrameworkSubTask { get; }

        public Type FrameworkUserAchievementGroup { get; }

        public Type FrameworkUserAskedForReward { get; }

        public Type FrameworkUserCompletedAchievements { get; }
        
        public Type FrameworkUserCompletedSubTask { get; }
        
        public Type FrameworkUserAskedForSubTask { get; }


        public Types(Type role, Type achievement, Type achievementGroup, Type reward, Type userAchievementGroup,
            Type userCompletedAchievement,
            Type userAskedReward,
            Type notification,
            Type subTask,
            Type frameworkUserAskedForSubTask,
            Type userCompletedSubTask)
        {
            FrameworkRole = role;
            FrameworkAchievement = achievement;
            FrameworkAchievementGroup = achievementGroup;
            FrameworkReward = reward;
            FrameworkUserAchievementGroup = userAchievementGroup;
            FrameworkUserCompletedAchievements = userCompletedAchievement;
            FrameworkUserAskedForReward = userAskedReward;
            FrameworkNotification = notification;
            FrameworkSubTask = subTask;
            FrameworkUserCompletedSubTask = userCompletedSubTask;
            FrameworkUserAskedForSubTask = frameworkUserAskedForSubTask;
        }
    }
}