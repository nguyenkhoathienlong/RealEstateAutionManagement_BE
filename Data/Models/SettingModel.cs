﻿using System;
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
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int DataUnit { get; set; }
    }

    public class SettingCreateModel : BaseModel
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int DataUnit { get; set; }
    }

    public class SettingUpdateModel : BaseModel
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public int DataUnit { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
