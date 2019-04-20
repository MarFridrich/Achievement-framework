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
    public class AchievementGroupQuery<TEntity, TDto> : QueryBase<TEntity, TDto, AchievementGroupFilterDto>
        where TEntity : BaHuAchievementGroup
        where TDto : BaHuAchievementGroupDto
    {
        public AchievementGroupQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }

        protected override void ApplyWhereClause(AchievementGroupFilterDto filter)
        {
            FilterGroupName(filter);
            FilterOwnerId(filter);
            FilterUserId(filter);
            FilterExpired(filter);
        }

        private void FilterGroupName(AchievementGroupFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Name))
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = g => g.Name.Contains(filter.Name);
            TmpPredicates.Add(toAdd);
        }

        private void FilterOwnerId(AchievementGroupFilterDto filter)
        {
            if (filter.OwnerId == 0)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd = g => g.OwnerId == filter.OwnerId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterUserId(AchievementGroupFilterDto filter)
        {
            if (filter.UserId == 0)
            {
                return;
            }

            var groupIds = Context.Set<BaHUserAchievementGroup>(ActualTypes.BaHUserAchievementGroup)
                .Where(uag => uag.UserId == filter.UserId)
                .Select(uag => uag.AchievementGroupId);
            
            Expression<Func<TEntity, bool>> toAdd = g => groupIds.Contains(g.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterExpired(AchievementGroupFilterDto filter)
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