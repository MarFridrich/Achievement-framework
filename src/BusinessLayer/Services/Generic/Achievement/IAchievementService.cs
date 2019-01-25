using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;

namespace BusinessLayer.Services.Generic.Achievement
{
    public interface IAchievementService<TAchievementDto, TUserDto>
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
    {
             Task<IEnumerable<TAchievementDto>> ListAllAsync();
     
             Task<TAchievementDto> Get(int id);
     
             Task<DAL.Entities.Achievement> Create(DAL.Entities.Achievement entity);
     
             Task Update(TAchievementDto entity);
     
             Task Delete(int id);

             Task<List<TUserDto>> GetUserWhichCompletedAchievement(int achievementId);

             Task<IEnumerable<TUserDto>> GetAllUsersWhichHaveAchievement(int achievementId);
             
             Task<TUserDto> GetAchievementGroupOwner(int achievementId);

    }
}