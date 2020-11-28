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

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryCitiesController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly string _successMsg = "Successfully Completed";
        public DeliveryCitiesController(db_byhanddeliveryContext context)
        {
            _context = context;
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
        public async Task <IActionResult> PostImagetoBlobStorage(List<IFormFile> File)
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