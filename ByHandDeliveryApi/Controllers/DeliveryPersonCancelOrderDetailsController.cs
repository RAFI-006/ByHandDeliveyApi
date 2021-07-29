using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using AutoMapper;
using ByHandDeliveryApi.DTO;
using ByHandDeliveryApi.GenericResponses;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPersonCancelOrderDetailsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly string _successMsg = "Successfully Completed";

    

        public DeliveryPersonCancelOrderDetailsController(db_byhanddeliveryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DeliveryPersonCancelOrderDetails
        [HttpGet]
        public  IActionResult GetTblDeliveryPersonCancelOrderDetails()
        {
            var response = new GenericResponse<List<DeliveryPersonCancelOrderDetailsDTO>>();

            try
            {
                var data = _context.TblDeliveryPersonCancelOrderDetails.ToList();
                var list = new List<DeliveryPersonCancelOrderDetailsDTO>();
                foreach (var item in data)
                {
                    list.Add(_mapper.Map<DeliveryPersonCancelOrderDetailsDTO>(item));
                }

                response.HasError = false;
                response.Message = _successMsg;
                response.Result = list;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }


            return response.ToHttpResponse();
        }



        [HttpGet("DeliveryPersonCancelOrderDetailsById")]
        public IActionResult DeliveryPersonCancelOrderDetailsById(int id)
        {
            var response = new GenericResponse<List<DeliveryPersonCancelOrderDetailsDTO>>();

            try
            {
                var data = _context.TblDeliveryPersonCancelOrderDetails.Where(p=>p.DeliveryPersonID == id).ToList();
                var list = new List<DeliveryPersonCancelOrderDetailsDTO>();
                foreach (var item in data)
                {
                    list.Add(_mapper.Map<DeliveryPersonCancelOrderDetailsDTO>(item));
                }

                response.HasError = false;
                response.Message = _successMsg;
                response.Result = list;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }


            return response.ToHttpResponse();
        }


        // GET: api/DeliveryPersonCancelOrderDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblDeliveryPersonCancelOrderDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonCancelOrderDetails = await _context.TblDeliveryPersonCancelOrderDetails.FindAsync(id);

            if (tblDeliveryPersonCancelOrderDetails == null)
            {
                return NotFound();
            }

            return Ok(tblDeliveryPersonCancelOrderDetails);
        }

        // PUT: api/DeliveryPersonCancelOrderDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblDeliveryPersonCancelOrderDetails([FromRoute] int id, [FromBody] TblDeliveryPersonCancelOrderDetails tblDeliveryPersonCancelOrderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblDeliveryPersonCancelOrderDetails.DeliveryPersonCancelOrderDetailID)
            {
                return BadRequest();
            }

            _context.Entry(tblDeliveryPersonCancelOrderDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblDeliveryPersonCancelOrderDetailsExists(id))
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

        // POST: api/DeliveryPersonCancelOrderDetails
        [HttpPost]
        public async Task<IActionResult> PostTblDeliveryPersonCancelOrderDetails([FromBody] DeliveryPersonCancelOrderDetailsDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<string> responses = new GenericResponse<string>();
            try
            {

                var mapppedData = _mapper.Map<TblDeliveryPersonCancelOrderDetails>(data);
                _context.Add(mapppedData);
                _context.SaveChanges();
                var res = _context.TblDeliveryPersonCancelOrderDetails.Where(p => p.DeliveryPersonCancelOrderDetailID == data.DeliveryPersonCancelOrderDetailID).FirstOrDefault();


                responses.Result = "Created Successfully";
                responses.Message = "Successfull";
                responses.HasError = false;

            }
            catch (Exception e)
            {
                responses.Message = e.Message;
                responses.HasError = true;
            }

            return responses.ToHttpResponse();
        }

        // DELETE: api/DeliveryPersonCancelOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblDeliveryPersonCancelOrderDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonCancelOrderDetails = await _context.TblDeliveryPersonCancelOrderDetails.FindAsync(id);
            if (tblDeliveryPersonCancelOrderDetails == null)
            {
                return NotFound();
            }

            _context.TblDeliveryPersonCancelOrderDetails.Remove(tblDeliveryPersonCancelOrderDetails);
            await _context.SaveChangesAsync();

            return Ok(tblDeliveryPersonCancelOrderDetails);
        }

        private bool TblDeliveryPersonCancelOrderDetailsExists(int id)
        {
            return _context.TblDeliveryPersonCancelOrderDetails.Any(e => e.DeliveryPersonCancelOrderDetailID == id);
        }
    }
}