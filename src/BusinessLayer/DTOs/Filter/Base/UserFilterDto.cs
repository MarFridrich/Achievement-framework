using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Filter.Enums;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class UserFilterDto : FilterDtoBase
    {
        public int AchievementId { get; set; }
        
        public int GroupId { get; set; }
        
        public UserFulfillType? AchievementFulfillType { get; set; }
        
    }
}