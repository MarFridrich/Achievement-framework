using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;

namespace BusinessLayer.Services.Generic.User
{
    public interface IUserService<TUserDto, TAchievementGroupDto, TAchievementDto>
        where TUserDto : UserDto
        where TAchievementGroupDto : AchievementGroupDto
        where TAchievementDto : AchievementDto
    {
        Task<IEnumerable<TUserDto>> ListAllAsync();
     
        Task<TUserDto> Get(int id);
     
        Task<DAL.Entities.User> Create(TUserDto entity);
     
        Task Update(TUserDto entity);
     
        Task Delete(int id);

        Task<IEnumerable<TAchievementGroupDto>> GetAchievementGroupsForUser(int userId);
        
        Task<IEnumerable<TAchievementDto>> GetAchievementsForUser(int userId);

        Task<IEnumerable<TUserDto>> GetUsersFromAchievementGroup(int groupId);
    }
}