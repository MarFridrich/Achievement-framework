using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.BaHuEntities.JoinTables
{
    /// <summary>
    /// Join table for achievements, which were accomplished by user.
    /// </summary>
    public class BaHUserCompletedAchievement 
    {
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public virtual BaHUser User { get; set; }
        
        [Required]
        [ForeignKey("Achievement")]
        public int AchievementId { get; set; }
        
        public virtual BaHuAchievement Achievement { get; set; }
        
        [Required]
        public DateTime AccomplishDate { get; set; }
    }
}