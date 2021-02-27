using ByHandDeliveryApi.DataModel.FireBase;
using ByHandDeliveryApi.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Services
{
    public static class FireBaseService
    {

        public static async Task<FireBaseModelResponse> PostNotifications(string fcmId,string title,string body)
        {
            var model = new FireBaseModel
            {
                To = fcmId,
                MutableContent = true,
                Notification = new Notification
                {
                    Title = "ByHandDelivery"
                },
                Data = new Data
                {
                    Content = new Content
                    {
                        Id = 100,
                        ChannelKey = "basic_channel",
                        Title = title,
                        Body = body,
                        NotificationLayout = "BigPicture",
                        LargeIcon = "https://www.dw.com/image/49519617_303.jpg",
                        ShowWhen = true,
                        AutoCancel =true,
                        Privacy = "Private"


                    }

                },



            };

            var uri = new Uri("https://fcm.googleapis.com/fcm/send");

            var response = await RequestProviderService.PostAsync<FireBaseModelResponse, FireBaseModel>(uri, model);

            return response;


        }




        public static async Task<FireBaseModelResponse> PostDeliveryBoyNotifications(string fcmId, string title, string body)
        {
            var model = new FireBaseModel
            {
                To = fcmId,
                MutableContent = true,
                Notification = new Notification
                {
                    Title = "ByHandDeliveryBoy"
                },
                Data = new Data
                {
                    Content = new Content
                    {
                        Id = 100,
                        ChannelKey = "basic_channel",
                        Title = title,
                        Body = body,
                        NotificationLayout = "BigPicture",
                        LargeIcon = "https://www.dw.com/image/49519617_303.jpg",
                        ShowWhen = true,
                        AutoCancel = true,
                        Privacy = "Private"


                    }

                },



            };

            var uri = new Uri("https://fcm.googleapis.com/fcm/send");

            var response = await RequestProviderService.PostDeliveryBoyAsync<FireBaseModelResponse, FireBaseModel>(uri, model);

            return response;


        }




    }
}
