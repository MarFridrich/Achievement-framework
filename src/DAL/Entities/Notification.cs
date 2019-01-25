using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Entities.Interfaces;

namespace DAL.Entities
{
    public class Notification : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string Message { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public User User { get; set; }
        
        [Required]
        public bool WasShowed { get; set; }
    }
}