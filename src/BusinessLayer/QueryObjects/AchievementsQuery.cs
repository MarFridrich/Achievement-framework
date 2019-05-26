using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.DTOs.Filter.Enums;
using BusinessLayer.Helpers;
using Castle.Core.Internal;
using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class AchievementsQuery<TEntity, TDto, TFilterDto> : QueryBase<TEntity, TDto, TFilterDto>
        where TEntity : BaHuAchievement, new()
        where TDto : DtoBase
        where TFilterDto : AchievementFilterDto
    {

        public AchievementsQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper,
            actualTypes)
        {
        }

        protected override void ApplyWhereClause(TFilterDto filter)
        {
            FilterByUserId(filter);
            FilterGroupId(filter);
            FilterAchievementEvaluation(filter);
            FilterPeopleDoneCount(filter);
            FilterFulfillType(filter);
        }


        private void FilterCompletedOnly(TFilterDto filter)
        {
            
            var achievementsCompleted = GetIdsOfAchievementsIdsWhichIsCompleted(filter);

            Expression<Func<TEntity, bool>> toAdd = ach => achievementsCompleted.Contains(ach.Id);
            TmpPredicates.Add(toAdd);
        }

        private IQueryable<int> GetIdsOfAchievementsIdsWhichHasSubTaskCompleted(TFilterDto filter)
        {
            return Context
                .Set<BaHuUserCompletedSubTask>(ActualTypes.BaHuUserCompletedSubTask)
                .Select(ucs => ucs.SubTask.AchievementId);
        }

        private IQueryable<int> GetIdsOfAchievementsIdsWhichIsCompleted(TFilterDto filter)
        {
            return Context
                .Set<BaHuUserCompletedAchievement>(ActualTypes.BaHuUserCompletedAchievements)
                .Select(uca => uca.AchievementId);
        }

        private void FilterPartialCompletedOnly(TFilterDto filter)
        {
            var achievementsPartialCompletedIds = GetIdsOfAchievementsIdsWhichHasSubTaskCompleted(filter);

            var achievementsCompletedIds = GetIdsOfAchievementsIdsWhichIsCompleted(filter);

            Expression<Func<TEntity, bool>> toAdd = ach =>
                achievementsPartialCompletedIds.Except(achievementsCompletedIds).Contains(ach.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterNotStartedOnly(TFilterDto filter)
        {
            var startedOrCompleted = GetIdsOfAchievementsIdsWhichIsCompleted(filter)
                .Union(GetIdsOfAchievementsIdsWhichHasSubTaskCompleted(filter));
            Expression<Func<TEntity, bool>> toAdd = ach => !startedOrCompleted.Contains(ach.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterNotCompletedOnly(TFilterDto filter)
        {
            var completed = GetIdsOfAchievementsIdsWhichIsCompleted(filter);
            Expression<Func<TEntity, bool>> toAdd = ach => !completed.Contains(ach.Id);
            
            TmpPredicates.Add(toAdd);
        }

        private void FilterAchievementEvaluation(TFilterDto filter)
        {
            if (filter.EvaluationType == null)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd = ach =>
                ach.Evaluation == (BaHuEvaluations) filter.EvaluationType;
            TmpPredicates.Add(toAdd);
        }

        private void FilterGroupId(TFilterDto filter)
        {
            if (filter.GroupId == 0)
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = ach =>
                ach.AchievementGroupId == filter.GroupId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterFulfillType(TFilterDto filter)
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

        private void FilterByUserId(TFilterDto filter)
        {
            if (filter.UserId == 0)
            {
                return;
            }

            var achievementsId = Context.Set<BaHuUserAchievementGroup>(ActualTypes.BaHuUserAchievementGroup)
                .Where(uag => uag.UserId == filter.UserId)
                .SelectMany(uag => uag.AchievementGroup.Achievements.Select(a => a.Id));

            Expression<Func<TEntity, bool>> toAdd = a => achievementsId.Contains(a.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterPeopleDoneCount(TFilterDto filter)
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