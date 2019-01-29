using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Common;
using DAL;
using GenericServices;

namespace BusinessLayer.Services.Generic.Achievement
{
    public class AchievementService<TAchievementDto, TUserDto> :
        CrudServiceBase<DAL.Entities.Achievement, TAchievementDto>, IAchievementService<TAchievementDto, TUserDto>
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
    {

        private readonly ICrud<DAL.Entities.User, UserDto> _userRepository;
        private readonly QueryBase<DAL.Entities.Achievement, TAchievementDto, AchievementFilterDto> _query;
        public AchievementService(IMapper mapper, ICrudServicesAsync service, AchievementDbContext context,
            QueryBase<DAL.Entities.Achievement, TAchievementDto, AchievementFilterDto> query,
            ICrud<DAL.Entities.User, UserDto> userRepository) 
            : base(mapper, service, context)
        {
            _userRepository = userRepository;
            _query = query;
        }

        public async Task<List<TUserDto>> GetUserWhichCompletedAchievement(int achievementId)
        {
            var achievement = await Get(achievementId);
            return achievement?
                .UserCompletedAchievements
                .Select(u => u.User)
                .Cast<TUserDto>()
                .ToList();
        }

        public async Task<IEnumerable<TUserDto>> GetAllUsersWhichHaveAchievement(int achievementId)
        {
            var achievement = await Get(achievementId);
            return achievement?
                .AchievementGroup
                .Users
                .Cast<TUserDto>()
                .ToList();
        }

        public async Task<TUserDto> GetAchievementGroupOwner(int achievementId)
        {
            var achievement = await GetWithIncludes(achievementId, nameof(DAL.Entities.Achievement.AchievementGroup),
                nameof(DAL.Entities.Achievement.AchievementGroup.Owner));
            return (TUserDto) achievement?
                .AchievementGroup
                .Owner;
        }

        public async Task<IEnumerable<TAchievementDto>> GetAllAchievementsOfUser(int userId)
        {
            var user = await _userRepository.Get(userId);
            return user?
                .UserGroups
                .SelectMany(g => g.AchievementGroup.Achievements)
                .Cast<TAchievementDto>()
                .ToList();
        }

        public async Task<QueryResult<TAchievementDto>> GetNonCompletedAchievementsOfUser(int userId)
        {
             //var user = await _userRepository.Get(userId);
             //if (user == null)
             //{
             //    return new List<TAchievementDto>();
             //}

             //return user
             //    .UserGroups
             //    .SelectMany(g => g.AchievementGroup.Achievements)
             //    .Except(user
             //        .UserCompletedAchievements
             //        .Select(c => c.Achievement))
             //    .Cast<TAchievementDto>()
             //    .ToList();

             return await _query.ExecuteAsync(new AchievementFilterDto
             {
                 OnlyNonCompletedForUserId = ValueTuple.Create(true, userId)
             });
        }

        public async Task<IEnumerable<TAchievementDto>> GetCompletedAchievementsOfUser(int userId)
        {
            var user = await _userRepository.GetWithIncludes(userId, nameof(DAL.Entities.User.UserCompletedAchievements));
            return user
                .UserCompletedAchievements
                .Select(ua => ua.Achievement)
                .Cast<TAchievementDto>()
                .ToList();
        }
        
    }
}