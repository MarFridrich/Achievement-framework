using System.Collections.Generic;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class AchievementGroupDto : DtoBase, ILinkToEntity<AchievementGroup>
    {
     
        public string Name { get; set; }
        
        public int OwnerId { get; set; }
        
        public UserDto Owner { get; set; }
        
        public virtual ICollection<UserDto> Users { get; set; }
        
        public virtual ICollection<AchievementDto> Achievements { get; set; }
    }
}