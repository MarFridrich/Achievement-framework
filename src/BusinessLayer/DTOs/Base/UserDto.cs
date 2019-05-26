using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class UserDto : DtoBase
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        public byte[] Image { get; set; }
        
        public ICollection<BaHuUserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        public ICollection<BaHuUserAchievementGroupDto> UserGroups { get; set; }
        
        public ICollection<BaHuAchievementGroupDto> OwnAchievementGroups { get; set; }
        
        public ICollection<BaHuUserAskedForRewardDto> UserAskedForRewards { get; set; }
        public ICollection<BaHuNotificationDto> Notifications { get; set; }
        
        public ICollection<BaHuUserAskedForSubTaskDto> UserAskedForSubTasks { get; set; }
        
        public ICollection<BaHuUserCompletedSubTaskDto> UserCompletedSubTasks { get; set; }

        public override string ToString()
        {
            return $"User {FirstName} {LastName} with username {UserName}";
        }
    }
}