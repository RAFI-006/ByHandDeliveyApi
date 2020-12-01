using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.DataModel.FireBase
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class FireBaseModelResponse
    {
        [JsonProperty("multicast_id")]
        public double MulticastId { get; set; }

        [JsonProperty("success")]
        public long Success { get; set; }

        [JsonProperty("failure")]
        public long Failure { get; set; }

        [JsonProperty("canonical_ids")]
        public long CanonicalIds { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
    }

}
