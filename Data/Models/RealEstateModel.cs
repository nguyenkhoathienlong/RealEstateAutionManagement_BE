using System;
using System.Collections.Generic;
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
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? LinkAttachment { get; set; }
        public DateTime ApproveTime { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
    }

    public class RealEstateCreateModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? LinkAttachment { get; set; }
        public DateTime ApproveTime { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
    }

    public class RealEstateUpdateModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? LinkAttachment { get; set; }
        public DateTime ApproveTime { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
