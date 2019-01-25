using DAL.Entities;
using DAL.Entities.Interfaces;

namespace DAL
{
    public interface IRole : IEntity
    {
        string Name { get; set; }
       
    }
}