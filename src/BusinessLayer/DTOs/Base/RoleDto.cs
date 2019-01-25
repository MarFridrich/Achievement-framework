using System.ComponentModel;
using BusinessLayer.DTOs.Common;
using DAL.Entities;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class RoleDto : DtoBase, ILinkToEntity<Role>
    {
        public string Name { get; set; }
    }
}