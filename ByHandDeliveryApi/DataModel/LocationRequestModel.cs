using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DataModel
{
    public class LocationRequestModel
    {
        public double SourceLat { get; set; }
        public double SourceLong { get; set; }
        public double DestLat { get; set; }
        public double DestLong { get; set; }

    }
}
