using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;

namespace DAL.Entities.JoinTables
{
    /// <summary>
    /// Represents table, for user and his groups. User can have many groups. 
    /// </summary>
    public class FrameworkUserAchievementGroup : IEntity
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public virtual FrameworkUser User { get; set; }
        
        [Required]
        [ForeignKey("AchievementGroup")]
        public int AchievementGroupId { get; set; }
        
        public virtual FrameworkAchievementGroup AchievementGroup { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}