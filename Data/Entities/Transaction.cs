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
        public int PaymentMethod { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }

        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; } = null!;
        public Guid? UserBidId { get; set; }
        [ForeignKey("UserBidId")]
        public UserBid? UserBid { get; set; }

    }
}
