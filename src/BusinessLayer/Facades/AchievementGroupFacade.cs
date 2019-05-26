using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Generic.Achievement;
using BusinessLayer.Services.Generic.AchievementGroup;
using DAL.BaHuEntities;

namespace BusinessLayer.Facades
{
    public class AchievementGroupFacade<TEntity, TAchievementGroupDto, TAchievementGroupFilterDto, TUserDto>
        where TEntity : BaHuAchievementGroup, new()
        where TAchievementGroupDto : BaHuAchievementGroupDto
        where TAchievementGroupFilterDto : AchievementGroupFilterDto, new()
        where TUserDto : UserDto
        
    {
        protected  readonly IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto, TAchievementGroupFilterDto>
            AchievementGroupService;

        protected IAchievementService<BaHuAchievement, BaHuAchievementDto, TUserDto, AchievementFilterDto> AchievementService;
        protected IRepository<BaHuSubTask> SubTaskRepository;
        public AchievementGroupFacade(
            IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto, TAchievementGroupFilterDto>
                achievementGroupService,
            IAchievementService<BaHuAchievement, BaHuAchievementDto, TUserDto, AchievementFilterDto>
                achievementService,
            IRepository<BaHuSubTask> subTaskRepository)
        {
            AchievementGroupService = achievementGroupService;
            AchievementService = achievementService;
            SubTaskRepository = subTaskRepository;
        }

        public async Task<QueryResult<TAchievementGroupDto>> ApplyFilter(TAchievementGroupFilterDto filter)
        {
            return await AchievementGroupService.ApplyFilter(filter);
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

        private async Task DeleteSubTasks(ICollection<BaHuSubTaskDto> subTasks)
        {
            foreach (var subTask in subTasks)
            {
                await SubTaskRepository.Delete(subTask.Id);
            }
        }

        public async Task DeleteGroup(int id)
        {
            var achievements = await AchievementService.ApplyFilter(new AchievementFilterDto()
            {
                GroupId = id,
                Includes = new []{nameof(BaHuAchievement.SubTasks)}
            });
            
            foreach (var achievement in achievements.Items)
            {
                await DeleteSubTasks(achievement.SubTasks);
                await AchievementService.Delete(achievement.Id);
            }
            
            await AchievementGroupService.Delete(id);
        }

        public async Task<QueryResult<TAchievementGroupDto>> GetOwnGroupsOfUser(int userId, int? page = null)
        {
            return await AchievementGroupService.ApplyFilter(new TAchievementGroupFilterDto
            {
                UserId = userId
            });
        }

        public async Task<QueryResult<TAchievementGroupDto>> GetGroupsWhichUserOwns(int userId, int? page = null)
        {
            return await AchievementGroupService.ApplyFilter(new TAchievementGroupFilterDto
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

        public async Task<bool> IsExpired(int groupId)
        {
            return await AchievementGroupService.IsExpired(groupId);
        }

        public async Task InsertUserIntoAchievementGroup(int userId, int groupId)
        {
            await AchievementGroupService.InsertUserIntoAchievementGroup(userId, groupId);
        }

        public async Task<QueryResult<TAchievementGroupDto>> GetAchievementsGroupsOfUser(int userId)
        {
            return await AchievementGroupService.ApplyFilter(new TAchievementGroupFilterDto
            {
                UserId = userId
            });
        }

        public async Task<QueryResult<TAchievementGroupDto>> GetGroupsWhichUserOwns(int userId)
        {
            return await AchievementGroupService.ApplyFilter(new TAchievementGroupFilterDto
            {
                OwnerId = userId
            });
        }
    }
}