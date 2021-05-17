using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DataModel
{
    public class TransactionDetailModel
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? TotalAmount {get;set;}
        public decimal? CommisionFee { get; set; }
        public decimal? DeliveyPersonCharge { get; set; }
        public string PaymentType { get; set; }
        public string PaymentRecievedTo { get; set; }


    }
}
