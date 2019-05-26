using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using DAL.BaHuEntities;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuSubTaskDto : DtoBase
    {
        [JsonIgnore]
        public int AchievementId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [JsonIgnore]
        public BaHuAchievement Achievement { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHuUserAskedForSubTaskDto> UserAskedForSubTasks { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHuUserCompletedSubTaskDto> UserCompletedSubTasks { get; set; }
    }
}