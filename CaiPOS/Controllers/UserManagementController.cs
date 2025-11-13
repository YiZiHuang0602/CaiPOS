using CaiPOS.Data;
using CaiPOS.Models;
using CaiPOS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaiPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly CaiPOSContext _context;

        public UserManagementController(CaiPOSContext context)
        {
            _context = context;
        }

        [HttpGet("GetUserInfornation")]
        public async Task<List<UserManagementDto>> GetUserInfornation()
        {
            var userDatas = new List<UserManagementDto>();
            var userData = await _context.UserManagement.ToListAsync();
            foreach (var i in userData)
            {
                var user = new UserManagementDto
                {
                    UserName = i.UserName,
                    Gender = i.Gender,
                    Phone = i.Phone,
                    Email = i.Email
                };
                userDatas.Add(user);
            }
            return userDatas;
        }

        [HttpPost("AddUserInformation")]
        public async Task<ApiResponse> AddUserInformation(UserManagementDto userManagementDto)
        {
            var foundUser = await _context.UserManagement.FirstOrDefaultAsync(u => u.UserName == userManagementDto.UserName);
            var userData = new UserManagement
            {
                UserName = userManagementDto.UserName,
                Gender = userManagementDto.Gender,
                Phone = userManagementDto.Phone,
                Password = userManagementDto.Password ?? string.Empty,
                Email = userManagementDto.Email,
            };

            if (foundUser == null)
            {
                _context.UserManagement.Add(userData);
                await _context.SaveChangesAsync();
                return new ApiResponse { Success = true, Message = "使用者新增成功" };
            }
            else if (foundUser != null)
            {
                return new ApiResponse { Success = false, Message = "使用者名稱已被其他用戶使用" };
            }
            return new ApiResponse { Success = false, Message = "使用者新增失敗" };
        }
    }
}
