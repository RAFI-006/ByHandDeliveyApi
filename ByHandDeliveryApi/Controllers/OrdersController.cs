using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using AutoMapper;
using System.Data.SqlClient;
using ByHandDeliveryApi.GenericResponses;
using ByHandDeliveryApi.DTO;
using ByHandDeliveryApi.DataModel;
using ByHandDeliveryApi.Services;
using System.Web.Http.Cors;
using Microsoft.Extensions.Options;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyAllowSpecificOrigins", headers: "*", methods: "*")]
    public class OrdersController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mappper;
        private readonly string SucessMessege = "Successfull";
        private readonly string ConnectionString = Startup.ConnectionString;
        private readonly ProductImagePathSetting ProductImagePath;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrdersController(db_byhanddeliveryContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOptions<ProductImagePathSetting> OptionProductSetting)
        {
            _context = context;
            _mappper = mapper;
            _httpContextAccessor = httpContextAccessor;
            ProductImagePath = OptionProductSetting.Value;

        }

        // GET: api/Orders
        [HttpGet]
        public IActionResult GetTblOrders()
        {
            var response = new GenericResponse<List<OrderDto>>();
            try
            {
                var c = new OrderDto();

                List<TblOrders> data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Include(p => p.TblOrderDeliveryAddress).ToList();



                var responseList = new List<OrderDto>();
                foreach (var item in data)
                {

                }
                response.Message = SucessMessege;
                response.HasError = false;
                response.Result = _mappper.Map<List<OrderDto>>(data);

            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = true;
            }

            return response.ToHttpResponse();
        }


        //[HttpPost("FilterOrders")]
        //public IActionResult FilterOrders([FromBody]FilterOrderModel model)
        //{
        //    var response = new GenericResponse<List<OrderDto>>();
        //    try
        //    {
        //        var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Include(p => p.TblOrderDeliveryAddress).ToList().Where(p => p.OrderStatusID == 9).ToList();

        //        if (model.City == null && model.Distance == 0)
        //        {
        //            response.Message = SucessMessege;
        //            response.HasError = false;
        //            response.Result = _mappper.Map<List<OrderDto>>(data);
        //        }
        //        else if (model.City == null)
        //        {

        //            var result = data.Where(p => Convert.ToInt16(p.Distance) < Convert.ToInt16(model.Distance) && p.OrderStatusID == 9).ToList();


        //            response.Message = SucessMessege;
        //            response.HasError = false;
        //            response.Result = _mappper.Map<List<OrderDto>>(result);
        //        }
        //        else if (model.Distance == 0)
        //        {

        //            var result = data.Where(p => p.City == model.City && p.OrderStatusID == 9).ToList();

        //            response.Message = SucessMessege;
        //            response.HasError = false;
        //            response.Result = _mappper.Map<List<OrderDto>>(result);
        //        }
        //        else
        //        {

        //            var result = data.Where(p => p.City == model.City && Convert.ToInt16(p.Distance) < Convert.ToInt16(model.Distance) && p.OrderStatusID == 9).ToList();

        //            response.Message = SucessMessege;
        //            response.HasError = false;
        //            response.Result = _mappper.Map<List<OrderDto>>(result);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        response.Message = e.Message;
        //        response.HasError = true;
        //    }

        //    return response.ToHttpResponse();
        //}


        // GET: api/Orders/5
        [HttpGet("{id}")]
        public IActionResult GetTblOrders([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<TblOrders>();
            try
            {
                var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).ToList();

                var tblOrders = data.Where(p => p.OrderID == id).FirstOrDefault();

                if (tblOrders == null)
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
            catch (Exception e)
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
            if (tblOrders.OrderStatusID == 10)
                _notificationMsg = "Your order has been accepted";
            else if (tblOrders.OrderStatusID == 11)
            {
                _notificationMsg = "Your order has been picked up from the pickup point " +
                    tblOrders.PickupAddress;
            }
            else if (tblOrders.OrderStatusID == 12)
                _notificationMsg = "Your order is sucessfully delivered to the delivered address";
            else if (tblOrders.OrderStatusID == 18)
                _notificationMsg = "Order has been cancelled by the user";
            var response = new GenericResponse<TblOrders>();
            try
            {
                if (TblOrdersExists(tblOrders.OrderID))
                {
                    _context.Entry(tblOrders).State = EntityState.Modified;
                    //  _context.TblOrderStatus.Update(tblOrders.OrderStatusId);
                    await _context.SaveChangesAsync();
                    response.Result = _context.TblOrders.Include(p => p.Customer).Include(p => p.DeliveryPerson).Where(p => p.OrderID == tblOrders.OrderID).FirstOrDefault();
                    response.Message = SucessMessege;
                    response.HasError = false;

                    if (tblOrders.OrderStatusID != 18)
                        await FireBaseService.PostNotifications(response.Result.Customer.FcmToken, "OrderID: " + tblOrders.OrderID, _notificationMsg);

                    else
                        await FireBaseService.PostDeliveryBoyNotifications(response.Result.DeliveryPerson.Fcmtoken, "OrderID: " + tblOrders.OrderID, _notificationMsg);
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
            string strCustomMessage = "                                                                                                                                                      ";
            int OrderID = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<int>();
            try
            {
                string strApproxTime =  tblOrders.OrderDeliveryAdd.ApproxTime.ToString();
               // string xmlApproxTime = "< ApproxTime > 00:45:00 </ ApproxTime >";
                   tblOrders.OrderDeliveryAdd.ApproxTime = TimeSpan.Parse(strApproxTime);
                string strOrderXML = ToXML(tblOrders);
                string strDeliveryAddressXML = ToXML(tblOrders.OrderDeliveryAdd);
                // ApproxTime is not properly generate in xml format, so I have done it manually.
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(strDeliveryAddressXML); 
                xDoc.DocumentElement.SelectSingleNode("ApproxTime").InnerText  = strApproxTime;
                strDeliveryAddressXML = xDoc.InnerXml;

                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prInsertOrderDetails", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter prmXMLOrderDetails = new SqlParameter();
                        prmXMLOrderDetails.ParameterName = "@xmlOrderDetails";
                        prmXMLOrderDetails.SqlDbType = System.Data.SqlDbType.Xml;
                        prmXMLOrderDetails.Direction = ParameterDirection.Input;
                        prmXMLOrderDetails.Value = strOrderXML;
                        cmd.Parameters.Add(prmXMLOrderDetails);

                        SqlParameter prmXMLDeliveryAddress = new SqlParameter();
                        prmXMLDeliveryAddress.ParameterName = "@xmlDeliveryAddressDetails";
                        prmXMLDeliveryAddress.SqlDbType = System.Data.SqlDbType.Xml;
                        prmXMLDeliveryAddress.Direction = ParameterDirection.Input;
                        prmXMLDeliveryAddress.Value = strDeliveryAddressXML;
                        cmd.Parameters.Add(prmXMLDeliveryAddress);

                        SqlParameter prmOrderID = new SqlParameter();
                        prmOrderID.SqlDbType = System.Data.SqlDbType.Int;
                        prmOrderID.ParameterName = "@OrderID";
                        prmOrderID.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(prmOrderID);

                        SqlParameter prmCustomMessage = new SqlParameter();
                        prmCustomMessage.ParameterName = "@strCustomMessage";
                        prmCustomMessage.Size = 500;
                        prmCustomMessage.SqlDbType = System.Data.SqlDbType.VarChar;
                        prmCustomMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(prmCustomMessage);
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        strCustomMessage = cmd.Parameters["@strCustomMessage"].Value.ToString();
                        OrderID = Convert.ToInt32(cmd.Parameters["@OrderID"].Value.ToString());

                        response.Result = OrderID;
                        response.Message = "Order Successfully Inserted";
                        response.HasError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.HasError = true;
            }

            return response.ToHttpResponse();
        }



        [HttpPut("editOrders")]
        public async Task<IActionResult> EditTblOrders([FromBody] OrderRequest tblOrders)
        {
            string DeliveryPersonFcmToken = "                                                                                                                                                      ";
            string strApproxTime = tblOrders.OrderDeliveryAdd.ApproxTime.ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<int>();

            try
            {
                string strOrderXML = ToXML(tblOrders);
                string strDeliveryAddressXML = ToXML(tblOrders.OrderDeliveryAdd);

                // ApproxTime is not properly generate in xml format, so I have done it manually.
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(strDeliveryAddressXML);
                xDoc.DocumentElement.SelectSingleNode("ApproxTime").InnerText = strApproxTime;
                strDeliveryAddressXML = xDoc.InnerXml;

                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prUpdateOrderDetails", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter prmXMLOrderDetails = new SqlParameter();
                        prmXMLOrderDetails.ParameterName = "@xmlOrderDetails";
                        prmXMLOrderDetails.SqlDbType = System.Data.SqlDbType.Xml;
                        prmXMLOrderDetails.Direction = ParameterDirection.Input;
                        prmXMLOrderDetails.Value = strOrderXML;
                        cmd.Parameters.Add(prmXMLOrderDetails);

                        SqlParameter prmXMLDeliveryAddress = new SqlParameter();
                        prmXMLDeliveryAddress.ParameterName = "@xmlDeliveryAddressDetails";
                        prmXMLDeliveryAddress.SqlDbType = System.Data.SqlDbType.Xml;
                        prmXMLDeliveryAddress.Direction = ParameterDirection.Input;
                        prmXMLDeliveryAddress.Value = strDeliveryAddressXML;
                        cmd.Parameters.Add(prmXMLDeliveryAddress);

                        SqlParameter prmDeliveryPersonFcmToken = new SqlParameter();
                        prmDeliveryPersonFcmToken.ParameterName = "@DeliveryPersonFcmToken";
                        prmDeliveryPersonFcmToken.Size = 2000;
                        prmDeliveryPersonFcmToken.SqlDbType = System.Data.SqlDbType.VarChar;
                        prmDeliveryPersonFcmToken.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(prmDeliveryPersonFcmToken);
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        DeliveryPersonFcmToken = cmd.Parameters["@DeliveryPersonFcmToken"].Value.ToString();

                        if (DeliveryPersonFcmToken.Trim() != "")
                        {
                            await FireBaseService.PostNotifications(DeliveryPersonFcmToken, "OrderID: " + tblOrders.OrderID, " Order has been updated by the customer. Please check the order again.");
                        }

                        response.Result = tblOrders.OrderID;
                        response.Message = "Order Succesfull Updated";
                        response.HasError = false;
                    }
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.HasError = true;
            }



            return response.ToHttpResponse();
        }



        //// POST: api/Orders
        //[HttpPost]
        //public async Task<IActionResult> PostTblOrders([FromBody] OrderRequest tblOrders)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    TblOrders order = null;
        //    var response = new GenericResponse<int>();

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            order = new TblOrders
        //            {
        //                OrderId = tblOrders.OrderId,
        //                CustomerId = tblOrders.CustomerId,
        //                PickupFromTime = tblOrders.PickupFromTime,
        //                PickupToTime = tblOrders.PickupToTime,
        //                DeliveryPersonId = tblOrders.DeliveryPersonId,
        //                PickupLocality = tblOrders.PickupLocality,
        //                MobileNo = tblOrders.MobileNo,
        //                PickupAddress = tblOrders.PickupAddress,
        //                ContactPersonMobile = tblOrders.ContactPersonMobile,
        //                ContactPerson = tblOrders.ContactPerson,
        //                InternalOrderNo = tblOrders.InternalOrderNo,
        //                Action = tblOrders.Action,
        //                Weight = tblOrders.Weight,
        //                GoodsType = tblOrders.GoodsType,
        //                ParcelValue = tblOrders.ParcelValue,
        //                OrderAmount = tblOrders.OrderAmount,
        //                PaymentTypeId = tblOrders.PaymentTypeId,
        //                OrderStatusId = tblOrders.OrderStatusId,
        //                PaymentStatusID = tblOrders.PaymentStatusID,
        //                CreatedDate = tblOrders.CreatedDate,
        //                FromLat = tblOrders.FromLat,
        //                FromLong = tblOrders.FromLong,
        //                Distance = tblOrders.Distance,
        //                City = tblOrders.City,
        //                CommissionFee = tblOrders.CommissionFee,
        //                PaymentFrom = tblOrders.PaymentFrom,
        //                ProductImage = tblOrders.ProductImage
        //            };
        //            _context.Add(order);

        //            _context.SaveChanges();

        //            OrderDeliveryAddDto orderData = tblOrders.OrderDeliveryAdd;
        //            orderData.OrderId = order.OrderId;


        //            _context.Add(_mappper.Map<TblOrderDeliveryAddress>(orderData));

        //            _context.SaveChanges();

        //            response.Result = order.OrderId;
        //            response.Message = "Order Succesfull Updated";
        //            response.HasError = false;

        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            response.Message = ex.Message;
        //            response.HasError = true;
        //        }
        //    }
        //    return response.ToHttpResponse();
        //}



        //[HttpPut("editOrders")]
        //public async Task<IActionResult> EditTblOrders([FromBody] OrderRequest tblOrders)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    TblOrders order = null;
        //    var response = new GenericResponse<int>();

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {

        //            order = new TblOrders
        //            {
        //                OrderId = tblOrders.OrderId,
        //                CustomerId = tblOrders.CustomerId,
        //                PickupFromTime = tblOrders.PickupFromTime,
        //                PickupToTime = tblOrders.PickupToTime,
        //                DeliveryPersonId = tblOrders.DeliveryPersonId,
        //                PickupLocality = tblOrders.PickupLocality,
        //                MobileNo = tblOrders.MobileNo,
        //                PickupAddress = tblOrders.PickupAddress,
        //                ContactPersonMobile = tblOrders.ContactPersonMobile,
        //                ContactPerson = tblOrders.ContactPerson,
        //                InternalOrderNo = tblOrders.InternalOrderNo,
        //                Action = tblOrders.Action,
        //                Weight = tblOrders.Weight,
        //                GoodsType = tblOrders.GoodsType,
        //                ParcelValue = tblOrders.ParcelValue,
        //                OrderAmount = tblOrders.OrderAmount,
        //                PaymentTypeId = tblOrders.PaymentTypeId,
        //                OrderStatusId = tblOrders.OrderStatusId,
        //                CreatedDate = tblOrders.CreatedDate,
        //                FromLat = tblOrders.FromLat,
        //                FromLong = tblOrders.FromLong,
        //                Distance = tblOrders.Distance,
        //                City = tblOrders.City,
        //                PaymentFrom = tblOrders.PaymentFrom,
        //                ProductImage = tblOrders.ProductImage,
        //                CommissionFee = tblOrders.CommissionFee,
        //                PaymentStatusID = tblOrders.PaymentStatusID

        //            };

        //            _context.Update(order);

        //            _context.SaveChanges();

        //            OrderDeliveryAddDto orderData = tblOrders.OrderDeliveryAdd;
        //            orderData.OrderId = order.OrderId;

        //            _context.Update(_mappper.Map<TblOrderDeliveryAddress>(orderData));

        //            _context.SaveChanges();

        //            response.Result = order.OrderId;
        //            response.Message = "Order Succesfull";
        //            response.HasError = false;

        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            response.Message = ex.Message;
        //            response.HasError = true;
        //        }
        //    }



        //    return response.ToHttpResponse();
        //}

        [HttpGet("customerOrder")]
        public IActionResult GetCustomerOrder(int customerID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<List<OrderDto>>();
            try
            {
                var data = _context.TblOrders.Include(p => p.DeliveryPerson).Include(p => p.TblOrderDeliveryAddress).ToList();

                var tblOrders = data.Where(p => p.CustomerID == customerID).ToList();

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



        [HttpGet("GetOrderDetails")]
        public IActionResult GetOrderDetails(int OrderID, int DeliveryPersonID, int CustomerID, string CustomerName, string CustomerMob, string CustomerEmail, DateTime FromOrderDate, DateTime ToOrderDate, string PaymentType, string PaymentStatus, string OrderStatus, string DeliveryCity, Decimal Distance)
        {

            DataSet ds = new DataSet();

            List<OrderWithDeliveryDetailsDTO> OrderList = new List<OrderWithDeliveryDetailsDTO>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var response = new GenericResponse<List<OrderDto>>();
            //var response = new GenericResponse<DataTable>();
            var response = new GenericResponse<List<OrderWithDeliveryDetailsDTO>>();

            try
            {
                if ((FromOrderDate.ToShortDateString() == "01/01/0001") || (FromOrderDate.ToShortDateString().Trim() == "1/1/0001") || (FromOrderDate.ToShortDateString().Trim() == "1/1/1753") || (FromOrderDate.ToShortDateString().Trim() == "01/01/1753"))
                {
                    FromOrderDate = Convert.ToDateTime("01/01/1800");
                }
                if ((ToOrderDate.ToShortDateString() == "01/01/0001") || (ToOrderDate.ToShortDateString().Trim() == "1/1/0001") || (ToOrderDate.ToShortDateString().Trim() == "1/1/1753") || (ToOrderDate.ToShortDateString().Trim() == "01/01/1753"))
                {
                    ToOrderDate = Convert.ToDateTime("01/01/1800");
                }

                //   var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.TblOrderDeliveryAddress).ToList();

                //   var tblOrders = data.Where(p => p.DeliveryPersonId == id && p.DeliveryPersonId != null).ToList();

                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("prSearchOrderDetails", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", DeliveryPersonID));
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
                        cmd.Parameters.Add(new SqlParameter("@CustomerName", CustomerName));
                        cmd.Parameters.Add(new SqlParameter("@CustomerMob", CustomerMob));
                        cmd.Parameters.Add(new SqlParameter("@CustomerEmail", CustomerEmail));
                        cmd.Parameters.Add(new SqlParameter("@FromOrderDate", FromOrderDate));
                        cmd.Parameters.Add(new SqlParameter("@ToOrderDate", ToOrderDate));
                        cmd.Parameters.Add(new SqlParameter("@PaymentType", PaymentType));
                        cmd.Parameters.Add(new SqlParameter("@PaymentStatus", PaymentStatus));
                        cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                        cmd.Parameters.Add(new SqlParameter("@DeliveryCity", DeliveryCity));
                        cmd.Parameters.Add(new SqlParameter("@Distance", Distance));
                        sql.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        sql.Close();
                        ds.Tables[0].TableName = "TblOrders";
                        ds.Tables[1].TableName = "TblOrderDeliveryAddress";

                        foreach (DataRow Row in ds.Tables[0].Rows)
                        {
                            OrderWithDeliveryDetailsDTO ObjOrderWithDetails = new OrderWithDeliveryDetailsDTO();

                            OrderCustomerDTO ObjOrderCustomerDTO = new OrderCustomerDTO();
                            OrderDeliveryPersonDTO ObjOrderDeliveryPersonDTO = new OrderDeliveryPersonDTO();

                            ObjOrderWithDetails.OrderID = Convert.ToInt32(Row["OrderID"]);
                            ObjOrderWithDetails.CustomerID = Convert.ToInt32(Row["CustomerID"]);
                            
                            ObjOrderWithDetails.DeliveryPersonID = Convert.ToInt32(Row["DeliveryPersonID"]);
                          
                            ObjOrderWithDetails.PickupLocality = Row["PickupLocality"].ToString();
                            ObjOrderWithDetails.City = Row["City"].ToString();
                            ObjOrderWithDetails.MobileNo = Row["MobileNo"].ToString();
                            ObjOrderWithDetails.PickupFromTime = Convert.ToDateTime(Row["PickupFromTime"]);
                            ObjOrderWithDetails.PickupToTime = Convert.ToDateTime(Row["PickupToTime"]);
                            ObjOrderWithDetails.PickupAddress = Row["PickupAddress"].ToString();
                            ObjOrderWithDetails.ContactPersonMobile = Row["ContactPersonMobile"].ToString();
                            ObjOrderWithDetails.ContactPerson = Row["ContactPerson"].ToString();
                            ObjOrderWithDetails.InternalOrderNo = Row["InternalOrderNo"].ToString();
                            ObjOrderWithDetails.Action = Row["Action"].ToString();
                            ObjOrderWithDetails.Weight = Row["Weight"].ToString();

                            ObjOrderWithDetails.GoodsType = Row["GoodsType"].ToString();
                            ObjOrderWithDetails.ParcelValue = Convert.ToInt32(Row["ParcelValue"]);
                            ObjOrderWithDetails.OrderAmount = Convert.ToInt32(Row["OrderAmount"]);
                            ObjOrderWithDetails.SecurityFee = Convert.ToInt32(Row["SecurityFee"]);
                            ObjOrderWithDetails.CommissionFee = Convert.ToInt32(Row["CommissionFee"]);
                            ObjOrderWithDetails.PaymentTypeID = Convert.ToInt32(Row["PaymentTypeID"]);
                            ObjOrderWithDetails.PaymentType = Row["PaymentType"].ToString();
                            ObjOrderWithDetails.OrderStatusID = Convert.ToInt32(Row["OrderStatusID"]);
                            ObjOrderWithDetails.OrderStatus = Row["OrderStatus"].ToString();

                            ObjOrderWithDetails.PaymentStatusID = Convert.ToInt32(Row["PaymentStatusID"]);
                            ObjOrderWithDetails.PaymentStatus = Row["PaymentStatus"].ToString();
                            ObjOrderWithDetails.CreatedDate  = Convert.ToDateTime(Row["CreatedDate"]);
                            ObjOrderWithDetails.FromLat = Convert.ToDecimal(Row["FromLat"]);
                            ObjOrderWithDetails.FromLong = Convert.ToDecimal(Row["FromLong"]);
                            ObjOrderWithDetails.PaymentFrom = Row["PaymentFrom"].ToString();
                            ObjOrderWithDetails.ProductImage = Row["ProductImage"].ToString();
                            ObjOrderWithDetails.PromoCode = Row["PromoCode"].ToString();

                            ObjOrderWithDetails.Discount = Convert.ToInt32(Row["Discount"]);
                            ObjOrderWithDetails.PointRedemption = Convert.ToInt32(Row["PointRedemption"]);

                            // Map to the Customer DTO
                            ObjOrderCustomerDTO.CustomerID = Convert.ToInt32(Row["CustomerID"]);
                            ObjOrderCustomerDTO.CustomerName = Row["CustomerName"].ToString();
                            ObjOrderCustomerDTO.CustomerMobileNo = Row["CustomerMobileNo"].ToString();
                            ObjOrderCustomerDTO.CustomerFCMToken = Row["CustomerFCMToken"].ToString();

                            // Map to the Delivery Person DTO
                            if (Convert.ToInt32(Row["DeliveryPersonID"]) != 0)
                            {
                                ObjOrderDeliveryPersonDTO.DeliveryPersonID = Convert.ToInt32(Row["DeliveryPersonID"]);
                                ObjOrderDeliveryPersonDTO.DeliveryPersonName = Row["DeliveryPersonName"].ToString();
                                ObjOrderDeliveryPersonDTO.DeliveryPersonMobileNo = Row["DeliveryPersonMobileNo"].ToString();
                                ObjOrderDeliveryPersonDTO.DeliveryPersonProfileImage = Row["DeliveryPersonProfileImage"].ToString();
                                ObjOrderDeliveryPersonDTO.DeliveryPersonFCMToken = Row["DeliveryPersonFCMToken"].ToString();
                                ObjOrderWithDetails.DeliveryPerson = ObjOrderDeliveryPersonDTO;
                            }
                            else
                            {
                                ObjOrderWithDetails.DeliveryPerson = null;
                            }
                            int i = 0;
                            DataRow[] dr = ds.Tables[1].Select("OrderID = " + ObjOrderWithDetails.OrderID);
                            int j = dr.Count();
                            OrderDeliveryAddDto[] ObjOrderDeliveryAddDTO = new OrderDeliveryAddDto[j];
                            foreach (DataRow DeliveryAddressRow in ds.Tables[1].Select("OrderID = " + ObjOrderWithDetails.OrderID))
                            {
                                OrderDeliveryAddDto ObjOrderDeliveryAdd = new OrderDeliveryAddDto();

                                ObjOrderDeliveryAdd.OrderDeliveryAddressID = Convert.ToInt32(DeliveryAddressRow["OrderDeliveryAddressID"]);
                                ObjOrderDeliveryAdd.OrderID = Convert.ToInt32(DeliveryAddressRow["OrderID"]);
                                ObjOrderDeliveryAdd.DropLocality = DeliveryAddressRow["DropLocality"].ToString();
                                ObjOrderDeliveryAdd.MobileNo = DeliveryAddressRow["MobileNo"].ToString();
                                ObjOrderDeliveryAdd.DeliveryFromTime = Convert.ToDateTime(DeliveryAddressRow["DeliveryFromTime"]);
                                ObjOrderDeliveryAdd.DeliveryToTime = Convert.ToDateTime(DeliveryAddressRow["DeliveryToTime"]);
                                ObjOrderDeliveryAdd.DeliveryAddress = DeliveryAddressRow["DeliveryAddress"].ToString();
                                ObjOrderDeliveryAdd.ContactPerson = DeliveryAddressRow["ContactPerson"].ToString();
                                ObjOrderDeliveryAdd.InternalOrderNo = DeliveryAddressRow["InternalOrderNo"].ToString();
                                ObjOrderDeliveryAdd.Action = DeliveryAddressRow["Action"].ToString();
                                ObjOrderDeliveryAdd.Latitude = Convert.ToDecimal(DeliveryAddressRow["Latitude"]);
                                ObjOrderDeliveryAdd.Longitude = Convert.ToDecimal(DeliveryAddressRow["Longitude"]);
                                ObjOrderDeliveryAdd.ApproxDistance  = Convert.ToDecimal(DeliveryAddressRow["ApproxDistance"]);
                                ObjOrderDeliveryAdd.ApproxTime = (TimeSpan)DeliveryAddressRow["ApproxTime"];
                                ObjOrderDeliveryAdd.ProductImage = DeliveryAddressRow["ProductImage"].ToString();
                                ObjOrderDeliveryAddDTO[i] = ObjOrderDeliveryAdd;
                                i++;
                            }
                            ObjOrderWithDetails.Customer = ObjOrderCustomerDTO;
                            ObjOrderWithDetails.OrderDeliveryAdd = ObjOrderDeliveryAddDTO;
                            OrderList.Add(ObjOrderWithDetails);
                        }
                      
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            response.Message = "No Orders Found";
                        }
                        else
                        {
                           // List<OrderWithDeliveryDetailsDTO>ObjOrderDetails = (ObjOrderWithDetails as IEnumerable<OrderWithDeliveryDetailsDTO>).Cast<OrderWithDeliveryDetailsDTO>().ToList();
                          

                            // response.Message = SucessMessege + " From Date: " + ToOrderDate.ToShortDateString() + " To Date: " + ToOrderDate.ToShortDateString();
                            response.Message = SucessMessege;
                        }
                        response.Result = OrderList;
                        response.HasError = false;
                    }
                }
            }

            catch (Exception e)
            {
                response.Message = e.Message + " From Date: " + ToOrderDate.ToShortDateString() + " To Date: " + ToOrderDate.ToShortDateString();
                response.HasError = false;
            }

            return response.ToHttpResponse();
        }


        [HttpGet("GetOrderDetailsFromLatAndLong")]
        public IActionResult GetOrderDetailsFromLatAndLong(Decimal  SourceLatitude, Decimal SourceLongitude)
        {

            DataSet ds = new DataSet();

            List<OrderWithDeliveryDetailsDTO> OrderList = new List<OrderWithDeliveryDetailsDTO>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var response = new GenericResponse<List<OrderDto>>();
            //var response = new GenericResponse<DataTable>();
            var response = new GenericResponse<List<OrderWithDeliveryDetailsDTO>>();

            try
            {
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("prSearchOrderDetailsFromLatAndLong", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@sourceLatitude", SourceLatitude));
                        cmd.Parameters.Add(new SqlParameter("@sourceLongitude", SourceLongitude));
                        sql.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        sql.Close();
                        ds.Tables[0].TableName = "TblOrders";
                        ds.Tables[1].TableName = "TblOrderDeliveryAddress";

                        foreach (DataRow Row in ds.Tables[0].Rows)
                        {
                            OrderWithDeliveryDetailsDTO ObjOrderWithDetails = new OrderWithDeliveryDetailsDTO();

                            OrderCustomerDTO ObjOrderCustomerDTO = new OrderCustomerDTO();
                            OrderDeliveryPersonDTO ObjOrderDeliveryPersonDTO = new OrderDeliveryPersonDTO();

                            ObjOrderWithDetails.OrderID = Convert.ToInt32(Row["OrderID"]);
                            ObjOrderWithDetails.CustomerID = Convert.ToInt32(Row["CustomerID"]);

                            ObjOrderWithDetails.DeliveryPersonID = Convert.ToInt32(Row["DeliveryPersonID"]);

                            ObjOrderWithDetails.PickupLocality = Row["PickupLocality"].ToString();
                            ObjOrderWithDetails.City = Row["City"].ToString();
                            ObjOrderWithDetails.MobileNo = Row["MobileNo"].ToString();
                            ObjOrderWithDetails.PickupFromTime = Convert.ToDateTime(Row["PickupFromTime"]);
                            ObjOrderWithDetails.PickupToTime = Convert.ToDateTime(Row["PickupToTime"]);
                            ObjOrderWithDetails.PickupAddress = Row["PickupAddress"].ToString();
                            ObjOrderWithDetails.ContactPersonMobile = Row["ContactPersonMobile"].ToString();
                            ObjOrderWithDetails.ContactPerson = Row["ContactPerson"].ToString();
                            ObjOrderWithDetails.InternalOrderNo = Row["InternalOrderNo"].ToString();
                            ObjOrderWithDetails.Action = Row["Action"].ToString();
                            ObjOrderWithDetails.Weight = Row["Weight"].ToString();

                            ObjOrderWithDetails.GoodsType = Row["GoodsType"].ToString();
                            ObjOrderWithDetails.ParcelValue = Convert.ToInt32(Row["ParcelValue"]);
                            ObjOrderWithDetails.OrderAmount = Convert.ToInt32(Row["OrderAmount"]);
                            ObjOrderWithDetails.SecurityFee = Convert.ToInt32(Row["SecurityFee"]);
                            ObjOrderWithDetails.CommissionFee = Convert.ToInt32(Row["CommissionFee"]);
                            ObjOrderWithDetails.PaymentTypeID = Convert.ToInt32(Row["PaymentTypeID"]);
                            ObjOrderWithDetails.PaymentType = Row["PaymentType"].ToString();
                            ObjOrderWithDetails.OrderStatusID = Convert.ToInt32(Row["OrderStatusID"]);
                            ObjOrderWithDetails.OrderStatus = Row["OrderStatus"].ToString();

                            ObjOrderWithDetails.PaymentStatusID = Convert.ToInt32(Row["PaymentStatusID"]);
                            ObjOrderWithDetails.PaymentStatus = Row["PaymentStatus"].ToString();
                            ObjOrderWithDetails.CreatedDate = Convert.ToDateTime(Row["CreatedDate"]);
                            ObjOrderWithDetails.FromLat = Convert.ToDecimal(Row["FromLat"]);
                            ObjOrderWithDetails.FromLong = Convert.ToDecimal(Row["FromLong"]);
                            ObjOrderWithDetails.PaymentFrom = Row["PaymentFrom"].ToString();
                            ObjOrderWithDetails.ProductImage = Row["ProductImage"].ToString();
                            ObjOrderWithDetails.PromoCode = Row["PromoCode"].ToString();

                            ObjOrderWithDetails.Discount = Convert.ToInt32(Row["Discount"]);
                            ObjOrderWithDetails.PointRedemption = Convert.ToInt32(Row["PointRedemption"]);

                            // Map to the Customer DTO
                            ObjOrderCustomerDTO.CustomerID = Convert.ToInt32(Row["CustomerID"]);
                            ObjOrderCustomerDTO.CustomerName = Row["CustomerName"].ToString();
                            ObjOrderCustomerDTO.CustomerMobileNo = Row["CustomerMobileNo"].ToString();
                            ObjOrderCustomerDTO.CustomerFCMToken = Row["CustomerFCMToken"].ToString();

                            // Map to the Delivery Person DTO
                            if (Convert.ToInt32(Row["DeliveryPersonID"]) != 0)
                            {
                                ObjOrderDeliveryPersonDTO.DeliveryPersonID = Convert.ToInt32(Row["DeliveryPersonID"]);
                                ObjOrderDeliveryPersonDTO.DeliveryPersonName = Row["DeliveryPersonName"].ToString();
                                ObjOrderDeliveryPersonDTO.DeliveryPersonMobileNo = Row["DeliveryPersonMobileNo"].ToString();
                                ObjOrderDeliveryPersonDTO.DeliveryPersonProfileImage = Row["DeliveryPersonProfileImage"].ToString();
                                ObjOrderDeliveryPersonDTO.DeliveryPersonFCMToken = Row["DeliveryPersonFCMToken"].ToString();
                                ObjOrderWithDetails.DeliveryPerson = ObjOrderDeliveryPersonDTO;
                            }
                            else
                            {
                                ObjOrderWithDetails.DeliveryPerson = null;
                            }
                            int i = 0;
                            DataRow[] dr = ds.Tables[1].Select("OrderID = " + ObjOrderWithDetails.OrderID);
                            int j = dr.Count();
                            OrderDeliveryAddDto[] ObjOrderDeliveryAddDTO = new OrderDeliveryAddDto[j];
                            foreach (DataRow DeliveryAddressRow in ds.Tables[1].Select("OrderID = " + ObjOrderWithDetails.OrderID))
                            {
                                OrderDeliveryAddDto ObjOrderDeliveryAdd = new OrderDeliveryAddDto();

                                ObjOrderDeliveryAdd.OrderDeliveryAddressID = Convert.ToInt32(DeliveryAddressRow["OrderDeliveryAddressID"]);
                                ObjOrderDeliveryAdd.OrderID = Convert.ToInt32(DeliveryAddressRow["OrderID"]);
                                ObjOrderDeliveryAdd.DropLocality = DeliveryAddressRow["DropLocality"].ToString();
                                ObjOrderDeliveryAdd.MobileNo = DeliveryAddressRow["MobileNo"].ToString();
                                ObjOrderDeliveryAdd.DeliveryFromTime = Convert.ToDateTime(DeliveryAddressRow["DeliveryFromTime"]);
                                ObjOrderDeliveryAdd.DeliveryToTime = Convert.ToDateTime(DeliveryAddressRow["DeliveryToTime"]);
                                ObjOrderDeliveryAdd.DeliveryAddress = DeliveryAddressRow["DeliveryAddress"].ToString();
                                ObjOrderDeliveryAdd.ContactPerson = DeliveryAddressRow["ContactPerson"].ToString();
                                ObjOrderDeliveryAdd.InternalOrderNo = DeliveryAddressRow["InternalOrderNo"].ToString();
                                ObjOrderDeliveryAdd.Action = DeliveryAddressRow["Action"].ToString();
                                ObjOrderDeliveryAdd.Latitude = Convert.ToDecimal(DeliveryAddressRow["Latitude"]);
                                ObjOrderDeliveryAdd.Longitude = Convert.ToDecimal(DeliveryAddressRow["Longitude"]);
                                ObjOrderDeliveryAdd.ApproxDistance = Convert.ToDecimal(DeliveryAddressRow["ApproxDistance"]);
                                ObjOrderDeliveryAdd.ApproxTime = (TimeSpan)DeliveryAddressRow["ApproxTime"];
                                ObjOrderDeliveryAdd.ProductImage = DeliveryAddressRow["ProductImage"].ToString();
                                ObjOrderDeliveryAddDTO[i] = ObjOrderDeliveryAdd;
                                i++;
                            }
                            ObjOrderWithDetails.Customer = ObjOrderCustomerDTO;
                            ObjOrderWithDetails.OrderDeliveryAdd = ObjOrderDeliveryAddDTO;
                            OrderList.Add(ObjOrderWithDetails);
                        }

                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            response.Message = "No Orders Found";
                        }
                        else
                        {
                            // List<OrderWithDeliveryDetailsDTO>ObjOrderDetails = (ObjOrderWithDetails as IEnumerable<OrderWithDeliveryDetailsDTO>).Cast<OrderWithDeliveryDetailsDTO>().ToList();


                            // response.Message = SucessMessege + " From Date: " + ToOrderDate.ToShortDateString() + " To Date: " + ToOrderDate.ToShortDateString();
                            response.Message = SucessMessege;
                        }
                        response.Result = OrderList;
                        response.HasError = false;
                    }
                }
            }

            catch (Exception e)
            {
                response.Message = e.Message ;
                response.HasError = false;
            }

            return response.ToHttpResponse();
        }


        //[HttpPut("CompleteOrderByDeliveryPerson")]
        //public async Task<IActionResult> CompleteOrderByDeliveryPerson(int OrderID, List<IFormFile> File)
        //{
        //    string CustomerFcmToken = "                                                                                                                                         ";
        //    string CompleteFilePath = "";
        //    string ImagePath = "";
        //    ImagePath = ProductImagePath.ProductsImagePath;

        //    GenericResponse<bool> response = new GenericResponse<bool>();
        //    try
        //    {
        //        string imagefileName = Guid.NewGuid().ToString() + Path.GetExtension(File[0].FileName);

        //        string FilePath = string.Concat(ImagePath, "/", imagefileName);

        //        using (var stream = System.IO.File.Create(FilePath))
        //        {
        //            await File[0].CopyToAsync(stream);
        //            stream.Close();
        //            string host = _httpContextAccessor.HttpContext.Request.Host.Value;
        //            CompleteFilePath = string.Concat("http://", host, "/", FilePath);
        //        }
        //        using (SqlConnection sql = new SqlConnection(ConnectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("prCompleteOrderByDeliveryPerson", sql))
        //            {
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
        //                cmd.Parameters.Add(new SqlParameter("@ProductImagePath", CompleteFilePath));
        //                cmd.Parameters.Add(new SqlParameter("@CustomerFcmToken", CustomerFcmToken));
        //                cmd.Parameters["@CustomerFcmToken"].Size = 4000;
        //                cmd.Parameters["@CustomerFcmToken"].Direction = ParameterDirection.Output;
        //                sql.Open();
        //                cmd.ExecuteNonQuery();
        //                CustomerFcmToken = cmd.Parameters["@CustomerFcmToken"].Value.ToString();
        //                response.HasError = false;
        //                response.Result = true;
        //                response.Message = "Order Status Change Successfully";
        //                await FireBaseService.PostNotifications(CustomerFcmToken, "OrderId" + OrderID, "Your order is sucessfully delivered to the delivered address");
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        response.HasError = false;
        //        response.Message = e.Message;
        //    }
        //    return response.ToHttpResponse();
        //}


        [HttpPut("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int OrderID, int DeliveryPersonID, string OrderStatus, List<IFormFile> File)
        {
            string _notificationMsg = "                                                                                                                                                            ";
            string CustomerFcmToken = "                                                                                                                                         ";
            string DeliveryPersonFcmToken = "                                                                                                                                         ";
            string CompleteFilePath = "";
            string ImagePath = "";
            ImagePath = ProductImagePath.ProductsImagePath;
            GenericResponse<bool> response = new GenericResponse<bool>();
            
            try
            {
                if (File.Count != 0)
                {
                    string imagefileName = Guid.NewGuid().ToString() + Path.GetExtension(File[0].FileName);

                    string FilePath = string.Concat(ImagePath, "/", imagefileName);

                    using (var stream = System.IO.File.Create(FilePath))
                    {
                        await File[0].CopyToAsync(stream);
                        stream.Close();
                        string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                        CompleteFilePath = string.Concat("http://", host, "/", FilePath);
                    }
                }
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prUpdateOrderStatus", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", DeliveryPersonID));
                        cmd.Parameters.Add(new SqlParameter("@OrderStatus", OrderStatus));
                        cmd.Parameters.Add(new SqlParameter("@ProductImagePath", CompleteFilePath));
                        cmd.Parameters.Add(new SqlParameter("@NotificationMessage", _notificationMsg));
                        cmd.Parameters.Add(new SqlParameter("@CustomerFcmToken", CustomerFcmToken));
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonFcmToken", DeliveryPersonFcmToken));
                        cmd.Parameters["@NotificationMessage"].Size = 4000;
                        cmd.Parameters["@NotificationMessage"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@CustomerFcmToken"].Size = 4000;
                        cmd.Parameters["@CustomerFcmToken"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@DeliveryPersonFcmToken"].Size = 4000;
                        cmd.Parameters["@DeliveryPersonFcmToken"].Direction = ParameterDirection.Output;
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        _notificationMsg = cmd.Parameters["@NotificationMessage"].Value.ToString();
                        CustomerFcmToken = cmd.Parameters["@CustomerFcmToken"].Value.ToString();
                        DeliveryPersonFcmToken = cmd.Parameters["@DeliveryPersonFcmToken"].Value.ToString();
                        response.HasError = false;
                        response.Result = true;
                        response.Message = "Order Status Change Successfully";

                        if (OrderStatus != "Canceled")
                            await FireBaseService.PostNotifications(CustomerFcmToken, "OrderId" + OrderID, _notificationMsg);

                        else
                            await FireBaseService.PostDeliveryBoyNotifications(DeliveryPersonFcmToken, "OrderId" + OrderID, _notificationMsg);

                    }
                }
            }
            catch (Exception e)
            {
                response.HasError = false;
                response.Message = e.Message;
            }
            return response.ToHttpResponse();
        }


        // Cancel order by Delivery Person
        [HttpPut("CancelOrderByDeliveryPerson")]
        public IActionResult CancelOrderByDeliveryPerson(int OrderID)
        {

            GenericResponse<bool> response = new GenericResponse<bool>();
            try
            {
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {


                    using (SqlCommand cmd = new SqlCommand("prCancelOrderByDeliveryPerson", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        response.HasError = false;
                        response.Result = true;
                        response.Message = "Order Cancel Successfully";
                    }
                }
            }
            catch (Exception e)
            {
                response.HasError = false;
                response.Message = e.Message;
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
                var data = _context.TblOrders.Include(p => p.Customer).Include(p => p.TblOrderDeliveryAddress).ToList();

                var tblOrders = data.Where(p => p.DeliveryPersonID == id && p.DeliveryPersonID != null).ToList();

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
                    var deliveryData = _context.TblOrderDeliveryAddress.Where(p => p.OrderID == id).FirstOrDefault();

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
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();
            }
            return response.ToHttpResponse();
        }

        private bool TblOrdersExists(int id)
        {
            return _context.TblOrders.Any(e => e.OrderID == id);
        }

        private string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

    }
}