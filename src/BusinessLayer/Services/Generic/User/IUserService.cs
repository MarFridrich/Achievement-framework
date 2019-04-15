using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;

namespace BusinessLayer.Services.Generic.User
{
    public interface IUserService<TUserDto, TAchievementGroupDto, TAchievementDto, TFilterDto>
        where TUserDto : UserDto
        where TAchievementGroupDto : AchievementGroupDto
        where TAchievementDto : AchievementDto
    {
        IQueryable<TUserDto> ListAllAsync();

        Task CreateList(IEnumerable<TUserDto> entity);
        Task<TUserDto> Get(int id);
        
        Task<TUserDto> GetWithIncludes(int id, params string[] includes);
     
        Task<int> Create(TUserDto entity);
     
        Task Update(TUserDto entity);

        Task<QueryResult<TUserDto>> ApplyFilter(TFilterDto filter);
     
        Task Delete(int id);

        Task<IEnumerable<TAchievementGroupDto>> GetAchievementGroupsForUser(int userId);
        
        Task<IEnumerable<TAchievementDto>> GetAchievementsForUser(int userId);

        Task<IEnumerable<TUserDto>> GetUsersFromAchievementGroup(int groupId);
    }
}