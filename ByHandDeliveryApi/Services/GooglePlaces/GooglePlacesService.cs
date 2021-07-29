
using ByHandDeliveryApi.DataModel.GooglePlaces;
using System;
using ByHandDeliveryApi.Services.RequestProvider;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByHandDeliveryApi.Services.GooglePlaces
{
  public  class GooglePlacesService 
     {
       
      //  private const string GMapsApiKey = "AIzaSyBABRX-a5a8HMXfZiW5NSJFhcEsARd88sM";
        private const string GMapsApiKey = "AIzaSyCSFVDxQd3XRlWZ0l4KUcS8BoJyPzzpg7w";
        private const string PLACE_DETAIL_END_POINT = "/maps/api/place/details/json";
        private const string AUTOCOMPLETE_END_POINT = "/maps/api/place/autocomplete/json";
        private const string DISTANCEMATRIX_END_POINT = "/maps/api/distancematrix/json";
        public const string GooglePlacesBaseUrl = "https://maps.googleapis.com";

        public GooglePlacesService()
        {
            
        }
        public async Task<PlacesAutoCompleteResponse> GetAutoCompleteGooglePlaces(string input)
        {
            
            var uri = new Uri(string.Format(CreateAutoCompleteApiRequest(input,"en"), string.Empty));

            var response = await RequestProviderService.GetAsync<PlacesAutoCompleteResponse>(uri);


            return response;
        }

        public async Task<PlacesDetailResponse> GetPlaceDetail(string place)
        {
            var uri = new Uri(string.Format(CreatePlaceDetailApiRequest(place), string.Empty));

            var response = await RequestProviderService.GetAsync<PlacesDetailResponse>(uri);



            return response;
        }

        public async Task<DistanceMatrixResponse> GetDistanceMatrix(string src, String dest)
        {
            var uri = new Uri(string.Format(CreateDistanceMatrixApiRequest(src,dest), string.Empty));

            var response = await RequestProviderService.GetAsync<DistanceMatrixResponse>(uri);



            return response;
        }


        public string CreateAutoCompleteApiRequest(string place,string lang)
        {
            var placesRequest = new AuthorizeRequest(GooglePlacesBaseUrl+ AUTOCOMPLETE_END_POINT);
            var dic = new Dictionary<string, string>();
            dic.Add("input", place);
            dic.Add("language", lang);
            dic.Add("key", GMapsApiKey);
            var placesUri = placesRequest.Create(dic);
            return placesUri;
        }

        public string CreatePlaceDetailApiRequest(string placeId)
        {
            var placesRequest = new AuthorizeRequest(GooglePlacesBaseUrl + PLACE_DETAIL_END_POINT);
            var dic = new Dictionary<string, string>();
            dic.Add("placeid", placeId);
            dic.Add("key", GMapsApiKey);
            var placesUri = placesRequest.Create(dic);
            return placesUri;
        }

        public string CreateDistanceMatrixApiRequest(string origin ,string des)
        {
            var placesRequest = new AuthorizeRequest(GooglePlacesBaseUrl + DISTANCEMATRIX_END_POINT);
            var dic = new Dictionary<string, string>();
            dic.Add("units", "metric");
            dic.Add("origins", origin);
            dic.Add("destinations", des);
            dic.Add("key", GMapsApiKey);
            var placesUri = placesRequest.Create(dic);
            return placesUri;
        }

    }
}
