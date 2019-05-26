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
    public class SubTaskService<TEntity, TSubTaskDto, TFilterDto> :
        RepositoryServiceBase<TEntity, TSubTaskDto, TFilterDto>, ISubTaskService<TEntity, TSubTaskDto, TFilterDto>
        where TEntity : BaHuSubTask, new()
        where TSubTaskDto : BaHuSubTaskDto
        where TFilterDto : SubTaskFilterDto, new()
    {

        protected SubTaskQuery<TEntity, TSubTaskDto, TFilterDto> Query;
        
        public SubTaskService(IMapper mapper,
            IRepository<TEntity> repository,
            DbContext context,
            Types actualModels, SubTaskQuery<TEntity, TSubTaskDto, TFilterDto> query) 
            : base(mapper,
            repository,
            context,
            actualModels)
        {
            Query = query;
        }

        public async Task<QueryResult<TSubTaskDto>> ApplyFilter(TFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task RemoveSubTasksFromAchievement(int achievementId)
        {
            var subTasks = await ApplyFilter(new TFilterDto
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
            var tryIfExists = await Context.Set<BaHuUserAskedForSubTask>(ActualModels.BaHuUserAskedForSubTask)
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
                (BaHuUserAskedForSubTask) Activator.CreateInstance(ActualModels.BaHuUserAskedForSubTask);
            userAskedForSubTask.UserId = userId;
            userAskedForSubTask.SubTaskId = subTaskId;
            userAskedForSubTask.DateTime = DateTime.Now;

            await Context.AddAsync(userAskedForSubTask);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveSubTaskToUser(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHuUserCompletedSubTask>(ActualModels.BaHuUserCompletedSubTask)
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
                (BaHuUserCompletedSubTask) Activator.CreateInstance(ActualModels.BaHuUserCompletedSubTask);
            userCompletedSubTask.SubTaskId = subTaskId;
            userCompletedSubTask.UserId = userId;
            userCompletedSubTask.AccomplishedTime = DateTime.Now;

            await Context.AddAsync(userCompletedSubTask);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAskForSubTask(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHuUserAskedForSubTask>(ActualModels.BaHuUserAskedForSubTask)
                .FirstOrDefaultAsync(ucs => ucs.SubTaskId == subTaskId && ucs.UserId == userId);
            if (tryIfExists == null) return false;
            
            Context.Remove(tryIfExists);
            await Context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> RemoveCompletedSubTaskFromUser(int userId, int subTaskId)
        {
            var tryIfExists = await Context.Set<BaHuUserCompletedSubTask>(ActualModels.BaHuUserCompletedSubTask)
                .FirstOrDefaultAsync(ucs => ucs.SubTaskId == subTaskId && ucs.UserId == userId);
            if (tryIfExists == null) return false;
            
            Context.Remove(tryIfExists);
            await Context.SaveChangesAsync();
            return true;

        }
    }
}