using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblDeliveryCity
    {
        public int DeliveryCityId { get; set; }
        public string DeliveryCity { get; set; }
        public bool? IsActive { get; set; }
    }
}
