using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.Achievement;
using BusinessLayer.Services.Generic.AchievementGroup;
using DAL.Entities;

namespace BusinessLayer.Facades
{
    public class AchievementGroupFacade<TEntity, TAchievementGroupDto, TUserDto>
        where TEntity : FrameworkAchievementGroup
        where TAchievementGroupDto : AchievementGroupDto
        where TUserDto : UserDto
    {
        protected  readonly IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto>
            AchievementGroupService;

        protected IAchievementService<FrameworkAchievement, AchievementDto, TUserDto> AchievementService;

        public AchievementGroupFacade(
            IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto>
                achievementGroupService, IAchievementService<FrameworkAchievement, AchievementDto, TUserDto> achievementService)
        {
            AchievementGroupService = achievementGroupService;
            AchievementService = achievementService;
        }

        public async Task<QueryResult<TAchievementGroupDto>> ApplyFilter(AchievementGroupFilterDto filter)
        {
            return await AchievementGroupService.ApplyFilter(filter);
        }
        
        public IEnumerable<TAchievementGroupDto> ListAllAsync(AchievementGroupFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckIfUserIsGroupAdmin(int groupId, int userId)
        {
            return await AchievementGroupService.CheckIfUserIsGroupAdmin(groupId, userId);
        }

        public async Task<TAchievementGroupDto> GetGroupById(int id)
        {
            return await AchievementGroupService.Get(id);
        }

        public async Task<TAchievementGroupDto> GetGroupByIdWithIncludes(int id, params string[] includes)
        {
            return await AchievementGroupService.GetWithIncludes(id, includes);
        }

        public async Task<int> CreateGroup(TAchievementGroupDto entity)
        {
            return await AchievementGroupService.Create(entity);
        }

        public async Task UpdateGroup(TAchievementGroupDto @group)
        {
            await AchievementGroupService.Update(@group);
        }

        public async Task DeleteGroup(int id)
        {
            var achievements = await AchievementService.ApplyFilter(new AchievementFilterDto
            {
                GroupId = id
            });
            
            foreach (var achievement in achievements.Items)
            {
                await AchievementService.Delete(achievement.Id);
            }
            
            await AchievementGroupService.Delete(id);
        }

        public async Task<QueryResult<TAchievementGroupDto>> GetOwnGroupsOfUser(int userId, int? page = null)
        {
            return await AchievementGroupService.ApplyFilter(new AchievementGroupFilterDto
            {
                UserId = userId
            });
        }

        public async Task<QueryResult<TAchievementGroupDto>> GetGroupsWhichUserOwns(int userId, int? page = null)
        {
            return await AchievementGroupService.ApplyFilter(new AchievementGroupFilterDto
            {
                OwnerId = userId,
                RequestedPageNumber = page
            });
        }

        public async Task<IEnumerable<TUserDto>> GetUsersInAchievementGroup(int groupId)
        {
            return await AchievementGroupService.GetUsersInAchievementGroup(groupId);
        }

        public async Task RemoveUserFromAchievementGroup(int userId, int groupId)
        {
            await AchievementGroupService.DeleteUserFromAchievementGroup(userId, groupId);
        }
        
        public async Task RemoveAllUsersFromGroup(int groupId)
        {
            await AchievementGroupService.DeleteAllUsersFromAchievementGroup(groupId);
        }
    }
}