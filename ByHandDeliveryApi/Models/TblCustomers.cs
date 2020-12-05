using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblCustomers
    {
        public TblCustomers()
        {
            TblOrders = new HashSet<TblOrders>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public int? Wallet { get; set; }
        public string CreditCardNo { get; set; }
        public string CreditCardHolderName { get; set; }
        public DateTime? CreditCardExpiry { get; set; }
        public string CreditCardCvv { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FcmToken { get; set; }

        public ICollection<TblOrders> TblOrders { get; set; }
    }
}
