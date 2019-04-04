using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Entities.JoinTables
{
    public class FrameworkUserAskedForSubTask
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public FrameworkUser User { get; set; }
        
        [Required]
        [ForeignKey("SubTask")]
        public int SubTaskId { get; set; }
        
        public FrameworkSubTask SubTask { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}