using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayer.DTOs.Common
{
    public abstract class DtoBase
    {
        [HiddenInput]
        [ReadOnly(true)]
        public int Id { get; set; }
    }
}