using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.BaHuEntities.JoinTables
{
    public class BaHuUserAskedForSubTask
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        [Required]
        [ForeignKey("SubTask")]
        public int SubTaskId { get; set; }
        
        public BaHuSubTask SubTask { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}