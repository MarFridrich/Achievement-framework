using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.Reward;
using DAL.BaHuEntities.Interfaces;

namespace BusinessLayer.Facades
{
    public class RewardFacade<TEntity, TRewardDto>
        where TEntity : IEntity
        where TRewardDto : BaHuRewardDto
    {
        protected IRewardService<TEntity, TRewardDto> RewardService;

        public RewardFacade(IRewardService<TEntity, TRewardDto> rewardService)
        {
            RewardService = rewardService;
        }

        public async Task<TRewardDto> GetRewardById(int id)
        {
            return await RewardService.Get(id);
        }

        public async Task<TRewardDto> GetRewardByIdWithIncludes(int id, params string[] includes)
        {
            return await RewardService.GetWithIncludes(id, includes);
        }

        public async Task UpdateReward(TRewardDto dto)
        {
            await RewardService.Update(dto);
        }

        public async Task RemoveReward(int id)
        {
            await RewardService.Delete(id);
        }

        public async Task<int> CreateReward(TRewardDto dto)
        {
            return await RewardService.Create(dto);
        }

        public async Task<QueryResult<TRewardDto>> ApplyFilter(RewardFilterDto filter)
        {
            return await RewardService.ApplyFilter(filter);
        }
    }
}