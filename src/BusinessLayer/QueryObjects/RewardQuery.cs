using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using DAL.BaHuEntities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class RewardQuery<TEntity, TDto, TFilterDto> : QueryBase<TEntity, TDto, TFilterDto>
        where TEntity : BaHuReward
        where TDto : BaHuRewardDto
        where TFilterDto : RewardFilterDto
    
    {
        public RewardQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }

        private void FilterName(TFilterDto filter)
        {
            if (string.IsNullOrEmpty(filter.Name))
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = r => r.Name.Contains(filter.Name);
            TmpPredicates.Add(toAdd);
        }
        
        private void FilterAchievementId(TFilterDto filter)
        {
            if (filter.AchievementId == 0)
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = r => r.Achievements.Any(a => a.Id == filter.AchievementId);
            TmpPredicates.Add(toAdd);
        }

        protected override void ApplyWhereClause(TFilterDto filter)
        {
            FilterName(filter);
            FilterAchievementId(filter);
        }
    }
}