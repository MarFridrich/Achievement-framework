using System.Collections.Generic;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using DAL.Entities.JoinTables;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class UserHasGroupsDto : DtoBase, ILinkToEntity<UserAchievementGroup>
    {
        public int UserId { get; set; }

        public int AchievementGroupId { get; set; }
        
        public ICollection<AchievementGroupDto> Achievements { get; set; }
    }
}