using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Category : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;


        //relationship
        public IList<RealEstate>? RealEstates { get; set; }

    }
}
