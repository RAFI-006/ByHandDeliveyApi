using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblOrders
    {
        public TblOrders()
        {
            TblOrderDeliveryAddress = new HashSet<TblOrderDeliveryAddress>();
            TblDeliveryPersonCancelOrderDetails = new HashSet<TblDeliveryPersonCancelOrderDetails>();
        }

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int? DeliveryPersonID { get; set; }
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
        public int? PaymentTypeID { get; set; }
        public int? OrderStatusID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? FromLat { get; set; }
        public decimal? FromLong { get; set; }
        
        public string PaymentFrom { get; set; }
        public string ProductImage { get; set; }
        public string PromoCode { get; set; }
        public int? Discount { get; set; }
        public int? PointRedemption { get; set; }
        public int? PaymentStatusID { get; set; }
        public decimal? CommissionFee { get; set; }



        public TblCustomers Customer { get; set; }
        public TblDeliveryPerson DeliveryPerson { get; set; }
        public ICollection<TblOrderDeliveryAddress> TblOrderDeliveryAddress { get; set; }
        public ICollection<TblDeliveryPersonCancelOrderDetails> TblDeliveryPersonCancelOrderDetails { get; set; }
    }
}
