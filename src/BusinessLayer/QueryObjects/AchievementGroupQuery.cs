using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class AchievementGroupQuery<TEntity, TDto, TFilterDto> : QueryBase<TEntity, TDto, TFilterDto>
        where TEntity : BaHuAchievementGroup
        where TDto : BaHuAchievementGroupDto
        where TFilterDto : AchievementGroupFilterDto
    {
        public AchievementGroupQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }

        protected override void ApplyWhereClause(TFilterDto filter)
        {
            FilterGroupName(filter);
            FilterOwnerId(filter);
            FilterUserId(filter);
            FilterExpired(filter);
        }

        private void FilterGroupName(TFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Name))
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = g => g.Name.Contains(filter.Name);
            TmpPredicates.Add(toAdd);
        }

        private void FilterOwnerId(TFilterDto filter)
        {
            if (filter.OwnerId == 0)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd = g => g.OwnerId == filter.OwnerId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterUserId(TFilterDto filter)
        {
            if (filter.UserId == 0)
            {
                return;
            }

//            var groupIds = Context.Set<BaHuUserAchievementGroup>(ActualTypes.BaHuUserAchievementGroup)
//                .Where(uag => uag.UserId == filter.UserId)
//                .Select(uag => uag.AchievementGroupId);

            Expression<Func<TEntity, bool>> toAdd = g => g.UserAchievementGroups
                .Any(uag => uag.UserId == filter.UserId);
//            Expression<Func<TEntity, bool>> toAdd = g => groupIds.Contains(g.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterExpired(TFilterDto filter)
        {
            if (filter.NonExpired == false)
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = g => g.ExpiredIn < DateTime.Today;
            TmpPredicates.Add(toAdd);
        }
    }
}