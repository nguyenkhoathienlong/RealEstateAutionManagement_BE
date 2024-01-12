using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class RealEstateImageModel
    {

    }

    public class RealEstateImageViewModel : BaseModel
    {
        public string Image { get; set; } = null!;
        public Guid RealEstateId { get; set; }
    }

    public class RealEstateImageCreateModel : BaseModel
    {
        public string Image { get; set; } = null!;
        public Guid RealEstateId { get; set; }
    }

    public class RealEstateImageUpdateModel : BaseModel
    {
        public string Image { get; set; } = null!;
        public Guid RealEstateId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }

}
