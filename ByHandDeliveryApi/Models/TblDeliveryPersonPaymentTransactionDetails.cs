using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryPersonPaymentTransactionDetails
    {
        public int DeliveryPersonAccountDetailID { get; set; }
        public int DeliveryPersonID { get; set; }

        public int Amount { get; set; }
        public string PaymentType { get; set; }
        public string CrDr { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comment { get; set; }

        public TblDeliveryPerson DeliveryPerson { get; set; }
    }
}
