using System.Collections.Generic;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using DAL.Entities.JoinTables;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class UserDto : DtoBase, ILinkToEntity<User>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public virtual ICollection<UserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        
        public virtual ICollection<UserAchievementGroupDto> UserGroups { get; set; }
    }
}