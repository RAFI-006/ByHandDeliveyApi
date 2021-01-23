using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class DeliveryPersonAccountDetailsDto
    {

        public int DeliveryPersonAccountDetailId { get; set; }
        public int DeliveryPersonId { get; set; }
        public int Amount { get; set; }
        public string PaymentType { get; set; }
        public string CrDr { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
