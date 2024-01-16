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
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public string? Image { get; set; }
    }

    public class FeedbackCreateModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public string? Image { get; set; }
    }

    public class FeedbackUpdateModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public string? Image { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }

    public class FeedbackQueryModel : QueryStringParameters
    {
        public FeedbackQueryModel()
        {
            OrderBy = "";
        }
        public string? Search { get; set; }
    }
}
