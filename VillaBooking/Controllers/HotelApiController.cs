using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VillaBooking.Data;
using VillaBooking.Models;
using VillaBooking.Models.Dto;

namespace VillaBooking.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<HotelDto>> GetHotels()
        {
            return Ok(HotelStore.HotelDtos);
        }

        [HttpGet("id")]
        public ActionResult<HotelDto> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var result = HotelStore.HotelDtos.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}