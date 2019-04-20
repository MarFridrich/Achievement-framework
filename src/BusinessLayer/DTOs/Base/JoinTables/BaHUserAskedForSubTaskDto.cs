using System;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHUserAskedForSubTaskDto
    {
        public int UserId { get; set; }
        
        public BaHUserDto User { get; set; }
        
        public int SubTaskId { get; set; }
        
        public BaHuSubTaskDto SubTask { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}