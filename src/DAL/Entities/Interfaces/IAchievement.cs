using System;
using System.Collections.Generic;
using DAL.Entities.JoinTables;

namespace DAL.Entities.Interfaces
{
    public interface IAchievement : IEntity
    {

        string Name { get; set; }

        string Description { get; set; }

        byte[] Image { get; set; }

        ICollection<FrameworkSubTask> SubTasks { get; set; }

        FrameworkEvaluations Evaluation { get; set; }

        DateTime ValidUntil { get; set; }

        int RewardId { get; set; }

        FrameworkReward Reward { get; set; }
        ICollection<FrameworkUserCompletedAchievements> UserCompletedAchievements { get; set; }

        int AchievementGroupId { get; set; }

        FrameworkAchievementGroup AchievementGroup { get; set; }

        FrameworkNotification Notification { get; set; }

    }
}