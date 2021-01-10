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
using ByHandDeliveryApi.DataModel;
using ByHandDeliveryApi.Services;

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


        [HttpPost("FilterOrders")]
        public IActionResult FilterOrders([FromBody]FilterOrderModel model)
        {
            var response = new GenericResponse<List<OrderDto>>();
            try
            {
                var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Include(p => p.TblOrderDeliveryAddress).ToList().Where(p => p.Status == 0).ToList();

                if (model.City == null && model.Distance == null)
                {
                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(data);
                }
                else if (model.City == null) {

                    var result = data.Where(p => Convert.ToInt16(p.Distance.Split('.')[0]) < Convert.ToInt16(model.Distance.Split('.')[0]) && p.Status == 0).ToList();


                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(result);
                }
                else if (model.Distance == null)
                {

                    var result = data.Where(p => p.City == model.City && p.Status==0).ToList();

                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(result);
                }
                else
                {

                    var result = data.Where(p => p.City == model.City && Convert.ToInt16(p.Distance.Split('.')[0]) < Convert.ToInt16(model.Distance.Split('.')[0]) && p.Status == 0).ToList();

                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(result);
                }

            }
            catch (Exception e)
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
            string _notificationMsg = "";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (tblOrders.Status == 1)
                _notificationMsg = "Your order has been accepted";
            else if (tblOrders.Status == 2)
            {
                _notificationMsg = "Your order has been picked up from the pickup point " +
                    tblOrders.PickupAddress;
            }
            else

                _notificationMsg = "Your order is sucessfully delivered to the delivered adsress";
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

              
                    var result = await FireBaseService.PostNotifications(response.Result.Customer.FcmToken, "OrderId" + tblOrders.OrderId, _notificationMsg);

                    if (result.Success == 1)
                    {

                    }
                    else
                    {
                
                    }
                }
                else
                {
                    response.Message = "Primary key not present";
                    response.HasError = false;

                }

            }
            catch (Exception e)
            {
                response.Message = e.InnerException.Message;
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
                var data = _context.TblOrders.Include(p => p.Customer).Include(p=>p.TblOrderDeliveryAddress).ToList();

                var tblOrders = data.Where(p => p.DeliveryPersonId == id && p.DeliveryPersonId!=null).ToList();

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


        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblOrders([FromRoute] int id)
        {
            var response = new GenericResponse<string>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var tblOrders = await _context.TblOrders.FindAsync(id);
                if (tblOrders == null)
                {
                    response.HasError = true;
                    response.Message = "Orders not found";

                }
                else
                {

                    var deliveryData = _context.TblOrderDeliveryAddress.Where(p => p.OrderId == id).FirstOrDefault();

                    if (deliveryData != null)
                    {
                        _context.TblOrderDeliveryAddress.Remove(deliveryData);
                        _context.TblOrders.Remove(tblOrders);
                        await _context.SaveChangesAsync();

                        response.HasError = false;
                        response.Message = "Successfully deleted orderId" + " " + id;

                    }
                }
         
            }
             catch(Exception e)
            {

                response.HasError = true;
                response.Message = e.InnerException.ToString();
            }




            return response.ToHttpResponse();
        }

        private bool TblOrdersExists(int id)
        {
            return _context.TblOrders.Any(e => e.OrderId == id);
        }
    }
}