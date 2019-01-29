using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Common;
using DAL;
using GenericServices;

namespace BusinessLayer.Services.Generic.Reward
{
    public class RewardService<TRewardDto> : CrudServiceBase<DAL.Entities.Reward, TRewardDto>, IRewardService<TRewardDto>
        where TRewardDto : RewardDto
    {
        private readonly ICrud<DAL.Entities.Achievement, AchievementDto> _achievementRepository;
        
        public RewardService(IMapper mapper, ICrudServicesAsync service, AchievementDbContext context,
            ICrud<DAL.Entities.Achievement, AchievementDto> achievementRepository) 
            : base(mapper, service, context)
        {
            _achievementRepository = achievementRepository;
        }

        public async Task<TRewardDto> GetRewardForAchievement(int achievementId)
        {
            var achievement = await _achievementRepository.Get(achievementId);

            return achievement.Reward as TRewardDto;
        }
    }
}