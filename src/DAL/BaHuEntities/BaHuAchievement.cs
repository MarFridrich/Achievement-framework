using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;

namespace DAL.BaHuEntities
{
    /// <summary>
    /// Represents base achievement
    /// </summary>
    public class BaHuAchievement : IEntity
    {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public byte[] Image { get; set; }
    
    public ICollection<BaHuSubTask> SubTasks { get; set; }

    [Required]
    public BaHuEvaluations Evaluation { get; set; }
    
    [Required]
    public DateTime ValidUntil { get; set; }
      
    [Required]
    [ForeignKey("Reward")]
    public int RewardId { get; set; }
    
    public BaHuReward Reward { get; set; }
    public ICollection<BaHUserCompletedAchievement> UserCompletedAchievements { get; set; }
    
    public ICollection<BaHUserAskedForReward> UserAskedForRewards { get; set; }
    [Required]
    [ForeignKey("AchievementGroup")]
    public int AchievementGroupId { get; set; }
    
    public BaHuAchievementGroup AchievementGroup { get; set; }
    
    public IEnumerable<BaHuNotification> Notifications { get; set; }
    }
}