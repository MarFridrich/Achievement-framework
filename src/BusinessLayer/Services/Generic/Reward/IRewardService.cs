using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;

namespace BusinessLayer.Services.Generic.Reward
{
    public interface IRewardService<TEntity, TRewardDto, TFilterDto>
        where TRewardDto : BaHuRewardDto
    {
        IQueryable<TRewardDto> ListAll();
        
        Task CreateList(IEnumerable<TRewardDto> entity);
     
        Task<TRewardDto> Get(int id);

        Task<QueryResult<TRewardDto>> ApplyFilter(TFilterDto filter);
        
        Task<TRewardDto> GetWithIncludes(int id, params string[] includes);
     
        Task<int> Create(TRewardDto entity);
     
        Task Update(TRewardDto entity);
     
        Task Delete(int id);
        
    }
}