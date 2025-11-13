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
                    Password = i.Password,
                    Email = i.Email
                };
                userDatas.Add(user);
            }
            return userDatas;
        }
    }
}
