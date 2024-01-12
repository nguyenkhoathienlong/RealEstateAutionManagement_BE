using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class FeedbackModel
    {

    }

    public class FeedbackViewModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }

    public class FeedbackCreateModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }

    public class FeedbackUpdateModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
