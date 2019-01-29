using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using BusinessLayer.QueryObjects.Base.Results;
using DAL;
using DAL.Entities;
using DAL.Entities.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using IRole = DAL.IRole;

namespace BusinessLayer.QueryObjects.Base
{
    public abstract class QueryBase<TEntity, TDto, TFilter>
        where TEntity : class, IEntity
        where TFilter : FilterDtoBase
    {
        private readonly IMapper _mapper;

        protected AchievementDbContext Context { get; set; }
        protected Expression<Func<TEntity, bool>> Predicate { get; set; }
        protected string OrderBy { get; set; }
        protected bool OrderByDescending { get; set; } = false;

        private const int DefaultPageSize = 10;

        public int PageSize { get; private set; } = DefaultPageSize;

        public int? DesiredPage { get; private set; }


        public QueryBase(AchievementDbContext context, IMapper mapper)
        {
            this.Context = context;
            this._mapper = mapper;
        }

        protected abstract void ApplyWhereClaus(TFilter filter);

        public async Task<QueryResult<TDto>> ExecuteAsync(TFilter filter)
        {
            ApplyWhereClaus(filter);
            IQueryable<TEntity> queryable = Context.Set<TEntity>();

            if (Predicate != null)
            {
                queryable = queryable.Where(Predicate);
            }

            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                var prop = TypeDescriptor.GetProperties(typeof(TEntity)).Find(OrderBy, true);
                queryable = OrderByDescending
                    ? queryable.OrderByDescending(x => prop.GetValue(x))
                    : queryable.OrderBy(x => prop.GetValue(x));
            }

            var list = await queryable.ToListAsync();
            var mappedList = _mapper.Map<IList<TDto>>(list);

            return new QueryResult<TDto>(mappedList, PageSize, DesiredPage);
        }
    }
}