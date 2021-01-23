using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryPerson
    {
        public TblDeliveryPerson()
        {
            TblDeliveryPersonAccountDetails = new HashSet<TblDeliveryPersonAccountDetails>();
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
        public string AadhaarFrontImage { get; set; }
        public string AadhaarBackImage { get; set; }
        public string Pan { get; set; }
        public string Panimage { get; set; }
        public string DrivingLicenceNo { get; set; }
        public string DrivingLicenceFrontImage { get; set; }
        public string DrivingLicenceBackImage { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleDocumemnt { get; set; }
        public string VehicleFrontPhoto { get; set; }
        public string VehicleBackPhoto { get; set; }
        public string VehicleInsuranceNo { get; set; }
        public string VehicleInsuranceDocumentPhoto { get; set; }
        public string DocumentFrontImage { get; set; }
        public string DocumentBackImage { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string Ifsc { get; set; }
        public string CanceledChequeImage { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Points { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsVerified { get; set; }

        public ICollection<TblDeliveryPersonAccountDetails> TblDeliveryPersonAccountDetails { get; set; }
        public ICollection<TblOrders> TblOrders { get; set; }
    }
}
