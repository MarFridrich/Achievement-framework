using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.DTOs.Filter.Enums;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL.BaHuEntities;
using DAL.BaHuEntities.Interfaces;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BusinessLayer.Services.Generic.Achievement
{
    public class AchievementService<TEntity, TAchievementDto, TUserDto, TFilterDto> :
        RepositoryServiceBase<TEntity, TAchievementDto, TFilterDto>, IAchievementService<TEntity, TAchievementDto, TUserDto, TFilterDto>
        where TEntity : BaHuAchievement, new()
        where TAchievementDto : BaHuAchievementDto
        where TUserDto : UserDto
        where TFilterDto : AchievementFilterDto, new()
    {

        protected readonly IRepository<BaHuReward> RewardRepository;
        protected readonly QueryBase<TEntity, TAchievementDto, TFilterDto> Query;
        
        public AchievementService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, AchievementsQuery<TEntity, TAchievementDto, TFilterDto> query,
            IRepository<BaHuReward> rewardRepository) : base(mapper, repository, context, actualModels)
        {
            RewardRepository = rewardRepository;
            Query = query;
        }
        
        public async Task<IEnumerable<TUserDto>> GetUserWhichCompletedAchievement(int achievementId)
        {
            var achievement = await Repository.Get(achievementId);
            return achievement?
                .UserCompletedAchievements
                .Select(u => Mapper.Map<TUserDto>(u.User));
        }

        public async Task<QueryResult<TAchievementDto>> ApplyFilter(TFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task<bool> CheckIfUserHasAchievement(int userId, int achievementId)
        {
            var achievement = await Context.Set<BaHuUserAchievementGroup>()
                .Where(uag => uag.UserId == userId)
                .Join(Context.Set<BaHuAchievement>(), uag => uag.AchievementGroupId, ach => ach.AchievementGroupId,
                    (_, ach) => ach)
                .FirstOrDefaultAsync(ach => ach.Id == achievementId);
            return achievement != null;
        }

        public async Task<IEnumerable<ValueTuple<TUserDto, DateTime>>> GetUsersWhichAskedForReward(int achievementId)
        {
            return await Context.Set<BaHuUserAskedForReward>()
                .Where(uar => uar.AchievementId == achievementId)
                .Join(Context.Set<DAL.BaHuEntities.User>(), uar => uar.UserId, u => u.Id, (uar, u) => ValueTuple.Create(Mapper.Map<TUserDto>(u), uar.DateTime))
                .ToListAsync();
        }

        public async Task<bool> AskForRewardByUser(int userId, int achievementId)
        {
            var tryIfExists = await Context.Set<BaHuUserAskedForReward>(ActualModels.BaHuUserAskedForReward)
                .FirstOrDefaultAsync(uca => uca.AchievementId == achievementId && uca.UserId == userId);
            if (tryIfExists != null)
            {
                return false;
            }
            
            if (userId == 0 || achievementId == 0)
            {
                return false;
            }

            var userAskedForReward =
                (BaHuUserAskedForReward) Activator.CreateInstance(ActualModels.BaHuUserAskedForReward);
            userAskedForReward.UserId = userId;
            userAskedForReward.AchievementId = achievementId;
            userAskedForReward.DateTime = DateTime.Now;


            await Context.AddAsync(userAskedForReward);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAskForReward(int userId, int achievementId)
        {
            var found = await Context.Set<BaHuUserAskedForReward>(ActualModels.BaHuUserAskedForReward)
                .FirstOrDefaultAsync(uar => uar.AchievementId == achievementId && uar.UserId == userId);
            if (found == null)
            {
                return false;
            }

            Context.Remove(found);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAchievementToUser(int userId, int achievementId)
        {
            var tryIfExists = Context
                .Set<BaHuUserCompletedAchievement>(ActualModels.BaHuUserCompletedAchievements)
                .FirstOrDefault(uca => uca.AchievementId == achievementId && uca.UserId == userId);
            if (tryIfExists != null)
            {
                return false;
            }
            
            var userCompletedAchievement =
                (BaHuUserCompletedAchievement) Activator.CreateInstance(ActualModels.BaHuUserCompletedAchievements);

            userCompletedAchievement.UserId = userId;
            userCompletedAchievement.AchievementId = achievementId;
            userCompletedAchievement.AccomplishDate = DateTime.Now;

            await Context.AddAsync(userCompletedAchievement);
            await Context.SaveChangesAsync();
            return true;

        }

        public async Task RemoveReward(int userId, int achievementId)
        {
            var tryIfExists = await Context.Set<BaHuUserCompletedAchievement>()
                .Where(uca => uca.UserId == userId && uca.AchievementId == achievementId)
                .FirstOrDefaultAsync();
            if (tryIfExists != null)
            {
                Context.Remove(tryIfExists);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<string> ExportGroupAchievementsToJson(int groupId)
        {
            var achievements = await ApplyFilter(new TFilterDto
            {
                GroupId = groupId,
                Includes = new []{ nameof(BaHuAchievement.Reward), nameof(BaHuAchievement.SubTasks)}
            });
            
            return JsonConvert.SerializeObject(achievements.Items);
        }

        public async Task ImportAchievementsFromFileAndAddToGroup(Stream file, int groupId)
        {
            using (var reader = new StreamReader(file))   
            {
                var all = await reader.ReadToEndAsync();
                var achievements = JsonConvert.DeserializeObject<List<TEntity>>(all);
                foreach (var achievement in achievements)
                {
                    var property = achievement.GetType().GetProperty("Reward", ActualModels.BaHuReward);
                    BaHuReward reward;
                    if (property.CanRead)
                    {
                        reward = (BaHuReward) property.GetValue(achievement);
                    }
                    else
                    {
                        reward = achievement.Reward;
                    }
                    
                    var rewardId = await RewardRepository.Create(reward);
                    var subTasksProperty = achievement.GetType().GetProperty("SubTasks",
                        typeof(ICollection<>).MakeGenericType(ActualModels.BaHuSubTask));
                    ICollection subTasks;
                    if (subTasksProperty.CanRead)
                    {
                        subTasks =  (ICollection) subTasksProperty.GetValue(achievement);
                    }
                    else
                    {
                        subTasks = (ICollection) achievement.SubTasks;
                    }
                   
                    achievement.RewardId = rewardId;
                    achievement.AchievementGroupId = groupId;
                    var id = await Create(Mapper.Map<TAchievementDto>(achievement));
                    foreach (var subTask in subTasks)
                    {
                        var casted = (BaHuSubTask) subTask;
                        casted.AchievementId = id;
                        Context.Add(casted);
                    }

                    Context.SaveChanges();
                }                  
                
            }

            await Context.SaveChangesAsync();

        }

        public byte[] MakeBytesFromString(string input)
        {
            return Encoding.Default.GetBytes(input);
        }

    }
}