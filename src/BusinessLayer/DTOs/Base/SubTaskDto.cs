using BusinessLayer.DTOs.Common;
using DAL.Entities;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class SubTaskDto : DtoBase, ILinkToEntity<SubTask>
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}