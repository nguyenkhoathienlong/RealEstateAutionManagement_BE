using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string PaymentId { get; set; } = null!;
        public bool Success { get; set; }
        public string Token { get; set; } = null!;
        public string VnPayResponseCode { get; set; } = null!;
    }
}
