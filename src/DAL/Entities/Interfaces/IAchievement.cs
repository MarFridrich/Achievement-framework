using System;
using System.Collections.Generic;
using DAL.Entities.JoinTables;

namespace DAL.Entities.Interfaces
{
    public interface IAchievement<TReward>
    {
        string Name { get; set; }

        string Description { get; set; }

        byte[] Image { get; set; }

        int? SubTaskDone { get; set; }

        int? SubTasks { get; set; }

        Evaluations Evaluation { get; set; }

        DateTime AccomplishDate { get; set; }

        DateTime ValidUntil { get; set; }

        TReward Reward { get; set; }
        
        ICollection<UserCompletedAchievements> UserCompletedAchievements { get; set; }
        
        AchievementGroup AchievementGroup { get; set; }

    }
}