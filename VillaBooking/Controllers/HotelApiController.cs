using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VillaBooking.Data;
using VillaBooking.Models;
using VillaBooking.Models.Dto;
using VillaBooking.Repository.IRepository;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VillaBooking.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class HotelApiController : ControllerBase
    {
        private readonly ILogger<HotelApiController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        protected ApiResponse _response;

        public HotelApiController(ILogger<HotelApiController> logger, ApplicationDbContext context, IMapper mapper,
            IHotelRepository hotelRepository)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _response = new();
        }

        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GetHotels([FromQuery(Name = "filterOccuapancy")]int? ocupancy, [FromQuery] string? search
        , int pageSize = 1, int pageNumber = 1)
        {
            _logger.LogInformation("Load all hotels");
            IEnumerable<Hotel> hotels;
            try
            {
                if (ocupancy > 0)
                {
                     hotels = await _hotelRepository.GetAllAsync(x => x.Occupancy == ocupancy, pageSize: pageSize, pageNumber:pageNumber);

                }
                else
                {
                    hotels = await _hotelRepository.GetAllAsync(pageSize: pageSize, pageNumber:pageNumber);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    hotels = hotels.Where(x => x.Name.ToLower().Contains(search));
                }

                Pagination pagination = new() {PageSize = pageSize, PageNumber = pageNumber};
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination) );

                _response.Result = _mapper.Map<List<HotelDto>>(hotels);
                _response.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id", Name = "GetById")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Get error with id {id}");
                    _response.HttpStatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var result = await _hotelRepository.GetAsync(x => x.Id == id);

                if (result == null)
                {
                    _response.HttpStatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;

                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<HotelDto>(result);
                _response.HttpStatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateHotel([FromBody] HotelCreateDto hotelDto)
        {
            try
            {
                var data = await _hotelRepository.GetAsync(x => x.Name.ToLower() == hotelDto.Name.ToLower());

                if (data != null)
                {
                    ModelState.AddModelError("CustomError", "Already exist");
                    return BadRequest(ModelState);
                }

                var hotel = _mapper.Map<Hotel>(hotelDto);
                hotel.CreatedDate = DateTime.Now;
                hotel.UpdatedDate = DateTime.Now;
                await _hotelRepository.CreateAsync(hotel);

                _response.HttpStatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<HotelDto>(hotel);
                return CreatedAtRoute("GetById", new {id = hotel.Id}, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return _response;
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var hotel = await _hotelRepository.GetAsync(x => x.Id == id, false);

                if (hotel == null)
                {
                    return NotFound();
                }

                await _hotelRepository.RemoveAsync(hotel);
                _response.HttpStatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return _response;
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateHotel(int id, [FromBody] HotelUpdateDto hotelDto)
        {
            try
            {
                if (id != hotelDto.Id)
                {
                    return BadRequest();
                }

                var hotel = await _hotelRepository.GetAsync(x => x.Id == id, false);
                if (hotel == null)
                {
                    return NotFound();
                }

                hotel = _mapper.Map<Hotel>(hotelDto);
                hotel.UpdatedDate = DateTime.Now;

                await _hotelRepository.UpdateAsync(hotel);
                _response.HttpStatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return _response;
        }

        [HttpPatch("id")]
        public async Task<ActionResult> UpdatePartialHotel(int id, JsonPatchDocument<HotelUpdateDto>? hotelPatch)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var hotel = await _hotelRepository.GetAsync(x => x.Id == id, false);
            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDto = _mapper.Map<HotelUpdateDto>(hotel);

            hotelPatch.ApplyTo(hotelDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            hotel = _mapper.Map<Hotel>(hotelDto);

            await _hotelRepository.UpdateAsync(hotel);

            return NoContent();
        }
    }
}