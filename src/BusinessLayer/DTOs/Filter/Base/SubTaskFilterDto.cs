using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class SubTaskFilterDto : FilterDtoBase
    {
        public int AchievementId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}