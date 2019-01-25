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
    public class UserAchievementGroup : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        [Required]
        [ForeignKey("AchievementGroup")]
        public int AchievementGroupId { get; set; }
        
        public AchievementGroup AchievementGroup { get; set; }

    }
}