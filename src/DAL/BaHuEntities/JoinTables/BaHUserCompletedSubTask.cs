using System;

namespace DAL.BaHuEntities.JoinTables
{
    public class BaHUserCompletedSubTask
    {
        
        public int UserId { get; set; }
        
        public BaHUser User { get; set; }
        
        public int SubTaskId { get; set; }
        
        public BaHuSubTask SubTask { get; set; }
        
        public DateTime AccomplishedTime { get; set; }
    }
}