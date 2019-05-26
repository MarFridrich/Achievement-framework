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
using DAL.BaHuEntities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.Notification
{
    public class NotificationService<TEntity, TNotificationDto, TFilterDto> :
        RepositoryServiceBase<TEntity, TNotificationDto, TFilterDto>,
        INotificationService<TEntity, TNotificationDto, TFilterDto>
    
        where TEntity : BaHuNotification, new()
        where TNotificationDto : BaHuNotificationDto    
        where TFilterDto : NotificationFilterDto, new()

    {
        protected readonly QueryBase<TEntity, TNotificationDto, TFilterDto> Query;


        public NotificationService(IMapper mapper, IRepository<TEntity> repository, DbContext context,
            Types actualModels, NotificationQuery<TEntity, TNotificationDto, TFilterDto> query) 
            : base(mapper, repository, context, actualModels)
        {
            Query = query;
        }

        public async Task<QueryResult<TNotificationDto>> ApplyFilter(TFilterDto filter)
        {
            return await Query.ExecuteAsync(filter);
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