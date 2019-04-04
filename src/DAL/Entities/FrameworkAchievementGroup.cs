using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;

namespace DAL.Entities
{   /// <summary>
    /// Class, which represents group of achievement. Achievement can have only one group.
    /// </summary>
    public class FrameworkAchievementGroup : IEntity, IAchievementGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        
        public byte[] Image { get; set; }
        
        public DateTime ExpiredIn { get; set; }
        public virtual FrameworkUser Owner { get; set; }
        
        public virtual ICollection<FrameworkAchievement> Achievements { get; set; }

        public virtual ICollection<FrameworkUserAchievementGroup> UserAchievementGroups { get; set; }
    }
}