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
    public class BaseConfigsController : ControllerBase
    {
        private readonly db_byhanddeliveryContext _context;

        public BaseConfigsController(db_byhanddeliveryContext context)
        {
            _context = context;
        }

        // GET: api/TblBaseConfigs
        [HttpGet("GetBaseConfig")]
        public IActionResult GetTblBaseConfig()
        {
            var response = new GenericResponse<TblBaseConfig>();

            try {

                response.StatusCode = 200;
                response.Message = "Sucessfully"; 

                response.Result = _context.TblBaseConfig.First();
                response.HasError = false;

            }
            catch(Exception e)
            {
                response.HasError = true;
                response.Message = e.InnerException.ToString();

            }
            return response.ToHttpResponse();
        }

        //// GET: api/TblBaseConfigs/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetTblBaseConfig([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblBaseConfig = await _context.TblBaseConfig.FindAsync(id);

        //    if (tblBaseConfig == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(tblBaseConfig);
        //}

        // PUT: api/TblBaseConfigs/5
        [HttpPut("UpdateBaseConfig")]
        public async Task<IActionResult> PutTblBaseConfig([FromBody] TblBaseConfig tblBaseConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = new GenericResponse<TblBaseConfig>();
           
            _context.Entry(tblBaseConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                response.Message = "Sucessfully Updated";
                response.HasError = false;
                response.Result = tblBaseConfig;

            }
            catch (Exception e)
            {

                response.Message = e.InnerException.ToString();
                response.HasError = true;

            }

            return response.ToHttpResponse();
        }

        // POST: api/TblBaseConfigs
        //[HttpPost]
        //public async Task<IActionResult> PostTblBaseConfig([FromBody] TblBaseConfig tblBaseConfig)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.TblBaseConfig.Add(tblBaseConfig);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTblBaseConfig", new { id = tblBaseConfig.Id }, tblBaseConfig);
        //}

        //// DELETE: api/TblBaseConfigs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblBaseConfig([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var tblBaseConfig = await _context.TblBaseConfig.FindAsync(id);
        //    if (tblBaseConfig == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblBaseConfig.Remove(tblBaseConfig);
        //    await _context.SaveChangesAsync();

        //    return Ok(tblBaseConfig);
        //}

        private bool TblBaseConfigExists(int id)
        {
            return _context.TblBaseConfig.Any(e => e.Id == id);
        }
    }
}