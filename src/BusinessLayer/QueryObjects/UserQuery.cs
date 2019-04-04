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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BusinessLayer.QueryObjects
{
    public class UserQuery<TUserDto> : QueryBase<FrameworkUser, TUserDto, UserFilterDto>
        where TUserDto : UserDto
    {
        public UserQuery(DbContext context, IMapper mapper, Types actualTypes) : base(context, mapper, actualTypes)
        {
        }


        private IQueryable<FrameworkUser> FilterUsersAchievementId(IQueryable<FrameworkUser> queryable, UserFilterDto filter)
        {
            if (filter.AchievementId == 0)
            {
                return queryable;
            }

            return queryable
                .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.AchievementGroup)
                .ThenInclude(ag => ag.Achievements)
                .Where(u => u.UserGroups
                                .SelectMany(ug => ug.AchievementGroup.Achievements)
                                .FirstOrDefault(a => a.Id == filter.AchievementId) != null);
        }

        private IQueryable<FrameworkUser> FilterCompletedAchievements(IQueryable<FrameworkUser> queryable, UserFilterDto filter)
        {
            return queryable
                .Join(Context.Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements),
                    u => u.Id, uca => uca.UserId, (u, uca) => new {u, uca})
                .Where(uuca => uuca.uca.Achievement.Id == filter.AchievementId)
                .Select(uuca => uuca.u);
        }

        private IQueryable<FrameworkUser> FilterAskedForReward(IQueryable<FrameworkUser> queryable, UserFilterDto filter)
        {
            var userIds = Context
                .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                .Where(uar => uar.AchievementId == filter.AchievementId)
                .Select(uar => uar.UserId);

            Expression<Func<FrameworkUser, bool>> toAdd = u => userIds.Contains(u.Id);
            TmpPredicates.Add(toAdd);
            return
                queryable
                    .Where(u => userIds.Contains(u.Id));
        }

        private IQueryable<FrameworkUser> FilterNonCompletedAchievements(IQueryable<FrameworkUser> queryable)
        {
            var completedIds = Context
                .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                .Select(uca => uca.UserId);
            
            Expression<Func<FrameworkUser, bool>> toAdd = u => !completedIds.Contains(u.Id);
            TmpPredicates.Add(toAdd);
            return queryable
                .Where(u => !completedIds.Contains(u.Id));
        }

        private IQueryable<FrameworkUser> FilterPartialDoneAchievements(IQueryable<FrameworkUser> queryable)
        {
            var completedIds = Context
                .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                .Select(uca => uca.UserId);
            var subTaskDoneIds = Context
                .Set<FrameworkUserCompletedSubTask>(ActualTypes.FrameworkUserCompletedSubTask)
                .Select(ucs => ucs.UserId);
            
            Expression<Func<FrameworkUser, bool>> toAdd = u => subTaskDoneIds.Except(completedIds)
                .Distinct()
                .Contains(u.Id);
            TmpPredicates.Add(toAdd);
            return queryable
                .Where(u => subTaskDoneIds.Except(completedIds)
                    .Distinct()
                    .Contains(u.Id));
        }

        private IQueryable<FrameworkUser> FilterNotStartedAchievements(IQueryable<FrameworkUser> queryable, UserFilterDto filter)
        {
            var completedIds = Context
                .Set<FrameworkUserCompletedAchievements>(ActualTypes.FrameworkUserCompletedAchievements)
                .Where(uca => uca.AchievementId == filter.AchievementId)
                .Select(uca => uca.UserId);
            var subTaskDoneIds = Context
                .Set<FrameworkUserCompletedSubTask>(ActualTypes.FrameworkUserCompletedSubTask)
                .Where(ucs => ucs.SubTask.AchievementId == filter.AchievementId)
                .Select(ucs => ucs.UserId);
            var askedIds = Context
                .Set<FrameworkUserAskedForReward>(ActualTypes.FrameworkUserAskedForReward)
                .Where(uar => uar.AchievementId == filter.AchievementId)
                .Select(uar => uar.UserId);
            
            Expression<Func<FrameworkUser, bool>> toAdd = u => !completedIds
                .Union(subTaskDoneIds)
                .Union(askedIds)
                .Distinct()
                .Contains(u.Id);
            TmpPredicates.Add(toAdd);
            
            return queryable
                .Where(u => !completedIds
                    .Union(subTaskDoneIds)
                    .Union(askedIds)
                    .Distinct()
                    .Contains(u.Id));
        }

        private IQueryable<FrameworkUser> FilterGroupId(IQueryable<FrameworkUser> queryable, UserFilterDto filter)
        {
            
            //Expression<Func<FrameworkUser, bool>> toAdd = u => 
            //TmpPredicates.Add(toAdd);
            
            return
                queryable
                    .Include(u => u.UserGroups)
                    .ThenInclude(ug => ug.AchievementGroup)
                    .ThenInclude(ag => ag.Achievements)
                    .Where(u => u.UserGroups.FirstOrDefault(ug => ug.AchievementGroup.Id == filter.GroupId) != null);

        }

        protected override void ApplyWhereClaus(UserFilterDto filter)
        {
            throw new System.NotImplementedException();
        }

        public async Task<QueryResult<TUserDto>> UseFilter(UserFilterDto filter)
        {
            var queryable = Context.Set<FrameworkUser>()
                .AsQueryable();
            
            if (filter.AchievementId != 0)
            {
                queryable = FilterUsersAchievementId(queryable, filter);
            }

            if (filter.GroupId != 0)
            {
                queryable = FilterGroupId(queryable, filter);
            }

            if (filter.AchievementFulfillType.HasValue && filter.AchievementId != 0)
            {
                switch (filter.AchievementFulfillType)
                {
                    case UserFulfillType.Done:
                        queryable = FilterCompletedAchievements(queryable, filter);
                        break;
                    case UserFulfillType.Partial:
                        queryable = FilterPartialDoneAchievements(queryable);
                        break;
                    case UserFulfillType.AskedForReward:
                        queryable = FilterAskedForReward(queryable, filter);
                        break;
                    case UserFulfillType.Nothing:
                        queryable = FilterNotStartedAchievements(queryable, filter);
                        break;
                    case null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }

            queryable = filter.Includes.Aggregate(queryable, (current, include) => current.Include(include));
            var itemsCount = queryable.Count();
                
            var list = await queryable.ToListAsync();
            var mappedList = Mapper.Map<IList<TUserDto>>(list);

            return new QueryResult<TUserDto>(mappedList, itemsCount, PageSize, filter.RequestedPageNumber);
            
            
        }
    }
}