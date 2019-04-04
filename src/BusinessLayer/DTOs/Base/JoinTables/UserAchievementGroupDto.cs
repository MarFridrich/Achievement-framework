using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class UserAchievementGroupDto : DtoBase
    {
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int AchievementGroupId { get; set; }
        
        public AchievementGroupDto AchievementGroup { get; set; }
    }
}