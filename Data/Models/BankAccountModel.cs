using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class BankAccountModel
    {
        
    }

    public class BankAccountViewModel : BaseModel
    {
        public string BankAccountNumber { get; set; } = null!;
        public string BankAccountName { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public int BankAccountType { get; set; }
        public Guid UserId { get; set; }
    }

    public class BankAccountCreateModel
    {
        public string BankAccountNumber { get; set; } = null!;
        public string BankAccountName { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public int BankAccountType { get; set; }
        public Guid UserId { get; set; }
    }

    public class BankAccountUpdateModel
    {
        public string BankAccountNumber { get; set; } = null!;
        public string BankAccountName { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public int BankAccountType { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }

    public class BankAccountQueryModel : QueryStringParameters
    {
        public BankAccountQueryModel()
        {
            OrderBy = "";
        }
        public string? Search { get; set; }
    }
}
