using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.Notification;
using DAL.BaHuEntities;

namespace BusinessLayer.Facades
{
    public class NotificationFacade<TEntity, TNotificationDto, TNotificationFilterDto>
        where TEntity : BaHuNotification, new()
        where TNotificationDto : BaHuNotificationDto
        where TNotificationFilterDto : NotificationFilterDto, new()
    {
        protected INotificationService<TEntity, TNotificationDto, TNotificationFilterDto> NotificationService;

        public NotificationFacade(INotificationService<TEntity, TNotificationDto, TNotificationFilterDto> notificationService)
        {
            NotificationService = notificationService;
        }

        public IQueryable<TNotificationDto> ListAll()
        {
            return NotificationService.ListAll();
        }

        public async Task<TNotificationDto> GetNotificationByIdWithIncludes(int id, params string[] includes)
        {
            return await NotificationService.GetWithIncludes(id, includes);
        }

        public async Task<TNotificationDto> GetNotificationById(int id)
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

        public async Task<QueryResult<TNotificationDto>> FilterNotification(TNotificationFilterDto filter)
        {
            return await NotificationService.ApplyFilter(filter);
        }

        public async Task MarkAsUnread(int id)
        {
            await NotificationService.MarkAsUnread(id);
        }

        public async Task<QueryResult<TNotificationDto>> GetNotificationForUser(int userId)
        {
            return await NotificationService.ApplyFilter(new TNotificationFilterDto
            {
                UserId = userId
            });
        }
    }
}