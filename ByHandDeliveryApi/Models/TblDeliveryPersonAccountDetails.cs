using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryPersonAccountDetails
    {
        public int DeliveryPersonAccountDetailId { get; set; }
        public int DeliveryPersonId { get; set; }
        public int Amount { get; set; }
        public string PaymentType { get; set; }
        public string CrDr { get; set; }
        public DateTime TransactionDate { get; set; }

        public TblDeliveryPerson DeliveryPerson { get; set; }
    }
}
