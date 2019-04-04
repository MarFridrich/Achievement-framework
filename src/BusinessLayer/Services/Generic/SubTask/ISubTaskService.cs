using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;

namespace BusinessLayer.Services.Generic.SubTask
{
    public interface ISubTaskService<TEntity, TSubTaskDto>
    {
        Task<TSubTaskDto> Get(int id);

        Task<QueryResult<TSubTaskDto>> ApplyFilter(SubTaskFilterDto filter);
        
        Task<TSubTaskDto> GetWithIncludes(int id, params string[] includes);
     
        Task<int> Create(TSubTaskDto entity);
     
        Task Update(TSubTaskDto entity);
     
        Task Delete(int id);

        Task RemoveSubTasksFromAchievement(int achievementId);
    }
}