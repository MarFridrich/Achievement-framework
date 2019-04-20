using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuRoleDto : DtoBase
    {
        public string Name { get; set; }
        
        public int AchievementGroupId { get; set; }
        
        public BaHuAchievementGroupDto AchievementGroup { get; set; }
    }
}