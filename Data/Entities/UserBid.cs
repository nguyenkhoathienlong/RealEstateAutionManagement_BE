using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserBid : BaseEntities
    {
        public float BidAmount { get; set; }
        public float BidNumber { get; set; }

        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? Users { get; set; }
        public Guid AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public Auction? Auctions { get; set; }
        public IList<Transaction>? Transactions { get; set; }
    }
}
