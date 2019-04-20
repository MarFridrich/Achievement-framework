using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Generic.SubTask;
using DAL.BaHuEntities;

namespace BusinessLayer.Facades
{
    public class SubTaskFacade<TEntity, TDto>
        where TEntity : BaHuSubTask
        where TDto : BaHuSubTaskDto
    {
        protected ISubTaskService<TEntity, TDto> SubTaskService;

        public SubTaskFacade(ISubTaskService<TEntity, TDto> subTaskService)
        {
            SubTaskService = subTaskService;
        }

        public async Task<TDto> GetSubTaskById(int id)
        {
            return await SubTaskService.Get(id);
        }

        public async Task UpdateSubTask(TDto entity)
        {
            await SubTaskService.Update(entity);
        }

        public async Task<int> CreateSubTask(TDto entity)
        {
            return await SubTaskService.Create(entity);
        }

        public async Task DeleteSubTask(int id)
        {
            await SubTaskService.Delete(id);
        }

        public async Task<TDto> GetSubTaskByIdWithIncludes(int id, params string[] includes)
        {
            return await SubTaskService.GetWithIncludes(id, includes);
        }

        public async Task UpdateSubTasks(IEnumerable<TDto> entities)
        {
            foreach (var entity in entities)
            {
                await SubTaskService.Update(entity);
            }
        }

        public async Task<IEnumerable<int>> CreateSubTasks(IEnumerable<TDto> entities)
        {
            var listOfIds = new List<int>();
            foreach (var entity in entities)
            {
                listOfIds.Add(await SubTaskService.Create(entity));
            }

            return listOfIds;
        }

        public async Task RemoveCompletedSubTaskFromUser(int userId, int subTaskId)
        {
            await SubTaskService.RemoveCompletedSubTaskFromUser(userId, subTaskId);
        }
    }
}