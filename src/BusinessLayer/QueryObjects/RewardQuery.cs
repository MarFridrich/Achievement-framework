using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using DAL.Entities;
using DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class RewardQuery<TEntity, TDto> : QueryBase<TEntity, TDto, RewardFilterDto>
        where TEntity : FrameworkReward
        where TDto : RewardDto
    
    {
        public RewardQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }

        private void FilterName(RewardFilterDto filter)
        {
            if (string.IsNullOrEmpty(filter.Name))
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = r => r.Name.Contains(filter.Name);
            TmpPredicates.Add(toAdd);
        }
        
        private void FilterAchievementId(RewardFilterDto filter)
        {
            if (filter.AchievementId == 0)
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = r => r.Achievements.FirstOrDefault(a => a.Id == filter.AchievementId) != null;
            TmpPredicates.Add(toAdd);
        }

        protected override void ApplyWhereClause(RewardFilterDto filter)
        {
            FilterName(filter);
            FilterAchievementId(filter);
        }
    }
}