using System;
using System.Collections.Generic;

namespace ByHandDeliveryApi.Models
{
    public partial class TblBaseConfigOld
    {
        public int Id { get; set; }
        public int? BaseRate { get; set; }
        public int? BaseRatePerKmAboveKg { get; set; }
        public int? AppVersion { get; set; }
        public int? BaseRatePerKmBelowKg { get; set; }
        public int? BaseKiloGram { get; set; }
    }
}
