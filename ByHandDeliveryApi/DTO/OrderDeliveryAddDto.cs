using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class OrderDeliveryAddDto
    {
        public int OrderDeliveryAddressId { get; set; }
        public int OrderId { get; set; }
        public string DropLocality { get; set; }
        public string MobileNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public TimeSpan? DeliveryFromTime { get; set; }
        public TimeSpan? DeliveryToTime { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }

        public string Time { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPerson { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string ProductImage { get; set; }
        public DateTime? CreatedDate { get; set; }



    }
}
