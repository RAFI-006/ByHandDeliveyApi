using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Models
{
    public class TblDeliveryPersonCancelOrderDetails
    {
        public int DeliveryPersonCancelOrderDetailID { get; set; }
        public int? DeliveryPersonID { get; set; }
        public int OrderID { get; set; }
        public DateTime CancellationDate { get; set; }
        public int CancellationFee { get; set; }

        public TblDeliveryPerson DeliveryPerson { get; set; }
        public TblOrders Orders { get; set; }
    }
}
