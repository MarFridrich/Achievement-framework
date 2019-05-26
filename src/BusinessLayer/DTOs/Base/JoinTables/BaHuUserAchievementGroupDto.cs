using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHuUserAchievementGroupDto : DtoBase
    {
        [Required]
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        [Required]
        
        public int AchievementGroupId { get; set; }
        
        public BaHuAchievementGroupDto AchievementGroup { get; set; }
    }
}