using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using ByHandDeliveryApi.GenericResponses;
using ByHandDeliveryApi.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Configuration;
using ByHandDeliveryApi.DataModel.FireBase;
using ByHandDeliveryApi.Services.GooglePlaces;
using ByHandDeliveryApi.DataModel.GooglePlaces;
using ByHandDeliveryApi.DataModel.GooglePlaces.ByHandDeliveryApi.DataModel.GooglePlaces;
using ByHandDeliveryApi.DataModel;
using ByHandDeliveryApi.DTO;
using AutoMapper;
using System.IO;
using Microsoft.Extensions.Options;
using System.Web.Http.Cors;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyAllowSpecificOrigins", headers: "*", methods: "*")]
    public class DeliveryCitiesController : ControllerBase
    {
     
        private readonly DocumentImagePathSetting DocumentImagePath;
        private readonly ProfileImagePathSetting ProfileImagePath;
        private readonly ProductImagePathSetting ProductImagePath;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _configuration;
        private readonly db_byhanddeliveryContext _context;
        private readonly string _successMsg = "Successfully Completed";
        private GooglePlacesService _googleService;
        private IMapper _mapper;
        public DeliveryCitiesController(db_byhanddeliveryContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOptions<ProfileImagePathSetting> OptionProfileSetting, IOptions<DocumentImagePathSetting> OptionDocumentSetting, IOptions<ProductImagePathSetting> OptionProductSetting)
        {
            _context = context;
            _configuration = configuration;
            _googleService = new GooglePlacesService();
            _mapper = mapper;

            _httpContextAccessor = httpContextAccessor;
            ProfileImagePath = OptionProfileSetting.Value;
            DocumentImagePath = OptionDocumentSetting.Value;
            ProductImagePath = OptionProductSetting.Value;

        }

        // GET: api/DeliveryCities
        [HttpGet]
        public IActionResult GetTblDeliveryCity()
        {
            var response = new GenericResponse<List<TblDeliveryCity>>();

            try
            {
                var data = _context.TblDeliveryCity.ToList();



                response.HasError = false;
                response.Message = _successMsg;
                response.Result = data;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }


            return response.ToHttpResponse();
        }

        // GET: api/DeliveryCities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblDeliveryCity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryCity = await _context.TblDeliveryCity.FindAsync(id);

            if (tblDeliveryCity == null)
            {
                return NotFound();
            }

            return Ok(tblDeliveryCity);
        }

        // PUT: api/DeliveryCities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblDeliveryCity([FromRoute] int id, [FromBody] TblDeliveryCity tblDeliveryCity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblDeliveryCity.DeliveryCityID)
            {
                return BadRequest();
            }

            _context.Entry(tblDeliveryCity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblDeliveryCityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("PostImagetoBlobStorage")]
        public async Task<IActionResult> PostImagetoBlobStorage(List<IFormFile> File, string ImageType)
        {
            string ImagePath = "";
            GenericResponse<string> response = new GenericResponse<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            switch (ImageType.ToLower().Trim())
            {
                case "profile":
                    ImagePath = ProfileImagePath.ProfilesImagePath;
                    break;
                case "product":
                    ImagePath = ProductImagePath.ProductsImagePath;
                    break;
                case "document":
                    ImagePath = DocumentImagePath.DocumentsImagePath;
                    break;
                default:
                    response.Message = "Invalid Image Type Name";
                    break;
            }

            try
            {
                string imagefileName = Guid.NewGuid().ToString() + Path.GetExtension(File[0].FileName);

                string FilePath = string.Concat(ImagePath, "/", imagefileName);

                using (var stream = System.IO.File.Create(FilePath))
                {
                    await File[0].CopyToAsync(stream);
                    stream.Close();
                    response.HasError = false;
                    response.Message = "Sucesss";
                    string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                    response.Result = string.Concat("http://", host, "/", FilePath);
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;
            }
            return response.ToHttpResponse();

        }

        [HttpGet("GetConfigKeys")]
        public async Task<IActionResult> GetConfigKeys()
        {

            var response = new GenericResponse<List<string>>();

            var result = new List<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var data = _context.TblDropDown.ToList();

                foreach (var item in data)
                    result.Add(item.Ddname);

                if (data != null)
                {
                    response.HasError = false;
                    response.Message = "Sucesss";
                    response.Result = result;
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Failed";
                    response.Result = null;
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;


            }

            return response.ToHttpResponse();

        }

        [HttpGet("GetConfig")]
        public async Task<IActionResult> GetConfig(string ConfigKey)
        {
          
            var response = new GenericResponse<List<DDValueDTO>>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var data = _context.TblDropDown.Include(p => p.TblDdvalues).Where(t=>t.DropDownKey == ConfigKey).FirstOrDefault();
            
                if (data!=null)
                {
                    response.HasError = false;
                    response.Message = "Sucesss";
                    response.Result = _mapper.Map<List<DDValueDTO>>(data.TblDdvalues);
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Failed";
                    response.Result = null;
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;


            }

            return response.ToHttpResponse();

        }



        [HttpPost("PostOtp")]
        public async Task<IActionResult> PostOtp(string number)
        {
            AzureBlobService service = new AzureBlobService();
            GenericResponse<string> response = new GenericResponse<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string accountSid = "AC07f7179f1b520b541532521565d1965c";
                string authToken = "675f83ea588c2a055fb30114656e982a";

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "1234",
                    from: new Twilio.Types.PhoneNumber("+12052360258"),
                    to: new Twilio.Types.PhoneNumber(number)
                );

                if (!string.IsNullOrEmpty(message.Sid))
                {
                    response.HasError = false;
                    response.Message = "Sucesss";
                    response.Result = message.Sid;
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Failed";


                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();


            }

            return response.ToHttpResponse();

        }

        [HttpGet("GoogleAutoCompleteSearch")]
        public async Task<IActionResult> GoogleAutoCompleteSearch(string place)
        {
            GenericResponse<List<Prediction>> response = new GenericResponse<List<Prediction>>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               

                PlacesAutoCompleteResponse result = await _googleService.GetAutoCompleteGooglePlaces(place);


                if (result.status == "OK")
                {

                    response.HasError = false;
                    response.Message = _successMsg;
                    response.Result = result.predictions;

                }
                else
                {
                    response.HasError = true;
                    response.Message = "Place Not Found";
                }

            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();

            }

            return response.ToHttpResponse();
        }

        [HttpGet("GooglePlaceDetails")]
        public async Task<IActionResult> GooglePlaceDetails(string placeId)
        {
            GenericResponse<PlaceDetailData> response = new GenericResponse<PlaceDetailData>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PlacesDetailResponse result = await _googleService.GetPlaceDetail(placeId);

                if (result.status == "OK")
                {
                    response.HasError = false;
                    response.Message = _successMsg;
                    response.Result = new PlaceDetailData {
                        FormattedAddress = result.result.formatted_address,
                        Geometry =  result.result.geometry


                    };

                }
                else
                {
                    response.HasError = true;
                    response.Message = "Place Not Found";
                }

            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();

            }

            return response.ToHttpResponse();
        }


        [HttpPost("GetDistanceBetweenTwoPoints")]
        public async Task<IActionResult> GetDistanceBetweenTwoPoints([FromBody] LocationRequestModel model)
        {
            GenericResponse<DistanceMatrixResponse> response = new GenericResponse<DistanceMatrixResponse>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string source = model.SourceLat.ToString() + "," + model.SourceLong.ToString();
                string des = model.DestLat.ToString() + "," + model.DestLong.ToString();

                DistanceMatrixResponse result = await _googleService.GetDistanceMatrix(source, des);

                if (result != null)
                {
                    response.HasError = false;
                    response.Message = _successMsg;
                    response.Result = result;

                }
                else
                {
                    response.HasError = true;
                    response.Message = "Place Not Found";
                }

            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();

            }

            return response.ToHttpResponse();
        }



        [HttpGet("TestFireBase")]
        public async Task<IActionResult> TestFireBaseNotification(string title,string body,string fcmToken)
        {
            GenericResponse<string> response = new GenericResponse<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                      string Ids = "e7Oup0mPSkqWbYHMLsvsce:APA91bGMKy0MfoVVxZKrC7PgFVMcx2FQX35r351kpcodcdS50GJtaW4ZbLFKZP9r9PIdtq7jMexGeLuNL-cQ1DnCOTV89M-ApV_MjivI4pXthdELl2NNVF4DecSqDgvHcvGsQATWMYLO";
             
               

                var result = await FireBaseService.PostNotifications(fcmToken,title,body);

                if (result.Success == 1)
                {

                    response.Message = "Succusfully send the Notification";
                    response.HasError = false;
                    response.Result = result.MulticastId.ToString();
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Something went wrong";

                }


            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();

            }

            return response.ToHttpResponse();
        }

        // POST: api/DeliveryCities
        [HttpPost]
        public async Task<IActionResult> PostTblDeliveryCity([FromBody] TblDeliveryCity tblDeliveryCity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TblDeliveryCity.Add(tblDeliveryCity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblDeliveryCity", new { id = tblDeliveryCity.DeliveryCityID }, tblDeliveryCity);
        }

        // DELETE: api/DeliveryCities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblDeliveryCity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryCity = await _context.TblDeliveryCity.FindAsync(id);
            if (tblDeliveryCity == null)
            {
                return NotFound();
            }

            _context.TblDeliveryCity.Remove(tblDeliveryCity);
            await _context.SaveChangesAsync();

            return Ok(tblDeliveryCity);
        }

        private bool TblDeliveryCityExists(int id)
        {
            return _context.TblDeliveryCity.Any(e => e.DeliveryCityID == id);
        }


        double GetDistanceFromLatLonInKm( double lat1, double lon1,  double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
           if (d > 1 & d <= 3)
            {
                d = d + 1;
            }
           else if (d > 3 & d<=6)
            {
                d = d + 2;
            }
            else if(d > 6 & d <= 10)
            {
                d = d + 3;
            }

            else if (d > 10 & d <= 15)
            {
                d = d + 4;
            }
            else if (d > 15 & d <= 25)
            {
                d = d + 5;
            }
            else if (d>25 & d<=35)
            {
                d = d + 6;
            }
            else if (d > 35 & d <= 45)
            {
                d = d + 7;
            }
            else if (d > 45 & d <= 60)
            {
                d = d + 8;
            }
            else if (d > 60 & d <= 70)
            {
                d = d + 9;
            }
            else if (d > 70 & d <= 80)
            {
                d = d + 10;
            }
           
            return d;
        }

        double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}