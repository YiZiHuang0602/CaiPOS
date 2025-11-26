using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class UserManagement
    {
        [Key]
        public Guid UserId {  get; set; } = Guid.NewGuid();

        public int UserNumber { get; set; } = 1;

        public string UserName { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string? Email { get; set; }
    }
}
