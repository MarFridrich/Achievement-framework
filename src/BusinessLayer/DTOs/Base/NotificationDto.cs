using System;
using BusinessLayer.DTOs.Common;
using DAL.Entities;

namespace BusinessLayer.DTOs.Base
{
    public class NotificationDto : DtoBase
    {
        public string Message { get; set; }

        public DateTime Created { get; set; }
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int? CreatedByUserId { get; set; }
        
        public UserDto CreatedByUser { get; set; }

        public bool WasShowed { get; set; }
        
        public AchievementDto Achievement { get;set; }
        public int? AchievementId { get; set; }
    }
}