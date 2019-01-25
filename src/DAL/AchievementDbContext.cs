using System.Data.Common;
using DAL.Entities;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AchievementDbContext : AchievementDbContext<Achievement, Reward, AchievementGroup>
    {
        public AchievementDbContext(DbContextOptions options) 
            : base(options)
        {
        }
        
    }
    public class AchievementDbContext<TAchievement, TReward, TAchievementGroup> 
        : AchievementDbContext<User, Role, TAchievement, TAchievementGroup, TReward, UserAchievementGroup,
            UserCompletedAchievements, Notification, SubTask>
        where TReward : Reward
        where TAchievement : Achievement
        where TAchievementGroup : AchievementGroup
    {
        
        public AchievementDbContext(DbContextOptions options)
            : base(options)
        {
        }

    }
    
  
    
    public class AchievementDbContext<TUser, TRole, TAchievement, TAchievementGroup, TReward, TUserAchievementGroups,
        TUserCompletedAchievements, TNotification, TSubTasks> : IdentityDbContext<TUser, TRole, int>
    
        where TRole : Role
        where TUser : User
        where TReward : Reward
        where TAchievement : Achievement
        where TNotification : Notification
        where TAchievementGroup : AchievementGroup
        where TUserAchievementGroups : UserAchievementGroup
        where TUserCompletedAchievements : UserCompletedAchievements
        where TSubTasks : SubTask
    
    {
        public DbSet<TReward> Rewards { get; set; }
        public DbSet<TAchievement> Achievements { get; set; }
        public DbSet<TNotification> Notifications { get; set; }
        public DbSet<TUserCompletedAchievements> UserCompletedAchievements { get; set; }
        public DbSet<TAchievementGroup> AchievementGroups { get; set; }
        public DbSet<TUserAchievementGroups> UserGroups { get; set; }
        public DbSet<TSubTasks> SubTasks { get; set; }
        
        
        public AchievementDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TUser>(u =>
            {
                u.HasMany<TUserAchievementGroups>().WithOne().HasForeignKey(ug => ug.AchievementGroupId).IsRequired();
            });
            
            builder.Entity<TUserAchievementGroups>()
                .HasKey(ug => new {ug.UserId, ug.AchievementGroupId});

        }
    }


    
}