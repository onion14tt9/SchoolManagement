using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using SchoolManagement.Domain.Dtos;
using System.Security.Cryptography;

namespace SchoolManagement.Helpers
{
    public class MailHelper
    {
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
            Console.WriteLine("Email Sent Successfully");
        }

        public string GenerateOTP()
        {
            int otpLength = 6; // Length of the OTP
            int maxDigits = (int)Math.Pow(10, otpLength) - 1; // Maximum value for the OTP

            // Generate a random number within the range of 0 and maxDigits
            Random random = new Random();
            int otpValue = random.Next(0, maxDigits);

            // Format the OTP as a 6-digit string
            string otpString = otpValue.ToString("D6");

            return otpString;
        }
    }
}
