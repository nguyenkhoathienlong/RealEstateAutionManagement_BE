using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TransactionModel
    {

    }

    public class TransactionViewModel : BaseModel
    {
        public float Amount { get; set; }
        public int PaymentMethod { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public Guid UserId { get; set; }
        public Guid? UserBidId { get; set; }
    }

    public class TransactionCreateModel : BaseModel
    {
        public float Amount { get; set; }
        public int PaymentMethod { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public Guid UserId { get; set; }
        public Guid? UserBidId { get; set; }
    }

    public class TransactionUpdateModel : BaseModel
    {
        public float Amount { get; set; }
        public int PaymentMethod { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public Guid UserId { get; set; }
        public Guid? UserBidId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
