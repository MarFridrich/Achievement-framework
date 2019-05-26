using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuAchievementDto : DtoBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
    
        public ICollection<BaHuSubTaskDto> SubTasks { get; set; }

        public BaHuEvaluationsDto Evaluation { get; set; }
    
        public DateTime ValidUntil { get; set; }
      
        [Required]
        [JsonIgnore]
        public int RewardId { get; set; }
    
        public BaHuRewardDto Reward { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHuUserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHuUserAskedForRewardDto> UserAskedForRewards { get; set; }
    
        [Required]
        [JsonIgnore]
        public int AchievementGroupId { get; set; }
    
        [JsonIgnore]
        public BaHuAchievementGroupDto AchievementGroup { get; set; }
    }
}