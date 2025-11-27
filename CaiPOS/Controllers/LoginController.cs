using CaiPOS.Data;
using CaiPOS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaiPOS.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly CaiPOSContext _context;
        private readonly IConfiguration _configuration;

        // 修正建構子，正確注入 IConfiguration
        public LoginController(ILogger<LoginController> logger, CaiPOSContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ApiResponse<object>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "帳號或密碼錯誤"
                };
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyForJwt_1234567890"));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["CaiPOS"],
                audience: _configuration["CaiPOSUsers"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "登入成功",
                Data = new { Token = tokenString , UserName = user.UserName}
            };
        }

        [HttpPost("Logout")]
        public ApiResponse<string> Logout()
        {
            return new ApiResponse<string>
            {
                Success = true,
                Message = "登出成功"
            };
        }
    }
}
