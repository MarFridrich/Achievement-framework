using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class RewardDto : DtoBase
    {
        [Required]
        public string Name { get; set; }
        
        [JsonIgnore]
        public ICollection<AchievementDto> Achievements { get; set; }
    }
}