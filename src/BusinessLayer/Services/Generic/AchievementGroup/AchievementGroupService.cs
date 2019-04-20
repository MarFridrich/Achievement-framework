using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.AchievementGroup
{
    public class AchievementGroupService<TEntity, TAchievementGroupDto, TUserDto> :
        RepositoryServiceBase<TEntity, TAchievementGroupDto, AchievementGroupFilterDto>,
        IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto>
    
        where TEntity : BaHuAchievementGroup
        where TAchievementGroupDto : BaHuAchievementGroupDto
        where TUserDto : BaHUserDto
    {
        protected readonly QueryBase<TEntity, TAchievementGroupDto, AchievementGroupFilterDto> Query;


        public AchievementGroupService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, AchievementGroupQuery<TEntity, TAchievementGroupDto> query) : base(mapper,
            repository,
            context, actualModels)
        {
            Query = query;
        }

        public async Task<QueryResult<TAchievementGroupDto>> ApplyFilter(AchievementGroupFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task<IEnumerable<TAchievementGroupDto>> GetAchievementsGroupsOfUserAsync(int userId)
        {
            var user  = await Context
                .Set<BaHUser>()
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
            return await Context.Set<BaHuAchievementGroup>(ActualModels.BaHuAchievementGroup)
                .Where(g => g.OwnerId == userId)
                .Select(g => Mapper.Map<TAchievementGroupDto>(g))
                .ToListAsync();
        }

        public async Task<bool> InsertUserIntoAchievementGroup(int userId, int groupId)
        {
            var tryIfExists = Context.Set<BaHUserAchievementGroup>(ActualModels.BaHUserAchievementGroup)
                .FirstOrDefault(uag => uag.UserId == userId && uag.AchievementGroupId == groupId);
            if (tryIfExists != null)
            {
                return true;
            }
            var userGroup = (BaHUserAchievementGroup)Activator.CreateInstance(ActualModels.BaHUserAchievementGroup);
            userGroup.UserId = userId;
            userGroup.AchievementGroupId = groupId;
            await Context.AddAsync(Mapper.Map(userGroup, userGroup.GetType(), ActualModels.BaHUserAchievementGroup));
            return true;
        }

        public async Task DeleteUserFromAchievementGroup(int userId, int groupId)
        {
           var userGroup = await Context.Set<BaHUserAchievementGroup>()
               .FirstOrDefaultAsync(g => g.AchievementGroupId == groupId && g.UserId == userId);

           Context.Set<BaHUserAchievementGroup>()
               .Remove(userGroup);
           await Context.SaveChangesAsync();
        }

        public async Task DeleteAllUsersFromAchievementGroup(int groupId)
        {
            var groups = Context.Set<BaHUserAchievementGroup>()
                .Where(g => g.AchievementGroupId == groupId);
            Context.Set<BaHUserAchievementGroup>()
                .RemoveRange(groups);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserIsGroupAdmin(int groupId, int userId)
        {
           var group = await Context.Set<BaHuAchievementGroup>()
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

        public async Task<bool> IsExpired(int groupId)
        {
            var group = await Get(groupId);
            return group.ExpiredIn < DateTime.Now;
        }
    }
}