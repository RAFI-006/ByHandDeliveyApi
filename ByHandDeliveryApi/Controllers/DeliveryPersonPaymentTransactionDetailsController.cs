using System;
using System.Data;
using AutoMapper;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByHandDeliveryApi.Models;
using ByHandDeliveryApi.GenericResponses;
using ByHandDeliveryApi.DTO;
using AutoMapper;

namespace ByHandDeliveryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPersonPaymentTransactionDetailsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly string _successMsg = "Sucessfully Completed";
        private readonly string ConnectionString = Startup.ConnectionString;
        public DeliveryPersonPaymentTransactionDetailsController(db_byhanddeliveryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DeliveryPersonPaymentTransactionDetails
        [HttpGet]
        public IActionResult GetTblDeliveryPersonPaymentTransactionDetails()
        {

            var response = new GenericResponse<List<DeliveryPersonPaymentTransactionDetailsDTO>>();

            try
            {
                var data = _context.TblDeliveryPersonPaymentTransactionDetails.ToList();
                var list = new List<DeliveryPersonPaymentTransactionDetailsDTO>();
                foreach (var item in data)
                {
                    list.Add(_mapper.Map<DeliveryPersonPaymentTransactionDetailsDTO>(item));
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




        [HttpGet("DeliveryPersonPaymentTransactionDetails")]
        public IActionResult DeliveryPersonPaymentTransactionDetails (int DeliveryPersonID, DateTime FromTransactionDate, DateTime ToTransactionDate)
        {

            DataTable dt = new DataTable();
            var response = new GenericResponse<DataTable>();

            try
            {
                if ((FromTransactionDate.ToShortDateString() == "01/01/0001") || (FromTransactionDate.ToShortDateString().Trim() == "1/1/0001"))
                {
                    FromTransactionDate = Convert.ToDateTime("01/01/1800");
                }
                if ((ToTransactionDate.ToShortDateString() == "01/01/0001") || (ToTransactionDate.ToShortDateString().Trim() == "1/1/0001"))
                {
                    ToTransactionDate = Convert.ToDateTime("01/01/1800");
                }
                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prDeliveryPersonPaymentTransactionDetails", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", DeliveryPersonID));
                        cmd.Parameters.Add(new SqlParameter("@FromTransactionDate", FromTransactionDate));
                        cmd.Parameters.Add(new SqlParameter("@ToTransactionDate", ToTransactionDate));
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
        public  IActionResult PostTblDeliveryPersonPaymentTransactionDetails([FromBody] DeliveryPersonPaymentTransactionDetailsDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<string> response  = new GenericResponse<string>();
            try
            {
                //var mapppedData = _mapper.Map<TblDeliveryPersonPaymentTransactionDetails>(data);
                //_context.Add(mapppedData);
                //_context.SaveChanges();
                //var res = _context.TblDeliveryPersonPaymentTransactionDetails.Where(p => p.DeliveryPersonAccountDetailID == data.DeliveryPersonAccountDetailID).FirstOrDefault();

                using (SqlConnection sql = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("prInsertDeliveryPersonPaymentTransaction", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DeliveryPersonID", data.DeliveryPersonID));
                        cmd.Parameters.Add(new SqlParameter("@Amount", data.Amount));
                        cmd.Parameters.Add(new SqlParameter("@PaymentType", data.PaymentType));
                        cmd.Parameters.Add(new SqlParameter("@CrDr", data.CrDr));
                        cmd.Parameters.Add(new SqlParameter("@Comment", data.Comment ));
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        response.Result = "Created Successfully";
                        response.Message = "Successfull";
                        response.HasError = false;
                    }
                }
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.HasError = true;
            }
            return response.ToHttpResponse();
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