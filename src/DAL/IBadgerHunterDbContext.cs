using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public interface IBadgerHunterDbContext<TRole, TAchievement, TAchievementGroup, TReward, TUserAchievementGroups,
        TUserCompletedAchievements, TNotification, TSubTasks>
    
        where TRole : BaHuRole
        where TReward : BaHuReward
        where TAchievement : BaHuAchievement
        where TNotification : BaHuNotification
        where TAchievementGroup : BaHuAchievementGroup
        where TUserAchievementGroups : BaHUserAchievementGroup
        where TUserCompletedAchievements : BaHUserCompletedAchievement
        where TSubTasks : BaHuSubTask
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