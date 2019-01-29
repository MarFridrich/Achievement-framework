using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Common;
using DAL;
using GenericServices;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.Notification
{
    public class NotificationService<TNotificationDto> : CrudServiceBase<DAL.Entities.Notification, TNotificationDto>, INotificationService<TNotificationDto>
        where TNotificationDto : NotificationDto
    {

        private readonly ICrud<DAL.Entities.User, UserDto> _userRepository;
        
        public NotificationService(IMapper mapper, ICrudServicesAsync service, AchievementDbContext context,
            ICrud<DAL.Entities.User, UserDto> userRepository) 
            : base(mapper, service, context)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TNotificationDto>> GetNotificationsForUser(int userId)
        {
            var user = await _userRepository.Get(userId);
            if (user == null)
            {
                return new List<TNotificationDto>();
            }

            return user
                .Notifications?
                .Cast<TNotificationDto>();
        } 
    }
}