using ByHandDeliveryApi.Models;
using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryPerson
    {
        public TblDeliveryPerson()
        {
            TblOrders = new HashSet<TblOrders>();
        }

        public int DeliveryPersonId { get; set; }
        public string PersonName { get; set; }
        public string MobileNo { get; set; }
        public string AlternateNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string AadhaarNo { get; set; }
        public string AadhaarImage { get; set; }
        public string Pan { get; set; }
        public string Panimage { get; set; }
        public string DrivingLicenceNo { get; set; }
        public string DrivingLicenceImage { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string Ifsc { get; set; }
        public string CanceledChequeImage { get; set; }
        public bool? IsActive { get; set; }
        public string Password { get; set; }
        public string DocumentFrontImage { get; set; }
        public string DocumentBackImage { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedDate { get; set; }



        public ICollection<TblOrders> TblOrders { get; set; }
    }
}
