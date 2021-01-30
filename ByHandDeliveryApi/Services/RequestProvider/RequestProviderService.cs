using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Services.RequestProvider
{
  public  class RequestProviderService 
    {
        private readonly JsonSerializerSettings _serializerSettings;
     
        public RequestProviderService()
        {
           
        }
        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
          
           // httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer "+ _settingService.AuthAccessToken);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            return httpClient;
        }


        

        public static async Task<TResult> GetAsync<TResult>(Uri uri)
        {
            var client = CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await client.SendAsync(request).ConfigureAwait(false);



            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


                var baseissuerResponseModel = await Task.Run(() =>
                   JsonConvert.DeserializeObject<TResult>(json)
                ).ConfigureAwait(false);
                return baseissuerResponseModel;



            }
            else
            {
                return default(TResult);
            }



        }
    

        public static  async Task<TResult> PostAsync<TResult,Tin>(Uri uri, Tin data)
        {
          
            HttpResponseMessage response = null;

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.Default, "application/json");
            var servertoken = "key=AAAAeHCrmj8:APA91bG8GQto_hAF7Gv9Ip7AMM7zfjhSjwGARfEjN3tXiU3dU0rOspCdnH8dzkDJnyO9aeaSBXF7WUskJYbIjrVPBR2X_Ixcm1PW9RTbjkj64uGdKyePo3Hk5Rq4AG0k5NIgc7ujpvNv";
            var deliveryServertoken = "key=AAAAYGu-2RY:APA91bGi3r0amTBleqJB8yk_WCoZEXE6dsnyWuT83G-8uXIZ931MHnjgoIPty3MOt6QuweSTftk4XfCboQ4on50xfojKTUH2ip8t1CmRPxLr47tSlWeDX8S7PMfkqBa1NmGMz5oNlRnS";
            var basicServerkey = "key=AAAAGE7fqxE:APA91bGwxWW4r71cfkFuFejrEnTCiAxA21cZQM3aZ_Tg-GWEpcAvMcZsPz9uuLi94f6IKX1uKSktaOEiA3sg7eL9H6CzfRLkUsiPxja0yEzIE8D7kXd-f3AgxUBO87-zj9AHvMg4sKVL";
            var basicHomeLoanKey = "key=AAAA3hLuklI:APA91bGmdL9cjdEdi2_iIDkRARCd5xqpU-FlFT6zfAx5iwxrldSCUYzV72Yh3Rxetep5Sysv7MS1ujJhwicSIdBlEoDAdMsWb8u0ZizFdGqzRXFqHPul53wg5p9Vs6XBEXjZ24HgQqjE";


            try
            {
                HttpClient httpClient = CreateHttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", basicServerkey);

                httpClient.DefaultRequestHeaders.Accept.Clear();


                response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);



               if (response.IsSuccessStatusCode)
                {
                    var jsondata = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = await Task.Run(() =>
                       JsonConvert.DeserializeObject<TResult>(jsondata)
                    ).ConfigureAwait(false);

                    return result;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

           return default(TResult);
        }


         }
}
