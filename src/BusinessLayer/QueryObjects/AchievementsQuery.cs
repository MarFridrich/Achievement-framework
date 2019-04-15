using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.DTOs.Filter.Enums;
using BusinessLayer.Helpers;
using DAL.Entities;
using DAL.Entities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class AchievementsQuery<TEntity, TDto> : QueryBase<TEntity, TDto, AchievementFilterDto>
        where TEntity : FrameworkAchievement, new()
        where TDto : DtoBase
    {

        public AchievementsQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper,
            actualTypes)
        {
        }

        protected override void ApplyWhereClause(AchievementFilterDto filter)
        {
            FilterByUserId(filter);
            FilterGroupId(filter);
            FilterAchievementEvaluation(filter);
            FilterPeopleDoneCount(filter);
            FilterFulfillType(filter);
        }


        private void FilterCompletedOnly(AchievementFilterDto filter)
        {
            
            var achievementsCompleted = GetIdsOfAchievementsIdsWhichIsCompleted(filter);

            Expression<Func<TEntity, bool>> toAdd = ach => achievementsCompleted.Contains(ach.Id);
            TmpPredicates.Add(toAdd);
        }

        private IQueryable<int> GetIdsOfAchievementsIdsWhichHasSubTaskCompleted(AchievementFilterDto filter)
        {
            return Context
                .Set<FrameworkUserCompletedSubTask>(ActualTypes.FrameworkUserCompletedSubTask)
                .Select(ucs => ucs.SubTask.AchievementId);
        }

        private IQueryable<int> GetIdsOfAchievementsIdsWhichIsCompleted(AchievementFilterDto filter)
        {
            return Context
                .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                .Select(uca => uca.AchievementId);
        }

        private void FilterPartialCompletedOnly(AchievementFilterDto filter)
        {
            var achievementsPartialCompletedIds = GetIdsOfAchievementsIdsWhichHasSubTaskCompleted(filter);

            var achievementsCompletedIds = GetIdsOfAchievementsIdsWhichIsCompleted(filter);

            Expression<Func<TEntity, bool>> toAdd = ach =>
                achievementsPartialCompletedIds.Except(achievementsCompletedIds).Contains(ach.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterNotStartedOnly(AchievementFilterDto filter)
        {
            var startedOrCompleted = GetIdsOfAchievementsIdsWhichIsCompleted(filter)
                .Union(GetIdsOfAchievementsIdsWhichHasSubTaskCompleted(filter));
            Expression<Func<TEntity, bool>> toAdd = ach => !startedOrCompleted.Contains(ach.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterNotCompletedOnly(AchievementFilterDto filter)
        {
            var completed = GetIdsOfAchievementsIdsWhichIsCompleted(filter);
            Expression<Func<TEntity, bool>> toAdd = ach => !completed.Contains(ach.Id);
            
            TmpPredicates.Add(toAdd);
        }

        private void FilterAchievementEvaluation(AchievementFilterDto filter)
        {
            if (filter.EvaluationType == null)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd = ach =>
                ach.Evaluation == (FrameworkEvaluations) filter.EvaluationType;
            TmpPredicates.Add(toAdd);
        }

        private void FilterGroupId(AchievementFilterDto filter)
        {
            if (filter.GroupId == 0)
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = ach =>
                ach.AchievementGroupId == filter.GroupId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterFulfillType(AchievementFilterDto filter)
        {
            if (filter.Type == null)
            {
                return;
            }

            switch (filter.Type)
            {
                case AchievementType.Done:
                    FilterCompletedOnly(filter);
                    break;
                case AchievementType.Partial:
                    FilterPartialCompletedOnly(filter);
                    break;
                case AchievementType.NotStarted:
                    FilterNotStartedOnly(filter);
                    break;
                case AchievementType.NonCompleted:
                    FilterNotCompletedOnly(filter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FilterByUserId(AchievementFilterDto filter)
        {
            if (filter.UserId == 0)
            {
                return;
            }

            var achievementsId = Context.Set<FrameworkUserAchievementGroup>(ActualTypes.FrameworkUserAchievementGroup)
                .Where(uag => uag.UserId == filter.UserId)
                .SelectMany(uag => uag.AchievementGroup.Achievements.Select(a => a.Id));

            Expression<Func<TEntity, bool>> toAdd = a => achievementsId.Contains(a.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterPeopleDoneCount(AchievementFilterDto filter)
        {
            if (filter.PeopleDoneCount == null)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd;
            if (filter.PeopleDoneCountLowerThan)
            {
                toAdd = ach => ach.UserCompletedAchievements.Count <= filter.PeopleDoneCount;
            }
            else
            {
                toAdd = ach => ach.UserCompletedAchievements.Count >= filter.PeopleDoneCount;
            }

            TmpPredicates.Add(toAdd);
        }
    }
}