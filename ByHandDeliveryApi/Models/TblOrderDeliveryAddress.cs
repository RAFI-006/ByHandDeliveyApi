using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblOrderDeliveryAddress
    {
        public int OrderDeliveryAddressId { get; set; }
        public int OrderId { get; set; }
        public string DropLocality { get; set; }
        public string MobileNo { get; set; }
        public DateTime? DeliveryFromTime { get; set; }
        public DateTime? DeliveryToTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPerson { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string ProductImage { get; set; }
        public DateTime? CreatedDate { get; set; }

        public TblOrders Order { get; set; }
    }
}
