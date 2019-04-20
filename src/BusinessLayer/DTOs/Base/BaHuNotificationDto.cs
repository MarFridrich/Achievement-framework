using System;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuNotificationDto : DtoBase
    {
        public string Message { get; set; }

        public DateTime Created { get; set; }
        public int UserId { get; set; }
        
        public BaHUserDto User { get; set; }
        
        public int? CreatedByUserId { get; set; }
        
        public BaHUserDto CreatedByUser { get; set; }

        public bool WasShowed { get; set; }
        
        public BaHuAchievementDto Achievement { get;set; }
        public int? AchievementId { get; set; }
    }
}