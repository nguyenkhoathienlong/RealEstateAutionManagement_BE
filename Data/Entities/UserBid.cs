using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserBid: BaseEntities
    {
        public float Amount { get; set; }
        public bool IsDeposit { get; set; }

        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; } = null!;
        public Guid AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public Auction Auctions { get; set; } = null!;
        public IList<Transaction>? Transactions { get; set; }
    }
}
