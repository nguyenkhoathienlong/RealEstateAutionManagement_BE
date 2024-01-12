using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CategoryModel : BaseModel
    {

    }

    public class CategoryViewModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryCreateModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryUpdateModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
