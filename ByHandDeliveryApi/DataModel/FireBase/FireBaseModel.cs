using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ByHandDeliveryApi.DataModel.FireBase
{


  

    public partial class FireBaseModel
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("mutable_content")]
        public bool MutableContent { get; set; }

        [JsonProperty("notification")]
        public Notification Notification { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("channelKey")]
        public string ChannelKey { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("notificationLayout")]
        public string NotificationLayout { get; set; }

        [JsonProperty("largeIcon")]
        public string LargeIcon { get; set; }

        [JsonProperty("showWhen")]
        public bool ShowWhen { get; set; }

        [JsonProperty("autoCancel")]
        public bool AutoCancel { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }
    }

    public partial class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}


