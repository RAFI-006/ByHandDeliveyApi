using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using ByHandDeliveryApi.GenericResponses;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblOrderStatusController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;

        public TblOrderStatusController(db_byhanddeliveryContext context)
        {
            _context = context;
        }

        // GET: api/TblOrderStatus
        [HttpGet]
        public IEnumerable<TblOrderStatus> GetTblOrderStatus()
        {
            return _context.TblOrderStatus;
        }

        // GET: api/TblOrderStatus/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetTblOrderStatus([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblOrderStatus = await _context.TblOrderStatus.FindAsync(id);

        //    if (tblOrderStatus == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tblOrderStatus);
        //}

        // PUT: api/TblOrderStatus/5

        [HttpPut]
        public async Task<IActionResult> PutTblOrderStatus([FromBody] TblOrderStatus tblOrderStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<TblOrderStatus> response = new GenericResponse<TblOrderStatus>();
            try
            {
                if (TblOrderStatusExists(tblOrderStatus.OrderStatusId))
                {
                    _context.Update(tblOrderStatus);
                    _context.SaveChanges();
                    var res = _context.TblOrderStatus.Where(p => p.OrderStatusId == tblOrderStatus.OrderStatusId).FirstOrDefault();
                    response.Result = res;
                    response.Message = "Successfull";
                    response.HasError = false;



                }
                else
                {
                    response.Message = "User not found";
                    response.HasError = true;

                }

            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = true;

            }

            return response.ToHttpResponse();
        }

        // POST: api/TblOrderStatus
        [HttpPost]
        public  IActionResult PostTblOrderStatus([FromBody] TblOrderStatus tblOrderStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<TblOrderStatus> response = new GenericResponse<TblOrderStatus>();

            try
            {
                _context.TblOrderStatus.Add(tblOrderStatus);
                 _context.SaveChanges();

                response.Message = "Added Sucessfully";
                response.HasError = false;
                response.Result = _context.TblOrderStatus.Where(p=>p.OrderStatusId == tblOrderStatus.OrderStatusId).FirstOrDefault();
            }
            catch(Exception e)
            {
                response.Message = e.Data.ToString();
                response.HasError = true;
            }

            return response.ToHttpResponse();
        }

        // DELETE: api/TblOrderStatus/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblOrderStatus([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblOrderStatus = await _context.TblOrderStatus.FindAsync(id);
        //    if (tblOrderStatus == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblOrderStatus.Remove(tblOrderStatus);
        //    await _context.SaveChangesAsync();

        //    return Ok(tblOrderStatus);
        //}

        private bool TblOrderStatusExists(int id)
        {
            return _context.TblOrderStatus.Any(e => e.OrderStatusId == id);
        }
    }
}