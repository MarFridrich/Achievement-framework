using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.Notification;
using DAL.BaHuEntities;

namespace BusinessLayer.Facades
{
    public class NotificationFacade<TEntity, TNotificationDto>
        where TEntity : BaHuNotification, new()
        where TNotificationDto : BaHuNotificationDto
    {
        protected INotificationService<TEntity, TNotificationDto> NotificationService;

        public NotificationFacade(INotificationService<TEntity, TNotificationDto> notificationService)
        {
            NotificationService = notificationService;
        }

        public async Task<BaHuNotificationDto> GetNotification(int id)
        {
            return await NotificationService.Get(id);
        }

        public async Task RemoveNotification(int id)
        {
            await NotificationService.Delete(id);
        }

        public async Task UpdateNotification(TNotificationDto dto)
        {
            await NotificationService.Update(dto);
        }

        public async Task<int> CreateNotification(TNotificationDto dto)
        {
            return await NotificationService.Create(dto);
        }

        public async Task<QueryResult<TNotificationDto>> FilterNotification(NotificationFilterDto filter)
        {
            return await NotificationService.ApplyFilter(filter);
        }

        public async Task MarkAsUnread(int id)
        {
            await NotificationService.MarkAsUnread(id);
        }
    }
}