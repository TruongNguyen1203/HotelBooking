using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<IEnumerable<HotelDto>> GetHotels()
        {
            return Ok(HotelStore.HotelDtos);
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id", Name = "GetById")]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<HotelDto> CreateHotel([FromBody] HotelDto hotelDto)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            if (HotelStore.HotelDtos.FirstOrDefault(x => x.Name.ToLower() == hotelDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError" , "Already exist");
                return BadRequest(ModelState);
            }
            if (hotelDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            hotelDto.Id = HotelStore.HotelDtos.OrderByDescending(x => x.Id).First().Id + 1;
            HotelStore.HotelDtos.Add(hotelDto);

            return CreatedAtRoute("GetById",new{id = hotelDto.Id},hotelDto);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var hotel = HotelStore.HotelDtos.FirstOrDefault(x => x.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            HotelStore.HotelDtos.Remove(hotel);

            return NoContent();
        }

        [HttpPut]
        public ActionResult UpdateHotel(int id, [FromBody] HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            var hotel = HotelStore.HotelDtos.FirstOrDefault(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            hotel.Name = hotelDto.Name;
            hotel.Sqft = hotelDto.Sqft;
            hotel.Ocupation = hotelDto.Ocupation;

            return NoContent();
        }

        [HttpPatch("id")]
        public ActionResult UpdatePartialHotel(int id, JsonPatchDocument<HotelDto>? hotelDto)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var hotel = HotelStore.HotelDtos.FirstOrDefault(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            
            hotelDto.ApplyTo(hotel, ModelState);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}