using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;

namespace DAL.Entities
{
    /// <summary>
    /// Represents base achievement
    /// </summary>
    public class FrameworkAchievement : IEntity
    {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public byte[] Image { get; set; }
    
    public virtual ICollection<FrameworkSubTask> SubTasks { get; set; }

    [Required]
    public FrameworkEvaluations Evaluation { get; set; }
    
    [Required]
    public DateTime ValidUntil { get; set; }
      
    [Required]
    [ForeignKey("Reward")]
    public int RewardId { get; set; }
    
    public virtual FrameworkReward Reward { get; set; }
    public virtual ICollection<FrameworkUserCompletedAchievements> UserCompletedAchievements { get; set; }
    
    
    public virtual ICollection<FrameworkUserAskedForReward> UserAskedForRewards { get; set; }
    [Required]
    [ForeignKey("AchievementGroup")]
    public int AchievementGroupId { get; set; }
    
    public virtual FrameworkAchievementGroup AchievementGroup { get; set; }
    
    public virtual IEnumerable<FrameworkNotification> Notifications { get; set; }
    }
}