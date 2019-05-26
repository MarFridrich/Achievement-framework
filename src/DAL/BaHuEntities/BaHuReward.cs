using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.BaHuEntities.Interfaces;

namespace DAL.BaHuEntities
{
    /// <summary>
    /// Class, represents base reward for accomplished achievement
    /// </summary>
    public class BaHuReward : IEntity
    {
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }
        
        public ICollection<BaHuAchievement> Achievements { get; set; }
    }
}