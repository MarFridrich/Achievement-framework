using System;
using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Enums;
using BusinessLayer.DTOs.Filter.Enums;
using GenericServices.Setup;

namespace BusinessLayer.DTOs.Filter
{
    public class AchievementFilterDto : FilterDtoBase
    {
        public int? PeopleDoneCount { get; set; }

        public bool PeopleDoneCountLowerThan { get; set; } = false;
        
        public EvaluationsType? EvaluationType { get; set; }
        

        public DateTime FromDateShow { get; set; } = DateTime.MaxValue;
        
        
        public ValueTuple<bool, int> OnlyNonCompletedForUserId { get; set; }

    }
}