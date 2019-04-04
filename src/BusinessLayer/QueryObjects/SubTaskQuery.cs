using System;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class SubTaskQuery<TEntity, TSubTaskDto> : QueryBase<TEntity, TSubTaskDto, SubTaskFilterDto>
        where TEntity : FrameworkSubTask
        where TSubTaskDto : SubTaskDto
    {
        public SubTaskQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
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
        protected override void ApplyWhereClaus(SubTaskFilterDto filter)
        {
            FilterName(filter);
            FilterAchievementId(filter);
            FilterDescription(filter);
        }
    }
}