using System;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuNotificationDto : DtoBase
    {
        [Required]
        public string Message { get; set; }

        public DateTime Created { get; set; }
        
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int? CreatedByUserId { get; set; }
        
        public UserDto CreatedByUser { get; set; }

        public bool WasShowed { get; set; }
        
        public BaHuAchievementDto Achievement { get;set; }
        
        public int? AchievementId { get; set; }
    }
}