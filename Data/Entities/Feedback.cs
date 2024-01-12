using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Feedback : BaseEntities
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public string? Image { get; set; }


        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; } = null!;
    }
}
