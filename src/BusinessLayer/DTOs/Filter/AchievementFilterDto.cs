using System.ComponentModel.DataAnnotations;
using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Enums;
using BusinessLayer.DTOs.Filter.Enums;

namespace BusinessLayer.DTOs.Filter
{
    public class AchievementFilterDto : FilterDtoBase
    {
        public int? PeopleDoneCount { get; set; }

        public bool PeopleDoneCountLowerThan { get; set; } = false;
        
        public EvaluationsType? EvaluationType { get; set; }

    }
}