using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DAL.Entities;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AchievementDbContext : AchievementDbContext<FrameworkAchievement, FrameworkReward, FrameworkAchievementGroup>
    {
        public AchievementDbContext(DbContextOptions options) 
            : base(options)
        {
        }
        
    }
    public class AchievementDbContext<TAchievement, TReward, TAchievementGroup> 
        : AchievementDbContext<FrameworkRole, TAchievement, TAchievementGroup, TReward, FrameworkUserAchievementGroup,
            FrameworkUserCompletedAchievements, FrameworkUserAskedForReward, FrameworkNotification, FrameworkSubTask, FrameworkUserAskedForSubTask, FrameworkUserCompletedSubTask>
        where TReward : FrameworkReward
        where TAchievement : FrameworkAchievement
        where TAchievementGroup : FrameworkAchievementGroup
    {
        
        public AchievementDbContext(DbContextOptions options)
            : base(options)
        {
        }

    }
    
  
    
    public class AchievementDbContext<TRole, TAchievement, TAchievementGroup, TReward, TUserAchievementGroups,
        TUserCompletedAchievements, TUserAskedForReward, TNotification, TSubTasks, TUserAskedForSubTask, TUserCompletedSubTask> : IdentityDbContext<FrameworkUser, TRole, int>, 
        IAchievementDbContext<TRole, TAchievement, TAchievementGroup, TReward, TUserAchievementGroups,
            TUserCompletedAchievements, TNotification, TSubTasks>

        where TRole : FrameworkRole
        where TReward : FrameworkReward
        where TAchievement : FrameworkAchievement
        where TNotification : FrameworkNotification
        where TAchievementGroup : FrameworkAchievementGroup
        where TUserAchievementGroups : FrameworkUserAchievementGroup
        where TUserCompletedAchievements : FrameworkUserCompletedAchievements
        where TSubTasks : FrameworkSubTask
        where TUserAskedForReward : FrameworkUserAskedForReward
        where TUserAskedForSubTask : FrameworkUserAskedForSubTask
        where TUserCompletedSubTask : FrameworkUserCompletedSubTask
    
    {
        public DbSet<TReward> Rewards { get; set; }
        public DbSet<TAchievement> Achievements { get; set; }
        public DbSet<TNotification> Notifications { get; set; }
        public DbSet<TUserCompletedAchievements> UserCompletedAchievements { get; set; }
        public DbSet<TAchievementGroup> AchievementGroups { get; set; }
        public DbSet<TUserAchievementGroups> UserGroups { get; set; }
        public DbSet<TSubTasks> SubTasks { get; set; }
        public DbSet<TUserAskedForReward> UserAskedForRewards { get; set; }
        public DbSet<TUserCompletedSubTask> UserCompletedSubTasks { get; set; }
        public DbSet<TUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public AchievementDbContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<FrameworkUserAchievementGroup>()
                .HasKey(ug => new {ug.UserId, ug.AchievementGroupId});

            builder.Entity<FrameworkUserAskedForReward>()
                .HasKey(uar => new {uar.UserId, uar.AchievementId});
            
            builder.Entity<FrameworkUserCompletedAchievements>()
                .HasKey(uca => new {uca.UserId, uca.AchievementId});

            builder.Entity<FrameworkUserCompletedSubTask>()
                .HasKey(ucs => new {ucs.UserId, ucs.SubTaskId});
            
            builder.Entity<FrameworkUserAskedForSubTask>()
                .HasKey(uas => new {uas.UserId, uas.SubTaskId});

            builder.Entity<FrameworkUserCompletedAchievements>()
                .HasOne(uca => uca.User)
                .WithMany(uca => uca.UserCompletedAchievements)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkUserCompletedSubTask>()
                .HasOne(ucs => ucs.SubTask)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkUserCompletedSubTask>()
                .HasOne(ucs => ucs.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkSubTask>()
                .HasOne(s => s.Achievement)
                .WithMany(a => a.SubTasks)
                .HasForeignKey(s => s.AchievementId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkAchievementGroup>()
                .HasOne(g => g.Owner)
                .WithMany()
                .HasForeignKey(g => g.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkUserAchievementGroup>()
                .HasOne(ag => ag.AchievementGroup)
                .WithMany(uag => uag.UserAchievementGroups)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkUserAchievementGroup>()
                .HasOne(uag => uag.User)
                .WithMany(u => u.UserGroups)
                .OnDelete(DeleteBehavior.Restrict);
            
            
            builder.Entity<FrameworkAchievement>()
                .HasMany(a => a.UserAskedForRewards)
                .WithOne(uar => uar.Achievement)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<FrameworkAchievement>()
                .HasOne(a => a.AchievementGroup)
                .WithMany(ag => ag.Achievements)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkAchievement>()
                .HasMany(a => a.Notifications)
                .WithOne(n => n.Achievement)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<FrameworkAchievement>()
                .HasOne(a => a.Reward)
                .WithMany(r => r.Achievements)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<FrameworkUser>()
                .HasMany(u => u.OwnAchievementGroups)
                .WithOne(ag => ag.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<FrameworkUser>()
                .HasMany(u => u.UserGroups)
                .WithOne(ag => ag.User)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FrameworkAchievementGroup>()
                .HasMany(ag => ag.UserAchievementGroups)
                .WithOne(uag => uag.AchievementGroup)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<FrameworkNotification>()
                .HasOne(n => n.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<FrameworkNotification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FrameworkUserAskedForSubTask>()
                .HasOne(uas => uas.User)
                .WithMany(u => u.UserAskedForSubTasks)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<FrameworkUserAskedForSubTask>()
                .HasOne(uas => uas.SubTask)
                .WithMany(s => s.UserAskedForSubTasks)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Ignore<FrameworkExtensibleUser>();




        }
    }


    
}