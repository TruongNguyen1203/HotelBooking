using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VillaBooking.Data;
using VillaBooking.Repository.IRepository;
using VillaBookingConsume.Models;
using VillaBookingConsume.Models.Dto.Authentication;

namespace VillaBooking.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<bool> IsUniqueUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            return user == null;
        }

        public async Task<LoginReponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Username == loginRequestDto.Username && x.Password == loginRequestDto.Password);
            if (user == null) return new LoginReponseDto()
            {
                Token = "",
                User = null
            };
            
            //if user was found generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDiscriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials =  new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDiscriptor);

            LoginReponseDto loginReponseDto = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            
            return loginReponseDto;
        }

        public async Task<LocalUser?> Register(RegistrationDto registrationDto)
        {
            var user = _mapper.Map <LocalUser>(registrationDto);

            _context.Users.Add(user);
            bool result = await _context.SaveChangesAsync() > 0;
            user.Password = "";
            return result ? user : null;
        }
    }
}