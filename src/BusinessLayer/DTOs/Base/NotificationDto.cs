using BusinessLayer.DTOs.Common;
using DAL.Entities;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class NotificationDto : DtoBase, ILinkToEntity<Notification>
    {
        public string Message { get; set; }

        public int UserId { get; set; }
        
        public UserDto User { get; set; }

        public bool WasShowed { get; set; }
    }
}