using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Unicareer.Controllers
{
    public class CaptchaController : Controller
    {
        // GET: Captcha/GetCaptchaImage
        [HttpGet]
        public IActionResult GetCaptchaImage()
        {
            // Tạo mã CAPTCHA ngẫu nhiên (4-6 ký tự)
            var random = new Random();
            var captchaText = GenerateRandomString(5);
            
            // Lưu mã CAPTCHA vào session
            HttpContext.Session.SetString("CaptchaCode", captchaText);

            // Tạo hình ảnh CAPTCHA
            using (var bitmap = new Bitmap(150, 50))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                // Cấu hình chất lượng đồ họa
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                // Nền trắng
                graphics.Clear(Color.White);

                // Vẽ các đường ngẫu nhiên để làm nhiễu
                var pen = new Pen(Color.LightGray, 1);
                for (int i = 0; i < 10; i++)
                {
                    var x1 = random.Next(0, 150);
                    var y1 = random.Next(0, 50);
                    var x2 = random.Next(0, 150);
                    var y2 = random.Next(0, 50);
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }

                // Vẽ các điểm ngẫu nhiên
                for (int i = 0; i < 50; i++)
                {
                    var x = random.Next(0, 150);
                    var y = random.Next(0, 50);
                    bitmap.SetPixel(x, y, Color.Gray);
                }

                // Vẽ text CAPTCHA
                var font = new Font("Arial", 20, FontStyle.Bold);
                var brush = new SolidBrush(Color.Black);
                
                // Vẽ text với một chút xoay và offset để khó đọc hơn
                for (int i = 0; i < captchaText.Length; i++)
                {
                    var charBrush = new SolidBrush(GetRandomColor());
                    var charFont = new Font("Arial", random.Next(18, 24), FontStyle.Bold);
                    var x = 20 + (i * 25) + random.Next(-3, 3);
                    var y = 15 + random.Next(-5, 5);
                    var angle = random.Next(-15, 15);
                    
                    graphics.TranslateTransform(x, y);
                    graphics.RotateTransform(angle);
                    graphics.DrawString(captchaText[i].ToString(), charFont, charBrush, 0, 0);
                    graphics.RotateTransform(-angle);
                    graphics.TranslateTransform(-x, -y);
                    
                    charBrush.Dispose();
                    charFont.Dispose();
                }

                font.Dispose();
                brush.Dispose();
                pen.Dispose();

                // Chuyển đổi sang mảng byte
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }

        // POST: Captcha/ValidateCaptcha
        [HttpPost]
        public IActionResult ValidateCaptcha([FromBody] string captchaCode)
        {
            var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            
            if (string.IsNullOrEmpty(sessionCaptcha) || 
                string.IsNullOrEmpty(captchaCode) ||
                !sessionCaptcha.Equals(captchaCode, StringComparison.OrdinalIgnoreCase))
            {
                return Json(new { valid = false });
            }

            // Xóa CAPTCHA sau khi validate thành công
            HttpContext.Session.Remove("CaptchaCode");
            return Json(new { valid = true });
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Loại bỏ các ký tự dễ nhầm lẫn
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private Color GetRandomColor()
        {
            var random = new Random();
            var colors = new[] { Color.Black, Color.DarkBlue, Color.DarkGreen, Color.DarkRed, Color.DarkMagenta };
            return colors[random.Next(colors.Length)];
        }
    }
}

