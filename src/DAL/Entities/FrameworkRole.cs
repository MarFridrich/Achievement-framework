using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Entities
{
    public class FrameworkRole : IdentityRole<int>, IEntity
    {
    }
}