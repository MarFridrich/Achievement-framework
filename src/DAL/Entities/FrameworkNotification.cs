using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;

namespace DAL.Entities
{
    public class FrameworkNotification : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public DateTime Created { get; set; }
        
        [ForeignKey("CreatedByUser")]
        public int? CreatedByUserId { get; set; }
        
        public virtual FrameworkUser CreatedByUser { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public virtual FrameworkUser User { get; set; }
       
        [ForeignKey("Achievement")]
        public int? AchievementId { get; set; }
        
        public virtual FrameworkAchievement Achievement { get; set; }
        
        [Required]
        public bool WasShowed { get; set; }
        
        
    }
}