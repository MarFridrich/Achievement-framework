using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Common;
using DAL;
using GenericServices;

namespace BusinessLayer.Services.Generic.User
{
    public class UserService<TUserDto, TAchievementGroupDto, TAchievementDto> : CrudServiceBase<DAL.Entities.User, TUserDto>, IUserService<TUserDto, TAchievementGroupDto, TAchievementDto>
        where TUserDto : UserDto
        where TAchievementGroupDto : AchievementGroupDto
        where TAchievementDto : AchievementDto

    {
        private readonly ICrud<DAL.Entities.AchievementGroup, AchievementGroupDto> _achievementGroupRepository;
        public UserService(IMapper mapper, ICrudServicesAsync service, AchievementDbContext context,
            ICrud<DAL.Entities.AchievementGroup, AchievementGroupDto> achievementGroupRepository) 
            : base(mapper, service, context)
        {
            _achievementGroupRepository = achievementGroupRepository;
        }

        public async Task<IEnumerable<TAchievementGroupDto>> GetAchievementGroupsForUser(int userId)
        {
            var user = await Get(userId);
            var userGroups = user?.UserGroups.ToList();
            return userGroups?
                .Select(a => a.AchievementGroup as TAchievementGroupDto)
                .ToList();
        } 
        
        public async Task<IEnumerable<TAchievementDto>> GetAchievementsForUser(int userId)
        {
            var user = await Service.ReadSingleAsync<DAL.Entities.User>(userId);
            return user?
                .UserGroups
                .SelectMany(a => Mapper.Map<IEnumerable<TAchievementDto>>(a.AchievementGroup.Achievements))
                .ToList();
        }

        public async Task<IEnumerable<TUserDto>> GetUsersFromAchievementGroup(int groupId)
        {
            var group = await _achievementGroupRepository.Get(groupId);
            return group?
                .Users
                .Cast<TUserDto>()
                .ToList();
        } 
    }
}