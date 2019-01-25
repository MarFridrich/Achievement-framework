using DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class Role : IdentityRole<int>, IEntity
    {
        
    }
}