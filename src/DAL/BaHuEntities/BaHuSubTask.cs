using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;

namespace DAL.BaHuEntities
{
    public class BaHuSubTask : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Achievement")]
        public int AchievementId { get; set; }
        
        public BaHuAchievement Achievement { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public ICollection<BaHuUserAskedForSubTask> UserAskedForSubTasks { get; set; }
        
        public ICollection<BaHuUserCompletedSubTask> UserCompletedSubTasks { get; set; }
    }
}