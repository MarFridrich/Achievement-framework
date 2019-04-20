using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;

namespace DAL.BaHuEntities
{   /// <summary>
    /// Class, which represents group of achievement. Achievement can have only one group.
    /// </summary>
    public class BaHuAchievementGroup : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        
        public byte[] Image { get; set; }
        
        public DateTime ExpiredIn { get; set; }
        public virtual BaHUser Owner { get; set; }
        
        public virtual ICollection<BaHuAchievement> Achievements { get; set; }

        public virtual ICollection<BaHUserAchievementGroup> UserAchievementGroups { get; set; }
    }
}