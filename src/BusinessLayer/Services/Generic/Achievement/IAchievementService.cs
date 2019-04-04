using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.Entities.Interfaces;

namespace BusinessLayer.Services.Generic.Achievement
{
    public interface IAchievementService<TEntity, TAchievementDto, TUserDto>
        where TEntity : class, IEntity
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
    {
             IQueryable<TAchievementDto> ListAllAsync();

             Task<QueryResult<TAchievementDto>> ApplyFilter(AchievementFilterDto filter);
             
             Task CreateList(IEnumerable<TAchievementDto> entity);
             Task<TAchievementDto> Get(int id);
     
             Task<int> Create(TAchievementDto entity);
     
             Task Update(TAchievementDto entity);
     
             Task Delete(int id);

             Task<IEnumerable<TAchievementDto>> LoadAllNavigationProperties(IEnumerable<TAchievementDto> entities);

             Task<TAchievementDto> LoadNavigationProperties(int id, IEnumerable<IEnumerable<string>> includes);
             Task<TAchievementDto> GetWithIncludes(int id, params string[] includes);

             Task<List<TUserDto>> GetUserWhichCompletedAchievement(int achievementId);

             Task<IEnumerable<TUserDto>> GetAllUsersWhichHaveAchievement(int achievementId);
             
             Task<TUserDto> GetAchievementGroupOwner(int achievementId);

             Task<IEnumerable<TAchievementDto>> GetAllAchievementsOfUser(int userId);

             Task<QueryResult<TAchievementDto>> GetNonCompletedAchievementsOfUser(int userId);

             Task<QueryResult<TAchievementDto>> GetAllAchievementsFromGroup(int groupId);

             Task<bool> CheckIfUserHasAchievement(int achievementId, int userId);

             Task<IEnumerable<ValueTuple<TUserDto, DateTime>>> GetUsersWhichAskedForReward(int achievementId);

             Task<string> ExportGroupAchievementsToJson(int groupId);

             byte[] MakeBytesFromString(string input);
             Task ImportAchievementsFromFileAndAddToGroup(Stream file, int groupId);

             Task<bool> AskForRewardByUser(int userId, int achievementId);
             
             Task<bool> RemoveAskForReward(int userId, int achievementId);

             Task ApproveAchievementToUser(int userId, int achievementId);

             Task RemoveReward(int userId, int achievementId);

    }
}