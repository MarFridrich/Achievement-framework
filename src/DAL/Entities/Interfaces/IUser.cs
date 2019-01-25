namespace DAL.Entities.Interfaces
{
    public interface IUser : IEntity
    {
        
        string Name { get; set; }
        
        string Surname { get; set; }
        
        IRole Role { get; set; }
    }
}