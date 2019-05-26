using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Services.Generic.User;

namespace BusinessLayer.Facades
{
    public class UserFacade<TUserDto, TFilterDto>
        where TUserDto : UserDto
        where TFilterDto : UserFilterDto, new() 
    {
        protected readonly IUserService<TUserDto, TFilterDto> UserService;

        public UserFacade(IUserService<TUserDto, TFilterDto> userService)
        {
            UserService = userService;
        }

        public IQueryable<TUserDto> ListAll()
        {
            return UserService.ListAll();
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

        public async Task<QueryResult<TUserDto>> ApplyFilter(TFilterDto filter)
        {
            return await UserService.ApplyFilter(filter);
        }

    }
}