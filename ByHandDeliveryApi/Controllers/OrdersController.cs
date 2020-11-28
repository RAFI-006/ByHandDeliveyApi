using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using AutoMapper;
using ByHandDeliveryApi.GenericResponses;
using ByHandDeliveryApi.DTO;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mappper;
        private readonly string SucessMessege = "Successfull";

        public OrdersController(db_byhanddeliveryContext context,IMapper mapper)
        {
            _context = context;
            _mappper = mapper;
           
        }

        // GET: api/Orders
        [HttpGet]
        public IActionResult GetTblOrders()
        {
            var response = new GenericResponse<List<OrderDto>>();
            try
            {
                var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Include(p=>p.TblOrderDeliveryAddress).ToList();
                var responseList = new List<OrderDto>();
                foreach(var item in data)
                {

                }
                response.Message = SucessMessege;
                response.HasError = false;
                response.Result = _mappper.Map<List<OrderDto>>(data);

            }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.HasError = true;
            }

            return response.ToHttpResponse();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public  IActionResult GetTblOrders([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<TblOrders>();
            try
            {
                var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).ToList();

                var tblOrders =  data.Where(p=>p.OrderId==id).FirstOrDefault();

                if(tblOrders ==null)
                {
                    response.Message = "Orders not found";
                    response.HasError = true;
                }
                else
                {
                    response.Result = tblOrders;
                    response.Message = SucessMessege;
                    response.HasError = false;
                }
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }




            return response.ToHttpResponse();
        }

        // PUT: api/Orders/5
        [HttpPut]
        public async Task<IActionResult> PutTblOrders(TblOrders tblOrders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = new GenericResponse<TblOrders>();
            try
            {

                if (TblOrdersExists(tblOrders.OrderId))
                {
                    _context.Entry(tblOrders).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    response.Result = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Where(p => p.OrderId == tblOrders.OrderId).FirstOrDefault();
                    response.Message = SucessMessege;
                    response.HasError = false;
                }
                else
                {
                    response.Message = "Primary key not present";
                    response.HasError = false;

                }

            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;

            }

            return response.ToHttpResponse();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> PostTblOrders([FromBody] TblOrders tblOrders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = new GenericResponse<int>();
            try
            {
                _context.TblOrders.Add(tblOrders);
                 _context.SaveChanges();

                response.Message = SucessMessege;
                response.HasError = false;
                var data= CreatedAtAction("GetTblOrders", new { id = tblOrders.OrderId }, tblOrders);
                response.Result = tblOrders.OrderId; 
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.HasError = true;
            }

            return response.ToHttpResponse();
        }

        [HttpGet("customerOrder")]
        public IActionResult GetCustomerOrder(int customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<List<OrderDto>>();
            try
            {
                var data = _context.TblOrders.Include(p => p.DeliveryPerson).Include(p => p.TblOrderDeliveryAddress).ToList();

                var tblOrders = data.Where(p => p.CustomerId == customerId).ToList();

                if (tblOrders == null)
                {
                    response.Message = "No Orders Found";
                    response.HasError = true;
                }
                else
                {
                    response.Result = _mappper.Map<List<OrderDto>>(tblOrders);
                    response.Message = SucessMessege;
                    response.HasError = false;
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }




            return response.ToHttpResponse();
        }


        [HttpGet("deliveryBoyOrders")]
        public IActionResult GetDeliveryBoyOrder(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<List<OrderDto>>();
            try
            {
                var data = _context.TblOrders.Include(p => p.Customer).ToList();

                var tblOrders = data.Where(p => p.DeliveryPersonId == id).ToList();

                if (tblOrders == null)
                {
                    response.Message = "No Orders Found";
                    response.HasError = true;
                }
                else
                {
                    response.Result = _mappper.Map<List<OrderDto>>(data);
                    response.Message = SucessMessege;
                    response.HasError = false;
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }




            return response.ToHttpResponse();
        }


        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblOrders([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblOrders = await _context.TblOrders.FindAsync(id);
        //    if (tblOrders == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblOrders.Remove(tblOrders);
        //    await _context.SaveChangesAsync();

        //    return Ok(tblOrders);
        //}

        private bool TblOrdersExists(int id)
        {
            return _context.TblOrders.Any(e => e.OrderId == id);
        }
    }
}