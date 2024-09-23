using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class UserServices : IUserService
    {
        private readonly ProjectbootcampContext _context;
        private readonly IConfiguration _configuration;
        public UserServices(ProjectbootcampContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ResLoginUserDto> Login(ReqLoginUserDto reqLoginUser)
        {
            var user = await _context.MstUsers.SingleOrDefaultAsync(u => u.Email == reqLoginUser.Email);
            if (user == null) 
            {
                throw new Exception("Invalid email or password");
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(reqLoginUser.Password, user.Password);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password");
            }

            var token = GenerateJwtToken(user);

            var loginResponse = new ResLoginUserDto
            {
                Token = token,
            };

            return loginResponse;
        }

        private string GenerateJwtToken(MstUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Name, user.Nama),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> Register(ReqRegisterUserDto reqRegisterUser)
        {
            var isAnyEmail = await _context.MstUsers.SingleOrDefaultAsync(u => u.Email == reqRegisterUser.Email);
            if (isAnyEmail != null) 
            {
                throw new Exception("Email already used");
            }

            var newUser = new MstUser
            {
                Nama = reqRegisterUser.Name,
                Email = reqRegisterUser.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(reqRegisterUser.Password),
                Role = reqRegisterUser.Role,
                Balance = reqRegisterUser.Balance,
            };

            await _context.MstUsers.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser.Email;
        }
    }
}
