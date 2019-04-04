using System;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class UserCompletedSubTaskDto
    {
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int SubTaskId { get; set; }
        
        public SubTaskDto SubTask { get; set; }
        
        public DateTime AccomplishedTime { get; set; }
    }
}