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

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryCitiesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly db_byhanddeliveryContext _context;
        private readonly string _successMsg = "Successfully Completed";
        private GooglePlacesService _googleService;
        public DeliveryCitiesController(db_byhanddeliveryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _googleService = new GooglePlacesService();
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

            if (id != tblDeliveryCity.DeliveryCityId)
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
        public async Task<IActionResult> PostImagetoBlobStorage(List<IFormFile> File)
        {
            AzureBlobService service = new AzureBlobService();
            GenericResponse<string> response = new GenericResponse<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string path = await service.UploadImageAsync(File[0]);

                if (!string.IsNullOrEmpty(path))
                {
                    response.HasError = false;
                    response.Message = "Sucesss";
                    response.Result = path;
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Failed";
                    response.Result = path;
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

        [HttpGet("TestFireBase")]
        public async Task<IActionResult> TestFireBaseNotification(string title,string body)
        {
            GenericResponse<string> response = new GenericResponse<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                      string Ids = "e7Oup0mPSkqWbYHMLsvsce:APA91bGMKy0MfoVVxZKrC7PgFVMcx2FQX35r351kpcodcdS50GJtaW4ZbLFKZP9r9PIdtq7jMexGeLuNL-cQ1DnCOTV89M-ApV_MjivI4pXthdELl2NNVF4DecSqDgvHcvGsQATWMYLO";
             
               

                var result = await FireBaseService.PostNotifications(Ids,title,body);

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

            return CreatedAtAction("GetTblDeliveryCity", new { id = tblDeliveryCity.DeliveryCityId }, tblDeliveryCity);
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
            return _context.TblDeliveryCity.Any(e => e.DeliveryCityId == id);
        }
    }
}