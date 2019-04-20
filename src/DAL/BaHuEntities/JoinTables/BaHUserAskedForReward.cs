using System;

namespace DAL.BaHuEntities.JoinTables
{
    public class BaHUserAskedForReward 
    {
        public int UserId { get; set; }
        
        public virtual BaHUser User { get; set; }
        
        public int AchievementId { get; set; }
        
        public virtual BaHuAchievement Achievement { get; set; }
        
        //public int NotificationId { get; set; }

        public DateTime DateTime { get; set; }
    }
}