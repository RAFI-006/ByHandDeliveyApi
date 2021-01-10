using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Models
{
    public partial class TblBaseConfig
    {
        public int Id { get; set; }
        public int BaseRate { get; set; }
        public int BaseRatePerKmAboveKg { get; set; }
        public int BaseRatePerKmBelowKg { get; set; }
        public int BaseKilogram { get; set; }
        public int AppVersion { get; set; }


    }
}
