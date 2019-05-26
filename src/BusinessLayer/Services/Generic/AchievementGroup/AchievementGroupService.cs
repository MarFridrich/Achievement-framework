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
    public class AchievementGroupService<TEntity, TAchievementGroupDto, TUserDto, TFilterDto> :
        RepositoryServiceBase<TEntity, TAchievementGroupDto, TFilterDto>,
        IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto, TFilterDto>
    
        where TEntity : BaHuAchievementGroup, new()
        where TAchievementGroupDto : BaHuAchievementGroupDto
        where TUserDto : UserDto
        where TFilterDto : AchievementGroupFilterDto, new()
    {
        protected readonly QueryBase<TEntity, TAchievementGroupDto, TFilterDto> Query;


        public AchievementGroupService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, AchievementGroupQuery<TEntity, TAchievementGroupDto, TFilterDto> query) : base(mapper,
            repository,
            context, actualModels)
        {
            Query = query;
        }

        public async Task<QueryResult<TAchievementGroupDto>> ApplyFilter(TFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }


        public async Task<bool> InsertUserIntoAchievementGroup(int userId, int groupId)
        {
            var tryIfExists = Context.Set<BaHuUserAchievementGroup>(ActualModels.BaHuUserAchievementGroup)
                .FirstOrDefault(uag => uag.UserId == userId && uag.AchievementGroupId == groupId);
            if (tryIfExists != null)
            {
                return true;
            }
            var userGroup = (BaHuUserAchievementGroup)Activator.CreateInstance(ActualModels.BaHuUserAchievementGroup);
            userGroup.UserId = userId;
            userGroup.AchievementGroupId = groupId;
            await Context.AddAsync(userGroup);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteUserFromAchievementGroup(int userId, int groupId)
        {
           var userGroup = await Context.Set<BaHuUserAchievementGroup>(ActualModels.BaHuUserAchievementGroup)
               .FirstOrDefaultAsync(g => g.AchievementGroupId == groupId && g.UserId == userId);

           Context.Set<BaHuUserAchievementGroup>()
               .Remove(userGroup);
           await Context.SaveChangesAsync();
        }

        public async Task DeleteAllUsersFromAchievementGroup(int groupId)
        {
            Context.Set<BaHuUserAchievementGroup>(ActualModels.BaHuUserAchievementGroup)
                .Where(g => g.AchievementGroupId == groupId)
                .DeleteFromQuery();
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