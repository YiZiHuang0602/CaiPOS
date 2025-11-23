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

        [HttpGet("GetAllUserInfornation")]
        public async Task<List<UserManagementDto>> GetAllUserInfornation()
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

        [HttpGet("SearchUserInformation")]
        public async Task<ApiResponse<UserManagementDto>> SearchUserInformation(string searchUser)
        {
            try
            {
                var foundUser = await _context.UserManagement.FirstOrDefaultAsync(u => u.UserName == searchUser);
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
                throw new Exception($"找不到{searchUser}使用者的資料");
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserManagementDto> { Success = false, Message = $"搜尋失敗: {ex.Message}" };
            }
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

        [HttpPatch("EditUserInformation")]
        public async Task<ApiResponse> EditUserInformation(string searchUser, [Bind("Username, Gender, Phone, Password")] UserManagementDto userManagementDto)
        {
            try
            {
                var foundUser = await _context.UserManagement.FirstOrDefaultAsync(u => u.UserName == searchUser);
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
                return new ApiResponse { Success = true, Message = "使用者資料更新成功" };
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
                var foundUser = await _context.UserManagement.FirstOrDefaultAsync(u => u.UserName == deleteUser);
                if(foundUser == null) return new ApiResponse { Success = false, Message = $"找不到{deleteUser}的資料" };
                _context.UserManagement.Remove(foundUser);
                await _context.SaveChangesAsync();
                return new ApiResponse { Success = true, Message = $"{deleteUser}的資料刪除成功" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, Message = $"刪除失敗: {ex.Message}" };
            }
        }
    }
}
