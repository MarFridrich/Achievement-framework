using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class SubTaskDto : DtoBase
    {
        [JsonIgnore]
        public int AchievementId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [JsonIgnore]
        public FrameworkAchievement Achievement { get; set; }
        
        [JsonIgnore]
        public ICollection<UserAskedForSubTaskDto> UserAskedForSubTasks { get; set; }
        
        [JsonIgnore]
        public ICollection<UserCompletedSubTaskDto> UserCompletedSubTasks { get; set; }
    }
}