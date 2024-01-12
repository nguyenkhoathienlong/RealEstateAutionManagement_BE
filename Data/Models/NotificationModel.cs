using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class NotificationModel
    {

    }

    public class NotificationViewModel : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid UserId { get; set; }
    }

    public class NotificationCreateModel : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid UserId { get; set; }
    }

    public class NotificationUpdateModel : BaseModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid UserId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
