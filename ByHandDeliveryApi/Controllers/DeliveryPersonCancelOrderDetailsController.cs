using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPersonCancelOrderDetailsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;

        public DeliveryPersonCancelOrderDetailsController(db_byhanddeliveryContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryPersonCancelOrderDetails
        [HttpGet]
        public IEnumerable<TblDeliveryPersonCancelOrderDetails> GetTblDeliveryPersonAccountDetails()
        {
            return _context.TblDeliveryPersonAccountDetails;
        }

        // GET: api/DeliveryPersonCancelOrderDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblDeliveryPersonCancelOrderDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonCancelOrderDetails = await _context.TblDeliveryPersonAccountDetails.FindAsync(id);

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
        public async Task<IActionResult> PostTblDeliveryPersonCancelOrderDetails([FromBody] TblDeliveryPersonCancelOrderDetails tblDeliveryPersonCancelOrderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TblDeliveryPersonAccountDetails.Add(tblDeliveryPersonCancelOrderDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblDeliveryPersonCancelOrderDetails", new { id = tblDeliveryPersonCancelOrderDetails.DeliveryPersonCancelOrderDetailID }, tblDeliveryPersonCancelOrderDetails);
        }

        // DELETE: api/DeliveryPersonCancelOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblDeliveryPersonCancelOrderDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonCancelOrderDetails = await _context.TblDeliveryPersonAccountDetails.FindAsync(id);
            if (tblDeliveryPersonCancelOrderDetails == null)
            {
                return NotFound();
            }

            _context.TblDeliveryPersonAccountDetails.Remove(tblDeliveryPersonCancelOrderDetails);
            await _context.SaveChangesAsync();

            return Ok(tblDeliveryPersonCancelOrderDetails);
        }

        private bool TblDeliveryPersonCancelOrderDetailsExists(int id)
        {
            return _context.TblDeliveryPersonAccountDetails.Any(e => e.DeliveryPersonCancelOrderDetailID == id);
        }
    }
}