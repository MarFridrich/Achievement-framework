using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Entities
{
    public class Role : IdentityRole<int>, IEntity
    {
        [ForeignKey("AchievementGroup")]
        public int AchievementGroupId { get; set; }
        
        public AchievementGroup AchievementGroup { get; set; }
    }
}