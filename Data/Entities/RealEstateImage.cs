using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class RealEstateImage : BaseEntities
    {
        public string Image { get; set; } = null!;


        //relationship
        public Guid RealEstateId { get; set; }
        [ForeignKey("RealEstateId")]
        public RealEstate RealEstates { get; set; } = null!;
    }
}
