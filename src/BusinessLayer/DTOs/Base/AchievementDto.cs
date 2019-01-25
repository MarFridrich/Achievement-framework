using System;
using System.Collections.Generic;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class AchievementDto : DtoBase, ILinkToEntity<Achievement>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    
        public ICollection<SubTaskDto> SubTasks { get; set; }

        public EvaluationsDto Evaluation { get; set; }
    
        public DateTime ValidUntil { get; set; }
      
        public int RewardId { get; set; }
    
        public RewardDto Reward { get; set; }
        
        public virtual ICollection<UserCompletedAchievementDto> UserCompletedAchievements { get; set; }
    
        public int AchievementGroupId { get; set; }
    
        public virtual AchievementGroupDto AchievementGroup { get; set; }
    }
}