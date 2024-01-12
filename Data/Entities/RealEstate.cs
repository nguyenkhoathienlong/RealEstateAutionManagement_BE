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
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? LinkAttachment { get; set; }
        public DateTime ApproveTime { get; set; }


        //relationship
        public IList<Auction>? Auctions { get; set; }
        public IList<RealEstateImage>? RealEstateImages { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Categories { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? Users { get; set; }
    }
}
