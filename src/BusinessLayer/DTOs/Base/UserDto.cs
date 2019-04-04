using System.Collections.Generic;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using DAL.Entities.JoinTables;

namespace BusinessLayer.DTOs.Base
{
    public class UserDto : DtoBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public byte[] Image { get; set; }
        
        public ICollection<UserCompletedAchievementDto> UserCompletedAchievements { get; set; }
        public ICollection<UserAchievementGroupDto> UserGroups { get; set; }
        
        public ICollection<AchievementGroupDto> OwnAchievementGroups { get; set; }
        
        public ICollection<UserAskedForRewardDto> UserAskedForRewards { get; set; }
        public ICollection<NotificationDto> Notifications { get; set; }
        
        public ICollection<UserAskedForSubTaskDto> UserAskedForSubTasks { get; set; }
        
        public ICollection<UserCompletedSubTaskDto> UserCompletedSubTasks { get; set; }

        public override string ToString()
        {
            return $"User {FirstName} {LastName} with username {UserName}";
        }
    }
}