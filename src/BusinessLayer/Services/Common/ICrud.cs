using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Common;
using DAL.Entities.Interfaces;

namespace BusinessLayer.Services.Common
{
    public interface ICrud<TEntity, TDto>
        where TEntity : class, IEntity, new()
        where TDto : DtoBase
    {
        Task<TEntity> Create(TDto entity);

        Task Update(TDto entity);

        Task<TDto> Get(int id);

        Task Delete(int id);

        Task<TDto> GetWithIncludes(int id, params string[] includes);
    }
}