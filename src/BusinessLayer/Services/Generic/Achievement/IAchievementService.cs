using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.BaHuEntities.Interfaces;

namespace BusinessLayer.Services.Generic.Achievement
{
    public interface IAchievementService<TEntity, TAchievementDto, TUserDto, TFilterDto>
        where TEntity : class, IEntity
        where TAchievementDto : BaHuAchievementDto
        where TUserDto : UserDto
    {
             IQueryable<TAchievementDto> ListAll();

             Task<QueryResult<TAchievementDto>> ApplyFilter(TFilterDto filter);
             
             Task CreateList(IEnumerable<TAchievementDto> entity);

             Task<TAchievementDto> Get(int id);
     
             Task<int> Create(TAchievementDto entity);
     
             Task Update(TAchievementDto entity);
     
             Task Delete(int id);

             Task<TAchievementDto> GetWithIncludes(int id, params string[] includes);

             Task<IEnumerable<TUserDto>> GetUserWhichCompletedAchievement(int achievementId);
             
             Task<bool> CheckIfUserHasAchievement(int userId, int achievementId);
             
             Task<IEnumerable<ValueTuple<TUserDto, DateTime>>> GetUsersWhichAskedForReward(int achievementId);

             Task<string> ExportGroupAchievementsToJson(int groupId);

             byte[] MakeBytesFromString(string input);

             Task ImportAchievementsFromFileAndAddToGroup(Stream file, int groupId);

             Task<bool> AskForRewardByUser(int userId, int achievementId);
             
             Task<bool> RemoveAskForReward(int userId, int achievementId);

             Task<bool> ApproveAchievementToUser(int userId, int achievementId);

             Task RemoveReward(int userId, int achievementId);

    }
}