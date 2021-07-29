using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class OrderDeliveryAddDto
    {
        public int OrderDeliveryAddressID { get; set; }
        public int OrderID { get; set; }
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
        public decimal? ApproxDistance { get; set; }
        public TimeSpan? ApproxTime { get; set; }
        public string ProductImage { get; set; }
    }
}
