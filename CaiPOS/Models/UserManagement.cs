using System.ComponentModel.DataAnnotations;

namespace CaiPOS.Models
{
    public class UserManagement
    {
        [Key]
        public Guid UserId {  get; set; } = Guid.NewGuid();
        public Guid MemberId { get; set; } = Guid.NewGuid();

        [Required, StringLength(20)]
        public required string UserName { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required, StringLength(10, MinimumLength = 10), RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "電話格式錯誤，必須以09開頭，共10碼")]
        public required string Phone { get; set; }

        [Required, StringLength(20, MinimumLength = 6)]
        public required string Password { get; set; }

        public string? Email { get; set; }
    }
}
