using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;

namespace DAL.Entities.JoinTables
{
    /// <summary>
    /// Join table for achievements, which were accomplished by user.
    /// </summary>
    public class FrameworkUserCompletedAchievements 
    {
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public virtual FrameworkUser User { get; set; }
        
        [Required]
        [ForeignKey("Achievement")]
        public int AchievementId { get; set; }
        
        public virtual FrameworkAchievement Achievement { get; set; }
        
        [Required]
        public DateTime AccomplishDate { get; set; }
    }
}