using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.Helpers;
using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;

namespace BusinessLayer.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig(Types types)
        {
            CreateMap<BaHuAchievementDto, BaHuAchievement>().ReverseMap();
            CreateMap<BaHuAchievementGroupDto, BaHuAchievementGroup>().ReverseMap();
            CreateMap<BaHuEvaluationsDto, BaHuEvaluations>().ReverseMap();
            CreateMap<BaHuNotificationDto, BaHuNotification>().ReverseMap();
            CreateMap<BaHuRewardDto, BaHuReward>().ReverseMap();
            CreateMap<BaHuRoleDto, BaHuRole>().ReverseMap();
            CreateMap<BaHuSubTaskDto, BaHuSubTask>().ReverseMap();
            CreateMap<BaHUserAchievementGroupDto, BaHUserAchievementGroup>().ReverseMap();
            CreateMap<BaHUserCompletedAchievementDto, BaHUserCompletedAchievement>().ReverseMap();
            CreateMap<BaHUserDto, BaHUser>().ReverseMap();
            CreateMap<BaHUserHasGroupsDto, BaHUserHasGroupsDto>().ReverseMap();
            CreateMap<BaHUserAskedForRewardDto, BaHUserAskedForReward>().ReverseMap();

            CreateMap(typeof(BaHuAchievementDto), types.BaHuAchievement).ReverseMap();
            CreateMap(typeof(BaHuAchievementGroupDto), types.BaHuAchievementGroup).ReverseMap();
            CreateMap(typeof(BaHuNotificationDto), types.BaHuNotification).ReverseMap();
            CreateMap(typeof(BaHuRewardDto), types.BaHuReward).ReverseMap();
            CreateMap(typeof(BaHuRoleDto), types.BaHuRole).ReverseMap();
            CreateMap(typeof(BaHuSubTaskDto), types.BaHuSubTask).ReverseMap();
            CreateMap(typeof(BaHUserAchievementGroupDto), types.BaHUserAchievementGroup).ReverseMap();
            CreateMap(typeof(BaHUserCompletedAchievementDto), types.BaHUserCompletedAchievements)
                .ReverseMap();
            CreateMap(typeof(BaHUserAskedForRewardDto), types.BaHUserAskedForReward).ReverseMap();
            CreateMap(typeof(BaHUserAskedForSubTaskDto), types.BaHUserAskedForSubTask).ReverseMap();
            CreateMap(typeof(BaHUserCompletedSubTaskDto), types.BaHUserCompletedSubTask).ReverseMap();
            
        }
    }
}