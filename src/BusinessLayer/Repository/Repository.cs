using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using BusinessLayer.Helpers;
using BusinessLayer.Services.Common;
using DAL;
using DAL.Entities;
using DAL.Entities.Interfaces;
using DAL.Entities.JoinTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BusinessLayer.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {

        private readonly IMapper _mapper;
        private readonly DbContext _context;
        private readonly Types _classTypes;
        private readonly Type _actualType;

        public Repository(IMapper mapper, DbContext context, Types classTypes)
        {
            _mapper = mapper;
            _context = context;
            _classTypes = classTypes;
            _actualType = classTypes.GetActualTypeForUsage(typeof(TEntity));
        }
        public async Task<int> Create(TEntity entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task CreateRange(IEnumerable<TEntity> list)
        {
            await _context.BulkInsertAsync(list);
        }

        public async Task Update(TEntity entity)
        {
            var exists = await _context.FindAsync(_actualType, entity.Id);
            if (exists != null)
            {
                _context
                    .Entry(exists)
                    .CurrentValues
                    .SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> Get(int id)
        {
            var entity = await _context.FindAsync(_actualType, id);
            return (TEntity) entity;
        }


        public async Task Delete(int id)
        {
            var entity = await _context.FindAsync(_actualType, id);
            _context.Set<TEntity>().Remove((TEntity)entity);
            await _context.SaveChangesAsync();
        }


        public async Task<TEntity> GetWithIncludes(int id, params string[] includes)
        {
            var entity = _context.Set<TEntity>(_actualType)
                .Where(e => e.Id == id);
            entity = includes.Aggregate(entity, (current, include) => current.Include(include));
            return await entity.FirstOrDefaultAsync();
        }


        public IQueryable<TEntity> ListAll()
        {
            return _context.Set<TEntity>(_actualType);
        }
    }
}