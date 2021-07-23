using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DataModel.GooglePlaces
{
   
        public partial class DistanceMatrixResponse
        {
            public List<string> destination_addresses { get; set; }
            public List<string> origin_addresses { get; set; }
            public List<Row> Rows { get; set; }
            public string Status { get; set; }
        }

        public partial class Row
        {
            public List<Element> Elements { get; set; }
        }

        public partial class Element
        {
            public Distance Distance { get; set; }
            public Distance Duration { get; set; }
            public string Status { get; set; }
        }

        public partial class Distance
        {
            public string Text { get; set; }
            public long Value { get; set; }
        }
    

}
