using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaBooking.Models;
using VillaBooking.Models.Dto;
using VillaBooking.Repository.IRepository;

namespace VillaBooking.Controllers
{
    [ApiController]
    [Route("api/areas")]
    public class AreaApiController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        protected new ApiResponse Response;

        public AreaApiController(IAreaRepository areaRepository, IMapper mapper, IHotelRepository hotelRepository)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            Response = new();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAreas()
        {
            try
            {
                var areas = await _areaRepository.GetAllAsync();

                Response.Result = _mapper.Map<List<AreaDto>>(areas);
                Response.HttpStatusCode = HttpStatusCode.OK;

                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return Response;

        }

        [HttpGet("id", Name = "GetArea")]
        public async Task<ActionResult<ApiResponse>> GetArea(int id)
        {
            try
            {
                var area = await _areaRepository.GetAsync(x => x.Id == id);

                if (area == null)
                {
                    Response.HttpStatusCode = HttpStatusCode.NotFound;
                    Response.IsSuccess = false;
                    return NotFound(Response);
                }

                Response.Result = _mapper.Map<AreaDto>(area);
                Response.HttpStatusCode = HttpStatusCode.OK;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return Response;
            
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] AreaDto areaDto)
        {
            if (areaDto == null)
            {
                return BadRequest(areaDto);
            }
            try
            {
                if ((await _areaRepository.GetAsync(x => x.Id == areaDto.Id)) != null)
                {
                    ModelState.AddModelError("Custom Error", "Area already exist!");
                    return BadRequest(ModelState);
                }

                if ((await _hotelRepository.GetAsync(x => x.Id == areaDto.HotelId)) == null)
                {
                    ModelState.AddModelError("Custom Error", "Invalid hotel id!");
                    return BadRequest(ModelState);
                }

                var area = _mapper.Map<Area>(areaDto);
                await _areaRepository.CreateAsync(area);
                Response.HttpStatusCode = HttpStatusCode.Created;
                Response.Result = area;
                return CreatedAtRoute("GetArea", new {id = area.Id}, Response);
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return Response;
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                var area = await _areaRepository.GetAsync(x => x.Id == id);
                if (area == null)
                {
                    return NotFound();
                }

                await _areaRepository.RemoveAsync(area);
                Response.HttpStatusCode = HttpStatusCode.NoContent;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return Response;
        }
        
        [HttpPut("id")]
        public async Task<ActionResult<ApiResponse>> Update([FromBody] AreaDto areaDto)
        {
            if (areaDto == null)
            {
                return BadRequest(areaDto);
            }
            try
            {
                if ((await _hotelRepository.GetAsync(x => x.Id == areaDto.HotelId)) == null)
                {
                    ModelState.AddModelError("Custom Error", "Invalid hotel id!");
                    return BadRequest(ModelState);
                }

                var area = _mapper.Map<Area>(areaDto);
                await _areaRepository.Update(area);
                Response.HttpStatusCode = HttpStatusCode.NoContent;
                Response.Result = area;
                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.ErrorMessages = new List<string>() {ex.ToString()};
            }

            return Response;
        }
    }
}