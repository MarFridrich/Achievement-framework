using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHuUserAskedForSubTaskDto
    {
        [Required]
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        [Required]
        public int SubTaskId { get; set; }
        
        public BaHuSubTaskDto SubTask { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}