using System;
using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Enums;
using BusinessLayer.DTOs.Filter.Enums;

namespace BusinessLayer.DTOs.Filter.Base
{
    public class AchievementFilterDto : FilterDtoBase
    {
        public int? PeopleDoneCount { get; set; }

        public bool PeopleDoneCountLowerThan { get; set; } = false;
        
        public EvaluationsTypeDto? EvaluationType { get; set; }
        

        public DateTime FromDateShow { get; set; } = DateTime.MaxValue;
        
        public int UserId { get; set; }
        
        public int GroupId { get; set; }
        public AchievementType? Type { get; set; }

    }
}