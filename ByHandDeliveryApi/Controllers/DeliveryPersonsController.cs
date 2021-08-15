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
using ByHandDeliveryApi.DTO;
using ByHandDeliveryApi.GenericResponses;
using System.Data.SqlClient;
using ByHandDeliveryApi.Services;
using System.Web.Http.Cors;
using ByHandDeliveryApi.DataModel;
using ByHandDeliveryApi.Security;

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
        private readonly string ConnectionString = Startup.ConnectionString;

        public DeliveryPersonsController(db_byhanddeliveryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //[HttpGet("GetAccountDetails")]
        //public IActionResult GetAccountDetails(int deliveryPersonId)
        //{
        //    var response = new GenericResponse<List<TblDeliveryPerson>>();

        //    try
        //    {
        //        // var parameter = new SqlParameter("@DropDownKey", "weight");
        //        var data = _context.TblDeliveryPerson.Where(p => p.DeliveryPersonId == deliveryPersonId).Include(p => p.TblDeliveryPersonAccountDetails).ToList();
        //        // var result = _context.Database.ExecuteSqlCommand("prDDValue", parameter);

        //        //var check = _context.TblDropDown.Include(p => p.TblDdvalues).Where(p=>p.Ddname ==  "Weight").ToList();



        //        //   var productCategory = "Electronics";


        //        var list = new List<DeliveryPersonDto>();
        //        //foreach (var item in data)
        //        //{
        //        //    list.Add(_mapper.Map<DeliveryPersonDto>(item));
        //        //}

        //        response.HasError = false;
        //        response.Message = _successMsg;
        //        response.Result = data;
        //    }
        //    catch (Exception e)
        //    {
        //        response.Message = e.Message;
        //        response.HasError = false;
        //    }


        //    return response.ToHttpResponse();
        //}


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



        [HttpGet("DeliveryPersonOrderTransactionDetails")]
        public IActionResult DeliveryPersonOrderTransactionDetails(int DeliveryPersonID, DateTime FromOrderDate, DateTime ToOrderDate)
        {
            DataTable dt = new DataTable();
            var response = new GenericResponse<DataTable>();

            try
            {
                if ((FromOrderDate.ToShortDateString() == "01/01/0001") || (FromOrderDate.ToShortDateString().Trim() == "1/1/0001"))
                {
                    FromOrderDate = Convert.ToDateTime("01/01/1800");
                }
                if ((ToOrderDate.ToShortDateString() == "01/01/0001") || (ToOrderDate.ToShortDateString().Trim() == "1/1/0001"))
                {
                    ToOrderDate = Convert.ToDateTime("01/01/1800");
                }
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prDeliveryPersonOrderTransactionDetails", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", DeliveryPersonID));
                        cmd.Parameters.Add(new SqlParameter("@FromOrderDate", FromOrderDate));
                        cmd.Parameters.Add(new SqlParameter("@ToOrderDate", ToOrderDate));
                        sql.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        sql.Close();
                        response.HasError = false;
                        response.Message = "Get Records Successfully";
                    }
                }
                response.HasError = false;
                response.Message = _successMsg;
                response.Result = dt;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }

            return response.ToHttpResponse();
        }


        [HttpGet("DeliveryPersonLedgerSummary")]
        public IActionResult DeliveryPersonLedgerSummary(int DeliveryPersonID, DateTime FromOrderDate, DateTime ToOrderDate)
        {
            DataTable dt = new DataTable();
            var response = new GenericResponse<DataTable>();
            try
            {
                if ((FromOrderDate.ToShortDateString() == "01/01/0001") || (FromOrderDate.ToShortDateString().Trim() == "1/1/0001"))
                {
                    FromOrderDate = Convert.ToDateTime("01/01/1800");
                }
                if ((ToOrderDate.ToShortDateString() == "01/01/0001") || (ToOrderDate.ToShortDateString().Trim() == "1/1/0001"))
                {
                    ToOrderDate = Convert.ToDateTime("01/01/1800");
                }
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prDeliveryPersonLedgerSummary", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", DeliveryPersonID));
                        cmd.Parameters.Add(new SqlParameter("@FromOrderDate", FromOrderDate));
                        cmd.Parameters.Add(new SqlParameter("@ToOrderDate", ToOrderDate));
                        sql.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        sql.Close();
                        response.HasError = false;
                        response.Message = "Get Records Successfully";
                    }
                }
                response.HasError = false;
                response.Message = _successMsg;
                response.Result = dt;

            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }


            return response.ToHttpResponse();
        }


        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(string phone, String password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<String>();
            try
            {
                var tblCustomers = _context.TblDeliveryPerson.Where(p => p.MobileNo == phone).FirstOrDefault();



                if (tblCustomers == null)
                {
                    response.HasError = true;
                    response.Message = "This number is not registered";
                }
                else
                {
                    tblCustomers.Password = password;
                    _context.TblDeliveryPerson.Update(tblCustomers);
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
                //var tblDeliveryBoy = _context.TblDeliveryPerson.Where(p => p.MobileNo == phone && PasswordHasher.VerifyPassword(pass,p.Password)).FirstOrDefault();
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
            GenericResponse<DeliveryPersonDto> response = new GenericResponse<DeliveryPersonDto>();
            var tblDeliveryPerson = await _context.TblDeliveryPerson.FindAsync(id);
            response.Result = _mapper.Map<DeliveryPersonDto>(tblDeliveryPerson);
            if (tblDeliveryPerson == null)
            {
                return NotFound();
            }

            return response.ToHttpResponse();
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
                if (TblDeliveryPersonExists(tblDeliveryPerson.DeliveryPersonID))
                {
             //        tblDeliveryPerson.Password = PasswordHasher.CreateHash(tblDeliveryPerson.Password);
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


        // PUT: api/DeliveryPersonKYCDetails/5
        [HttpPut ("UpdateDeliveryPersonKYCDetails")]
        public async Task<IActionResult> UpdateDeliveryPersonKYCDetails ([FromBody] DeliveryPersonDetailDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<DeliveryPersonDetailDto> response = new GenericResponse<DeliveryPersonDetailDto>();
            try
            {
                if (TblDeliveryPersonDetailExists(data.DeliveryPersonID))
                {

                    using (SqlConnection sql = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("prUpdateDeliveryPersonKYCInfo", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", data.DeliveryPersonID));
                            cmd.Parameters.Add(new SqlParameter("@AadhaarNo", data.AadhaarNo));
                            cmd.Parameters.Add(new SqlParameter("@AadhaarFrontImage", data.AadhaarFrontImage));
                            cmd.Parameters.Add(new SqlParameter("@AadhaarBackImage", data.AadhaarBackImage));
                            cmd.Parameters.Add(new SqlParameter("@PAN", data.Pan));
                            cmd.Parameters.Add(new SqlParameter("@PANImage", data.Panimage));
                            cmd.Parameters.Add(new SqlParameter("@DrivingLicenceNo", data.DrivingLicenceNo));
                            cmd.Parameters.Add(new SqlParameter("@DrivingLicenceFrontImage", data.DrivingLicenceFrontImage));
                            cmd.Parameters.Add(new SqlParameter("@DrivingLicenceBackImage", data.DrivingLicenceBackImage));
                            cmd.Parameters.Add(new SqlParameter("@VehicleNo", data.VehicleNo));
                            cmd.Parameters.Add(new SqlParameter("@VehicleDocumentImage", data.VehicleDocumentImage));
                            cmd.Parameters.Add(new SqlParameter("@VehicleFrontPhoto", data.VehicleFrontPhoto));
                            cmd.Parameters.Add(new SqlParameter("@VehicleBackPhoto", data.VehicleBackPhoto));
                            cmd.Parameters.Add(new SqlParameter("@VehicleInsuranceNo", data.VehicleInsuranceNo));
                            cmd.Parameters.Add(new SqlParameter("@VehicleInsuranceDocumentImage", data.VehicleInsuranceDocumentImage));
                            cmd.Parameters.Add(new SqlParameter("@AccountName", data.AccountName));
                            cmd.Parameters.Add(new SqlParameter("@AccountNo", data.AccountNo));
                            cmd.Parameters.Add(new SqlParameter("@BankName", data.BankName));
                            cmd.Parameters.Add(new SqlParameter("@IFSC", data.Ifsc));
                            cmd.Parameters.Add(new SqlParameter("@CanceledChequeImage", data.CanceledChequeImage));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            sql.Close();
                            var res = _context.TblDeliveryPersonDetails.Where(p => p.DeliveryPersonID == data.DeliveryPersonID).FirstOrDefault();
                            response.Result = _mapper.Map<DeliveryPersonDetailDto>(res);
                            response.Message = "Successfull";
                            response.HasError = false;
                        }
                    }
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


        // GET: api/DeliveryPersonKYCDetails
        [HttpGet("DeliveryPersonKYCDetails")]
        public IActionResult DeliveryPersonKYCDetails()
        {
            var response = new GenericResponse<List<DeliveryPersonDetailDto>>();
            try
            {
                var data = _context.TblDeliveryPersonDetails.ToList();
                var list = new List<DeliveryPersonDetailDto>();
                foreach (var item in data)
                {
                    list.Add(_mapper.Map<DeliveryPersonDetailDto>(item));
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


        // GET: api/DeliveryPersonKYCDetails/25
        [HttpGet("DeliveryPersonKYCDetailsByID")]
        
        public  IActionResult DeliveryPersonKYCDetailsByID( int id)
        {
            var response = new GenericResponse<DeliveryPersonDetailDto>();

            try
            {
                var data = _context.TblDeliveryPersonDetails.Where(p => p.DeliveryPersonID == id).First();

                response.HasError = false;
                response.Message = _successMsg;
                response.Result = _mapper.Map<DeliveryPersonDetailDto>(data);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }
            return response.ToHttpResponse();
        }


        // GET: api/DeliveryPersonKYCDetails/25
        [HttpGet("DeliveryPersonWalletByID")]

        public IActionResult DeliveryPersonWalletByID(int id)
        {
            var response = new GenericResponse<DeliveryPersonWalletDto>();

            try
            {
                var data = _context.TblDeliveryPersonWallet.Where(p => p.DeliveryPersonID == id).First();

                response.HasError = false;
                response.Message = _successMsg;
                response.Result = _mapper.Map<DeliveryPersonWalletDto>(data);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = false;
            }
            return response.ToHttpResponse();
        }


        [HttpPut("UpdateDeliveryPersonWallet")]
        public IActionResult UpdateDeliveryPersonWallet(int DeliveryPersonID, int Wallet )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new GenericResponse<String>();
            try
            {
                var tblDeliveryPersonWallet  = _context.TblDeliveryPersonWallet.Where(p => p.DeliveryPersonID == DeliveryPersonID).FirstOrDefault();
                if (tblDeliveryPersonWallet == null)
                {
                    response.HasError = true;
                    response.Message = "DeliveryPersonID is not valid";
                }
                else
                {
                    tblDeliveryPersonWallet.Wallet = Wallet;
                    _context.TblDeliveryPersonWallet.Update(tblDeliveryPersonWallet);
                    _context.SaveChanges();
                    response.HasError = false;
                    response.Message = _successMsg;
                    response.Result = "Wallet Successfully Updated";
                }
            }
            catch (Exception e)
            {
                response.HasError = true;
                response.Message = e.Message;
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
                    var user = _context.TblDeliveryPerson.Where((p) => p.DeliveryPersonID == deliveryBoyId).FirstOrDefault();
                    user.IsVerified = isVerified;
                    _context.Update(_mapper.Map<TblDeliveryPerson>(user));
                    _context.SaveChanges();
                    var res = _context.TblDeliveryPerson.Where(p => p.DeliveryPersonID == deliveryBoyId).FirstOrDefault();

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
                    using (SqlConnection sql = new SqlConnection(ConnectionString))
                    {
                      //  data.Password = PasswordHasher.CreateHash(data.Password);
                        using (SqlCommand cmd = new SqlCommand("prInsertDeliveryPerson", sql))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@PersonName", data.PersonName));
                            cmd.Parameters.Add(new SqlParameter("@MobileNo", data.MobileNo));
                            cmd.Parameters.Add(new SqlParameter("@AlternateNo", data.AlternateNo));
                            cmd.Parameters.Add(new SqlParameter("@Address", data.Address));
                            cmd.Parameters.Add(new SqlParameter("@City", data.City));
                            cmd.Parameters.Add(new SqlParameter("@Pincode", data.Pincode));
                            cmd.Parameters.Add(new SqlParameter("@EmailID", data.EmailID));
                            cmd.Parameters.Add(new SqlParameter("@Password", data.Password));
                            cmd.Parameters.Add(new SqlParameter("@FCMToken", data.Fcmtoken));
                            cmd.Parameters.Add(new SqlParameter("@ReferPromoCode", data.ReferPromoCode));
                            cmd.Parameters.Add(new SqlParameter("@ProfileImage", data.ProfileImage));

                            cmd.Parameters.Add(new SqlParameter("@AadhaarNo", data.AadhaarNo));
                            cmd.Parameters.Add(new SqlParameter("@AadhaarFrontImage", data.AadhaarFrontImage));
                            cmd.Parameters.Add(new SqlParameter("@AadhaarBackImage", data.AadhaarBackImage));
                            cmd.Parameters.Add(new SqlParameter("@PAN", data.Pan));
                            cmd.Parameters.Add(new SqlParameter("@PANImage", data.Panimage));
                            cmd.Parameters.Add(new SqlParameter("@DrivingLicenceNo", data.DrivingLicenceNo));
                            cmd.Parameters.Add(new SqlParameter("@DrivingLicenceFrontImage", data.DrivingLicenceFrontImage));
                            cmd.Parameters.Add(new SqlParameter("@DrivingLicenceBackImage", data.DrivingLicenceBackImage));
                            cmd.Parameters.Add(new SqlParameter("@VehicleNo", data.VehicleNo));
                            cmd.Parameters.Add(new SqlParameter("@VehicleDocumentImage", data.VehicleDocumentImage));
                            cmd.Parameters.Add(new SqlParameter("@VehicleFrontPhoto", data.VehicleFrontPhoto));
                            cmd.Parameters.Add(new SqlParameter("@VehicleBackPhoto", data.VehicleBackPhoto));
                            cmd.Parameters.Add(new SqlParameter("@VehicleInsuranceNo", data.VehicleInsuranceNo));
                            cmd.Parameters.Add(new SqlParameter("@VehicleInsuranceDocumentImage", data.VehicleInsuranceDocumentImage));
                            cmd.Parameters.Add(new SqlParameter("@AccountName", data.AccountName));
                            cmd.Parameters.Add(new SqlParameter("@AccountNo", data.AccountNo));
                            cmd.Parameters.Add(new SqlParameter("@BankName", data.BankName ));
                            cmd.Parameters.Add(new SqlParameter("@IFSC", data.Ifsc));
                            cmd.Parameters.Add(new SqlParameter("@CanceledChequeImage", data.CanceledChequeImage));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            var res = _context.TblDeliveryPerson.Where(p => p.MobileNo == data.MobileNo).FirstOrDefault();
                            responses.Result = _mapper.Map<DeliveryPersonDto>(res);
                            responses.Message = "Successfull";
                            responses.HasError = false;
                        }
                    }
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




        [HttpGet("IsValidPromocode")]
        public  IActionResult IsValidPromocode(string Promocode)
        {
         
            GenericResponse<bool> response  = new GenericResponse<bool>();
            try
            {
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {


                    using (SqlCommand cmd = new SqlCommand("prValidatePromocode", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@PromoCode", Promocode));
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        response.HasError = false;
                        response.Result = true;
                        response.Message = "Valid Promocode";
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
            return _context.TblDeliveryPerson.Any(e => e.DeliveryPersonID == id);
        }

        private bool TblDeliveryPersonExists(string mobile)
        {
            return _context.TblDeliveryPerson.Any(e => e.MobileNo == mobile);
        }
        private bool TblDeliveryPersonDetailExists(int id)
        {
            return _context.TblDeliveryPersonDetails.Any(e => e.DeliveryPersonID == id);
        }
    }
}