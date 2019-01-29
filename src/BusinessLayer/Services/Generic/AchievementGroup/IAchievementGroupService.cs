using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Generic.AchievementGroup
{
    public interface IAchievementGroupService<TAchievementGroupDto>
    {
        
        Task<IEnumerable<TAchievementGroupDto>> ListAllAsync();
     
        Task<TAchievementGroupDto> Get(int id);
     
        Task<DAL.Entities.AchievementGroup> Create(TAchievementGroupDto entity);
     
        Task Update(TAchievementGroupDto entity);
     
        Task Delete(int id);

        Task<IEnumerable<TAchievementGroupDto>> GetAchievementsGroupsOfUserAsync(int userId);

        Task<IEnumerable<TAchievementGroupDto>> GetGroupsWhereUserIsAdminAsync(int userId);

        Task<bool> InsertUserIntoAchievementGroup(int userId, int groupId);

        Task DeleteUserFromAchievementGroup(int userId, int groupId);

        Task<bool> CheckIfUserIsGroupAdmin(int groupId, int userId);

    }
}