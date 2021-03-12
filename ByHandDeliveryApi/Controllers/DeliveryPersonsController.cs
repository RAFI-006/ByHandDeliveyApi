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
using System.Data.SqlClient;
using ByHandDeliveryApi.Services;
using System.Web.Http.Cors;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyAllowSpecificOrigins", headers: "*", methods: "*")]
    public class DeliveryPersonsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly string _successMsg = "Successfully Completed";


        public DeliveryPersonsController(db_byhanddeliveryContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet ("GetAccountDetails")]
        public IActionResult GetAccountDetails(int deliveryPersonId)
        {
            var response = new GenericResponse<List<TblDeliveryPerson>>();

            try
            {
               // var parameter = new SqlParameter("@DropDownKey", "weight");
                var data = _context.TblDeliveryPerson.Where(p=>p.DeliveryPersonId == deliveryPersonId).Include(p=>p.TblDeliveryPersonAccountDetails).ToList();
                // var result = _context.Database.ExecuteSqlCommand("prDDValue", parameter);

                 //var check = _context.TblDropDown.Include(p => p.TblDdvalues).Where(p=>p.Ddname ==  "Weight").ToList();


                
                //   var productCategory = "Electronics";


                var list = new List<DeliveryPersonDto>();
                //foreach (var item in data)
                //{
                //    list.Add(_mapper.Map<DeliveryPersonDto>(item));
                //}

                response.HasError = false;
                response.Message = _successMsg;
                response.Result = data;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }


            return response.ToHttpResponse();
        }


        // GET: api/DeliveryPersons
        [HttpGet]
        public IActionResult GetTblDeliveryPerson()
        {
            var response = new GenericResponse<List<DeliveryPersonDto>>();

            try
            {
                var data = _context.TblDeliveryPerson.ToList();
                var list = new List<DeliveryPersonDto>();
                foreach (var item in data)
                {
                    list.Add(_mapper.Map<DeliveryPersonDto>(item));
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

        [HttpGet("Login")]
        public IActionResult Login(string phone, string pass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<DeliveryPersonDto>();
            try
            {
                var tblDeliveryBoy = _context.TblDeliveryPerson.Where(p => p.MobileNo == phone && p.Password == pass).FirstOrDefault();

                if (tblDeliveryBoy == null)
                {
                    response.HasError = true;
                    response.Message = "User not found";
                }
                else
                {
                    response.HasError = false;
                    response.Message = _successMsg;
                    response.Result = _mapper.Map<DeliveryPersonDto>(tblDeliveryBoy);
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;

            }

            return response.ToHttpResponse();

        }

        // GET: api/DeliveryPersons/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblDeliveryPerson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblDeliveryPerson = await _context.TblDeliveryPerson.FindAsync(id);

            if (tblDeliveryPerson == null)
            {
                return NotFound();
            }

            return Ok(tblDeliveryPerson);
        }

        // PUT: api/DeliveryPersons/5
        [HttpPut]
        public async  Task<IActionResult> PutTblDeliveryPerson([FromBody] DeliveryPersonDto tblDeliveryPerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<DeliveryPersonDto> response = new GenericResponse<DeliveryPersonDto>();
            try
            {
                if (TblDeliveryPersonExists(tblDeliveryPerson.DeliveryPersonId))
                {
                
                        _context.Update(_mapper.Map<TblDeliveryPerson>(tblDeliveryPerson));
                    _context.SaveChanges();
                    var res = _context.TblDeliveryPerson.Where(p => p.MobileNo == tblDeliveryPerson.MobileNo).FirstOrDefault();
                    response.Result = _mapper.Map<DeliveryPersonDto>(res);



               

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


        [HttpPut("VerifyDeliveryBoy")]
        public async Task<IActionResult> VerifyDeliveryPerosn(int deliveryBoyId , bool isVerified)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<string> response = new GenericResponse<string>();
            try
            {
                if (TblDeliveryPersonExists(deliveryBoyId))
                {
                    var user = _context.TblDeliveryPerson.Where((p) => p.DeliveryPersonId == deliveryBoyId).FirstOrDefault();
                    user.IsVerified = isVerified;
                    _context.Update(_mapper.Map<TblDeliveryPerson>(user));
                    _context.SaveChanges();
                    var res = _context.TblDeliveryPerson.Where(p => p.DeliveryPersonId == deliveryBoyId).FirstOrDefault();




                    if (res.IsVerified == true)
                    {
                        response.Result = "Your a verified user";
                        await FireBaseService.PostDeliveryBoyNotifications(res.Fcmtoken, "Verification Msg", "Your Profie is sucessfully reviewed");
                    }
                    else
                    {
                        response.Result = "Your Verification is on hold";
                        await FireBaseService.PostDeliveryBoyNotifications(res.Fcmtoken, "Verification Msg", "Your Verification is on hold , Pls contact the Support Team");


                    }

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


        // POST: api/DeliveryPersons
        [HttpPost]
        public IActionResult PostTblDeliveryPerson([FromBody] DeliveryPersonDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            GenericResponse<DeliveryPersonDto> responses = new GenericResponse<DeliveryPersonDto>();
            try
            {
                if (!TblDeliveryPersonExists(data.MobileNo))
                {
                    _context.Add(_mapper.Map<TblDeliveryPerson>(data));
                    _context.SaveChanges();
                    var res = _context.TblDeliveryPerson.Where(p => p.MobileNo == data.MobileNo).FirstOrDefault();
                    responses.Result = _mapper.Map<DeliveryPersonDto>(res);
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

        [HttpGet("IsNumberRegistered")]
        public IActionResult IsUserRegisered(string number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<bool> responses = new GenericResponse<bool>();

            try
            {
                var data = TblDeliveryPersonExists(number);

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
        // DELETE: api/DeliveryPersons/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblDeliveryPerson([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblDeliveryPerson = await _context.TblDeliveryPerson.FindAsync(id);
        //    if (tblDeliveryPerson == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblDeliveryPerson.Remove(tblDeliveryPerson);
        //    await _context.SaveChangesAsync();

        //    return Ok(tblDeliveryPerson);
        //}

        private bool TblDeliveryPersonExists(int id)
        {
            return _context.TblDeliveryPerson.Any(e => e.DeliveryPersonId == id);
        }
        private bool TblDeliveryPersonExists(string mobile)
        {
            return _context.TblDeliveryPerson.Any(e => e.MobileNo == mobile);
        }
    }
}