using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class FrameworkUser : IdentityUser<int>, IEntity, IUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public virtual FrameworkExtensibleUser ExtensibleUser { get; set; }
        
        public ICollection<FrameworkUserCompletedAchievements> UserCompletedAchievements { get; set; }
        public ICollection<FrameworkUserAchievementGroup> UserGroups { get; set; }
        public ICollection<FrameworkAchievementGroup> OwnAchievementGroups { get; set; }
        
        public ICollection<FrameworkNotification> Notifications { get; set; }
        
        public ICollection<FrameworkUserAskedForReward> UserAskedForRewards { get; set; }
        
        public ICollection<FrameworkUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public ICollection<FrameworkUserCompletedSubTask> UserCompletedSubTasks { get; set; }
    }
}