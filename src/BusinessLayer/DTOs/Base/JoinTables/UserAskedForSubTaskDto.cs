using System;

namespace BusinessLayer.DTOs.Base.JoinTables
{
    public class UserAskedForSubTaskDto
    {
        public int UserId { get; set; }
        
        public UserDto User { get; set; }
        
        public int SubTaskId { get; set; }
        
        public SubTaskDto SubTask { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}