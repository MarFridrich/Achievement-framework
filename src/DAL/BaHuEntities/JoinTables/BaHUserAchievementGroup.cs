using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.BaHuEntities.Interfaces;

namespace DAL.BaHuEntities.JoinTables
{
    /// <summary>
    /// Represents table, for user and his groups. User can have many groups. 
    /// </summary>
    public class BaHUserAchievementGroup
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public virtual BaHUser User { get; set; }
        
        [Required]
        [ForeignKey("AchievementGroup")]
        public int AchievementGroupId { get; set; }
        
        public virtual BaHuAchievementGroup AchievementGroup { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}