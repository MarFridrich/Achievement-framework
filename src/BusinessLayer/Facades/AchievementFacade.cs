using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Config;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Generic.Achievement;
using BusinessLayer.Services.Generic.Notification;
using BusinessLayer.Services.Generic.Reward;
using BusinessLayer.Services.Generic.SubTask;
using Castle.Core.Internal;
using DAL.Entities;
using DAL.Entities.JoinTables;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Facades
{
    public class AchievementFacade<TEntity, TAchievementDto, TUserDto, TRewardDto, TSubTask, TSubTaskDto>
        where TEntity : FrameworkAchievement
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
        where TRewardDto : RewardDto
        where TSubTask : FrameworkSubTask
        where TSubTaskDto : SubTaskDto
    {
        protected IAchievementService<TEntity, TAchievementDto, TUserDto> AchievementService;
        protected ISubTaskService<TSubTask, TSubTaskDto> SubTaskService;
        protected readonly IRepository<FrameworkReward> RewardRepository;
        protected readonly IRepository<FrameworkSubTask> SubTasksRepository;
        protected readonly Types ActualTypes;
        protected readonly IMapper Mapper;

        public AchievementFacade(IAchievementService<TEntity, TAchievementDto, TUserDto> achievementService, IRepository<FrameworkReward> rewardRepository,
            IMapper mapper, Types actualTypes, IRepository<FrameworkSubTask> subTasksRepository, ISubTaskService<TSubTask, TSubTaskDto> subTaskService)
        {
            AchievementService = achievementService;
            RewardRepository = rewardRepository;
            Mapper = mapper;
            ActualTypes = actualTypes;
            SubTasksRepository = subTasksRepository;
            SubTaskService = subTaskService;
        }


        public async Task<QueryResult<TAchievementDto>> ApplyFilter(AchievementFilterDto filter)
        {
            return await AchievementService.ApplyFilter(filter);
        }
        public async Task<TAchievementDto> GetAchievementByIdAsync(int id)
        {
            return await AchievementService.Get(id);
        }

        public async Task<TAchievementDto> GetAchievementWithIncludes(int id, params string[] includes)
        {
            return await AchievementService.GetWithIncludes(id, includes);
        }

        public async Task<IEnumerable<(TUserDto, DateTime)>> GetUsersWhichAskedForReward(int achievementId)
        {
            return await AchievementService.GetUsersWhichAskedForReward(achievementId);
        }

        public async Task DeleteAchievementAsync(int id)
        {
            var achievement = await GetAchievementWithIncludes(id);
            await SubTaskService.RemoveSubTasksFromAchievement(id);
            await RewardRepository.Delete(achievement.RewardId);
        }

        public async Task<int> CreateAchievement(TAchievementDto achievement)
        {
            if (achievement.AchievementGroupId == 0)
            {
                return 0;
            }

            var created = await AchievementService.Create(achievement);
            return created;
        }

        public async Task UpdateAchievement(TAchievementDto achievement)
        {
            await AchievementService.Update(achievement);
        }
        
        public async Task<IEnumerable<TAchievementDto>> GetAllAchievementsByUserId(int userId)
        {
            return await AchievementService.GetAllAchievementsOfUser(userId);
        }

        public async Task<int> CreateAchievementWithReward(TAchievementDto achievement, TRewardDto reward)
        {
            var rewardId =
                await RewardRepository.Create(
                    Mapper.Map(reward, typeof(TRewardDto), ActualTypes.FrameworkReward) as FrameworkReward);
            if (rewardId == 0)
            {
                return 0;
            }

            achievement.RewardId = rewardId;
            return await AchievementService.Create(achievement);
        }
        
        public async Task<int> CreateAchievementWithRewardAndSubTasks(TAchievementDto achievement, TRewardDto reward, IEnumerable<TSubTaskDto> subTasks)
        {
            var rewardId =
                await RewardRepository.Create(
                    Mapper.Map(reward, typeof(TRewardDto), ActualTypes.FrameworkReward) as FrameworkReward);
            if (rewardId == 0)
            {
                return 0;
            }
            achievement.RewardId = rewardId;
            var achievementId = await AchievementService.Create(achievement);
            foreach (var subTask in subTasks)
            {
                subTask.AchievementId = achievementId;
                await SubTasksRepository.Create((FrameworkSubTask) Mapper.Map(subTask, typeof(TSubTaskDto),
                    ActualTypes.FrameworkSubTask));
            }
            return achievementId;
        }

        public async Task<byte[]> ExportGroupAchievementsToJsonBytes(int groupId)
        {
            var jsonString = await AchievementService.ExportGroupAchievementsToJson(groupId);
            return AchievementService.MakeBytesFromString(jsonString);
        }
        
        public async Task ImportAchievementsToGroup(Stream file, int groupId)
        {
            await AchievementService.ImportAchievementsFromFileAndAddToGroup(file, groupId);
        }

        public async Task<TAchievementDto> LoadNavigationProperties(int id, IEnumerable<IEnumerable<string>> properties)
        {
            return await AchievementService.LoadNavigationProperties(id, properties);
        }

        public async Task RemoveRewardFromUser(int achievementId, int userId)
        {
            await AchievementService.RemoveReward(userId, achievementId);
        }

    }

}