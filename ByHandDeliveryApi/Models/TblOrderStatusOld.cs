using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblOrderStatusOld
    {
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int? OrderStatusCode { get; set; }
    }
}
