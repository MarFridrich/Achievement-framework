using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.BaHuEntities.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.QueryObjects.Base
{
    public abstract class QueryBase<TEntity, TDto, TFilter>
        where TEntity : class, IEntity
        where TFilter : FilterDtoBase
    {
        protected readonly IMapper Mapper;

        protected DbContext Context { get; set; }
        
        protected Types ActualTypes { get; set; }

        private readonly Type _realModel;
        
        protected List<Expression<Func<TEntity, bool>>> TmpPredicates = new List<Expression<Func<TEntity, bool>>>();
        protected Expression<Func<TEntity, bool>> Predicate { get; set; }
        protected string OrderBy { get; set; }
        protected bool OrderByDescending { get; set; } = false;

        private const int DefaultPageSize = 10;

        public int PageSize { get; private set; } = DefaultPageSize;

        public int? DesiredPage { get; private set; }


        public QueryBase(DbContext context, IMapper mapper, Types actualTypes)
        {
            Context = context;
            Mapper = mapper;
            ActualTypes = actualTypes;
            _realModel = actualTypes.GetActualTypeForUsage(typeof(TEntity));
        }

        protected abstract void ApplyWhereClause(TFilter filter);
        private void CombineTmpPredicatesToOne()
        {
            
            foreach (var predicate in TmpPredicates)
            {
                Predicate = Predicate != null ? Predicate.And(predicate) : predicate;
            }
        }

        private static void CheckPagingSizes(TFilter filter)
        {
            if (filter.RequestedPageNumber.HasValue && filter.RequestedPageNumber < 1)
            {
                throw new ArgumentException("Requested page number must be greater than 0");
            }
        }

        private void ClearPredicates()
        {
            TmpPredicates.Clear();
            Predicate = null;
            OrderBy = null;
            OrderByDescending = false;
        }

        private void SetVariablesFromFilter(TFilter filter)
        {
            OrderBy = filter.SortCriteria;
            OrderByDescending = !filter.SortAscending;
        }
        public async Task<QueryResult<TDto>> ExecuteAsync(TFilter filter)
        {
            
            ClearPredicates();
            SetVariablesFromFilter(filter);
            ApplyWhereClause(filter);
            CombineTmpPredicatesToOne();
            var queryable = Context.Set<TEntity>(_realModel);

            queryable = filter.Includes.Aggregate(queryable, (current, include) => current.Include(include));
            if (Predicate != null)
            {
                queryable = queryable.Where(Predicate);
            }

            var itemsCount = queryable.Count();
            
            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                queryable = OrderByDescending
                    ? queryable.OrderByDescending(OrderBy)
                    : queryable.OrderBy(OrderBy);
            }
            
            if (filter.RequestedPageNumber.HasValue)
            {
                CheckPagingSizes(filter);
                if (filter.PageSize > 0)
                {
                    PageSize = filter.PageSize;
                }
                
                queryable = queryable.Skip(PageSize * (filter.RequestedPageNumber.Value - 1)).Take(PageSize);
            }
    
            var list = await queryable.ToListAsync();
            var mappedList = Mapper.Map<IList<TDto>>(list);

            return new QueryResult<TDto>(mappedList, itemsCount, PageSize, filter.RequestedPageNumber);
        }
    }
}