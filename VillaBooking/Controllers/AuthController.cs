using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VillaBooking.Models;
using VillaBooking.Repository.IRepository;
using VillaBookingConsume.Models.Dto.Authentication;

namespace VillaBooking.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected ApiResponse _apiResponse;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _apiResponse = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var login = await _userRepository.Login(loginRequestDto);
            if (login.User == null || string.IsNullOrEmpty(login.Token))
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Username or password is incorrect.");
                return BadRequest(_apiResponse);
            }
            
            _apiResponse.HttpStatusCode = HttpStatusCode.OK;
            _apiResponse.Result = login;
            return Ok(_apiResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegistrationDto registrationDto)
        {
            var isUniqueUser = await _userRepository.IsUniqueUser(registrationDto.Username);

            if (!isUniqueUser)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Username already exists");
                return BadRequest(_apiResponse);
            }

            var user = await _userRepository.Register(registrationDto);
            if (user == null)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Error while registering user");
                return BadRequest(_apiResponse);
            }

            _apiResponse.HttpStatusCode = HttpStatusCode.OK;
            _apiResponse.Result = user;
            return _apiResponse;
        }
    }
}