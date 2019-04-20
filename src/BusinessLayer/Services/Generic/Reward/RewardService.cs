using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL.BaHuEntities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.Reward
{
    public class RewardService<TEntity, TRewardDto> :
        RepositoryServiceBase<TEntity, TRewardDto, RewardFilterDto>, IRewardService<TEntity, TRewardDto>
        
        where TRewardDto : BaHuRewardDto
        where TEntity : BaHuReward, new()
    {
        private readonly IRepository<BaHuAchievement> _achievementRepository;
        protected readonly RewardQuery<TEntity, TRewardDto> Query;

        public RewardService(IMapper mapper, IRepository<TEntity> repository, DbContext context, Types actualModels,
            IRepository<BaHuAchievement> achievementRepository,
            RewardQuery<TEntity, TRewardDto> query) 
            : base(mapper,
            repository,
            context,
            actualModels
            )
        {
            _achievementRepository = achievementRepository;
            Query = query;
        }

        public async Task<QueryResult<TRewardDto>> ApplyFilter(RewardFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task<TRewardDto> GetRewardForAchievement(int achievementId)
        {
            var achievement = await _achievementRepository.Get(achievementId);

            return Mapper.Map<TRewardDto>(achievement.Reward);
        }

        
    }
}