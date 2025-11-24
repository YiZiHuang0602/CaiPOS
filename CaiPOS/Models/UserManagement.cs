using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class UserManagement
    {
        [Key]
        public Guid UserId {  get; set; } = Guid.NewGuid();
        public Guid UserNumber { get; set; } = Guid.NewGuid();

        public string UserName { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string? Email { get; set; }
    }
}
