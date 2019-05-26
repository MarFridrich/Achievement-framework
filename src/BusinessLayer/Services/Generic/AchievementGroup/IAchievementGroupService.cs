using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.BaHuEntities;

namespace BusinessLayer.Services.Generic.AchievementGroup
{
    public interface IAchievementGroupService<TEntity, TAchievementGroupDto, TUserDto, TFilterDto>
        where TEntity : BaHuAchievementGroup
        where TAchievementGroupDto : BaHuAchievementGroupDto
        where TUserDto : UserDto
    {
        
        IQueryable<TAchievementGroupDto> ListAll();
        
        Task CreateList(IEnumerable<TAchievementGroupDto> entity);

        Task<QueryResult<TAchievementGroupDto>> ApplyFilter(TFilterDto filter);
     
        Task<TAchievementGroupDto> Get(int id);
        
        Task<TAchievementGroupDto> GetWithIncludes(int id, params string[] includes);
     
        Task<int> Create(TAchievementGroupDto entity);
     
        Task Update(TAchievementGroupDto entity);
     
        Task Delete(int id);

        Task<bool> InsertUserIntoAchievementGroup(int userId, int groupId);

        Task DeleteUserFromAchievementGroup(int userId, int groupId);
        
        Task DeleteAllUsersFromAchievementGroup(int groupId);

        Task<bool> CheckIfUserIsGroupAdmin(int groupId, int userId);

        Task<IEnumerable<TUserDto>> GetUsersInAchievementGroup(int groupId);

        Task<bool> IsExpired(int groupId);

    }
}