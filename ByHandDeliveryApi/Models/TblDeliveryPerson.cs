using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryPerson
    {
        public TblDeliveryPerson()
        {
            tblDeliveryPersonDetails = new HashSet<TblDeliveryPersonDetails>();
            TblDeliveryPersonPaymentTransactionDetails = new HashSet<TblDeliveryPersonPaymentTransactionDetails>();
            TblOrders = new HashSet<TblOrders>();
            TblDeliveryPersonCancelOrderDetails = new HashSet<TblDeliveryPersonCancelOrderDetails>();
        }

        public int DeliveryPersonId { get; set; }
        public string PersonName { get; set; }
        public string MobileNo { get; set; }
        public string AlternateNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsVerified { get; set; }
        public string Fcmtoken { get; set; }
        public string MyPromocode { get; set; }
        public string ReferPromocode { get; set; }
        public string ProfileImage { get; set; }


       
        
        public ICollection<TblDeliveryPersonDetails> tblDeliveryPersonDetails { get; set; }
        public ICollection<TblDeliveryPersonPaymentTransactionDetails> TblDeliveryPersonPaymentTransactionDetails { get; set; }
        public ICollection<TblDeliveryPersonCancelOrderDetails> TblDeliveryPersonCancelOrderDetails { get; set; }
        public ICollection<TblDeliveryPersonWallet> TblDeliveryPersonWallet { get; set; }
        public ICollection<TblOrders> TblOrders { get; set; }
    }
}
