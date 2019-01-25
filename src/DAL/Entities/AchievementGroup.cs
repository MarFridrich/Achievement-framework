using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;

namespace DAL.Entities
{   /// <summary>
    /// Class, which represents group of achievement. Achievement can have only one group.
    /// </summary>
    public class AchievementGroup : IEntity, IAchievementGroup
    
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        
        public User Owner { get; set; }
        
        public virtual ICollection<User> Users { get; set; }
        
        public virtual ICollection<Achievement> Achievements { get; set; }
    }
}