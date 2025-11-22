using System.ComponentModel.DataAnnotations;

namespace Unicareer.Models.ViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "Mã xác thực là bắt buộc")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác thực phải có 6 chữ số")]
        [Display(Name = "Mã xác thực")]
        public string VerificationCode { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}

