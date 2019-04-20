using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;
using Microsoft.AspNetCore.Identity;

namespace DAL.BaHuEntities
{
    public class BaHUser : IdentityUser<int>, IEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public virtual BaHuExtensibleUser ExtensibleUser { get; set; }
        
        public ICollection<BaHUserCompletedAchievement> UserCompletedAchievements { get; set; }
        public ICollection<BaHUserAchievementGroup> UserGroups { get; set; }
        public ICollection<BaHuAchievementGroup> OwnAchievementGroups { get; set; }
        
        public ICollection<BaHuNotification> Notifications { get; set; }
        
        public ICollection<BaHUserAskedForReward> UserAskedForRewards { get; set; }
        
        public ICollection<BaHUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public ICollection<BaHUserCompletedSubTask> UserCompletedSubTasks { get; set; }
    }
}