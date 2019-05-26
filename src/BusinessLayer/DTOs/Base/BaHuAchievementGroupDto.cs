using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuAchievementGroupDto : DtoBase
    {
     
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int OwnerId { get; set; }
        
        public byte[] Image { get; set; }
        
        public UserDto Owner { get; set; } 
        
        public DateTime ExpiredIn { get; set; }
        public  ICollection<BaHuAchievementDto> Achievements { get; set; } = new List<BaHuAchievementDto>();
        
        public  ICollection<BaHuUserAchievementGroupDto> UserAchievementGroups { get; set; } = new List<BaHuUserAchievementGroupDto>();
    }
}