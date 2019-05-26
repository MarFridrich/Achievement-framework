using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.DTOs.Filter.Enums;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class SubTaskQuery<TEntity, TSubTaskDto, TFilterDto> : QueryBase<TEntity, TSubTaskDto, TFilterDto>
        where TEntity : BaHuSubTask
        where TSubTaskDto : BaHuSubTaskDto
        where TFilterDto : SubTaskFilterDto
    {
        private SubTaskFilterDto _filter { get; set; }
        
        public SubTaskQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }
        
        
        private IQueryable<int> GetCompletedSubTasksIds()
        {
            var userCompletedSubTasks =
                Context.Set<BaHuUserCompletedSubTask>(ActualTypes.BaHuUserCompletedSubTask);
            if (_filter.UserId != 0)
            {
                userCompletedSubTasks = userCompletedSubTasks.Where(ucs => ucs.UserId == _filter.UserId);
            }

            if (_filter.AchievementId != 0)
            {
                userCompletedSubTasks =
                    userCompletedSubTasks.Where(ucs => ucs.SubTask.AchievementId == _filter.AchievementId);
            }
            
            return userCompletedSubTasks
                .Select(ucs => ucs.SubTaskId);
        }

        private IQueryable<int> GetAskedForSubTasksIds()
        {
            var askedForSubTask = Context.Set<BaHuUserAskedForSubTask>(ActualTypes.BaHuUserAskedForSubTask);
            if (_filter.UserId != 0)
            {
                askedForSubTask = askedForSubTask.Where(uas => uas.UserId == _filter.UserId);
            }

            if (_filter.AchievementId != 0)
            {
                askedForSubTask = askedForSubTask.Where(uas => uas.SubTask.AchievementId == _filter.AchievementId);
            }

            return askedForSubTask.Select(uas => uas.SubTaskId);
        }

        private void FilterAskedForSubTask()
        {
            var askedForSubTask = GetAskedForSubTasksIds();
            Expression<Func<TEntity, bool>> toAdd = s => askedForSubTask.Contains(s.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterCompletedOnly()
        {
            var completedIds = GetCompletedSubTasksIds();
            Expression<Func<TEntity, bool>> toAdd = s => completedIds.Contains(s.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterNonCompletedOnly()
        {
            var completedIds = GetCompletedSubTasksIds();
            Expression<Func<TEntity, bool>> toAdd = s => !completedIds.Contains(s.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterAccomplishType(SubTaskFilterDto filter)
        {
            if (filter.AccomplishType == SubTaskAccomplishTypes.All)
            {
                return;
            }

            switch (filter.AccomplishType)
            {
                case SubTaskAccomplishTypes.Completed:
                    FilterCompletedOnly();
                    break;
                case SubTaskAccomplishTypes.NonCompleted:
                    FilterNonCompletedOnly();
                    break;
                case SubTaskAccomplishTypes.AskedForSubTask:
                    FilterAskedForSubTask();
                    break;
                default:
                    return;
            }
        }

        private void FilterName(SubTaskFilterDto filter)
        {
            if (string.IsNullOrEmpty(filter.Name))
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = s => s.Name.Contains(filter.Name);
            TmpPredicates.Add(toAdd);
        }
        
        private void FilterDescription(SubTaskFilterDto filter)
        {
            if (string.IsNullOrEmpty(filter.Description))
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = s => s.Description.Contains(filter.Description);
            TmpPredicates.Add(toAdd);
        }
        
        private void FilterAchievementId(SubTaskFilterDto filter)
        {
            if (filter.AchievementId == 0)
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = s => s.AchievementId == filter.AchievementId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterUserId(SubTaskFilterDto filter)
        {
            if (filter.UserId == 0)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd = s => s.Achievement
                                                             .AchievementGroup
                                                             .UserAchievementGroups
                                                             .Where(u => u.UserId == filter.UserId) != null;
            TmpPredicates.Add(toAdd);
        }
        protected override void ApplyWhereClause(TFilterDto filter)
        {
            _filter = filter;
            FilterName(filter);
            FilterAchievementId(filter);
            FilterUserId(filter);
            FilterDescription(filter);
            FilterAccomplishType(filter);
        }
    }
}