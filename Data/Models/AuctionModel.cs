using Data.Entities;
using Data.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Description { get; set; } = null!;
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public float StartingPrice { get; set; }
        public float BidIncrement { get; set; }
        public float? MaxBidIncrement { get; set; }
        public float RegistrationFee { get; set; }
        public float Deposit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ApproveTime { get; set; }
        public AuctionStatus Status { get; set; }
        public Guid CreateByUserId { get; set; }
        public Guid? ApproveByUserId { get; set; }
        public Guid RealEstateId { get; set; }
        //public float? HighestPrice { get; set; }
        //public Guid? WinnerId { get; set; }
        //public List<UserBidViewModel>? UserBids { get; set; }
    }

    public class AuctionCreateModel
    {
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Starting price must be a positive number.")]
        public float StartingPrice { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Bid increment must be a positive number.")]
        public float BidIncrement { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Max bid increment must be a positive number.")]
        public float? MaxBidIncrement { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Guid RealEstateId { get; set; }
    }

    public class AuctionUpdateModel
    {
        public string Description { get; set; } = null!;
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public float StartingPrice { get; set; }
        public float BidIncrement { get; set; }
        public float? MaxBidIncrement { get; set; }
        public float RegistrationFee { get; set; }
        public float Deposit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ApproveTime { get; set; }
        public AuctionStatus Status { get; set; }
        public Guid CreateByUserId { get; set; }
        public Guid? ApproveByUserId { get; set; }
        public Guid RealEstateId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }

    public class AuctionQueryModel : QueryStringParameters
    {
        public AuctionQueryModel()
        {
            OrderBy = "";
        }
        public string? Search { get; set; }
    }
}
