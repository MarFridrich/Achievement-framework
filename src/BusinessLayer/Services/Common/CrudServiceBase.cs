using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using DAL;
using DAL.Entities.Interfaces;
using GenericServices;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Common
{
    public abstract class CrudServiceBase<TEntity, TDto> : ServiceBase<TDto>
        where TEntity : class, IEntity, new()
        where TDto : DtoBase
    {
        protected readonly ICrudServicesAsync Service;
        

        protected AchievementDbContext Context;

        protected CrudServiceBase(IMapper mapper, ICrudServicesAsync service)
            : base(mapper)
        {
            Service = service;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            return await Service.CreateAndSaveAsync(entity);
            
        }

        public virtual async Task Update(TDto entity)
        {
            await Service.UpdateAndSaveAsync(entity);
        }

        public virtual async Task<TDto> Get(int id)
        {
           return await Service.ReadSingleAsync<TDto>(id);
        }

        public virtual async Task Delete(int id)
        {
            await Service.DeleteAndSaveAsync<TEntity>(id);
        }

        public virtual async Task<IEnumerable<TDto>> ListAllAsync()
        {
            return await Service.ReadManyNoTracked<TDto>().ToListAsync();
        }

        public virtual async Task<TDto> GetWithIncludes(int id, params string[] includes)
        {
            var query = Context.Set<TEntity>();
            foreach (var include in includes)
            {
                query.Include(include);
            }

            var entity = Mapper.Map<TDto>(await query.FirstOrDefaultAsync(e => e.Id.Equals(id)));
            return entity;
        }
        

    }
}