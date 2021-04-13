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
    public class DeliveryPersonPaymentTransactionDetailsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;

        public DeliveryPersonPaymentTransactionDetailsController(db_byhanddeliveryContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryPersonPaymentTransactionDetails
        [HttpGet]
        public IEnumerable<TblDeliveryPersonPaymentTransactionDetails> GetTblDeliveryPersonPaymentTransactionDetails()
        {
            return _context.TblDeliveryPersonPaymentTransactionDetails;
        }

        // GET: api/DeliveryPersonPaymentTransactionDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblDeliveryPersonPaymentTransactionDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonPaymentTransactionDetails = await _context.TblDeliveryPersonPaymentTransactionDetails.FindAsync(id);

            if (tblDeliveryPersonPaymentTransactionDetails == null)
            {
                return NotFound();
            }

            return Ok(tblDeliveryPersonPaymentTransactionDetails);
        }

        // PUT: api/DeliveryPersonPaymentTransactionDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblDeliveryPersonPaymentTransactionDetails([FromRoute] int id, [FromBody] TblDeliveryPersonPaymentTransactionDetails tblDeliveryPersonPaymentTransactionDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblDeliveryPersonPaymentTransactionDetails.DeliveryPersonAccountDetailID)
            {
                return BadRequest();
            }

            _context.Entry(tblDeliveryPersonPaymentTransactionDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblDeliveryPersonPaymentTransactionDetailsExists(id))
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

        // POST: api/DeliveryPersonPaymentTransactionDetails
        [HttpPost]
        public async Task<IActionResult> PostTblDeliveryPersonPaymentTransactionDetails([FromBody] TblDeliveryPersonPaymentTransactionDetails tblDeliveryPersonPaymentTransactionDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TblDeliveryPersonPaymentTransactionDetails.Add(tblDeliveryPersonPaymentTransactionDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblDeliveryPersonPaymentTransactionDetails", new { id = tblDeliveryPersonPaymentTransactionDetails.DeliveryPersonAccountDetailID }, tblDeliveryPersonPaymentTransactionDetails);
        }

        // DELETE: api/DeliveryPersonPaymentTransactionDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblDeliveryPersonPaymentTransactionDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPersonPaymentTransactionDetails = await _context.TblDeliveryPersonPaymentTransactionDetails.FindAsync(id);
            if (tblDeliveryPersonPaymentTransactionDetails == null)
            {
                return NotFound();
            }

            _context.TblDeliveryPersonPaymentTransactionDetails.Remove(tblDeliveryPersonPaymentTransactionDetails);
            await _context.SaveChangesAsync();

            return Ok(tblDeliveryPersonPaymentTransactionDetails);
        }

        private bool TblDeliveryPersonPaymentTransactionDetailsExists(int id)
        {
            return _context.TblDeliveryPersonPaymentTransactionDetails.Any(e => e.DeliveryPersonAccountDetailID == id);
        }
    }
}