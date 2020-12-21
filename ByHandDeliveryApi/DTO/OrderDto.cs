using ByHandDeliveryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? DeliveryPersonId { get; set; }
        public string PickupLocality { get; set; }
        public string MobileNo { get; set; }
        public DateTime? PickupDate { get; set; }
        public string PickupFromTime { get; set; }
        public string PickupToTime { get; set; }
        public string PickupAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonMobile { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }
        public string Weight { get; set; }
        public string GoodsType { get; set; }
        public int? ParcelValue { get; set; }
        public int? OrderAmount { get; set; }
        public bool? Cod { get; set; }
        public bool? FromTheBalance { get; set; }
        public bool? ByCreditCard { get; set; }
        public int? Status { get; set; }
        public string Time { get; set; }
        public string FromLat { get; set; }
        public string FromLong { get; set; }
        public string Distance { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string CreatedDate { get; set; }
        public string PaymentFrom { get; set; }


        public CustomersDto Customer { get; set; }
        public DeliveryPersonDto DeliveryPerson { get; set; }
        public ICollection<OrderDeliveryAddDto> TblOrderDeliveryAddress { get; set; }


    }
}
