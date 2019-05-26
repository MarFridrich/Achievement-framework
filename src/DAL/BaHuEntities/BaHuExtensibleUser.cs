using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.BaHuEntities.Interfaces;

namespace DAL.BaHuEntities
{
    public class BaHuExtensibleUser : IEntity
    
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}