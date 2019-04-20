using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHUserAchievementGroupDto : DtoBase
    {
        public int UserId { get; set; }
        
        public BaHUserDto User { get; set; }
        
        public int AchievementGroupId { get; set; }
        
        public BaHuAchievementGroupDto AchievementGroup { get; set; }
    }
}