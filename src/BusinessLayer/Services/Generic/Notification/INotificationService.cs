using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using DAL.BaHuEntities.Interfaces;

namespace BusinessLayer.Services.Generic.Notification
{
    public interface INotificationService<TNotification, TNotificationDto, TFilterDto>
        where TNotification : IEntity, new()
        where TNotificationDto : BaHuNotificationDto
    {
        IQueryable<TNotificationDto> ListAll();

        Task<QueryResult<TNotificationDto>> ApplyFilter(TFilterDto filter);

        Task CreateList(IEnumerable<TNotificationDto> entity);
     
        Task<TNotificationDto> Get(int id);
        
        Task<TNotificationDto> GetWithIncludes(int id, params string[] includes);
     
        Task<int> Create(TNotificationDto entity);
     
        Task Update(TNotificationDto entity);
     
        Task Delete(int id);

        Task MarkAsUnread(int id);
    }
}