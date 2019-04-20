using System;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using DAL.BaHuEntities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects
{
    public class NotificationQuery<TEntity, TDto> : QueryBase<TEntity, TDto, NotificationFilterDto>
        where TEntity : BaHuNotification, new()
        where TDto : DtoBase
    {
        public NotificationQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }


        private void FilterUserId(NotificationFilterDto filter)
        {
            if (filter.UserId == 0)
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = g => g.UserId == filter.UserId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterShowedOnly(NotificationFilterDto filter)
        {
            if (filter.UnReadOnly == false)
            {
                return;
            }
            Expression<Func<TEntity, bool>> toAdd = g => g.WasShowed == false;
            TmpPredicates.Add(toAdd);
            
        }

        private void FilterMessageString(NotificationFilterDto filter)
        {
            if (string.IsNullOrEmpty(filter.Message))
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = g => g.Message.Contains(filter.Message);
            TmpPredicates.Add(toAdd);
        }

        private void FilterAchievementId(NotificationFilterDto filter)
        {
            if (filter.AchievementId == 0)
            {
                return;
            }

            Expression<Func<TEntity, bool>> toAdd = g =>
                g.AchievementId.HasValue && g.AchievementId == filter.AchievementId;
            TmpPredicates.Add(toAdd);
        }

        private void FilterDateTime(NotificationFilterDto filter)
        {
            if (filter.OlderThen == DateTime.MaxValue)
            {
                return;
            }
            
            Expression<Func<TEntity, bool>> toAdd = g => g.Created < filter.OlderThen;
            TmpPredicates.Add(toAdd);
        }
        protected override void ApplyWhereClause(NotificationFilterDto filter)
        {
            FilterUserId(filter);
            FilterShowedOnly(filter);
            FilterAchievementId(filter);
            FilterMessageString(filter);
            FilterDateTime(filter);
        }
    }
}