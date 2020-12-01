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

            try
            {
                HttpClient httpClient = CreateHttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", servertoken);

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
