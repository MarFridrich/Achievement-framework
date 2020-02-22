using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHuUserCompletedSubTaskDto
    {
        [Required]
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        [Required]
        public int SubTaskId { get; set; }

        public DateTime AccomplishedTime { get; set; }
    }
}