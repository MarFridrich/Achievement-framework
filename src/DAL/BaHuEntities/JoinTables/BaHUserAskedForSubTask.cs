using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.BaHuEntities.JoinTables
{
    public class BaHUserAskedForSubTask
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public BaHUser User { get; set; }
        
        [Required]
        [ForeignKey("SubTask")]
        public int SubTaskId { get; set; }
        
        public BaHuSubTask SubTask { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}