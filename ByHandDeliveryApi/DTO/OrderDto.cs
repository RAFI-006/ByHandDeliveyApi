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
        public string City { get; set; }
        public string MobileNo { get; set; }
        public DateTime? PickupFromTime { get; set; }
        public DateTime? PickupToTime { get; set; }
        public string PickupAddress { get; set; }
        public string ContactPersonMobile { get; set; }
        public string ContactPerson { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }
        public string Weight { get; set; }
        public string GoodsType { get; set; }
        public int? ParcelValue { get; set; }
        public int? OrderAmount { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? PaymentStatusID { get; set; }
        public int? OrderStatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? FromLat { get; set; }
        public decimal? FromLong { get; set; }
        public decimal? Distance { get; set; }
        public string PaymentFrom { get; set; }
        public string ProductImage { get; set; }
        public string PromoCode { get; set; }
        public int? Discount { get; set; }
        public int? PointRedemption { get; set; }
        public CustomersDto Customer { get; set; }
        public DeliveryPersonDto DeliveryPerson { get; set; }
        public ICollection<OrderDeliveryAddDto> TblOrderDeliveryAddress { get; set; }


    }
}
