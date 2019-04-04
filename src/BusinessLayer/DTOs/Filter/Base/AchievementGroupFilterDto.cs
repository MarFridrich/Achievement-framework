using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class AchievementGroupFilterDto : FilterDtoBase
    {
        public bool NonExpired { get; set; }
        
        public int OwnerId { get; set; }
        
        public int UserId { get; set; }
        
        public string Name { get; set; }
        
    }
}