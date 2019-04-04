using DAL.Entities;
using DAL.Entities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public interface IAchievementDbContext<TRole, TAchievement, TAchievementGroup, TReward, TUserAchievementGroups,
        TUserCompletedAchievements, TNotification, TSubTasks>
    
        where TRole : FrameworkRole
        where TReward : FrameworkReward
        where TAchievement : FrameworkAchievement
        where TNotification : FrameworkNotification
        where TAchievementGroup : FrameworkAchievementGroup
        where TUserAchievementGroups : FrameworkUserAchievementGroup
        where TUserCompletedAchievements : FrameworkUserCompletedAchievements
        where TSubTasks : FrameworkSubTask
    {
        
        DbSet<TReward> Rewards { get; set; }
        DbSet<TAchievement> Achievements { get; set; }
        DbSet<TNotification> Notifications { get; set; }
        DbSet<TUserCompletedAchievements> UserCompletedAchievements { get; set; }
        DbSet<TAchievementGroup> AchievementGroups { get; set; }
        DbSet<TUserAchievementGroups> UserGroups { get; set; }
        DbSet<TSubTasks> SubTasks { get; set; }
    }
}