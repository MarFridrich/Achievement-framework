using System;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHUserAskedForRewardDto : DtoBase
    {
        public int UserId { get; set; }
        
        public int AchievementId { get; set; }

        public DateTime DateTime { get; set; }
    }
}