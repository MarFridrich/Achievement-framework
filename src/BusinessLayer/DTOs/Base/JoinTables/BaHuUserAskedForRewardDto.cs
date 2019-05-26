using System;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHuUserAskedForRewardDto : DtoBase
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int AchievementId { get; set; }

        public DateTime DateTime { get; set; }
    }
}