using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Notification : BaseEntities
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? Users { get; set; }
    }
}
