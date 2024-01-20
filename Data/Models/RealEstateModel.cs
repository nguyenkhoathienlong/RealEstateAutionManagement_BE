using Data.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class RealEstateModel
    {

    }

    public class RealEstateViewModel : BaseModel
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? LinkAttachment { get; set; }
        public DateTime? ApproveTime { get; set; }
        public RealEstateStatus Status { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ApproveByUserId { get; set; }
    }

    public class RealEstateCreateModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;

        public string? LinkAttachment { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public Guid? CategoryId { get; set; }

        [Required(ErrorMessage = "Images are required.")]
        public List<IFormFile> Images { get; set; } = null!;
    }

    public class RealEstateUpdateModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;

        public string? LinkAttachment { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public Guid? CategoryId { get; set; }

        //[Required(ErrorMessage = "Images are required.")]
        //public List<IFormFile> Images { get; set; } = null!;
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }

    public class RealEstateQueryModel : QueryStringParameters
    {
        public RealEstateQueryModel()
        {
            OrderBy = "";
        }
        public string? Search { get; set; }
    }

    public class ApproveRealEstateModel
    {
        public bool IsApproved { get; set; }
    }
}
