using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DTO
{
    public class OrderDeliveryPersonDTO
    {
        public int? DeliveryPersonID { get; set; }
        public string DeliveryPersonName { get; set; }
        public string DeliveryPersonMobileNo { get; set; }
        public string DeliveryPersonProfileImage { get; set; }
        public string DeliveryPersonFCMToken { get; set; }
    }
}
