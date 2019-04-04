using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.Config;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using BusinessLayer.Services.Generic.Achievement;
using DAL;
using DAL.Entities;
using DAL.Entities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.AchievementGroup
{
    public class AchievementGroupService<TEntity, TAchievementGroupDto, TUserDto> :
        RepositoryServiceBase<TEntity, TAchievementGroupDto, AchievementGroupFilterDto>,
        IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto>
    
        where TEntity : FrameworkAchievementGroup
        where TAchievementGroupDto : AchievementGroupDto
        where TUserDto : UserDto
    {
        protected readonly IRepository<FrameworkUser> UserRepository;
        protected readonly IRepository<FrameworkUserAchievementGroup> UserAchievementGroupRepository;
        protected readonly QueryBase<TEntity, TAchievementGroupDto, AchievementGroupFilterDto> Query;


        public AchievementGroupService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, AchievementGroupQuery<TEntity, TAchievementGroupDto> query,
            IRepository<FrameworkUser> userRepository,
            IRepository<FrameworkUserAchievementGroup> userAchievementGroupRepository) : base(mapper,
            repository,
            context, actualModels)
        {
            UserRepository = userRepository;
            UserAchievementGroupRepository = userAchievementGroupRepository;
            Query = query;
        }

        public async Task<QueryResult<TAchievementGroupDto>> ApplyFilter(AchievementGroupFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task<IEnumerable<TAchievementGroupDto>> GetAchievementsGroupsOfUserAsync(int userId)
        {
            var user  = await Context
                .Set<FrameworkUser>()
                .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.AchievementGroup)
                .ThenInclude(ag => ag.Achievements)
                .ThenInclude(ach => ach.Reward)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return user?
                .UserGroups?
                .Select(g => Mapper.Map<TAchievementGroupDto>(g.AchievementGroup))
                .ToList();
        }

        public async Task<IEnumerable<TAchievementGroupDto>> GetGroupsWhereUserIsAdminAsync(int userId)
        {
            return await Context.Set<FrameworkAchievementGroup>(ActualModels.FrameworkAchievementGroup)
                .Where(g => g.OwnerId == userId)
                .Select(g => Mapper.Map<TAchievementGroupDto>(g))
                .ToListAsync();
        }

        public async Task<bool> InsertUserIntoAchievementGroup(int userId, int groupId)
        {
            var userGroup = (FrameworkUserAchievementGroup)Activator.CreateInstance(ActualModels.FrameworkUserAchievementGroup);
            userGroup.UserId = userId;
            userGroup.AchievementGroupId = groupId;
            await UserAchievementGroupRepository.Create(userGroup);
            return true;
        }

        public async Task DeleteUserFromAchievementGroup(int userId, int groupId)
        {
           var userGroup = await Context.Set<FrameworkUserAchievementGroup>()
               .FirstOrDefaultAsync(g => g.AchievementGroupId == groupId && g.UserId == userId);

           Context.Set<FrameworkUserAchievementGroup>()
               .Remove(userGroup);
           await Context.SaveChangesAsync();
        }

        public async Task DeleteAllUsersFromAchievementGroup(int groupId)
        {
            var groups = Context.Set<FrameworkUserAchievementGroup>()
                .Where(g => g.AchievementGroupId == groupId);
            Context.Set<FrameworkUserAchievementGroup>()
                .RemoveRange(groups);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserIsGroupAdmin(int groupId, int userId)
        {
           var group = await Context.Set<FrameworkAchievementGroup>()
               .FirstOrDefaultAsync(g => g.OwnerId == userId && g.Id == groupId);
           return @group != null;
        }

        public async Task<IEnumerable<TUserDto>> GetUsersInAchievementGroup(int groupId)
        {
            var group = await Context.Set<TEntity>()
                .Include(g => g.UserAchievementGroups)
                .ThenInclude(uag => uag.User)
                .FirstOrDefaultAsync(g => g.Id == groupId);
            return Mapper.Map<IEnumerable<TUserDto>>(group.UserAchievementGroups.Select(uag => uag.User));
        }

    }
}