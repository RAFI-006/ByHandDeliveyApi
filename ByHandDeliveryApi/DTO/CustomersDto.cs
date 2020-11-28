using ByHandDeliveryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class CustomersDto
    {
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

     

    }
}
