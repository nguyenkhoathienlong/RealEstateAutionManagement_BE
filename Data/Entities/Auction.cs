using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Auction : BaseEntities
    {
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
        public AuctionStatus Status { get; set; } = AuctionStatus.Pending;


        //relationship
        public Guid CreateByUserId { get; set; }
        [ForeignKey("CreateByUserId")]
        public User CreatedByUser { get; set; } = null!;
        public Guid? ApproveByUserId { get; set; }
        [ForeignKey("ApproveByUserId")]
        public User? ApprovedByUser { get; set; }
        public Guid RealEstateId { get; set; }
        [ForeignKey("RealEstateId")]
        public RealEstate RealEstates { get; set; } = null!;
    }
}
