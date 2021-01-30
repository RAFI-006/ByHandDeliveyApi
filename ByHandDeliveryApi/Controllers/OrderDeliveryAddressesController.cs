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
    public class OrderDeliveryAddressesController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;
        private readonly IMapper _mapper;

        public OrderDeliveryAddressesController(db_byhanddeliveryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/OrderDeliveryAddresses
        [HttpGet]
        public IEnumerable<TblOrderDeliveryAddress> GetTblOrderDeliveryAddress()
        {
            return _context.TblOrderDeliveryAddress;
        }

        // GET: api/OrderDeliveryAddresses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblOrderDeliveryAddress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblOrderDeliveryAddress = await _context.TblOrderDeliveryAddress.FindAsync(id);

            if (tblOrderDeliveryAddress == null)
            {
                return NotFound();
            }

            return Ok(tblOrderDeliveryAddress);
        }

        //[HttpGet("GetMasterWeight")]
        //public async Task<IActionResult> GetMasterWeight()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

         

        //    if (tblOrderDeliveryAddress == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tblOrderDeliveryAddress);
        //}


        // PUT: api/OrderDeliveryAddresses/5
        [HttpPut]
        public IActionResult PutTblOrderDeliveryAddress([FromBody] OrderDeliveryAddDto tblOrderDeliveryAddress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericResponse<OrderDeliveryAddDto> response = new GenericResponse<OrderDeliveryAddDto>();
            try
            {
                if (TblOrderDeliveryAddressExists(tblOrderDeliveryAddress.OrderDeliveryAddressId))
                {
                    _context.Update(_mapper.Map<TblOrderDeliveryAddress>(tblOrderDeliveryAddress));
                    _context.SaveChanges();
                    var res = _context.TblOrderDeliveryAddress.Where(p => p.OrderDeliveryAddressId == tblOrderDeliveryAddress.OrderDeliveryAddressId).FirstOrDefault();
                    response.Result = _mapper.Map<OrderDeliveryAddDto>(res);
                    response.Message = "Successfull";
                    response.HasError = false;



                }
                else
                {
                    response.Message = "Address not found";
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

        // POST: api/OrderDeliveryAddresses
        [HttpPost]
        public IActionResult PostTblOrderDeliveryAddress([FromBody] OrderDeliveryAddDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            GenericResponse<string> responses = new GenericResponse<string>();
            try
            {
             
                    var mapppedData = _mapper.Map<TblOrderDeliveryAddress>(data);
                    _context.Add(mapppedData);
                    _context.SaveChanges();
                    var res = _context.TblOrderDeliveryAddress.Where(p => p.OrderDeliveryAddressId == data.OrderDeliveryAddressId).FirstOrDefault();
                    

                    responses.Result = "Created Successfully";
                    responses.Message = "Successfull";
                    responses.HasError = false;
  
            }
            catch (Exception e)
            {
                responses.Message = e.Message;
                responses.HasError = true;
            }

            return responses.ToHttpResponse();
        }

        // DELETE: api/OrderDeliveryAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblOrderDeliveryAddress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tblOrderDeliveryAddress = await _context.TblOrderDeliveryAddress.FindAsync(id);
            if (tblOrderDeliveryAddress == null)
            {
                return NotFound();
            }

            _context.TblOrderDeliveryAddress.Remove(tblOrderDeliveryAddress);
            await _context.SaveChangesAsync();

            return Ok(tblOrderDeliveryAddress);
        }

        private bool TblOrderDeliveryAddressExists(int id)
        {
            return _context.TblOrderDeliveryAddress.Any(e => e.OrderDeliveryAddressId == id);
        }
    }
}