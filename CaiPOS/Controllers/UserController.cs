using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text;
using CaiPOS.Data;
using CaiPOS.Model;
using CaiPOS.ViewModel;
using System.Security.Cryptography;

namespace CaiPOS.Controllers
{
    public class UserController : ControllerBase
    {
        private DbConn _conn;
        private static string CreateUserNumber()
        {
            Random rnd = new Random();
            var s = new StringBuilder();
            s.Append("User-");
            for (int i = 0; i < 3; i++)
            {
                s.Append(rnd.Next(1, 9));
            }
            return s.ToString();
        }

        private static List<string> SHA256Password(string password)
        {
            List<string> result = new List<string>();
            using (var sha = SHA256.Create())
            {
                StringBuilder sb = new StringBuilder();
                Random rand = new Random();
                for (int i = 0;i < 6;i++)
                {
                    sb.Append((char)rand.Next(33, 47));
                }
                result.Add(sb.ToString());
                sb.Clear();
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(bytes);
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                result.Add(sb.ToString());
                return result;
            }
        }
        public UserController(DbConn conn)
        {
            _conn = conn;
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = new List<UserDto>();
            var userList = await _conn.Users.ToListAsync();
            foreach(var user in userList)
            {
                users.Add(new UserDto { Name = user.Name, PhoneNumber = user.PhoneNumber});
            }
            return users;
        }

        [HttpGet("GetUserByUserName")]
        public async Task<ApiResponse<UserDto>> GetUserByUserName(string userName)
        {
            try
            {
                var user = await _conn.Users.FirstOrDefaultAsync(u => u.Name == userName);
                return user != null ?
                    new ApiResponse<UserDto>
                    {
                        Success = true,
                        Message = "查詢成功",
                        Data = new UserDto { Name = user.Name, PhoneNumber = user.PhoneNumber }
                    }:throw new KeyNotFoundException($"找不到名為 {userName} 的使用者");
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("CreateUser")]
        public async Task<ApiResponse> CreateUser(UserDto uDto)
        {
            try
            {
                if(string.IsNullOrEmpty(uDto.Name) || string.IsNullOrEmpty(uDto.PhoneNumber) || string.IsNullOrEmpty(uDto.Password))
                {
                    throw new ArgumentNullException("姓名、電話、密碼欄位皆不可為空");
                }
                var passwords = SHA256Password(uDto.Password);
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserNumber = CreateUserNumber(),
                    Name = uDto.Name,
                    PhoneNumber = uDto.PhoneNumber,
                    Salt = passwords[0],
                    Password = passwords[1],
                };
                await _conn.Users.AddAsync(user);
                await _conn.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "新增成功"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<ApiResponse>DeleteUser(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException($"找不到名為 {userName} 的使用者");
                var user = await _conn.Users.FirstOrDefaultAsync(u => u.Name == userName);
                _conn.Users.Remove(user);
                await _conn.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "刪除成功"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPatch("EditUser")]
        public async Task<ApiResponse> EditUser(string userName, UserDto uDto)
        {
            try 
            {
                if (string.IsNullOrEmpty(uDto.Name)) throw new ArgumentNullException($"使用者姓名不可為空");
                var user = _conn.Users.FirstOrDefault(u => u.Name == userName);
                if (user == null) throw new KeyNotFoundException($"找不到名為 {userName} 的使用者");
                var passwords = SHA256Password(uDto.Password);
                user.Name = uDto.Name;
                user.PhoneNumber = uDto.PhoneNumber;
                user.Salt = passwords[0];
                user.Password = passwords[1];
                await _conn.SaveChangesAsync();
                return new ApiResponse
                {
                    Success = true,
                    Message = "修改成功"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
