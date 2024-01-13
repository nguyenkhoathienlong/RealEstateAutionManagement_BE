using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class UserBidModel
    {

    }

    public class UserBidViewModel : BaseModel
    {
        public float Amount { get; set; }
        public bool IsDeposit { get; set; }
        public Guid UserId { get; set; }
        public Guid AuctionId { get; set; }
    }

    public class UserBidCreateModel
    {
        public float Amount { get; set; }
        public bool IsDeposit { get; set; }
        public Guid UserId { get; set; }
        public Guid AuctionId { get; set; }
    }

    public class UserBidUpdateModel
    {
        public float Amount { get; set; }
        public bool IsDeposit { get; set; }
        public Guid UserId { get; set; }
        public Guid AuctionId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
