using AutoMapper;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.Services.Common
{
    public abstract class ServiceBase
    {
        protected readonly IMapper Mapper;

        protected ServiceBase(IMapper mapper)
        {
            Mapper = mapper;
        }
        
    }
}