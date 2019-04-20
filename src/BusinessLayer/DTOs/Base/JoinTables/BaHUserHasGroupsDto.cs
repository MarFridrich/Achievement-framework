using System.Collections.Generic;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHUserHasGroupsDto : DtoBase
    {
        public int UserId { get; set; }

        public int AchievementGroupId { get; set; }
        
        public ICollection<BaHuAchievementGroupDto> Achievements { get; set; }
    }
}