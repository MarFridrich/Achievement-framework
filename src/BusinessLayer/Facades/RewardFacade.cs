using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.Reward;
using DAL.BaHuEntities;
using DAL.BaHuEntities.Interfaces;

namespace BusinessLayer.Facades
{
    public class RewardFacade<TEntity, TRewardDto, TRewardFilterDto>
        where TEntity : BaHuReward, new()
        where TRewardDto : BaHuRewardDto
        where TRewardFilterDto : RewardFilterDto, new() 
    {
        protected IRewardService<TEntity, TRewardDto, TRewardFilterDto> RewardService;

        public RewardFacade(IRewardService<TEntity, TRewardDto, TRewardFilterDto> rewardService)
        {
            RewardService = rewardService;
        }

        public IQueryable<TRewardDto> ListAll()
        {
            return RewardService.ListAll();
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

        public async Task<QueryResult<TRewardDto>> ApplyFilter(TRewardFilterDto filter)
        {
            return await RewardService.ApplyFilter(filter);
        }

        public async Task<QueryResult<TRewardDto>> GetRewardForAchievement(int id)
        {
            return await RewardService.ApplyFilter(new TRewardFilterDto
            {
                AchievementId = id
            });
        }
    }
}