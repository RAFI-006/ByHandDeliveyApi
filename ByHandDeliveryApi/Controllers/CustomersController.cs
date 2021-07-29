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
using System.Web.Http.Cors;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyAllowSpecificOrigins", headers: "*", methods: "*")]
    public class CustomersController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly string _successMsg = "Successfully Completed";

        public CustomersController(db_byhanddeliveryContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public IActionResult GetTblCustomers()
        {
            var response = new GenericResponse<List<CustomersDto>>();

            try
            {
                var data = _context.TblCustomers.ToList();
                var list = new List<CustomersDto>();
                foreach (var item in data)
                {
                    list.Add(_mapper.Map<CustomersDto>(item));
                }

                response.HasError = false;
                response.Message = _successMsg;
                response.Result = list;
            }
            catch(Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }


            return response.ToHttpResponse();
        }


        [HttpGet("Login")]
        public IActionResult Login( string phone,string pass,string fcmtoken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<CustomersDto>();
            try
            {
                var tblCustomers =  _context.TblCustomers.Where(p=>p.MobileNo==phone && p.Password== pass).FirstOrDefault();

                if (tblCustomers == null)
                {
                    response.HasError = true;
                    response.Message = "User not found";
                }
                else
                {
                    if (fcmtoken == null)
                    {

                        response.HasError = false;
                        response.Message = _successMsg;
                        response.Result = _mapper.Map<CustomersDto>(tblCustomers);
                    }
                    else
                    {
                        tblCustomers.FcmToken = fcmtoken;
                        _context.TblCustomers.Update(tblCustomers);
                        _context.SaveChanges();
                        response.HasError = false;
                        response.Message = _successMsg;
                        var data = _context.TblCustomers.Where(p => p.CustomerID == tblCustomers.CustomerID).FirstOrDefault();
                        response.Result = _mapper.Map<CustomersDto>(data);
                    }
                    
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;

            }

            return response.ToHttpResponse();

        }


        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(string phone,String password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<String>();
            try
            {
                var tblCustomers = _context.TblCustomers.Where(p => p.MobileNo == phone).FirstOrDefault();

          

                if (tblCustomers == null)
                {
                    response.HasError = true;
                    response.Message = "This number is not registered";
                }
                else
                {
                    tblCustomers.Password = password;
                    _context.TblCustomers.Update(tblCustomers);
                    _context.SaveChanges();
                    response.HasError = false;
                    response.Message = _successMsg;
                    response.Result = "Password Successfully Updated";

                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;

            }

            return response.ToHttpResponse();

        }

        [HttpGet("IsCustomerRegistered")]
        public IActionResult IsUserRegisered(string number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<bool> responses = new GenericResponse<bool>();

            try
            {
                var data = TblCustomersExists(number);

                responses.HasError = false;
                responses.Result = data;
                responses.Message = _successMsg;


            }
            catch (Exception e)
            {
                responses.HasError = false;
                responses.Message = e.InnerException.ToString();

            }

            return responses.ToHttpResponse();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblCustomers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<CustomersDto>();
            try
            {
                var tblCustomers = await _context.TblCustomers.FindAsync(id);

                if (tblCustomers == null)
                {
                    response.HasError = true;
                    response.Message = "User not found";
                }
                else
                {
                    response.HasError = false;
                    response.Message = "Successfull";
                    response.Result = _mapper.Map<CustomersDto>(tblCustomers);
                }
            }
            catch(Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;

            }

            return response.ToHttpResponse();

        }

        // PUT: api/Customers/5
        [HttpPut]
        public IActionResult PutTblCustomers([FromBody] CustomersDto tblCustomers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<CustomersDto> response = new GenericResponse<CustomersDto>();
            try
            { 
            if (TblCustomersExists(tblCustomers.CustomerID))
            {
                _context.Update(_mapper.Map<TblCustomers>(tblCustomers));
                _context.SaveChanges();
                    var res = _context.TblCustomers.Where(p => p.MobileNo == tblCustomers.MobileNo).FirstOrDefault();
                    response.Result = _mapper.Map<CustomersDto>(res);
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

        // POST: api/Customers
        [HttpPost]
        public  IActionResult PostTblCustomers([FromBody] CustomersDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<CustomersDto> responses = new GenericResponse<CustomersDto>();
            try
            {
                if (!TblCustomersExists(data.MobileNo))
                {
                    _context.Add(_mapper.Map<TblCustomers>(data));
                    _context.SaveChanges();
                    var res = _context.TblCustomers.Where(p=>p.MobileNo == data.MobileNo).FirstOrDefault();
                    responses.Result = _mapper.Map<CustomersDto>(res);
                    responses.Message = "Successfull";
                    responses.HasError = false;
                }
                else
                {
                    responses.Message = "User Already Exist";
                    responses.HasError = false;
                }

            }
            catch (Exception e)
            {
                responses.Message = e.Message;
                responses.HasError = true;
            }

            return responses.ToHttpResponse();
        }

        // DELETE: api/Customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblCustomers([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblCustomers = await _context.TblCustomers.FindAsync(id);
        //    if (tblCustomers == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblCustomers.Remove(tblCustomers);
        //    await _context.SaveChangesAsync();

        //    return Ok(tblCustomers);
        //}

        private bool TblCustomersExists(int id)
        {
            return _context.TblCustomers.Any(e => e.CustomerID == id);
        }

        private bool TblCustomersExists(string phoneNumber)
        {
            return _context.TblCustomers.Any(e => e.MobileNo == phoneNumber);
        }
    }
}