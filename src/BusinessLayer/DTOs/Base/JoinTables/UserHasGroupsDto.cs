using System.Collections.Generic;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class UserHasGroupsDto : DtoBase
    {
        public int UserId { get; set; }

        public int AchievementGroupId { get; set; }
        
        public ICollection<AchievementGroupDto> Achievements { get; set; }
    }
}