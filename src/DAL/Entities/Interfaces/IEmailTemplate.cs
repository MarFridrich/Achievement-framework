using DAL.Entities;
using DAL.Entities.Interfaces;

namespace DAL
{
    public interface IEmailTemplate : IEntity
    {
        string Message { get; set; }
    }
}