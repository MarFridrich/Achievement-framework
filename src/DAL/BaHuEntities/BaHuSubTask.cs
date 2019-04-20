using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;

namespace DAL.BaHuEntities
{
    public class BaHuSubTask : IEntity
    {
        public int Id { get; set; }
        
        public int AchievementId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public BaHuAchievement Achievement { get; set; }
        
        public ICollection<BaHUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public ICollection<BaHUserCompletedSubTask> UserCompletedSubTasks { get; set; }
    }
}