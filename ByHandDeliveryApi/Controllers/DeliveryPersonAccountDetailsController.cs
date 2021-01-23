using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using ByHandDeliveryApi.DTO;
using ByHandDeliveryApi.GenericResponses;
using AutoMapper;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPersonAccountDetailsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mapper;

        public DeliveryPersonAccountDetailsController(db_byhanddeliveryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/DeliveryPersonAccountDetails
        [HttpGet]
        public IEnumerable<TblDeliveryPersonAccountDetails> GetTblDeliveryPersonAccountDetails()
        {
            return _context.TblDeliveryPersonAccountDetails;
        }

        // GET: api/DeliveryPersonAccountDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblDeliveryPersonAccountDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonAccountDetails = await _context.TblDeliveryPersonAccountDetails.FindAsync(id);

            if (tblDeliveryPersonAccountDetails == null)
            {
                return NotFound();
            }

            return Ok(tblDeliveryPersonAccountDetails);
        }

        // PUT: api/DeliveryPersonAccountDetails/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTblDeliveryPersonAccountDetails([FromRoute] int id, [FromBody] TblDeliveryPersonAccountDetails tblDeliveryPersonAccountDetails)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tblDeliveryPersonAccountDetails.DeliveryPersonAccountDetailId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tblDeliveryPersonAccountDetails).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TblDeliveryPersonAccountDetailsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/DeliveryPersonAccountDetails
        [HttpPost]
        public async Task<IActionResult> PostTblDeliveryPersonAccountDetails([FromBody] DeliveryPersonAccountDetailsDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<DeliveryPersonAccountDetailsDto> responses = new GenericResponse<DeliveryPersonAccountDetailsDto>();
            try
            {
                
                    _context.Add(_mapper.Map<TblDeliveryPersonAccountDetails>(data));
                    _context.SaveChanges();
                  
                    responses.Result = data;
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

        // DELETE: api/DeliveryPersonAccountDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblDeliveryPersonAccountDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonAccountDetails = await _context.TblDeliveryPersonAccountDetails.FindAsync(id);
            if (tblDeliveryPersonAccountDetails == null)
            {
                return NotFound();
            }

            _context.TblDeliveryPersonAccountDetails.Remove(tblDeliveryPersonAccountDetails);
            await _context.SaveChangesAsync();

            return Ok(tblDeliveryPersonAccountDetails);
        }

        private bool TblDeliveryPersonAccountDetailsExists(int id)
        {
            return _context.TblDeliveryPersonAccountDetails.Any(e => e.DeliveryPersonAccountDetailId == id);
        }
    }
}