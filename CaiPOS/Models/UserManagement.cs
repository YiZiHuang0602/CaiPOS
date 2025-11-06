using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class UserManagement
    {
        public Guid UserId {  get; set; }
        public int MemberId { get; set; }

        [Required, StringLength(20)]
        public required string UserName { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required, StringLength(10, MinimumLength = 10), RegularExpression(@"^09[0-9]{8}$")]
        public required string Phone { get; set; }

        [Required, StringLength(20, MinimumLength = 6)]
        public required string Password { get; set; }

        public string? Email { get; set; }
    }
}
