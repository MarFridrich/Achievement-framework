using System;
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
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.SubTask
{
    public class SubTaskService<TEntity, TSubTaskDto> :
        RepositoryServiceBase<TEntity, TSubTaskDto, SubTaskFilterDto>, ISubTaskService<TEntity, TSubTaskDto>
        where TEntity : BaHuSubTask
        where TSubTaskDto : BaHuSubTaskDto
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
        
        public async Task<bool> AskForSubTaskByUser(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHUserAskedForSubTask>(ActualModels.BaHUserAskedForSubTask)
                .FirstOrDefaultAsync(ucs => ucs.SubTaskId == subTaskId && ucs.UserId == userId);
            if (tryIfExists != null)
            {
                return false;
            }
            
            if (userId == 0 || subTaskId == 0)
            {
                return false;
            }

            var userAskedForSubTask =
                (BaHUserAskedForSubTask) Activator.CreateInstance(ActualModels.BaHUserAskedForSubTask);
            userAskedForSubTask.UserId = userId;
            userAskedForSubTask.SubTaskId = subTaskId;
            userAskedForSubTask.DateTime = DateTime.Now;

            await Context.AddAsync(userAskedForSubTask);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveSubTaskToUser(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHUserCompletedSubTask>(ActualModels.BaHUserCompletedSubTask)
                .FirstOrDefaultAsync(ucs => ucs.SubTaskId == subTaskId && ucs.UserId == userId);
            if (tryIfExists != null)
            {
                return false;
            }
            
            if (userId == 0 || subTaskId == 0)
            {
                return false;
            }
            
            var userCompletedSubTask =
                (BaHUserCompletedSubTask) Activator.CreateInstance(ActualModels.BaHUserCompletedSubTask);
            userCompletedSubTask.SubTaskId = subTaskId;
            userCompletedSubTask.UserId = userId;
            userCompletedSubTask.AccomplishedTime = DateTime.Now;

            await Context.AddAsync(userCompletedSubTask);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveAskForSubTask(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHUserAskedForSubTask>(ActualModels.BaHUserAskedForSubTask)
                .FirstOrDefaultAsync(ucs => ucs.SubTaskId == subTaskId && ucs.UserId == userId);
            if (tryIfExists != null)
            {
                Context.Remove(tryIfExists);
                await Context.SaveChangesAsync();
            }
        }

        public async Task RemoveCompletedSubTaskFromUser(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHUserCompletedSubTask>(ActualModels.BaHUserCompletedSubTask)
                .FirstOrDefaultAsync(ucs => ucs.SubTaskId == subTaskId && ucs.UserId == userId);
            if (tryIfExists != null)
            {
                Context.Remove(tryIfExists);
                await Context.SaveChangesAsync();
            }
        }
    }
}