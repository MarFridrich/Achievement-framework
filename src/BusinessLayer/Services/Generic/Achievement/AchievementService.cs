using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Common;
using GenericServices;

namespace BusinessLayer.Services.Generic.Achievement
{
    public class AchievementService<TAchievementDto, TUserDto> :
        CrudServiceBase<DAL.Entities.Achievement, TAchievementDto>, IAchievementService<TAchievementDto, TUserDto>
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
    {

        public AchievementService(IMapper mapper, ICrudServicesAsync service) : base(mapper, service)
        {
        }

        public async Task<List<TUserDto>> GetUserWhichCompletedAchievement(int achievementId)
        {
            var achievement = await Get(achievementId);
            return achievement?
                .UserCompletedAchievements
                .Select(u => u.User)
                .Cast<TUserDto>()
                .ToList();
        }

        public async Task<IEnumerable<TUserDto>> GetAllUsersWhichHaveAchievement(int achievementId)
        {
            var achievement = await Get(achievementId);
            return achievement?
                .AchievementGroup
                .Users
                .Cast<TUserDto>()
                .ToList();
        }

        public async Task<TUserDto> GetAchievementGroupOwner(int achievementId)
        {
            var achievement = await GetWithIncludes(achievementId, nameof(DAL.Entities.Achievement.AchievementGroup),
                nameof(DAL.Entities.Achievement.AchievementGroup.Owner));
            return (TUserDto) achievement?
                .AchievementGroup
                .Owner;
        }
        
    }
}