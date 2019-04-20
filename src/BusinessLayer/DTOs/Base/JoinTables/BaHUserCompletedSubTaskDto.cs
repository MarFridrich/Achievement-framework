using System;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class BaHUserCompletedSubTaskDto
    {
        public int UserId { get; set; }
        
        public BaHUserDto User { get; set; }
        
        public int SubTaskId { get; set; }
        
        public BaHuSubTaskDto SubTask { get; set; }
        
        public DateTime AccomplishedTime { get; set; }
    }
}