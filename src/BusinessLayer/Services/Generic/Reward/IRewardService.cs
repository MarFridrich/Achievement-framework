using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;

namespace BusinessLayer.Services.Generic.Reward
{
    public interface IRewardService<TRewardDto>
        where TRewardDto : RewardDto
    {
        Task<IEnumerable<TRewardDto>> ListAllAsync();
     
        Task<TRewardDto> Get(int id);
     
        Task<DAL.Entities.Reward> Create(TRewardDto entity);
     
        Task Update(TRewardDto entity);
     
        Task Delete(int id);

        Task<TRewardDto> GetRewardForAchievement(int achievementId);
    }
}