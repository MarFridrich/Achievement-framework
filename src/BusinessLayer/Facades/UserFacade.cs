using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.User;

namespace BusinessLayer.Facades
{
    public class UserFacade<TUserDto, TAchievementGroupDto, TAchievementDto>
        where TUserDto : UserDto
        where TAchievementGroupDto : AchievementGroupDto
        where TAchievementDto : AchievementDto
    {
        protected readonly IUserService<TUserDto, TAchievementGroupDto, TAchievementDto> UserService;

        public UserFacade(IUserService<TUserDto, TAchievementGroupDto, TAchievementDto> userService)
        {
            UserService = userService;
        }

        public IEnumerable<TUserDto> ListAllAsync(UserFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<TUserDto> GetUserById(int id)
        {
            return await UserService.Get(id);
        }

        public async Task<TUserDto> GetUserByIdWithIncludes(int id, params string[] includes)
        {
            return await UserService.GetWithIncludes(id, includes);
        }

        public async Task<int> CreateUser(TUserDto entity)
        {
            return await UserService.Create(entity);
        }

        public async Task UpdateUser(TUserDto user)
        {
            await UserService.Update(user);
        }

        public async Task DeleteUser(int id)
        {
            await UserService.Delete(id);
        }

        public async Task<QueryResult<TUserDto>> ApplyFilter(UserFilterDto filter)
        {
            return await UserService.ApplyFilter(filter);
        }

    }
}