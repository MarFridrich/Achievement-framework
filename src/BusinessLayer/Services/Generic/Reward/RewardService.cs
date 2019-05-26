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
    public class RewardService<TEntity, TRewardDto, TFilterDto> :
        RepositoryServiceBase<TEntity, TRewardDto, TFilterDto>, IRewardService<TEntity, TRewardDto, TFilterDto>
        
        where TEntity : BaHuReward, new()
        where TRewardDto : BaHuRewardDto
        where TFilterDto : RewardFilterDto, new()
    {
        protected readonly RewardQuery<TEntity, TRewardDto, TFilterDto> Query;

        public RewardService(IMapper mapper, IRepository<TEntity> repository, DbContext context, Types actualModels,
            RewardQuery<TEntity, TRewardDto, TFilterDto> query) 
            : base(mapper,
            repository,
            context,
            actualModels
            )
        {
            Query = query;
        }

        public async Task<QueryResult<TRewardDto>> ApplyFilter(TFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }
        
    }
}