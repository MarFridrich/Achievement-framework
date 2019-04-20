using System;
using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuAchievementDto : DtoBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
    
        public ICollection<BaHuSubTaskDto> SubTasks { get; set; }

        public BaHuEvaluationsDto Evaluation { get; set; }
    
        public DateTime ValidUntil { get; set; }
      
        [JsonIgnore]
        public int RewardId { get; set; }
    
        public BaHuRewardDto Reward { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHUserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHUserAskedForRewardDto> UserAskedForRewards { get; set; }
    
        [JsonIgnore]
        public int AchievementGroupId { get; set; }
    
        [JsonIgnore]
        public virtual BaHuAchievementGroupDto AchievementGroup { get; set; }
    }
}