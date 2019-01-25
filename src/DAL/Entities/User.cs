using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User : IdentityUser<int>, IEntity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public virtual ICollection<UserCompletedAchievements> UserCompletedAchievements { get; set; }
        
        public virtual ICollection<UserAchievementGroup> UserGroups { get; set; }
        
        public virtual ICollection<AchievementGroup> AchievementGroups { get; set; }
        
        public virtual ICollection<Notification> Notifications { get; set; }
        
    }
}