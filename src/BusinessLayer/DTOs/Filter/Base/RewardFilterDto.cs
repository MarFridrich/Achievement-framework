using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class RewardFilterDto : FilterDtoBase
    {
        public int AchievementId { get; set; }
        
        public string Name { get; set; }
    }
}