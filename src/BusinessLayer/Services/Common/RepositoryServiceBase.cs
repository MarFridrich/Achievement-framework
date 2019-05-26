using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using BusinessLayer.Helpers;
using BusinessLayer.Repository;
using DAL.BaHuEntities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Common
{
    public abstract class RepositoryServiceBase<TEntity, TDto, TFilter> : ServiceBase
        where TEntity : class, IEntity
        where TDto : DtoBase
        where TFilter : FilterDtoBase
    {
        protected readonly DbContext Context;
        protected readonly IRepository<TEntity> Repository;
        protected readonly Types ActualModels;
        private readonly Type _actualModel;
        

        protected RepositoryServiceBase(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels)
            : base(mapper)
        {
            Context = context;
            Repository = repository;
            ActualModels = actualModels;
            _actualModel = actualModels.GetActualTypeForUsage(typeof(TEntity));
        }

        public virtual async Task<int> Create(TDto entity)
        {
            return await Repository.Create((TEntity) Mapper.Map(entity, typeof(TDto), _actualModel));
        }

        public virtual async Task CreateList(IEnumerable<TDto> entity)
        {
            await Repository.CreateRange((IEnumerable<TEntity>) Mapper.Map(entity, typeof(IEnumerable<TDto>),
                typeof(IEnumerable<>).MakeGenericType(_actualModel)));
        }

        public virtual async Task Update(TDto entity)
        {
            await Repository.Update((TEntity) Mapper.Map(entity, typeof(TDto), _actualModel));
        }

        public virtual async Task<TDto> Get(int id)
        {
            return Mapper.Map<TDto>(await Repository.Get(id));
        }

        public virtual async Task Delete(int id)
        {
            await Repository.Delete(id);
        }

        public virtual IQueryable<TDto> ListAll()
        {
            var list = Repository.ListAll();
            return list
                .Select(e => Mapper.Map<TDto>(e));
        }

        public virtual async Task<TDto> GetWithIncludes(int id, params string[] includes)
        {

            return Mapper.Map<TDto>(await Repository.GetWithIncludes(id, includes));
        }
        

    }
}