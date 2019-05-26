using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;
using Microsoft.AspNetCore.Identity;

namespace DAL.BaHuEntities
{
    public class User : IdentityUser<int>, IEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public BaHuExtensibleUser ExtensibleUser { get; set; }
        public ICollection<BaHuUserCompletedAchievement> UserCompletedAchievements { get; set; }
        public ICollection<BaHuUserAchievementGroup> UserGroups { get; set; }
        
        public ICollection<BaHuAchievementGroup> UserOwnsGroups { get; set; }
        
        public ICollection<BaHuNotification> Notifications { get; set; }
        
        public ICollection<BaHuUserAskedForReward> UserAskedForRewards { get; set; }
        
        public ICollection<BaHuUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public ICollection<BaHuUserCompletedSubTask> UserCompletedSubTasks { get; set; }
    }
}