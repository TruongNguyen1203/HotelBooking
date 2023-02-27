using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto;
using VillaBookingConsume.Service.IService;

namespace VillaBookingConsume.Controllers
{
    public class Hotel : Controller
    {
        private readonly IHotelService _hotelService;
        // GET
        public Hotel(IHotelService hotelService)
        {
            _hotelService = hotelService;
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
    }
}