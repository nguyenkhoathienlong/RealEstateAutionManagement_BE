using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Transaction : BaseEntities
    {
        public float Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }

        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; } = null!;
        public Guid? AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public Auction? Auctions { get; set; }
    }
}
