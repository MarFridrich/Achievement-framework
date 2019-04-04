using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Common;
using DAL;
using DAL.Entities.Interfaces;

namespace BusinessLayer.Repository
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        Task<int> Create(TEntity entity);

        Task CreateRange(IEnumerable<TEntity> list);

        Task Update(TEntity entity);

        Task<TEntity> Get(int id);
        
        Task Delete(int id);
        
        Task<TEntity> GetWithIncludes(int id, params string[] includes);

        IQueryable<TEntity> ListAll();
    }
}