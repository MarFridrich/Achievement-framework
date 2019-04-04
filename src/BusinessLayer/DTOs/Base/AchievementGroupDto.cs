using System;
using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using DAL.Entities;

namespace BusinessLayer.DTOs.Base
{
    public class AchievementGroupDto : DtoBase
    {
     
        public string Name { get; set; }
        
        public int OwnerId { get; set; }
        
        public byte[] Image { get; set; }
        
        public UserDto Owner { get; set; } 
        
        public DateTime ExpiredIn { get; set; }
        public virtual ICollection<AchievementDto> Achievements { get; set; } = new List<AchievementDto>();
        
        public virtual ICollection<UserAchievementGroupDto> UserAchievementGroups { get; set; } = new List<UserAchievementGroupDto>();
    }
}