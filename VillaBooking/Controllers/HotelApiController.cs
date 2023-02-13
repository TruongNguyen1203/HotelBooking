using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VillaBooking.Data;
using VillaBooking.Models;
using VillaBooking.Models.Dto;

namespace VillaBooking.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelApiController : ControllerBase
    {
        private readonly ILogger<HotelApiController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HotelApiController(ILogger<HotelApiController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            _logger.LogInformation("Load all hotels");
            var hotels = await _context.Hotels.ToListAsync();
            return Ok( _mapper.Map<List<HotelDto>>(hotels));
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id", Name = "GetById")]
        public async Task<ActionResult<HotelDto>> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"Get error with id {id}");
                return BadRequest();
            }
            var result = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);

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
        public async Task<ActionResult<HotelDto>> CreateHotel([FromBody] HotelDto hotelDto)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            var data = await _context.Hotels.FirstOrDefaultAsync(x => x.Name.ToLower() == hotelDto.Name.ToLower());
            
            if (data != null)
            {
                ModelState.AddModelError("CustomError" , "Already exist");
                return BadRequest(ModelState);
            }
            if (hotelDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var hotel = _mapper.Map<Hotel>(hotelDto);
            hotel.CreatedDate = DateTime.Now;
            hotel.UpdatedDate = DateTime.Now;
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetById",new{id = hotelDto.Id},hotelDto);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var hotel = await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
    
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateHotel(int id, [FromBody] HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            var hotel = await _context.Hotels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel = _mapper.Map<Hotel>(hotelDto);
            hotel.UpdatedDate = DateTime.Now;
            
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpPatch("id")]
        public async Task<ActionResult> UpdatePartialHotel(int id, JsonPatchDocument<HotelDto>? hotelPatch)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var hotel = await _context.Hotels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDto = _mapper.Map<HotelDto>(hotel);
            
            hotelPatch.ApplyTo(hotelDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            hotel = _mapper.Map<Hotel>(hotelDto);

            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}