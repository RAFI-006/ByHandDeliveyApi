﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class DeliveryPersonDto
    {

        public int DeliveryPersonID { get; set; }
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
        public string ReferPromoCode { get; set; }
        public string ProfileImage { get; set; }

        public string AadhaarNo { get; set; }
        public string AadhaarFrontImage { get; set; }
        public string AadhaarBackImage { get; set; }
        public string Pan { get; set; }
        public string Panimage { get; set; }
        public string DrivingLicenceNo { get; set; }
        public string DrivingLicenceFrontImage { get; set; }
        public string DrivingLicenceBackImage { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleDocumentImage { get; set; }
        public string VehicleFrontPhoto { get; set; }
        public string VehicleBackPhoto { get; set; }
        public string VehicleInsuranceNo { get; set; }
        public string VehicleInsuranceDocumentImage { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string Ifsc { get; set; }
        public string CanceledChequeImage { get; set; }
       




    }
}
