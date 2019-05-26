using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BadgerHunterDbContext<TExtendedUser, TRole, TAchievement, TAchievementGroup, TReward, TUserAchievementGroups,
        TUserCompletedAchievements, TUserAskedForReward, TNotification, TSubTasks, TUserAskedForSubTask, TUserCompletedSubTask> : IdentityDbContext<User, TRole, int>
        
        where TRole : BaHuRole
        where TReward : BaHuReward
        where TAchievement : BaHuAchievement
        where TNotification : BaHuNotification
        where TAchievementGroup : BaHuAchievementGroup
        where TUserAchievementGroups : BaHuUserAchievementGroup
        where TUserCompletedAchievements : BaHuUserCompletedAchievement
        where TSubTasks : BaHuSubTask
        where TUserAskedForReward : BaHuUserAskedForReward
        where TUserAskedForSubTask : BaHuUserAskedForSubTask
        where TUserCompletedSubTask : BaHuUserCompletedSubTask
        where TExtendedUser : BaHuExtensibleUser
    
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
        public DbSet<TExtendedUser> ExtendedUsers { get; set; }
        
        public BadgerHunterDbContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<BaHuUserAchievementGroup>()
                .HasKey(ug => new {ug.UserId, ug.AchievementGroupId});

            builder.Entity<BaHuUserAskedForReward>()
                .HasKey(uar => new {uar.UserId, uar.AchievementId});
            
            builder.Entity<BaHuUserCompletedAchievement>()
                .HasKey(uca => new {uca.UserId, uca.AchievementId});

            builder.Entity<BaHuUserCompletedSubTask>()
                .HasKey(ucs => new {ucs.UserId, ucs.SubTaskId});
            
            builder.Entity<BaHuUserAskedForSubTask>()
                .HasKey(uas => new {uas.UserId, uas.SubTaskId});

            builder.Entity<BaHuUserCompletedAchievement>()
                .HasOne(uca => uca.User)
                .WithMany(uca => uca.UserCompletedAchievements)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuSubTask>()
                .HasOne(s => s.Achievement)
                .WithMany(a => a.SubTasks)
                .HasForeignKey(s => s.AchievementId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuAchievementGroup>()
                .HasOne(g => g.Owner)
                .WithMany()
                .HasForeignKey(g => g.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuUserAchievementGroup>()
                .HasOne(ag => ag.AchievementGroup)
                .WithMany(uag => uag.UserAchievementGroups)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuUserAchievementGroup>()
                .HasOne(uag => uag.User)
                .WithMany(u => u.UserGroups)
                .OnDelete(DeleteBehavior.Restrict);
            
            
            builder.Entity<BaHuAchievement>()
                .HasMany(a => a.UserAskedForRewards)
                .WithOne(uar => uar.Achievement)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.Entity<BaHuAchievement>()
                .HasOne(a => a.AchievementGroup)
                .WithMany(ag => ag.Achievements)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuAchievement>()
                .HasMany(a => a.Notifications)
                .WithOne(n => n.Achievement)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<BaHuAchievement>()
                .HasOne(a => a.Reward)
                .WithMany(r => r.Achievements)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<User>()
                .HasMany(u => u.UserOwnsGroups)
                .WithOne(ag => ag.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<User>()
                .HasMany(u => u.UserGroups)
                .WithOne(ag => ag.User)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BaHuAchievementGroup>()
                .HasMany(ag => ag.UserAchievementGroups)
                .WithOne(uag => uag.AchievementGroup)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<BaHuNotification>()
                .HasOne(n => n.CreatedByUser)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuNotification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BaHuUserAskedForSubTask>()
                .HasOne(uas => uas.User)
                .WithMany(u => u.UserAskedForSubTasks)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<BaHuUserAskedForSubTask>()
                .HasOne(uas => uas.SubTask)
                .WithMany(s => s.UserAskedForSubTasks)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            builder.Entity<BaHuUserCompletedSubTask>()
                .HasOne(uas => uas.SubTask)
                .WithMany(s => s.UserCompletedSubTasks)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<BaHuUserCompletedSubTask>()
                .HasOne(uas => uas.User)
                .WithMany(s => s.UserCompletedSubTasks)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Ignore<BaHuExtensibleUser>();

        }
    }


    
}