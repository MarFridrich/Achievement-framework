using DAL.BaHuEntities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.BaHuEntities
{
    public class BaHuRole : IdentityRole<int>, IEntity
    {
    }
}