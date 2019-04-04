using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Entities.Interfaces;

namespace DAL.Entities
{
    /// <summary>
    /// Class, represents base reward for accomplished achievement
    /// </summary>
    public class FrameworkReward : IEntity
    {
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }
        
        public virtual ICollection<FrameworkAchievement> Achievements { get; set; }
    }
}