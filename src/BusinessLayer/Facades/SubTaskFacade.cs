using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.SubTask;
using DAL.BaHuEntities;

namespace BusinessLayer.Facades
{
    public class SubTaskFacade<TEntity, TSubTaskDto, TSubTaskFilterDto>
        where TEntity : BaHuSubTask, new()
        where TSubTaskDto : BaHuSubTaskDto
        where TSubTaskFilterDto : SubTaskFilterDto, new()
    {
        protected ISubTaskService<TEntity, TSubTaskDto, TSubTaskFilterDto> SubTaskService;

        public SubTaskFacade(ISubTaskService<TEntity, TSubTaskDto, TSubTaskFilterDto> subTaskService)
        {
            SubTaskService = subTaskService;
        }

        public async Task<TSubTaskDto> GetSubTaskById(int id)
        {
            return await SubTaskService.Get(id);
        }

        public async Task UpdateSubTask(TSubTaskDto entity)
        {
            await SubTaskService.Update(entity);
        }

        public async Task<int> CreateSubTask(TSubTaskDto entity)
        {
            return await SubTaskService.Create(entity);
        }

        public async Task DeleteSubTask(int id)
        {
            await SubTaskService.Delete(id);
        }

        public async Task<QueryResult<TSubTaskDto>> ApplyFilter(TSubTaskFilterDto filter)
        {
            return await SubTaskService.ApplyFilter(filter);
        }

        public async Task<TSubTaskDto> GetSubTaskByIdWithIncludes(int id, params string[] includes)
        {
            return await SubTaskService.GetWithIncludes(id, includes);
        }

        public async Task UpdateSubTasks(IEnumerable<TSubTaskDto> entities)
        {
            foreach (var entity in entities)
            {
                await SubTaskService.Update(entity);
            }
        }

        public async Task<IEnumerable<int>> CreateSubTasks(IEnumerable<TSubTaskDto> entities)
        {
            var listOfIds = new List<int>();
            foreach (var entity in entities)
            {
                listOfIds.Add(await SubTaskService.Create(entity));
            }

            return listOfIds;
        }

        public async Task<bool> RemoveCompletedSubTaskFromUser(int userId, int subTaskId)
        {
            return await SubTaskService.RemoveCompletedSubTaskFromUser(userId, subTaskId);
        }

        public async Task<bool> AskForSubTaskByUser(int userId, int subTaskId)
        {
            return await SubTaskService.AskForSubTaskByUser(userId, subTaskId);
        }

        public async Task<bool> ApproveSubTaskToUser(int userId, int subTaskId)
        {
            return await SubTaskService.ApproveSubTaskToUser(userId, subTaskId);
        }

        public async Task<bool> RemoveAskForSubTask(int userId, int subTaskId)
        {
            return await SubTaskService.RemoveAskForSubTask(userId, subTaskId);
        }
    }
}