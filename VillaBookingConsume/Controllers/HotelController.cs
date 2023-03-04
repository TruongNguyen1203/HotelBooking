using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ultility;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto;
using VillaBookingConsume.Service.IService;

namespace VillaBookingConsume.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        private readonly IMapper _mapper;
        // GET
        public HotelController(IHotelService hotelService, IMapper mapper)
        {
            _hotelService = hotelService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = new List<HotelDto>();
            var response = await _hotelService.GetAllAsync<ApiResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<HotelDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(HotelCreateDto hotelCreateDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _hotelService.CreateAsync<ApiResponse>(hotelCreateDto, HttpContext.Session.GetString(Constant.Token));
                if (res != null && res.IsSuccess)
                {
                    TempData["success"] = "Create hotel successfully"; 
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] =  "Error encountered.";

            return View(hotelCreateDto);
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var res = await _hotelService.GetByIdAsync<ApiResponse>(id);
            if (res != null && res.IsSuccess)
            {
                var hotel = JsonConvert.DeserializeObject<HotelDto>(Convert.ToString(res.Result));
                return View(_mapper.Map<HotelUpdateDto>(hotel));
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(HotelUpdateDto updateDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _hotelService.UpdateAsync<ApiResponse>(updateDto, HttpContext.Session.GetString(Constant.Token));
                if (res != null && res.IsSuccess)
                {
                    TempData["success"] = "Update hotel successfully"; 
                    return RedirectToAction(nameof(Index));
                }
            }
            
            TempData["error"] =  "Error encountered.";

            return View(updateDto);
        }
    }
}