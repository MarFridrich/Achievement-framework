using BusinessLayer.DTOs.Common;
using DAL.Entities;
using GenericServices;

namespace BusinessLayer.DTOs.Base
{
    public class RewardDto : DtoBase, ILinkToEntity<Reward>
    {
        public string Name { get; set; }
    }
}