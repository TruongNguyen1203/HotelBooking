using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ultility;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto;
using VillaBookingConsume.Service.IService;

namespace VillaBookingConsume.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ApiResponse apiResponse = await _authService.LoginAsync<ApiResponse>(loginRequestDto);
            if (apiResponse != null && apiResponse.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(apiResponse.Result));
                var idenity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                idenity.AddClaim(new Claim(ClaimTypes.Name, model.User.Username));
                idenity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));
                var princial = new ClaimsPrincipal(idenity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princial); 
                HttpContext.Session.SetString(Constant.Token, model.Token);
                
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", apiResponse.ErrorMessages.FirstOrDefault());
                return View(loginRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            ApiResponse apiResponse = await _authService.RegisterAsync<ApiResponse>(registrationDto);

            if (apiResponse != null && apiResponse.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(Constant.Token, "");
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}