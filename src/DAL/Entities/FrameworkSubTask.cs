using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;

namespace DAL.Entities
{
    public class FrameworkSubTask : IEntity
    {
        public int Id { get; set; }
        
        public int AchievementId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public FrameworkAchievement Achievement { get; set; }
        
        public ICollection<FrameworkUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public ICollection<FrameworkUserCompletedSubTask> UserCompletedSubTasks { get; set; }
    }
}