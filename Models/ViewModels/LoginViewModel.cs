using System.ComponentModel.DataAnnotations;

namespace Unicareer.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool GhiNho { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã CAPTCHA")]
        [Display(Name = "Mã CAPTCHA")]
        public string CaptchaCode { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}

