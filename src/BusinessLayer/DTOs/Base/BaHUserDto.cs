using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.DTOs.Base
{
    public class BaHUserDto : DtoBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public byte[] Image { get; set; }
        
        public ICollection<BaHUserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        public ICollection<BaHUserAchievementGroupDto> UserGroups { get; set; }
        
        public ICollection<BaHuAchievementGroupDto> OwnAchievementGroups { get; set; }
        
        public ICollection<BaHUserAskedForRewardDto> UserAskedForRewards { get; set; }
        public ICollection<BaHuNotificationDto> Notifications { get; set; }
        
        public ICollection<BaHUserAskedForSubTaskDto> UserAskedForSubTasks { get; set; }
        
        public ICollection<BaHUserCompletedSubTaskDto> UserCompletedSubTasks { get; set; }

        public override string ToString()
        {
            return $"User {FirstName} {LastName} with username {UserName}";
        }
    }
}