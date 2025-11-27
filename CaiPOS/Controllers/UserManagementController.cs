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

        private int createUserNumber()
        {
            var lastUser = _context.Users.OrderByDescending(u => u.UserNumber).FirstOrDefault();
            return lastUser != null ? lastUser.UserNumber + 1 : 1;
        }

        [HttpGet("GetAllUserInfornation")]
        public async Task<List<UserManagementDto>> GetAllUserInfornation()
        {
            var userDatas = new List<UserManagementDto>();
            var userData = await _context.Users.ToListAsync();
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

        [HttpGet("SearchUserInformation")]
        public async Task<ApiResponse<UserManagementDto>> SearchUserInformation(string searchUser)
        {
            try
            {
                var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == searchUser);
                if (foundUser != null)
                {
                    return new ApiResponse<UserManagementDto>
                    {
                        Success = true,
                        Message = "使用者資料搜尋成功",
                        Data = new UserManagementDto
                        {
                            UserName = foundUser.UserName,
                            Gender = foundUser.Gender,
                            Phone = foundUser.Phone,
                            Email = foundUser.Email
                        }
                    };
                }
                throw new Exception($"找不到「{searchUser}」使用者的資料");
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserManagementDto> { Success = false, Message = $"搜尋失敗: {ex.Message}" };
            }
        }

        [HttpPost("AddUserInformation")]
        public async Task<ApiResponse> AddUserInformation([FromBody] UserManagementDto userManagementDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return new ApiResponse
                {
                    Success = false,
                    Message = string.Join(";", errors)
                };
            }
            var exists = await _context.Users.AnyAsync(u => u.UserName == userManagementDto.UserName);
            if (exists)
            {
                return new ApiResponse { Success = false, Message = "使用者名稱已被其他用戶使用" };
            }
            var emailExists = await _context.Users.AnyAsync(u => u.Email == userManagementDto.Email);
            if (emailExists)
            {
                return new ApiResponse { Success = false, Message = "電子郵件已被其他用戶使用" };
            }
            var userData = new UserManagement
            {
                UserId = Guid.NewGuid(),
                UserNumber = createUserNumber(),
                UserName = userManagementDto.UserName,
                Gender = userManagementDto.Gender,
                Phone = userManagementDto.Phone,
                Password = userManagementDto.Password ?? string.Empty,
                Email = userManagementDto.Email,
            };

            _context.Users.Add(userData);
            await _context.SaveChangesAsync();

            return new ApiResponse { Success = true, Message = "使用者註冊成功" };
        }

        [HttpPatch("EditUserInformation")]
        public async Task<ApiResponse> EditUserInformation(string searchUser, [Bind("Username, Gender, Phone, Password")] UserManagementDto userManagementDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return new ApiResponse
                {
                    Success = false,
                    Message = string.Join(";", errors)
                };
            }

            try
            {
                var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == searchUser);
                if (foundUser == null) return new ApiResponse { Success = false, Message = $"找不到{searchUser}的資料" };
                if (ModelState.IsValid)
                {
                    foundUser.UserName = userManagementDto.UserName;
                    foundUser.Gender = userManagementDto.Gender;
                    foundUser.Phone = userManagementDto.Phone;
                    
                    if (!string.IsNullOrEmpty(foundUser.Password))
                    {
                        foundUser.Password = userManagementDto.Password;
                    }
                }

                await _context.SaveChangesAsync();
                return new ApiResponse { Success = true, Message = "{searchUser}使用者資料更新成功" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, Message = $"更新失敗: {ex.Message}" };
            }
        }

        [HttpDelete("DeleteUserInformation")]
        public async Task<ApiResponse> DeleteUserInformation(string deleteUser)
        {
            try
            {
                var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == deleteUser);
                if(foundUser == null) return new ApiResponse { Success = false, Message = $"找不到{deleteUser}的資料" };
                _context.Users.Remove(foundUser);
                await _context.SaveChangesAsync();
                return new ApiResponse { Success = true, Message = $"{deleteUser}的資料成功刪除" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, Message = $"刪除失敗: {ex.Message}" };
            }
        }
    }
}
