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
using DAL.Entities;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BusinessLayer.Services.Generic.Achievement
{
    public class AchievementService<TEntity, TAchievementDto, TUserDto> :
        RepositoryServiceBase<TEntity, TAchievementDto, AchievementFilterDto>, IAchievementService<TEntity, TAchievementDto, TUserDto>
        where TEntity : FrameworkAchievement, IEntity, new()
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
    {

        protected readonly IRepository<FrameworkUser> UserRepository;
        protected readonly IRepository<FrameworkAchievementGroup> AchievementGroupRepository;
        protected readonly IRepository<FrameworkReward> RewardRepository;
        protected readonly QueryBase<TEntity, TAchievementDto, AchievementFilterDto> Query;
        

        private readonly string[] _navigationProperties = new[]
        {
            nameof(FrameworkAchievement.AchievementGroup), nameof(FrameworkAchievement.UserCompletedAchievements),
            nameof(FrameworkAchievement.Reward), nameof(FrameworkAchievement.Notifications),
            nameof(FrameworkAchievement.SubTasks)
        };
        
        
        public AchievementService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, AchievementsQuery<TEntity, TAchievementDto> query,
            IRepository<FrameworkUser> userRepository,
            IRepository<FrameworkAchievementGroup> achievementGroupRepository,
            IRepository<FrameworkReward> rewardRepository) : base(mapper, repository, context, actualModels)
        {
            UserRepository = userRepository;
            AchievementGroupRepository = achievementGroupRepository;
            RewardRepository = rewardRepository;
            Query = query;
        }
        
        public async Task<IEnumerable<TAchievementDto>> LoadAllNavigationProperties(IEnumerable<TAchievementDto> entities)
        {
           
            var foundEntity = await Context.Set<FrameworkAchievement>(ActualModels.FrameworkAchievement)
                .Where(ach => entities
                    .Select(e => e.Id)
                    .Contains(ach.Id))
                .ToListAsync();
            
            foreach (var entity in foundEntity)
            {
                foreach (var property in _navigationProperties)
                {
                    var test = typeof(FrameworkAchievement);
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
            var test = Context.Set<FrameworkAchievement>()
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
            var achievement = await Repository.GetWithIncludes(achievementId, nameof(FrameworkAchievement.AchievementGroup),
                nameof(FrameworkAchievement.AchievementGroup.Owner));
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
            var achievement = await Context.Set<FrameworkUserAchievementGroup>()
                .Where(uag => uag.UserId == userId)
                .Join(Context.Set<FrameworkAchievement>(), uag => uag.AchievementGroupId, ach => ach.AchievementGroupId,
                    (_, ach) => ach)
                .FirstOrDefaultAsync(ach => ach.Id == achievementId);
            return achievement != null;
        }

        public async Task<IEnumerable<ValueTuple<TUserDto, DateTime>>> GetUsersWhichAskedForReward(int achievementId)
        {
            return await Context.Set<FrameworkUserAskedForReward>()
                .Where(uar => uar.AchievementId == achievementId)
                .Join(Context.Set<FrameworkUser>(), uar => uar.UserId, u => u.Id, (uar, u) => ValueTuple.Create(Mapper.Map<TUserDto>(u), uar.DateTime))
                .ToListAsync();
        }

        public async Task<bool> AskForRewardByUser(int userId, int achievementId)
        {
            var tryIfExists = await Context.Set<FrameworkUserAskedForReward>(ActualModels.FrameworkUserAskedForReward)
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
                (FrameworkUserAskedForReward) Activator.CreateInstance(ActualModels.FrameworkUserAskedForReward);
            userAskedForReward.UserId = userId;
            userAskedForReward.AchievementId = achievementId;
            userAskedForReward.DateTime = DateTime.Now;


            await Context.AddAsync(userAskedForReward);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAskForReward(int userId, int achievementId)
        {
            var found = await Context.Set<FrameworkUserAskedForReward>(ActualModels.FrameworkUserAskedForReward)
                .FirstOrDefaultAsync(uar => uar.AchievementId == achievementId && uar.UserId == userId);
            if (found == null)
            {
                return false;
            }

            Context.Remove(found);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task ApproveAchievementToUser(int userId, int achievementId)
        {
            var userAskedForReward =
                (FrameworkUserAskedForReward) Activator.CreateInstance(ActualModels.FrameworkUserAskedForReward);
            userAskedForReward.UserId = userId;
            userAskedForReward.AchievementId = achievementId;
            Context
                .Remove(userAskedForReward);

            var userCompletedAchievement =
                (FrameworkUserCompletedAchievements) Activator.CreateInstance(ActualModels.FrameworkUserCompletedAchievements);

            userCompletedAchievement.UserId = userId;
            userCompletedAchievement.AchievementId = achievementId;
            userCompletedAchievement.AccomplishDate = DateTime.Now;

            await Context.AddAsync(userCompletedAchievement);
            await Context.SaveChangesAsync();

        }

        public async Task RemoveReward(int userId, int achievementId)
        {
            var userCompletedAchievement =
                (FrameworkUserCompletedAchievements) Activator.CreateInstance(ActualModels.FrameworkUserCompletedAchievements);

            userCompletedAchievement.UserId = userId;
            userCompletedAchievement.AchievementId = achievementId;
            Context.Remove(userCompletedAchievement);
            await Context.SaveChangesAsync();

        }

        public async Task<string> ExportGroupAchievementsToJson(int groupId)
        {
            var achievements = await ApplyFilter(new AchievementFilterDto
            {
                GroupId = groupId,
                Includes = new []{ nameof(FrameworkAchievement.Reward), nameof(FrameworkAchievement.SubTasks)}
            });
            
            return JsonConvert.SerializeObject(achievements.Items);
        }

        public async Task ImportAchievementsFromFileAndAddToGroup(Stream file, int groupId)
        {
            var group = await AchievementGroupRepository.Get(groupId);
            using (var reader = new StreamReader(file))
            {
                var all = await reader.ReadToEndAsync();
                var achievements = JsonConvert.DeserializeObject<List<TEntity>>(all);
                foreach (var achievement in achievements)
                {
                    var property = achievement.GetType().GetProperty("Reward", ActualModels.FrameworkReward);
                    var reward = (FrameworkReward) property.GetValue(achievement);
                    var rewardId = await RewardRepository.Create(reward);
                    achievement.RewardId = rewardId;
                    achievement.AchievementGroupId = groupId;
                }                  
                Context.Set<TEntity>()
                    .AddRange(achievements);
            }

            await Context.SaveChangesAsync();

        }

        public byte[] MakeBytesFromString(string input)
        {
            return Encoding.Default.GetBytes(input);
        }

    }
}