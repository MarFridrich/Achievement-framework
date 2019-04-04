using System;
using DAL.Entities.Interfaces;

namespace DAL.Entities.JoinTables
{
    public class FrameworkUserAskedForReward 
    {
        public int UserId { get; set; }
        
        public virtual FrameworkUser User { get; set; }
        
        public int AchievementId { get; set; }
        
        public virtual FrameworkAchievement Achievement { get; set; }
        
        //public int NotificationId { get; set; }

        public DateTime DateTime { get; set; }
    }
}