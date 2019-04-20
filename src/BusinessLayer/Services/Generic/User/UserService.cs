using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL.BaHuEntities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.User
{
    public class UserService<TUserDto, TAchievementGroupDto, TAchievementDto, TFilterDto> :
        RepositoryServiceBase<BaHUser, TUserDto, UserFilterDto>,
        IUserService<TUserDto, TAchievementGroupDto, TAchievementDto, TFilterDto>
        where TUserDto : BaHUserDto
        where TAchievementGroupDto : BaHuAchievementGroupDto
        where TAchievementDto : BaHuAchievementDto
        where TFilterDto : UserFilterDto

    {
        private readonly IRepository<BaHuAchievementGroup> _achievementGroupRepository;
        private readonly UserQuery<TUserDto, TFilterDto> _userQuery;


        public UserService(IMapper mapper, IRepository<BaHUser> repository, DbContext context, Types actualModels,
            IRepository<BaHuAchievementGroup> achievementGroupRepository, UserQuery<TUserDto, TFilterDto> userQuery) : base(mapper, repository, context,
            actualModels)
        {
            _achievementGroupRepository = achievementGroupRepository;
            _userQuery = userQuery;
        }

        public async Task<QueryResult<TUserDto>> ApplyFilter(TFilterDto filter)
        {
            return await _userQuery.ExecuteAsync(filter);
        }

        public async Task<IEnumerable<TAchievementGroupDto>> GetAchievementGroupsForUser(int userId)
        {
            var user = await Get(userId);
            var userGroups = user?.UserGroups.ToList();
            return userGroups?
                .Select(a => a.AchievementGroup as TAchievementGroupDto)
                .ToList();
        } 
        
        public async Task<IEnumerable<TAchievementDto>> GetAchievementsForUser(int userId)
        {
            var user = await Repository.Get(userId);
            return user?
                .UserGroups
                .SelectMany(a => Mapper.Map<IEnumerable<TAchievementDto>>(a.AchievementGroup.Achievements))
                .ToList();
        }

        public async Task<IEnumerable<TUserDto>> GetUsersFromAchievementGroup(int groupId)
        {
            var group = await _achievementGroupRepository.Get(groupId);
            return group?
                .UserAchievementGroups
                .Select(ug => Mapper.Map<TUserDto>(ug.User))
                .ToList();
        }

    }
}