using AutoMapper;
using BusinessLayer.DTOs.Common;

namespace BusinessLayer.Services.Common
{
    public abstract class ServiceBase<TDto>
    where TDto : DtoBase
    {
        protected readonly IMapper Mapper;

        protected ServiceBase(IMapper mapper)
        {
            Mapper = mapper;
        }
        
    }
}