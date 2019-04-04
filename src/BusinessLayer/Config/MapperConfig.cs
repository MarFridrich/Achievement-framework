using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Base.JoinTables;
using BusinessLayer.Helpers;
using DAL.Entities;
using DAL.Entities.JoinTables;

namespace BusinessLayer.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig(Types types)
        {
            CreateMap<AchievementDto, FrameworkAchievement>().ReverseMap();
            CreateMap<AchievementGroupDto, FrameworkAchievementGroup>().ReverseMap();
            CreateMap<EvaluationsDto, FrameworkEvaluations>().ReverseMap();
            CreateMap<NotificationDto, FrameworkNotification>().ReverseMap();
            CreateMap<RewardDto, FrameworkReward>().ReverseMap();
            CreateMap<RoleDto, FrameworkRole>().ReverseMap();
            CreateMap<SubTaskDto, FrameworkSubTask>().ReverseMap();
            CreateMap<UserAchievementGroupDto, FrameworkUserAchievementGroup>().ReverseMap();
            CreateMap<UserCompletedAchievementDto, FrameworkUserCompletedAchievements>().ReverseMap();
            CreateMap<UserDto, FrameworkUser>().ReverseMap();
            CreateMap<UserHasGroupsDto, UserHasGroupsDto>().ReverseMap();
            CreateMap<UserAskedForRewardDto, FrameworkUserAskedForReward>().ReverseMap();

            CreateMap(typeof(AchievementDto), types.FrameworkAchievement).ReverseMap();
            CreateMap(typeof(AchievementGroupDto), types.FrameworkAchievementGroup).ReverseMap();
            CreateMap(typeof(NotificationDto), types.FrameworkNotification).ReverseMap();
            CreateMap(typeof(RewardDto), types.FrameworkReward).ReverseMap();
            CreateMap(typeof(RoleDto), types.FrameworkRole).ReverseMap();
            CreateMap(typeof(SubTaskDto), types.FrameworkSubTask).ReverseMap();
            CreateMap(typeof(UserAchievementGroupDto), types.FrameworkUserAchievementGroup).ReverseMap();
            CreateMap(typeof(UserCompletedAchievementDto), types.FrameworkUserCompletedAchievements)
                .ReverseMap();
            CreateMap(typeof(UserAskedForRewardDto), types.FrameworkUserAskedForReward).ReverseMap();
            CreateMap(typeof(UserAskedForSubTaskDto), types.FrameworkUserAskedForSubTask).ReverseMap();
            CreateMap(typeof(UserCompletedSubTaskDto), types.FrameworkUserCompletedSubTask).ReverseMap();
            
        }
    }
}