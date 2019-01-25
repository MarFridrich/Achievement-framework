using System;
using BusinessLayer.DTOs.Common;
using DAL.Entities.JoinTables;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class UserCompletedAchievementDto : DtoBase, ILinkToEntity<UserCompletedAchievements>
    {        
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int AchievementId { get; set; }
        
        public AchievementDto Achievement { get; set; }
        
        public DateTime AccomplishDate { get; set; }
    }
}