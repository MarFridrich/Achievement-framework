using BusinessLayer.DTOs.Common;
using DAL.Entities.JoinTables;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class UserAchievementGroupDto : DtoBase, ILinkToEntity<UserAchievementGroup>
    {
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int AchievementGroupId { get; set; }
        
        public AchievementGroupDto AchievementGroup { get; set; }
    }
}