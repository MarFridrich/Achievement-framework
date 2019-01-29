using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;

namespace BusinessLayer.Services.Generic.Notification
{
    public interface INotificationService<TNotificationDto>
        where TNotificationDto : NotificationDto
    {
        Task<IEnumerable<TNotificationDto>> ListAllAsync();
     
        Task<TNotificationDto> Get(int id);
     
        Task<DAL.Entities.Notification> Create(TNotificationDto entity);
     
        Task Update(TNotificationDto entity);
     
        Task Delete(int id);

        Task<IEnumerable<TNotificationDto>> GetNotificationsForUser(int userId);
    }
}