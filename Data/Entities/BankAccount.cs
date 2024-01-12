using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class BankAccount : BaseEntities
    {
        [Required]
        public string BankAccountNumber { get; set; } = null!;
        [Required]
        public string BankAccountName { get; set; } = null!;
        [Required]
        public string BankName { get; set; } = null!;
        [Required]
        public int BankAccountType { get; set; }

        //relationship
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; } = null!;
    }
}
