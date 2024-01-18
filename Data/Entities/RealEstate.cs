using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class RealEstate : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? LinkAttachment { get; set; }
        public DateTime? ApproveTime { get; set; }
        public RealEstateStatus Status { get; set; } = RealEstateStatus.Pending;


        //relationship
        public IList<Auction> Auctions { get; set; } = null!;
        public IList<RealEstateImage> RealEstateImages { get; set; } = null!;
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Categories { get; set; } = null!;
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; } = null!;
    }
}
