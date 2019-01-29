using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DTOs.Base;
using BusinessLayer.Services.Common;
using DAL;
using DAL.Entities.JoinTables;
using GenericServices;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.AchievementGroup
{
    public class AchievementGroupService<TAchievementGroupDto> : CrudServiceBase<DAL.Entities.AchievementGroup, TAchievementGroupDto>, IAchievementGroupService<TAchievementGroupDto>
        where TAchievementGroupDto : AchievementGroupDto
    {
        private readonly ICrud<DAL.Entities.User, UserDto> _userRepository;
        private readonly ICrud<UserAchievementGroup, UserAchievementGroupDto> _userAchievementGroupRepository;

        
        public AchievementGroupService(IMapper mapper, ICrudServicesAsync service, 
            ICrud<DAL.Entities.User, UserDto> userRepository, AchievementDbContext context, 
            ICrud<UserAchievementGroup, UserAchievementGroupDto> userAchievementGroupRepository) 
            : base(mapper, service, context)
        {
            _userRepository = userRepository;
            _userAchievementGroupRepository = userAchievementGroupRepository;
        }

        public async Task<IEnumerable<TAchievementGroupDto>> GetAchievementsGroupsOfUserAsync(int userId)
        {
            var user  = await _userRepository.Get(userId);
            return user?
                .UserGroups?
                .Select(g => g.AchievementGroup)
                .Cast<TAchievementGroupDto>()
                .ToList();
        }
        
        public async Task<IEnumerable<TAchievementGroupDto>> GetGroupsWhereUserIsAdminAsync(int userId)
        {
            return await Context.AchievementGroups
                .Where(g => g.OwnerId == userId)
                .Cast<TAchievementGroupDto>()
                .ToListAsync();
        }

        public async Task<bool> InsertUserIntoAchievementGroup(int userId, int groupId)
        {
            var user = await _userRepository.Get(userId);
            if (user == null)
            {
                return false;
            }
            
            await _userAchievementGroupRepository.Create(new UserAchievementGroupDto
            {
                AchievementGroupId = groupId,
                UserId = userId
            });
            
            return true;
        }

        public async Task DeleteUserFromAchievementGroup(int userId, int groupId)
        {
            var group = await Context
                .UserGroups
                .FirstAsync(g => g.AchievementGroupId == groupId && g.UserId == userId);
            
            Context
                .UserGroups
                .Remove(group);

            await Context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfUserIsGroupAdmin(int groupId, int userId)
        {
            var group = await Context
                .AchievementGroups
                .FirstOrDefaultAsync(g => g.OwnerId == userId && g.Id == groupId);
            return @group != null;
        }
    }
}