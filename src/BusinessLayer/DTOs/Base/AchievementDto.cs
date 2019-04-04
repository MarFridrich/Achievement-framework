using System;
using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class AchievementDto : DtoBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
    
        public ICollection<SubTaskDto> SubTasks { get; set; }

        public EvaluationsDto Evaluation { get; set; }
    
        public DateTime ValidUntil { get; set; }
      
        [JsonIgnore]
        public int RewardId { get; set; }
    
        public RewardDto Reward { get; set; }
        
        [JsonIgnore]
        public ICollection<UserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        
        [JsonIgnore]
        public ICollection<UserAskedForRewardDto> UserAskedForRewards { get; set; }
    
        [JsonIgnore]
        public int AchievementGroupId { get; set; }
    
        [JsonIgnore]
        public virtual AchievementGroupDto AchievementGroup { get; set; }
    }
}