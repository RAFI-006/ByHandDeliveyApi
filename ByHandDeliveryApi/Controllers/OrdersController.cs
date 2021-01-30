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
                var c = new OrderDto();
                
               List <TblOrders> data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Include(p=>p.TblOrderDeliveryAddress).ToList();
                 
               
                
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
                var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Include(p => p.TblOrderDeliveryAddress).ToList().Where(p=>p.OrderStatusId==9).ToList();

                if (model.City == null && model.Distance == null)
                {
                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(data);
                }
                else if (model.City == null) {

                    var result = data.Where(p => Convert.ToInt16(p.Distance) < Convert.ToInt16(model.Distance) && p.OrderStatusId== 9).ToList();


                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(result);
                }
                else if (model.Distance == null)
                {

                    var result = data.Where(p => p.City == model.City && p.OrderStatusId== 9).ToList();

                    response.Message = SucessMessege;
                    response.HasError = false;
                    response.Result = _mappper.Map<List<OrderDto>>(result);
                }
                else
                {

                    var result = data.Where(p => p.City == model.City && Convert.ToInt16(p.Distance) < Convert.ToInt16(model.Distance) && p.OrderStatusId == 9).ToList();

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

            if (tblOrders.OrderStatusId == 10)
                _notificationMsg = "Your order has been accepted";
            else if (tblOrders.OrderStatusId == 11)
            {
                _notificationMsg = "Your order has been picked up from the pickup point " +
                    tblOrders.PickupAddress;
            }
            else

                _notificationMsg = "Your order is sucessfully delivered to the delivered address";
            var response = new GenericResponse<TblOrders>();
            try
            {

                if (TblOrdersExists(tblOrders.OrderId))
                {
                    _context.Entry(tblOrders).State = EntityState.Modified;
                  //  _context.TblOrderStatus.Update(tblOrders.OrderStatusId);
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
        public async Task<IActionResult> PostTblOrders([FromBody] OrderRequest tblOrders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TblOrders order = null;
            var response = new GenericResponse<int>();
         
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                         order = new TblOrders
                        {
                            OrderId = tblOrders.OrderId,
                            CustomerId = tblOrders.CustomerId,
                            DeliveryPersonId = tblOrders.DeliveryPersonId,
                            PickupLocality = tblOrders.PickupLocality,
                            MobileNo = tblOrders.MobileNo,
                            PickupDate = DateTime.Now,
                            PickupToTime =new  TimeSpan(Convert.ToInt16(tblOrders.ToTime.Split(':')[0]), Convert.ToInt16(tblOrders.ToTime.Split(':')[1]),58),
                            PickupFromTime =  new TimeSpan(Convert.ToInt16(tblOrders.FromTime.Split(':')[0]), Convert.ToInt16(tblOrders.FromTime.Split(':')[1]), 58),
                            PickupAddress = tblOrders.PickupAddress,
                            ContactPersonMobile = tblOrders.ContactPersonMobile,
                            ContactPerson = tblOrders.ContactPerson,
                            InternalOrderNo = tblOrders.InternalOrderNo,
                            Action = tblOrders.Action,
                            Weight = tblOrders.Weight,
                            GoodsType = tblOrders.GoodsType,
                            ParcelValue = tblOrders.ParcelValue,
                            OrderAmount = tblOrders.OrderAmount,
                            PaymentTypeId = tblOrders.PaymentTypeId,
                            OrderStatusId = tblOrders.OrderStatusId,
                            CreatedDate = tblOrders.CreatedDate,
                            FromLat = tblOrders.FromLat,
                            FromLong = tblOrders.FromLong,
                            Distance = tblOrders.Distance,
                            City = tblOrders.City,
                            PaymentFrom = tblOrders.PaymentFrom,
                            ProductImage = tblOrders.ProductImage




                        };

                        _context.Add(order);

                        _context.SaveChanges();

                        OrderDeliveryAddDto orderData = tblOrders.OrderDeliveryAdd;
                        orderData.OrderId = order.OrderId;
                        orderData.DeliveryFromTime = new TimeSpan(Convert.ToInt16(orderData.FromTime.Split(':')[0]), Convert.ToInt16(tblOrders.FromTime.Split(':')[1]), 58);
                        orderData.DeliveryToTime = new TimeSpan(Convert.ToInt16(orderData.ToTime.Split(':')[0]), Convert.ToInt16(orderData.ToTime.Split(':')[1]), 58);
                    orderData.DeliveryDate = DateTime.Now;
                _context.Add(_mappper.Map<TblOrderDeliveryAddress>(orderData));

                        _context.SaveChanges();
                    
                    response.Result = order.OrderId;
                    response.Message = "Order Succesfull";
                    response.HasError = false;

                    transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Message =ex.Message;
                        response.HasError = true;
                    }
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
                        response.Message = "Successfully deleted orderId";
                        response.Result = id.ToString();

                    }
                    else
                    {
                        _context.TblOrders.Remove(tblOrders);
                        await _context.SaveChangesAsync();

                        response.HasError = false;
                        response.Message = "Successfully deleted orderId";
                        response.Result = id.ToString();

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