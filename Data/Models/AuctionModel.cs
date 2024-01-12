using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class AuctionModel
    {
        
    }

    public class AuctionViewModel : BaseModel
    {
        public string? Description { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public float StartingPrice { get; set; }
        public float BidIncrement { get; set; }
        public float MaxBidIncrement { get; set; }
        public float RegistrationFee { get; set; }
        public float Deposit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApproveTime { get; set; }
        public int Status { get; set; }
        public Guid CreateByUserId { get; set; }
        public Guid ApproveByUserId { get; set; }
        public Guid RealEstateId { get; set; }
    }

    public class AuctionCreateModel : BaseModel
    {
        public string? Description { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public float StartingPrice { get; set; }
        public float BidIncrement { get; set; }
        public float MaxBidIncrement { get; set; }
        public float RegistrationFee { get; set; }
        public float Deposit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApproveTime { get; set; }
        public int Status { get; set; }
        public Guid CreateByUserId { get; set; }
        public Guid ApproveByUserId { get; set; }
        public Guid RealEstateId { get; set; }
    }

    public class AuctionUpdateModel : BaseModel
    {
        public string? Description { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public float StartingPrice { get; set; }
        public float BidIncrement { get; set; }
        public float MaxBidIncrement { get; set; }
        public float RegistrationFee { get; set; }
        public float Deposit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApproveTime { get; set; }
        public int Status { get; set; }
        public Guid CreateByUserId { get; set; }
        public Guid ApproveByUserId { get; set; }
        public Guid RealEstateId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
