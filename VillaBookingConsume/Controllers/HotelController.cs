using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelCreateDto hotelCreateDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _hotelService.CreateAsync<ApiResponse>(hotelCreateDto);
                if (res != null && res.IsSuccess)
                {
                    TempData["success"] = "Create hotel successfully"; 
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] =  "Error encountered.";

            return View(hotelCreateDto);
        }
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
        public async Task<IActionResult> Update(HotelUpdateDto updateDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _hotelService.UpdateAsync<ApiResponse>(updateDto);
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