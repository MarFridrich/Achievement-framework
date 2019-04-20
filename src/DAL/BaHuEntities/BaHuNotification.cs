using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.BaHuEntities.Interfaces;

namespace DAL.BaHuEntities
{
    public class BaHuNotification : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public DateTime Created { get; set; }
        
        [ForeignKey("CreatedByUser")]
        public int? CreatedByUserId { get; set; }
        
        public BaHUser CreatedByUser { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public BaHUser User { get; set; }
       
        [ForeignKey("Achievement")]
        public int? AchievementId { get; set; }
        
        public BaHuAchievement Achievement { get; set; }
        
        [Required]
        public bool WasShowed { get; set; }
        
        
    }
}