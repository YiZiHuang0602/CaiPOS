using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CaiPOS.ViewModel
{
    public class UserManagementDto
    {
        [Required(ErrorMessage = "使用者名稱為必填欄位")]
        [StringLength(20, ErrorMessage = "使用者名稱不能超過 20 個字元")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "性別為必填欄位")]
        [RegularExpression(@"^(男|女)$", ErrorMessage = "性別只能填「男」或「女」")]
        public required string Gender { get; set; }

        [Required, RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "電話格式錯誤，必須以09開頭，共10碼")]
        public required string Phone { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessage ="密碼需介於 8-20 字元")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S+$",
            ErrorMessage = "密碼需包含大寫、小寫、數字，且不得包含空白")]
        public string? Password { get; set; }

        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Email 不可包含空白")]
        public string? Email { get; set; }
    }
}
