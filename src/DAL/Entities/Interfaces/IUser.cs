using System.Collections.Generic;
using DAL.Entities.JoinTables;

namespace DAL.Entities.Interfaces
{
    public interface IUser : IEntity
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        FrameworkExtensibleUser ExtensibleUser { get; set; }


        ICollection<FrameworkUserCompletedAchievements> UserCompletedAchievements { get; set; }

        ICollection<FrameworkUserAchievementGroup> UserGroups { get; set; }
        ICollection<FrameworkAchievementGroup> OwnAchievementGroups { get; set; }

        ICollection<FrameworkNotification> Notifications { get; set; }
    }
}