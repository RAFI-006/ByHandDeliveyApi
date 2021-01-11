using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblOrderStatus
    {
        //public TblOrderStatus()
        //{
        //    TblOrders = new HashSet<TblOrders>();
        //}

        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int? OrderStatusCode { get; set; }

      //  public ICollection<TblOrders> TblOrders { get; set; }
    }
}
