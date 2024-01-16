using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class SettingModel
    {
        
    }

    public class SettingViewModel : BaseModel
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Description { get; set; } = null!;
        public SettingDataUnit DataUnit { get; set; }
    }

    public class SettingCreateModel
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Description { get; set; } = null!;
        public SettingDataUnit DataUnit { get; set; }
    }

    public class SettingUpdateModel
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Description { get; set; } = null!;
        public SettingDataUnit DataUnit { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
