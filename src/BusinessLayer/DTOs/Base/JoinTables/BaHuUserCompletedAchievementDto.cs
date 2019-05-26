using System;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHuUserCompletedAchievementDto : DtoBase
    {        
        [Required]
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        [Required]
        public int AchievementId { get; set; }
        
        public BaHuAchievementDto Achievement { get; set; }
        
        public DateTime AccomplishDate { get; set; }
    }
}