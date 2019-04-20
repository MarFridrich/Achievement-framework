using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Base
{
    public class BaHuRewardDto : DtoBase
    {
        [Required]
        public string Name { get; set; }
        
        [JsonIgnore]
        public ICollection<BaHuAchievementDto> Achievements { get; set; }
    }
}