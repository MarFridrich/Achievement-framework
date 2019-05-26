using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Generic.Achievement;
using BusinessLayer.Services.Generic.SubTask;
using BusinessLayer.Services.Generic.User;
using DAL.BaHuEntities;

namespace BusinessLayer.Facades
{
    public class AchievementFacade<TEntity, TAchievementDto, TAchievementFilterDto, TUserDto, TRewardDto, TSubTask, TSubTaskDto>
        where TEntity : BaHuAchievement, new()
        where TAchievementDto : BaHuAchievementDto
        where TAchievementFilterDto : AchievementFilterDto, new()
        where TUserDto : UserDto
        where TRewardDto : BaHuRewardDto
        where TSubTask : BaHuSubTask
        where TSubTaskDto : BaHuSubTaskDto
        
    {
        protected IAchievementService<TEntity, TAchievementDto, TUserDto, TAchievementFilterDto> AchievementService;
        protected ISubTaskService<TSubTask, TSubTaskDto, SubTaskFilterDto> SubTaskService;
        protected readonly IRepository<BaHuReward> RewardRepository;
        protected readonly IRepository<BaHuSubTask> SubTasksRepository;
        protected readonly Types ActualModels;
        protected readonly IMapper Mapper;

        public AchievementFacade(
            IAchievementService<TEntity, TAchievementDto, TUserDto, TAchievementFilterDto> achievementService,
            ISubTaskService<TSubTask, TSubTaskDto, SubTaskFilterDto> subTaskService,
            IRepository<BaHuReward> rewardRepository,
            IRepository<BaHuSubTask> subTasksRepository,
            Types actualModels,
            IMapper mapper)
        {
            AchievementService = achievementService;
            SubTaskService = subTaskService;
            RewardRepository = rewardRepository;
            SubTasksRepository = subTasksRepository;
            ActualModels = actualModels;
            Mapper = mapper;
        }


        public async Task<QueryResult<TAchievementDto>> ApplyFilter(TAchievementFilterDto filter)
        {
            return await AchievementService.ApplyFilter(filter);
        }
        public async Task<TAchievementDto> GetAchievementByIdAsync(int id)
        {
            return await AchievementService.Get(id);
        }

        public async Task CreateListOfAchievements(IEnumerable<TAchievementDto> achievements)
        {
            await AchievementService.CreateList(achievements);
        }

        public async Task<TAchievementDto> GetAchievementWithIncludes(int id, params string[] includes)
        {
            return await AchievementService.GetWithIncludes(id, includes);
        }

        public async Task<IEnumerable<(TUserDto, DateTime)>> GetUsersWhichAskedForReward(int achievementId)
        {
            return await AchievementService.GetUsersWhichAskedForReward(achievementId);
        }

        public async Task<IEnumerable<TUserDto>> GetUsersWhichCompletedAchievement(int achievementId)
        {
            return await AchievementService.GetUserWhichCompletedAchievement(achievementId);
        }
        
        public async Task<QueryResult<TAchievementDto>> GetAllAchievementsOfGroup(int groupId)
        {
            return await AchievementService.ApplyFilter(new TAchievementFilterDto
            {
                GroupId = groupId
            });
        }

        public async Task<QueryResult<TAchievementDto>> GetNonCompletedAchievementsOfUser(int userId)
        {
            return await AchievementService.ApplyFilter(new TAchievementFilterDto
            {
                UserId = userId,
                Type = DTOs.Filter.Enums.AchievementType.NonCompleted
            });
        }

        public async Task<QueryResult<TAchievementDto>> GetAllAchievementsForUser(int userId)
        {
            return await AchievementService.ApplyFilter(new TAchievementFilterDto
            {
                UserId = userId
            });
        }

        public IQueryable<TAchievementDto> ListAllAchievements()
        {
            return AchievementService.ListAll();
        }

        public async Task DeleteAchievementAsync(int id)
        {
            var achievement = await GetAchievementWithIncludes(id);
            await SubTaskService.RemoveSubTasksFromAchievement(id);
            await RewardRepository.Delete(achievement.RewardId);
            await AchievementService.Delete(id);
        }

        private static BaHuRewardDto GetRewardFromAchievement(TAchievementDto achievement)
        {
            var properties = achievement.GetType().GetProperties().Where(p => p.Name == "Reward");
            BaHuRewardDto reward;
            reward = properties
                .FirstOrDefault(p => p.GetValue(achievement) != null)?
                .GetValue(achievement) as BaHuRewardDto;
            

            return reward;
        }

        private static IEnumerable<BaHuSubTaskDto> GetSubTasksFromAchievement(TAchievementDto achievement)
        {
            var properties = achievement.GetType().GetProperties().Where(p => p.Name == "SubTasks");
            IEnumerable<BaHuSubTaskDto> subtasks;
            subtasks = properties
                .FirstOrDefault(p => p.GetValue(achievement) != null)?
                .GetValue(achievement) as IEnumerable<BaHuSubTaskDto>;

            return subtasks;
        }

        public async Task<int> CreateAchievement(TAchievementDto achievement)
        {
            if (achievement.AchievementGroupId == 0)
            {
                return 0;
            }

            var reward = GetRewardFromAchievement(achievement);
            var subTasks = GetSubTasksFromAchievement(achievement);
            return await CreateAchievementWithRewardAndSubTasks(achievement, reward, subTasks);
        }

        public async Task UpdateAchievement(TAchievementDto achievement)
        {
            await AchievementService.Update(achievement);
        }
        
        public async Task<QueryResult<TAchievementDto>> GetAllAchievementsByUserId(int userId)
        {
            return await AchievementService.ApplyFilter(new TAchievementFilterDto
            {
                UserId = userId
            });
        }

        private static void RemoveNavigationPropertiesFromAchievement(TAchievementDto achievement)
        {
            achievement.Reward = null;
            achievement.SubTasks = null;
            achievement.AchievementGroup = null;
            achievement.UserCompletedAchievements = null;
            achievement.UserAskedForRewards = null;
        }
        
        private async Task<int> CreateAchievementWithRewardAndSubTasks(TAchievementDto achievement, BaHuRewardDto reward, IEnumerable<BaHuSubTaskDto> subTasks)
        {
            RemoveNavigationPropertiesFromAchievement(achievement);
            var rewardId = achievement.RewardId;
            if (rewardId == 0)
            {
                rewardId =
                    await RewardRepository.Create(
                        Mapper.Map(reward, typeof(TRewardDto), ActualModels.BaHuReward) as BaHuReward);
            } 
            if (rewardId == 0)
            {
                return 0;
            }
            achievement.RewardId = rewardId;
            var achievementId = await AchievementService.Create(achievement);
            if (subTasks == null) return achievementId;
            foreach (var subTask in subTasks)
            {
                subTask.AchievementId = achievementId;
                await SubTasksRepository.Create((BaHuSubTask) Mapper.Map(subTask, typeof(TSubTaskDto),
                    ActualModels.BaHuSubTask));
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

        public async Task RemoveRewardFromUser(int achievementId, int userId)
        {
            await AchievementService.RemoveReward(userId, achievementId);
        }

        public async Task<bool> CheckIfUserHasAchievement(int userId, int achievementId)
        {
            return await AchievementService.CheckIfUserHasAchievement(userId, achievementId);
        }

        public async Task<bool> AskForRewardByUser(int userId, int achievementId)
        {
            return await AchievementService.AskForRewardByUser(userId, achievementId);
        }

        public async Task<bool> RemoveAskForRewardForUser(int userId, int achievementId)
        {
            return await AchievementService.RemoveAskForReward(userId, achievementId);
        }

        public async Task<bool> ApproveAchievementToUser(int userId, int achievementId)
        {
            return await AchievementService.ApproveAchievementToUser(userId, achievementId);
        }
        
        public async Task ApproveAchievementToUserAndRemoveAskForReward(int userId, int achievementId)
        {
            await AchievementService.ApproveAchievementToUser(userId, achievementId);
            await AchievementService.RemoveAskForReward(userId, achievementId);
        }
        

    }

}