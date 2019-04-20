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
    public class AchievementService<TEntity, TAchievementDto, TUserDto> :
        RepositoryServiceBase<TEntity, TAchievementDto, AchievementFilterDto>, IAchievementService<TEntity, TAchievementDto, TUserDto>
        where TEntity : BaHuAchievement, IEntity, new()
        where TAchievementDto : BaHuAchievementDto
        where TUserDto : BaHUserDto
    {

        protected readonly IRepository<BaHUser> UserRepository;
        protected readonly IRepository<BaHuAchievementGroup> AchievementGroupRepository;
        protected readonly IRepository<BaHuReward> RewardRepository;
        protected readonly QueryBase<TEntity, TAchievementDto, AchievementFilterDto> Query;
        

        private readonly string[] _navigationProperties = new[]
        {
            nameof(BaHuAchievement.AchievementGroup), nameof(BaHuAchievement.UserCompletedAchievements),
            nameof(BaHuAchievement.Reward), nameof(BaHuAchievement.Notifications),
            nameof(BaHuAchievement.SubTasks)
        };
        
        
        public AchievementService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, AchievementsQuery<TEntity, TAchievementDto> query,
            IRepository<BaHUser> userRepository,
            IRepository<BaHuAchievementGroup> achievementGroupRepository,
            IRepository<BaHuReward> rewardRepository) : base(mapper, repository, context, actualModels)
        {
            UserRepository = userRepository;
            AchievementGroupRepository = achievementGroupRepository;
            RewardRepository = rewardRepository;
            Query = query;
        }
        
        public async Task<IEnumerable<TAchievementDto>> LoadAllNavigationProperties(IEnumerable<TAchievementDto> entities)
        {
           
            var foundEntity = await Context.Set<BaHuAchievement>(ActualModels.BaHuAchievement)
                .Where(ach => entities
                    .Select(e => e.Id)
                    .Contains(ach.Id))
                .ToListAsync();
            
            foreach (var entity in foundEntity)
            {
                foreach (var property in _navigationProperties)
                {
                    var test = typeof(BaHuAchievement);
                    if (test.GetProperty(property).PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                    {
                        Context.Entry(entity)
                            .Collection(property)
                            .Load();
                    }
                    else
                    {
                        Context.Entry(entity)
                            .Reference(property)
                            .Load();
                    }
                    
                }
                 
            }

            return Mapper.Map<IEnumerable<TAchievementDto>>(foundEntity);

        }

        public async Task<List<TUserDto>> GetUserWhichCompletedAchievement(int achievementId)
        {
            var achievement = await Repository.Get(achievementId);
            var test = Context.Set<BaHuAchievement>()
                .Include(ach => ach.UserCompletedAchievements);
            return achievement?
                .UserCompletedAchievements
                .Select(u => Mapper.Map<TUserDto>(u.User))
                .ToList();
        }

        public async Task<QueryResult<TAchievementDto>> ApplyFilter(AchievementFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }
        

        public async Task<IEnumerable<TUserDto>> GetAllUsersWhichHaveAchievement(int achievementId)
        {
            var achievement = await Repository.Get(achievementId);
            return achievement?
                .AchievementGroup
                .UserAchievementGroups
                .Select(ug => Mapper.Map<TUserDto>(ug.User))
                .ToList();
        }

        public async Task<TUserDto> GetAchievementGroupOwner(int achievementId)
        {
            var achievement = await Repository.GetWithIncludes(achievementId, nameof(BaHuAchievement.AchievementGroup),
                nameof(BaHuAchievement.AchievementGroup.Owner));
            return Mapper.Map<TUserDto>(achievement?
                .AchievementGroup
                .Owner);

        }

        public async Task<IEnumerable<TAchievementDto>> GetAllAchievementsOfUser(int userId)
        {
            var result = await Query.ExecuteAsync(new AchievementFilterDto
            {
                UserId = userId
            });

            return result.Items;
        }

        public async Task<QueryResult<TAchievementDto>> GetNonCompletedAchievementsOfUser(int userId)
        {

            return await Query.ExecuteAsync(new AchievementFilterDto
            {
                UserId = userId,
                Type = AchievementType.NonCompleted
            });
        }

        public async Task<QueryResult<TAchievementDto>> GetAllAchievementsFromGroup(int groupId)
        {
            return await Query.ExecuteAsync(new AchievementFilterDto
            {
                GroupId = groupId
            });
        }

        public async Task<bool> CheckIfUserHasAchievement(int achievementId, int userId)
        {
            var achievement = await Context.Set<BaHUserAchievementGroup>()
                .Where(uag => uag.UserId == userId)
                .Join(Context.Set<BaHuAchievement>(), uag => uag.AchievementGroupId, ach => ach.AchievementGroupId,
                    (_, ach) => ach)
                .FirstOrDefaultAsync(ach => ach.Id == achievementId);
            return achievement != null;
        }

        public async Task<IEnumerable<ValueTuple<TUserDto, DateTime>>> GetUsersWhichAskedForReward(int achievementId)
        {
            return await Context.Set<BaHUserAskedForReward>()
                .Where(uar => uar.AchievementId == achievementId)
                .Join(Context.Set<BaHUser>(), uar => uar.UserId, u => u.Id, (uar, u) => ValueTuple.Create(Mapper.Map<TUserDto>(u), uar.DateTime))
                .ToListAsync();
        }

        public async Task<bool> AskForRewardByUser(int userId, int achievementId)
        {
            var tryIfExists = await Context.Set<BaHUserAskedForReward>(ActualModels.BaHUserAskedForReward)
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
                (BaHUserAskedForReward) Activator.CreateInstance(ActualModels.BaHUserAskedForReward);
            userAskedForReward.UserId = userId;
            userAskedForReward.AchievementId = achievementId;
            userAskedForReward.DateTime = DateTime.Now;


            await Context.AddAsync(userAskedForReward);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAskForReward(int userId, int achievementId)
        {
            var found = await Context.Set<BaHUserAskedForReward>(ActualModels.BaHUserAskedForReward)
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
                .Set<BaHUserCompletedAchievement>(ActualModels.BaHUserCompletedAchievements)
                .FirstOrDefault(uca => uca.AchievementId == achievementId && uca.UserId == userId);
            if (tryIfExists != null)
            {
                return false;
            }
            
            var userCompletedAchievement =
                (BaHUserCompletedAchievement) Activator.CreateInstance(ActualModels.BaHUserCompletedAchievements);

            userCompletedAchievement.UserId = userId;
            userCompletedAchievement.AchievementId = achievementId;
            userCompletedAchievement.AccomplishDate = DateTime.Now;

            await Context.AddAsync(userCompletedAchievement);
            await Context.SaveChangesAsync();
            return true;

        }

        public async Task RemoveReward(int userId, int achievementId)
        {
            var tryIfExists = await Context.Set<BaHUserCompletedAchievement>()
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
            var achievements = await ApplyFilter(new AchievementFilterDto
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