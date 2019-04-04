using System;
using DAL.Entities.Interfaces;

namespace DAL.Entities.JoinTables
{
    public class FrameworkUserCompletedSubTask
    {
        
        public int UserId { get; set; }
        
        public FrameworkUser User { get; set; }
        
        public int SubTaskId { get; set; }
        
        public FrameworkSubTask SubTask { get; set; }
        
        public DateTime AccomplishedTime { get; set; }
    }
}