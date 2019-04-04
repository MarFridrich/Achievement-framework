using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BusinessLayer.DTOs.Common
{
    public abstract class DtoBase
    {
        [HiddenInput]
        [ReadOnly(true)]
        [JsonIgnore]
        public int Id { get; set; }
    }
}