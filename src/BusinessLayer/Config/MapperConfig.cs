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
            CreateMap<BaHuUserAchievementGroupDto, BaHuUserAchievementGroup>().ReverseMap();
            CreateMap<BaHuUserCompletedAchievementDto, BaHuUserCompletedAchievement>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<BaHuUserAskedForRewardDto, BaHuUserAskedForReward>().ReverseMap();

            CreateMap(typeof(BaHuAchievementDto), types.BaHuAchievement).ReverseMap();
            CreateMap(typeof(BaHuAchievementGroupDto), types.BaHuAchievementGroup).ReverseMap();
            CreateMap(typeof(BaHuNotificationDto), types.BaHuNotification).ReverseMap();
            CreateMap(typeof(BaHuRewardDto), types.BaHuReward).ReverseMap();
            CreateMap(typeof(BaHuRoleDto), types.BaHuRole).ReverseMap();
            CreateMap(typeof(BaHuSubTaskDto), types.BaHuSubTask).ReverseMap();
            CreateMap(typeof(BaHuUserAchievementGroupDto), types.BaHuUserAchievementGroup).ReverseMap();
            CreateMap(typeof(BaHuUserCompletedAchievementDto), types.BaHuUserCompletedAchievements)
                .ReverseMap();
            CreateMap(typeof(BaHuUserAskedForRewardDto), types.BaHuUserAskedForReward).ReverseMap();
            CreateMap(typeof(BaHuUserAskedForSubTaskDto), types.BaHuUserAskedForSubTask).ReverseMap();
            CreateMap(typeof(BaHuUserCompletedSubTaskDto), types.BaHuUserCompletedSubTask).ReverseMap();
            
        }
    }
}