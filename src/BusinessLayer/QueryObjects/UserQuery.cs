using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.DTOs.Filter.Enums;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.Entities;
using DAL.Entities.JoinTables;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BusinessLayer.QueryObjects
{
    public class UserQuery<TUserDto, TFilter> : QueryBase<FrameworkUser, TUserDto, TFilter>
        where TUserDto : UserDto
        where TFilter : UserFilterDto
    {
        
        private IList<int> _completedAchievementsIds { get; set; }
        private IList<int> _askedAchievementsIds { get; set; }
        
        private IList<int> _completedSubTasksIds { get; set; }

        private TFilter _filter { get; set; }


        public UserQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }

        private IList<int> GetIdsOfCompletedAchievements()
        {
            if (_filter.AchievementId != 0)
            {
                return Context
                    .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                    .Where(uc => uc.AchievementId == _filter.AchievementId)
                    .Select(uca => uca.UserId)
                    .ToList();
            }

            if (_filter.GroupId != 0)
            {
                return Context
                    .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                    .Where(uc => uc.Achievement.AchievementGroupId == _filter.GroupId)
                    .Select(uca => uca.UserId)
                    .ToList();
            }
            
            return Context
                .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                .Select(uca => uca.UserId)
                .ToList();
        }
        
        private IList<int> GetIdsOfAskedAchievements()
        {
            if (_filter.AchievementId != 0)
            {
                return Context
                    .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                    .Where(uc => uc.AchievementId == _filter.AchievementId)
                    .Select(uar => uar.UserId)
                    .ToList();
            }

            if (_filter.GroupId != 0)
            {
                return Context
                    .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                    .Where(uc => uc.Achievement.AchievementGroupId == _filter.GroupId)
                    .Select(uar => uar.UserId)
                    .ToList();
            }
            return Context
                .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                .Select(uar => uar.UserId)
                .ToList();
        }

        private IList<int> GetIdsOfCompletedSubTasks()
        {
            if (_filter.AchievementId != 0)
            {
                return  Context
                    .Set<FrameworkUserCompletedSubTask>(ActualTypes.FrameworkUserCompletedSubTask)
                    .Where(uc => uc.SubTask.AchievementId == _filter.AchievementId)
                    .Select(ucs => ucs.UserId)
                    .ToList();
            }

            if (_filter.GroupId != 0)
            {
                return  Context
                    .Set<FrameworkUserCompletedSubTask>(ActualTypes.FrameworkUserCompletedSubTask)
                    .Where(uc => uc.SubTask.Achievement.AchievementGroupId == _filter.GroupId)
                    .Select(ucs => ucs.UserId)
                    .ToList();
            }
            return  Context
                .Set<FrameworkUserCompletedSubTask>(ActualTypes.FrameworkUserCompletedSubTask)
                .Select(ucs => ucs.UserId)
                .ToList();
        }
        
        private IList<int> GetIdsOfAskedUsers()
        {
            if (_filter.AchievementId != 0)
            {
                return Context
                    .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                    .Where(ur => ur.AchievementId == _filter.AchievementId)
                    .Select(uar => uar.UserId)
                    .Concat(Context
                    .Set<FrameworkUserAskedForSubTask>(ActualTypes.FrameworkUserAskedForSubTask)
                    .Where(u => u.SubTask.AchievementId == _filter.AchievementId)
                    .Select(uas => uas.UserId))
                    .ToList();
            }

            if (_filter.GroupId != 0)
            {
                return  Context
                    .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                    .Where(ur => ur.Achievement.AchievementGroupId == _filter.GroupId)
                    .Select(uar => uar.UserId)
                    .Concat(Context
                        .Set<FrameworkUserAskedForSubTask>(ActualTypes.FrameworkUserAskedForSubTask)
                        .Where(u => u.SubTask.Achievement.AchievementGroupId == _filter.GroupId)
                        .Select(uas => uas.UserId))
                    .ToList();
            }
            return  Context
                .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                .Select(uar => uar.UserId)
                .Concat(Context
                    .Set<FrameworkUserAskedForSubTask>(ActualTypes.FrameworkUserAskedForSubTask)
                    .Select(uas => uas.UserId))
                .ToList();
        }
        
        private void FilterUsersAchievementId(UserFilterDto filter)
        {
            if (filter.AchievementId == 0)
            {
                return;
            }
            
            Expression<Func<FrameworkUser, bool>> toAdd = u => u.UserGroups
                                .SelectMany(ug => ug.AchievementGroup.Achievements)
                                .FirstOrDefault(a => a.Id == filter.AchievementId) != null;
            TmpPredicates.Add(toAdd);
        }

        private void FilterCompletedAchievements()
        {
            Expression<Func<FrameworkUser, bool>> toAdd = u => _completedAchievementsIds.Contains(u.Id);
            TmpPredicates.Add(toAdd);
            //Expression<Func<FrameworkUser, bool>> tet = u => u.UserCompletedAchievements.Where(uc => uc.UserId == u.i);
            //return queryable
            //    .Join(Context.Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements),
            //        u => u.Id, uca => uca.UserId, (u, uca) => new {u, uca})
            //    .Where(uuca => uuca.uca.Achievement.Id == filter.AchievementId)
            //    .Select(uuca => uuca.u);
        }

        private void FilterAskedForReward()
        {

            Expression<Func<FrameworkUser, bool>> toAdd = u => GetIdsOfAskedUsers().Contains(u.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterNonCompletedAchievements()
        {
            
            Expression<Func<FrameworkUser, bool>> toAdd = u => !_completedAchievementsIds.Contains(u.Id);
            TmpPredicates.Add(toAdd);
        }

        private void FilterPartialDoneAchievements()
        {
            Expression<Func<FrameworkUser, bool>> toAdd = u => _completedSubTasksIds.Except(_completedAchievementsIds)
                .Distinct()
                .Contains(u.Id);
            TmpPredicates.Add(toAdd);
        }


        private void FilterNotStartedAchievements()
        {
            Expression<Func<FrameworkUser, bool>> toAdd = u => !_completedAchievementsIds
                .Union(_completedSubTasksIds)
                .Union(_askedAchievementsIds)
                .Distinct()
                .Contains(u.Id);
           
            TmpPredicates.Add(toAdd);
        }

        private void FilterGroupId(UserFilterDto filter)
        {
            if (filter.GroupId == 0)
            {
               return;
            }
            
            Expression<Func<FrameworkUser, bool>> toAdd = u =>
                u.UserGroups.FirstOrDefault(ug => ug.AchievementGroup.Id == filter.GroupId) != null;
            TmpPredicates.Add(toAdd);

        }

        private void FilterFulfillType(UserFilterDto filter)
        {
            if (!filter.AchievementFulfillType.HasValue || filter.AchievementId == 0) return;
            switch (filter.AchievementFulfillType)
            {
                case UserFulfillType.Done:
                    FilterCompletedAchievements();
                    break;
                case UserFulfillType.Partial
                    :FilterPartialDoneAchievements();
                    break;
                case UserFulfillType.AskedForReward:
                    FilterAskedForReward();
                    break;
                case UserFulfillType.Nothing:
                    FilterNotStartedAchievements();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void ApplyWhereClause(TFilter filter)
        {
            _filter = filter;
            _completedAchievementsIds = GetIdsOfCompletedAchievements();
            _askedAchievementsIds = GetIdsOfAskedAchievements();
            _completedSubTasksIds = GetIdsOfCompletedSubTasks();
            FilterFulfillType(filter);
            FilterGroupId(filter);
            FilterUsersAchievementId(filter);
        }
    }
}