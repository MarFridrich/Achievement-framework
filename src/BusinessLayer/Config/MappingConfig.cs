using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.Entities;
using DAL.Entities.JoinTables;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Config
{
    public class MappingConfig
    {
        private static Types _types;
        
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<AchievementDto, FrameworkAchievement>().ReverseMap();
            config.CreateMap<AchievementGroupDto, FrameworkAchievementGroup>().ReverseMap();
            config.CreateMap<EvaluationsDto, FrameworkEvaluations>().ReverseMap();
            config.CreateMap<NotificationDto, FrameworkNotification>().ReverseMap();
            config.CreateMap<RewardDto, FrameworkReward>().ReverseMap();
            config.CreateMap<RoleDto, FrameworkRole>().ReverseMap();
            config.CreateMap<SubTaskDto, FrameworkSubTask>().ReverseMap();
            config.CreateMap<UserAchievementGroupDto, FrameworkUserAchievementGroup>().ReverseMap();
            config.CreateMap<UserCompletedAchievementDto, FrameworkUserCompletedAchievements>().ReverseMap();
            config.CreateMap<UserDto, FrameworkUser>().ReverseMap();
            config.CreateMap<UserHasGroupsDto, UserHasGroupsDto>().ReverseMap();
            config.CreateMap<UserAskedForRewardDto, FrameworkUserAskedForReward>().ReverseMap();

            config.CreateMap(typeof(AchievementDto), _types.FrameworkAchievement).ReverseMap();
            config.CreateMap(typeof(AchievementGroupDto), _types.FrameworkAchievementGroup).ReverseMap();
            config.CreateMap(typeof(NotificationDto), _types.FrameworkNotification).ReverseMap();
            config.CreateMap(typeof(RewardDto), _types.FrameworkReward).ReverseMap();
            config.CreateMap(typeof(RoleDto), _types.FrameworkRole).ReverseMap();
            config.CreateMap(typeof(SubTaskDto), _types.FrameworkSubTask).ReverseMap();
            config.CreateMap(typeof(UserAchievementGroupDto), _types.FrameworkUserAchievementGroup).ReverseMap();
            config.CreateMap(typeof(UserCompletedAchievementDto), _types.FrameworkUserCompletedAchievements)
                .ReverseMap();
            config.CreateMap(typeof(UserAskedForRewardDto), _types.FrameworkUserAskedForReward).ReverseMap();

        }
    }
}