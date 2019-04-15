using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Filter.Enums;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class SubTaskFilterDto : FilterDtoBase
    {
        public int AchievementId { get; set; }
        
        public int UserId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public SubTaskAccomplishTypes AccomplishType { get; set; } = SubTaskAccomplishTypes.All;

    }
}