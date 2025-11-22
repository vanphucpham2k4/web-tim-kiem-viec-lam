using System.Net;
using System.Net.Mail;

namespace Unicareer.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendVerificationCodeAsync(string toEmail, string verificationCode);
        
        // Thông báo cho nhà tuyển dụng
        Task<bool> SendNewApplicationNotificationAsync(string recruiterEmail, string candidateName, string jobTitle, string companyName);
        Task<bool> SendJobExpiringNotificationAsync(string recruiterEmail, string jobTitle, int daysLeft, DateTime expiryDate);
        Task<bool> SendJobExpiredNotificationAsync(string recruiterEmail, string jobTitle, DateTime expiryDate);
        Task<bool> SendPendingReviewNotificationAsync(string recruiterEmail, string candidateName, string jobTitle, int daysWaiting);
        
        // Thông báo cho ứng viên
        Task<bool> SendApplicationStatusChangedNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string status, string? note = null, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null);
        Task<bool> SendInterviewScheduledNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null);
        Task<bool> SendApplicationAcceptedNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null);
        Task<bool> SendApplicationRejectedNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string? reason = null, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null);
        Task<bool> SendSavedJobExpiringNotificationAsync(string candidateEmail, string candidateName, string jobTitle, int daysLeft, DateTime expiryDate);
        Task<bool> SendSavedJobExpiredNotificationAsync(string candidateEmail, string candidateName, string jobTitle, DateTime expiryDate);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["Email:SmtpUsername"];
                var smtpPassword = _configuration["Email:SmtpPassword"];
                var fromEmail = _configuration["Email:FromEmail"] ?? smtpUsername;
                var fromName = _configuration["Email:FromName"] ?? "Unicareer";

                _logger.LogInformation("Email configuration - Server: {Server}, Port: {Port}, Username: {Username}, FromEmail: {FromEmail}", 
                    smtpServer, smtpPort, smtpUsername, fromEmail);

                if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    _logger.LogError("Email configuration is missing. SmtpUsername or SmtpPassword is empty.");
                    return false;
                }

                _logger.LogInformation("Attempting to connect to SMTP server: {Server}:{Port}", smtpServer, smtpPort);

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.Timeout = 30000; // 30 seconds timeout

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(fromEmail, fromName);
                        message.To.Add(toEmail);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        _logger.LogInformation("Sending email to {ToEmail} from {FromEmail}", toEmail, fromEmail);
                        await client.SendMailAsync(message);
                        _logger.LogInformation("Email sent successfully to {Email}", toEmail);
                    }
                }

                return true;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "SMTP error sending email to {Email}. StatusCode: {StatusCode}, Message: {Message}", 
                    toEmail, smtpEx.StatusCode, smtpEx.Message);
                if (smtpEx.InnerException != null)
                {
                    _logger.LogError("SMTP Inner exception: {InnerMessage}", smtpEx.InnerException.Message);
                }
                return false;
            }
            catch (System.Net.Sockets.SocketException socketEx)
            {
                _logger.LogError(socketEx, "Network error sending email to {Email}. Message: {Message}", toEmail, socketEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}. Exception Type: {Type}, Message: {Message}", 
                    toEmail, ex.GetType().Name, ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {InnerMessage}", ex.InnerException.Message);
                }
                return false;
            }
        }

        public async Task<bool> SendVerificationCodeAsync(string toEmail, string verificationCode)
        {
            var subject = "Mã xác thực đặt lại mật khẩu - Unicareer";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .code-box {{ background: white; border: 2px solid #10b981; border-radius: 8px; padding: 20px; text-align: center; margin: 20px 0; }}
                        .code {{ font-size: 32px; font-weight: bold; color: #10b981; letter-spacing: 5px; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                        .warning {{ background: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Đặt lại mật khẩu</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào,</p>
                            <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản của mình. Vui lòng sử dụng mã xác thực sau:</p>
                            
                            <div class='code-box'>
                                <div class='code'>{verificationCode}</div>
                            </div>
                            
                            <div class='warning'>
                                <strong>⚠️ Lưu ý:</strong> Mã xác thực này có hiệu lực trong 10 phút. Không chia sẻ mã này với bất kỳ ai.
                            </div>
                            
                            <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(toEmail, subject, body);
        }

        // ========== THÔNG BÁO CHO NHÀ TUYỂN DỤNG ==========

        public async Task<bool> SendNewApplicationNotificationAsync(string recruiterEmail, string candidateName, string jobTitle, string companyName)
        {
            var subject = $"Đơn ứng tuyển mới - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: white; border-left: 4px solid #3b82f6; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                        .btn {{ display: inline-block; padding: 12px 24px; background: #3b82f6; color: white; text-decoration: none; border-radius: 5px; margin-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Đơn ứng tuyển mới</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào,</p>
                            <p>Bạn có một đơn ứng tuyển mới cho tin tuyển dụng của mình:</p>
                            
                            <div class='info-box'>
                                <p><strong>Ứng viên:</strong> {candidateName}</p>
                                <p><strong>Vị trí:</strong> {jobTitle}</p>
                                <p><strong>Công ty:</strong> {companyName}</p>
                            </div>
                            
                            <p>Vui lòng đăng nhập vào hệ thống để xem chi tiết và xử lý đơn ứng tuyển này.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(recruiterEmail, subject, body);
        }

        public async Task<bool> SendJobExpiringNotificationAsync(string recruiterEmail, string jobTitle, int daysLeft, DateTime expiryDate)
        {
            var subject = $"Tin tuyển dụng sắp hết hạn - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .warning-box {{ background: #fff3cd; border-left: 4px solid #f59e0b; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Cảnh báo hết hạn</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào,</p>
                            <p>Tin tuyển dụng của bạn sắp hết hạn:</p>
                            
                            <div class='warning-box'>
                                <p><strong>Tin tuyển dụng:</strong> {jobTitle}</p>
                                <p><strong>Còn lại:</strong> {daysLeft} ngày</p>
                                <p><strong>Ngày hết hạn:</strong> {expiryDate:dd/MM/yyyy}</p>
                            </div>
                            
                            <p>Vui lòng đăng nhập vào hệ thống để gia hạn hoặc cập nhật tin tuyển dụng nếu cần.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(recruiterEmail, subject, body);
        }

        public async Task<bool> SendJobExpiredNotificationAsync(string recruiterEmail, string jobTitle, DateTime expiryDate)
        {
            var subject = $"Tin tuyển dụng đã hết hạn - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .alert-box {{ background: #fee2e2; border-left: 4px solid #ef4444; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Tin tuyển dụng đã hết hạn</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào,</p>
                            <p>Tin tuyển dụng của bạn đã hết hạn:</p>
                            
                            <div class='alert-box'>
                                <p><strong>Tin tuyển dụng:</strong> {jobTitle}</p>
                                <p><strong>Ngày hết hạn:</strong> {expiryDate:dd/MM/yyyy}</p>
                            </div>
                            
                            <p>Vui lòng đăng nhập vào hệ thống để cập nhật trạng thái tin tuyển dụng.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(recruiterEmail, subject, body);
        }

        public async Task<bool> SendPendingReviewNotificationAsync(string recruiterEmail, string candidateName, string jobTitle, int daysWaiting)
        {
            var subject = $"Đơn ứng tuyển chờ xem xét - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #06b6d4 0%, #0891b2 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: white; border-left: 4px solid #06b6d4; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Đơn ứng tuyển chờ xem xét</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào,</p>
                            <p>Bạn có đơn ứng tuyển đang chờ xem xét:</p>
                            
                            <div class='info-box'>
                                <p><strong>Ứng viên:</strong> {candidateName}</p>
                                <p><strong>Vị trí:</strong> {jobTitle}</p>
                                <p><strong>Đã chờ:</strong> {daysWaiting} ngày</p>
                            </div>
                            
                            <p>Vui lòng đăng nhập vào hệ thống để xem xét và xử lý đơn ứng tuyển này.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(recruiterEmail, subject, body);
        }

        // ========== THÔNG BÁO CHO ỨNG VIÊN ==========

        public async Task<bool> SendApplicationStatusChangedNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string status, string? note = null, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null)
        {
            var subject = $"Cập nhật trạng thái đơn ứng tuyển - {jobTitle}";
            var statusText = status switch
            {
                "Dang xem xet" => "Đang xem xét",
                "Cho phong van" => "Chờ phỏng vấn",
                "Da phong van" => "Đã phỏng vấn",
                "Tuyen dung" => "Tuyển dụng",
                "Tu choi" => "Từ chối",
                "Khong phu hop" => "Không phù hợp",
                _ => status
            };

            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: white; border-left: 4px solid #10b981; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Cập nhật trạng thái</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào {candidateName},</p>
                            <p>Đơn ứng tuyển của bạn đã được cập nhật trạng thái:</p>
                            
                            <div class='info-box'>
                                <p><strong>Vị trí:</strong> {jobTitle}</p>
                                <p><strong>Công ty:</strong> {companyName}</p>
                                <p><strong>Trạng thái:</strong> {statusText}</p>
                                {(string.IsNullOrEmpty(note) ? "" : $"<p><strong>Ghi chú:</strong> {note}</p>")}
                            </div>
                            
                            {(string.IsNullOrEmpty(contactPerson) && string.IsNullOrEmpty(contactEmail) && string.IsNullOrEmpty(contactPhone) && string.IsNullOrEmpty(contactAddress) ? "" : $@"
                            <div class='info-box' style='background: #eff6ff; border-left-color: #3b82f6;'>
                                <h6 style='margin-top: 0; color: #1e40af;'><i class='bi bi-telephone'></i> Thông tin liên hệ</h6>
                                {(string.IsNullOrEmpty(contactPerson) ? "" : $"<p><strong>Người liên hệ:</strong> {contactPerson}</p>")}
                                {(string.IsNullOrEmpty(contactEmail) ? "" : $"<p><strong>Email:</strong> <a href='mailto:{contactEmail}'>{contactEmail}</a></p>")}
                                {(string.IsNullOrEmpty(contactPhone) ? "" : $"<p><strong>Điện thoại:</strong> <a href='tel:{contactPhone}'>{contactPhone}</a></p>")}
                                {(string.IsNullOrEmpty(contactAddress) ? "" : $"<p><strong>Địa chỉ:</strong> {contactAddress}</p>")}
                            </div>
                            ")}
                            
                            <p>Vui lòng đăng nhập vào hệ thống để xem chi tiết.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(candidateEmail, subject, body);
        }

        public async Task<bool> SendInterviewScheduledNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null)
        {
            var subject = $"Lịch phỏng vấn - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #06b6d4 0%, #0891b2 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .success-box {{ background: #dbeafe; border-left: 4px solid #06b6d4; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Lịch phỏng vấn</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào {candidateName},</p>
                            <p>Chúc mừng! Bạn đã được mời phỏng vấn:</p>
                            
                            <div class='success-box'>
                                <p><strong>Vị trí:</strong> {jobTitle}</p>
                                <p><strong>Công ty:</strong> {companyName}</p>
                            </div>
                            
                            {(string.IsNullOrEmpty(contactPerson) && string.IsNullOrEmpty(contactEmail) && string.IsNullOrEmpty(contactPhone) && string.IsNullOrEmpty(contactAddress) ? "" : $@"
                            <div class='info-box' style='background: #eff6ff; border-left-color: #3b82f6;'>
                                <h6 style='margin-top: 0; color: #1e40af;'><i class='bi bi-telephone'></i> Thông tin liên hệ</h6>
                                {(string.IsNullOrEmpty(contactPerson) ? "" : $"<p><strong>Người liên hệ:</strong> {contactPerson}</p>")}
                                {(string.IsNullOrEmpty(contactEmail) ? "" : $"<p><strong>Email:</strong> <a href='mailto:{contactEmail}'>{contactEmail}</a></p>")}
                                {(string.IsNullOrEmpty(contactPhone) ? "" : $"<p><strong>Điện thoại:</strong> <a href='tel:{contactPhone}'>{contactPhone}</a></p>")}
                                {(string.IsNullOrEmpty(contactAddress) ? "" : $"<p><strong>Địa chỉ:</strong> {contactAddress}</p>")}
                            </div>
                            ")}
                            
                            <p>Vui lòng đăng nhập vào hệ thống để xem chi tiết và chuẩn bị cho buổi phỏng vấn.</p>
                            
                            <p>Chúc bạn thành công!<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(candidateEmail, subject, body);
        }

        public async Task<bool> SendApplicationAcceptedNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null)
        {
            var subject = $"Chúc mừng! Bạn đã được tuyển dụng - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .success-box {{ background: #d1fae5; border-left: 4px solid #10b981; padding: 15px; margin: 20px 0; text-align: center; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🎉 Chúc mừng!</h1>
                            <p>Bạn đã được tuyển dụng</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào {candidateName},</p>
                            <p>Chúng tôi rất vui mừng thông báo:</p>
                            
                            <div class='success-box'>
                                <h2 style='color: #10b981; margin: 0;'>Bạn đã được chấp nhận!</h2>
                                <p style='margin: 10px 0 0 0;'><strong>Vị trí:</strong> {jobTitle}</p>
                                <p style='margin: 5px 0;'><strong>Công ty:</strong> {companyName}</p>
                            </div>
                            
                            {(string.IsNullOrEmpty(contactPerson) && string.IsNullOrEmpty(contactEmail) && string.IsNullOrEmpty(contactPhone) && string.IsNullOrEmpty(contactAddress) ? "" : $@"
                            <div class='info-box' style='background: #eff6ff; border-left-color: #3b82f6;'>
                                <h6 style='margin-top: 0; color: #1e40af;'><i class='bi bi-telephone'></i> Thông tin liên hệ</h6>
                                {(string.IsNullOrEmpty(contactPerson) ? "" : $"<p><strong>Người liên hệ:</strong> {contactPerson}</p>")}
                                {(string.IsNullOrEmpty(contactEmail) ? "" : $"<p><strong>Email:</strong> <a href='mailto:{contactEmail}'>{contactEmail}</a></p>")}
                                {(string.IsNullOrEmpty(contactPhone) ? "" : $"<p><strong>Điện thoại:</strong> <a href='tel:{contactPhone}'>{contactPhone}</a></p>")}
                                {(string.IsNullOrEmpty(contactAddress) ? "" : $"<p><strong>Địa chỉ:</strong> {contactAddress}</p>")}
                            </div>
                            ")}
                            
                            <p>Vui lòng đăng nhập vào hệ thống để xem chi tiết và liên hệ với nhà tuyển dụng.</p>
                            
                            <p>Chúc mừng bạn và chúc bạn thành công trong công việc mới!<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(candidateEmail, subject, body);
        }

        public async Task<bool> SendApplicationRejectedNotificationAsync(string candidateEmail, string candidateName, string jobTitle, string companyName, string? reason = null, string? contactPerson = null, string? contactEmail = null, string? contactPhone = null, string? contactAddress = null)
        {
            var subject = $"Thông báo kết quả đơn ứng tuyển - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #6b7280 0%, #4b5563 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: white; border-left: 4px solid #6b7280; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Thông báo kết quả</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào {candidateName},</p>
                            <p>Cảm ơn bạn đã quan tâm đến vị trí:</p>
                            
                            <div class='info-box'>
                                <p><strong>Vị trí:</strong> {jobTitle}</p>
                                <p><strong>Công ty:</strong> {companyName}</p>
                                {(string.IsNullOrEmpty(reason) ? "" : $"<p><strong>Lý do:</strong> {reason}</p>")}
                            </div>
                            
                            {(string.IsNullOrEmpty(contactPerson) && string.IsNullOrEmpty(contactEmail) && string.IsNullOrEmpty(contactPhone) && string.IsNullOrEmpty(contactAddress) ? "" : $@"
                            <div class='info-box' style='background: #eff6ff; border-left-color: #3b82f6;'>
                                <h6 style='margin-top: 0; color: #1e40af;'><i class='bi bi-telephone'></i> Thông tin liên hệ</h6>
                                {(string.IsNullOrEmpty(contactPerson) ? "" : $"<p><strong>Người liên hệ:</strong> {contactPerson}</p>")}
                                {(string.IsNullOrEmpty(contactEmail) ? "" : $"<p><strong>Email:</strong> <a href='mailto:{contactEmail}'>{contactEmail}</a></p>")}
                                {(string.IsNullOrEmpty(contactPhone) ? "" : $"<p><strong>Điện thoại:</strong> <a href='tel:{contactPhone}'>{contactPhone}</a></p>")}
                                {(string.IsNullOrEmpty(contactAddress) ? "" : $"<p><strong>Địa chỉ:</strong> {contactAddress}</p>")}
                            </div>
                            ")}
                            
                            <p>Rất tiếc, đơn ứng tuyển của bạn không được chấp nhận lần này. Chúng tôi hy vọng bạn sẽ tiếp tục tìm kiếm cơ hội phù hợp khác trên Unicareer.</p>
                            
                            <p>Chúc bạn may mắn trong các đơn ứng tuyển tiếp theo!<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(candidateEmail, subject, body);
        }

        public async Task<bool> SendSavedJobExpiringNotificationAsync(string candidateEmail, string candidateName, string jobTitle, int daysLeft, DateTime expiryDate)
        {
            var subject = $"Tin tuyển dụng bạn đã lưu sắp hết hạn - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .warning-box {{ background: #fff3cd; border-left: 4px solid #f59e0b; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Tin tuyển dụng sắp hết hạn</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào {candidateName},</p>
                            <p>Tin tuyển dụng bạn đã lưu sắp hết hạn:</p>
                            
                            <div class='warning-box'>
                                <p><strong>Tin tuyển dụng:</strong> {jobTitle}</p>
                                <p><strong>Còn lại:</strong> {daysLeft} ngày</p>
                                <p><strong>Ngày hết hạn:</strong> {expiryDate:dd/MM/yyyy}</p>
                            </div>
                            
                            <p>Nếu bạn quan tâm đến vị trí này, hãy nhanh chóng ứng tuyển trước khi hết hạn.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(candidateEmail, subject, body);
        }

        public async Task<bool> SendSavedJobExpiredNotificationAsync(string candidateEmail, string candidateName, string jobTitle, DateTime expiryDate)
        {
            var subject = $"Tin tuyển dụng bạn đã lưu đã hết hạn - {jobTitle}";
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #6b7280 0%, #4b5563 100%); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .info-box {{ background: white; border-left: 4px solid #6b7280; padding: 15px; margin: 20px 0; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Unicareer</h1>
                            <p>Tin tuyển dụng đã hết hạn</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào {candidateName},</p>
                            <p>Tin tuyển dụng bạn đã lưu đã hết hạn:</p>
                            
                            <div class='info-box'>
                                <p><strong>Tin tuyển dụng:</strong> {jobTitle}</p>
                                <p><strong>Ngày hết hạn:</strong> {expiryDate:dd/MM/yyyy}</p>
                            </div>
                            
                            <p>Bạn có thể tìm kiếm các cơ hội việc làm khác phù hợp với mình trên Unicareer.</p>
                            
                            <p>Trân trọng,<br>Đội ngũ Unicareer</p>
                        </div>
                        <div class='footer'>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>";

            return await SendEmailAsync(candidateEmail, subject, body);
        }
    }
}

