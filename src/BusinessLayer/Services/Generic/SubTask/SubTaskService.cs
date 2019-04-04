using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.SubTask
{
    public class SubTaskService<TEntity, TSubTaskDto> :
        RepositoryServiceBase<TEntity, TSubTaskDto, SubTaskFilterDto>, ISubTaskService<TEntity, TSubTaskDto>
        where TEntity : FrameworkSubTask
        where TSubTaskDto : SubTaskDto
    {

        protected SubTaskQuery<TEntity, TSubTaskDto> Query;
        
        public SubTaskService(IMapper mapper,
            IRepository<TEntity> repository,
            DbContext context,
            Types actualModels, SubTaskQuery<TEntity, TSubTaskDto> query) 
            : base(mapper,
            repository,
            context,
            actualModels)
        {
            Query = query;
        }

        public async Task<QueryResult<TSubTaskDto>> ApplyFilter(SubTaskFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task RemoveSubTasksFromAchievement(int achievementId)
        {
            var subTasks = await ApplyFilter(new SubTaskFilterDto
            {
                AchievementId = achievementId
            });
            foreach (var subTask in subTasks.Items)
            {
                await Delete(subTask.Id);
            }
        }
    }
}