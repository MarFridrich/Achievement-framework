using System;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class NotificationFilterDto : FilterDtoBase
    {
        public int UserId { get; set; }

        public bool UnReadOnly { get; set; } = false;
        
        public string Message { get; set; }
        
        public int AchievementId { get; set; }
        
        public DateTime OlderThen { get; set; } = DateTime.MaxValue;
    }
}