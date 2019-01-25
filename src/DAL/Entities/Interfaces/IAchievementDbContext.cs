using DAL.Entities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities.Interfaces
{
    public interface IAchievementDbContext<TUser, TRole, TAchievement, TReward, TNotification, TAchievementGroup>
        where TRole : Role
        where TUser : User
        where TReward : Reward
        where TAchievement : Achievement
        where TNotification : Notification
        where TAchievementGroup : AchievementGroup
    {
        DbSet<TReward> Rewards { get; set; }
        DbSet<TAchievement> Achievements { get; set; }
        DbSet<TNotification> Notifications { get; set; }
        DbSet<UserCompletedAchievements> UserCompletedAchievements { get; set; }
        //public DbSet<AchievementGroupBase<IAchievement<IReward>>> AchievementGroups { get; set; }
        DbSet<TAchievementGroup> AchievementGroups { get; set; }
    }
}