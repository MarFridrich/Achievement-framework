using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.Notification
{
    public class NotificationService<TNotification, TNotificationDto> :
        RepositoryServiceBase<TNotification, TNotificationDto, NotificationFilterDto>,
        INotificationService<TNotification, TNotificationDto>
        where TNotificationDto : NotificationDto
        where TNotification : FrameworkNotification, new()

    {

        protected readonly IRepository<FrameworkUser> UserRepository;
        protected readonly QueryBase<TNotification, TNotificationDto, NotificationFilterDto> Query;


        public NotificationService(IMapper mapper, IRepository<TNotification> repository, DbContext context,
            Types actualModels, NotificationQuery<TNotification, TNotificationDto> query,
            IRepository<FrameworkUser> userRepository) : base(mapper, repository, context, actualModels)
        {
            UserRepository = userRepository;
            Query = query;
        }

        public async Task<QueryResult<TNotificationDto>> ApplyFilter(NotificationFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
        }

        public async Task<IEnumerable<TNotificationDto>> GetNotificationsForUser(int userId)
        {
            var user = await UserRepository.Get(userId);
            if (user == null)
            {
                return new List<TNotificationDto>();
            }

            return user
                .Notifications?
                .Select(n => Mapper.Map<TNotificationDto>(n));
        }

        public async Task MarkAsUnread(int id)
        {
            var notification = await Get(id);
            if (notification == null)
            {
                return;
            }

            notification.WasShowed = false;
            await Update(notification);
        }
    }
}