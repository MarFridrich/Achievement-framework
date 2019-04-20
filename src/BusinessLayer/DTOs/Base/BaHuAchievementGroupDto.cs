using System;
using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuAchievementGroupDto : DtoBase
    {
     
        public string Name { get; set; }
        
        public int OwnerId { get; set; }
        
        public byte[] Image { get; set; }
        
        public BaHUserDto Owner { get; set; } 
        
        public DateTime ExpiredIn { get; set; }
        public virtual ICollection<BaHuAchievementDto> Achievements { get; set; } = new List<BaHuAchievementDto>();
        
        public virtual ICollection<BaHUserAchievementGroupDto> UserAchievementGroups { get; set; } = new List<BaHUserAchievementGroupDto>();
    }
}