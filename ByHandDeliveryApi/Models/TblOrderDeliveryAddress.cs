using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models

{
    public partial class TblOrderDeliveryAddress
    {
        public int OrderDeliveryAddressId { get; set; }
        public int OrderId { get; set; }
        public string MobileNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryFromTime { get; set; }
        public string DeliveryToTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPerson { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }

        public TblOrders Order { get; set; }
    }
}
