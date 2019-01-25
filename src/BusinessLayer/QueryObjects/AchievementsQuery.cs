using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BusinessLayer.DTOs.Filter;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Enums;
using DAL;
using DAL.Entities;
using DAL.Entities.Interfaces;
using LinqKit;
using FilterDtoBase = BusinessLayer.DTOs.Common.FilterDtoBase;

namespace BusinessLayer.QueryObjects
{
    public class AchievementsQuery : QueryBase<Achievement, AchievementDto, AchievementFilterDto>
    {
        public AchievementsQuery(
            AchievementDbContext context, IMapper mapper) :
            base(context, mapper)
        {
        }

        protected override void ApplyWhereClaus(AchievementFilterDto filter)
        {
            var predicates = new List<Expression<Func<Achievement, bool>>>();
            FilterAchievementEvaluation(predicates, filter);
            FilterPeopleDoneCount(predicates,filter);
            foreach (var predicate in predicates)
            {
                Predicate = x => Predicate.Invoke(x) && predicate.Invoke(x);
            }
        }

        private void FilterAchievementEvaluation(ICollection<Expression<Func<Achievement, bool>>> list,
            AchievementFilterDto filter)
        {
            if (filter.EvaluationType == null)
            {
                return;
            }
            
            Expression<Func<Achievement, bool>> toAdd = ach => ach.Evaluation == (DAL.Entities.Evaluations) filter.EvaluationType;
            list.Add(toAdd);
        }

        private void FilterPeopleDoneCount(List<Expression<Func<Achievement, bool>>> list,
            AchievementFilterDto filter)
        {
            if (filter.PeopleDoneCount == null)
            {
                return;
            }

            Expression<Func<Achievement, bool>> toAdd;
            if (filter.PeopleDoneCountLowerThan)
            {
                toAdd = ach => ach.UserCompletedAchievements.Count <= filter.PeopleDoneCount;
            }
            else
            {
                toAdd = ach => ach.UserCompletedAchievements.Count >= filter.PeopleDoneCount;
            }
            
            list.Add(toAdd);
        }
    }
}