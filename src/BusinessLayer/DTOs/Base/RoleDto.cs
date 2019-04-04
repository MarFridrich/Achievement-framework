using System.ComponentModel;
using BusinessLayer.DTOs.Common;
using DAL.Entities;

namespace BusinessLayer.DTOs.Base
{
    public class RoleDto : DtoBase
    {
        public string Name { get; set; }
        
        public int AchievementGroupId { get; set; }
        
        public AchievementGroupDto AchievementGroup { get; set; }
    }
}