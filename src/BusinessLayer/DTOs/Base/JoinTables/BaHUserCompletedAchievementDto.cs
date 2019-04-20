using System;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHUserCompletedAchievementDto : DtoBase
    {        
        public int UserId { get; set; }
        
        public BaHUserDto User { get; set; }
        
        public int AchievementId { get; set; }
        
        public BaHuAchievementDto Achievement { get; set; }
        
        public DateTime AccomplishDate { get; set; }
    }
}