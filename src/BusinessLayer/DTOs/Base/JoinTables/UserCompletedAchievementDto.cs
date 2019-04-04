using System;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class UserCompletedAchievementDto : DtoBase
    {        
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int AchievementId { get; set; }
        
        public AchievementDto Achievement { get; set; }
        
        public DateTime AccomplishDate { get; set; }
    }
}