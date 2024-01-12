﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Setting : BaseEntities
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int DataUnit { get; set; }
    }
}
