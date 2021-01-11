using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblOrders
    {
        public TblOrders()
        {
            TblOrderDeliveryAddress = new HashSet<TblOrderDeliveryAddress>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int? DeliveryPersonId { get; set; }
        public string PickupLocality { get; set; }
        public string MobileNo { get; set; }
        public DateTime? PickupDate { get; set; }
        public string PickupFromTime { get; set; }
        public string PickupToTime { get; set; }
        public string PickupAddress { get; set; }
        public string ContactPersonMobile { get; set; }
        public string ContactPerson { get; set; }
        public string InternalOrderNo { get; set; }
        public string Action { get; set; }
        public string Weight { get; set; }
        public string GoodsType { get; set; }
        public int? ParcelValue { get; set; }
        public int? OrderAmount { get; set; }
        public bool? Cod { get; set; }
        public bool? FromTheBalance { get; set; }
        public bool? ByCreditCard { get; set; }
        public int? OrderStatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Time { get; set; }
        public string FromLat { get; set; }
        public string FromLong { get; set; }
        public string Distance { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string PaymentFrom { get; set; }
        public string ProductImage { get; set; }

        public TblCustomers Customer { get; set; }
        public TblDeliveryPerson DeliveryPerson { get; set; }
        public TblOrderStatus OrderStatus { get; set; }
        public ICollection<TblOrderDeliveryAddress> TblOrderDeliveryAddress { get; set; }
    }
}
