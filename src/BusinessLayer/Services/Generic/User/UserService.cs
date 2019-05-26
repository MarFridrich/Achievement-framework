using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessLayer.DTOs.Base;
using BusinessLayer.DTOs.Filter.Base;
using BusinessLayer.Helpers;
using BusinessLayer.QueryObjects;
using BusinessLayer.QueryObjects.Base.Results;
using BusinessLayer.Repository;
using BusinessLayer.Services.Common;
using DAL.BaHuEntities;
using DAL.BaHuEntities.JoinTables;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Generic.User
{
    public class UserService<TUserDto, TFilterDto> :
        RepositoryServiceBase<DAL.BaHuEntities.User, TUserDto, TFilterDto>,
        IUserService<TUserDto, TFilterDto>
        where TUserDto : UserDto
        where TFilterDto : UserFilterDto, new()

    {
        private readonly UserQuery<TUserDto, TFilterDto> _userQuery;


        public UserService(IMapper mapper, IRepository<DAL.BaHuEntities.User> repository, DbContext context, Types actualModels,
            UserQuery<TUserDto, TFilterDto> userQuery) : base(mapper, repository, context,
            actualModels)
        {
            _userQuery = userQuery;
        }

        public async Task<QueryResult<TUserDto>> ApplyFilter(TFilterDto filter)
        {
            return await _userQuery.ExecuteAsync(filter);
        }

    }
}