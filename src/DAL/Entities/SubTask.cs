using System.ComponentModel.DataAnnotations;
using DAL.Entities.Interfaces;

namespace DAL.Entities
{
    public class SubTask : IEntity
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public Achievement Achievement { get; set; }
    }
}