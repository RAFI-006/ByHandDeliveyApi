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
        public string MobileNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryFromTime { get; set; }
        public string DeliveryToTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactPerson { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PinCode { get; set; }
        public string Time { get; set; }

    }
}
