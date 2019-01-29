using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Generic.Achievement;
using DAL.Entities;

namespace BusinessLayer.Facades
{
    public class AchievementFacade<TAchievementDto, TUserDto>
        where TAchievementDto : AchievementDto
        where TUserDto : UserDto
    {
        private readonly IAchievementService<TAchievementDto, TUserDto> achievementService;

        public AchievementFacade(IAchievementService<TAchievementDto, TUserDto> achivementService)
        {
            this.achievementService = achivementService;
        }

        public async Task<TAchievementDto> GetAchievementByIdAsync(int id)
        {
            return await achievementService.Get(id);
        }

        public async Task DeleteAchievementAsync(int id)
        {
            await achievementService.Delete(id);
        }

        public async Task<int> CreateAchievement(TAchievementDto achievement)
        {
            if (achievement.AchievementGroupId == 0)
            {
                return 0;
            }

            var created = await achievementService.Create(achievement);
            return created.Id;
        }

        public async Task UpdateAchievement(TAchievementDto achievement)
        {
            await achievementService.Update(achievement);
        }

        public async Task<IEnumerable<TAchievementDto>> GetAllAchievementsByUserId(int userId)
        {
            return await achievementService.GetAllAchievementsOfUser(userId);
        }
    }

}