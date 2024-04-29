using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Helpers;
using SchoolManagement.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SchoolManagement.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthHelper _authHelper;
        private readonly MailHelper _mailHelper;
        //private readonly IUrlHelper _urlHelper;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, AuthHelper authHelper, MailHelper mailHelper
            //, IUrlHelper urlHelper
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
            _mailHelper = mailHelper;
            //_urlHelper = urlHelper;
        }
        public async Task<TokenModel> Login(SignInDto request)
        {
            var user = await _unitOfWork.UserRepository.GetByUsername(request.Username) ?? throw new BadRequestException("Invalid username/password");
            if (!_authHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new BadRequestException("Wrong password");
            }
            return await _authHelper.GenerateToken(user);
        }

        public async Task<User> Register(SignUpDto request)
        {
            User user = new User();
            _authHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Name = request.Name;
            user.Email = request.Email;
            user.Role = UserRole.Student;
            user.IsDeleted = false;
            user.IsVerified = false; 
            user.Otp = _mailHelper.GenerateOTP();
            user.OtpCreationTime = DateTime.Now;
            user.LastModifiedPasswordDate = DateTime.Now;

            var email = new EmailDto();
            email.Subject = "[SM] Verification email of " + user.Name;
            //string confirmLink = _urlHelper.Action("VerifyConfirmForRegister", "Auth", new { email = user.Email }, _urlHelper.ActionContext.HttpContext.Request.Scheme);
            email.Body = $"Please click the link below to confirm your email address:<br/>" +
                //$"{confirmLink}<br/> " +
                $"";
            email.To = user.Email;
            _mailHelper.SendEmail(email);
            return await _unitOfWork.UserRepository.AddEntity(user);
        }

        public async Task<TokenModel> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);
            var tokenValidateParam = new TokenValidationParameters
            {

                ValidateIssuer = false,
                ValidateAudience = false,


                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false
            };
            try
            {
                //AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        throw new BadRequestException("Invalid token");
                    }
                }

                //Check accessToken expired
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = _authHelper.ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.Now)
                {
                    throw new BadRequestException("Access token has not yet expired");
                }

                //Check refreshtoken exist in DB
                var storedToken = await _unitOfWork.TokenRepository.GetByToken(model.RefreshToken);
                if (storedToken == null)
                {
                    throw new NotFoundException("Refresh token does not exist")
 ;              }

                //Check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    throw new BadRequestException("Refresh token has been used");
                }
                if (storedToken.IsRevoked)
                {
                    throw new BadRequestException("Refresh token has been revoked");
                }

                //Check AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    throw new BadRequestException("Token doesn't match");
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                await _unitOfWork.TokenRepository.UpdateEntity(storedToken);

                //create new token
                var user = await _unitOfWork.UserRepository.GetAsync(storedToken.UserId); 
                var token = await _authHelper.GenerateToken(user);

                return token;
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong");
            }
        }

        public async Task<bool> ResendOtp(string email)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(email) ?? throw new NotFoundException("Email not exist");          
            user.Otp = _mailHelper.GenerateOTP();
            user.OtpCreationTime = DateTime.Now;

            var emailDto = new EmailDto();
            emailDto.Subject = "[SM] Verification email" + user.Name;
            emailDto.Body = $"PLease use this otp: {user.Otp}";
            emailDto.To = user.Email;
            await _unitOfWork.UserRepository.UpdateEntity(user);
            _mailHelper.SendEmail(emailDto);
            return true;
            
        }

        public async Task<bool> ResetPassword(ResetPasswordDto request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(request.Email) ?? throw new NotFoundException("User not found");
            if (!request.Password.Equals(request.ConfirmPassword))
            {
                throw new BadRequestException("Confirm Password does not match!");
            }
            _authHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LastModifiedPasswordDate = DateTime.Now;
            await _unitOfWork.UserRepository.UpdateEntity(user);
            return true;
        }

        public async Task SendChangePasswordRemindingMail()
        {
            var users = await _unitOfWork.UserRepository.GetAllNonAdminUsers();
            foreach(var user in users)
            {
                var passwordExpirationDate = user.LastModifiedPasswordDate.AddMonths(3);
                var notificationDate = passwordExpirationDate.AddDays(-7);

                if (DateTime.Now >= notificationDate && DateTime.Now < passwordExpirationDate)
                {
                    int dayRemaining = (passwordExpirationDate - DateTime.Now).Days;
                    var emailDto = new EmailDto();
                    emailDto.Subject = "[SM] Alert changing password";
                    emailDto.Body = $"The password of {user.Username} used to log in SM system will be expired in {dayRemaining} days {passwordExpirationDate}. <br/><br/> Please change your password.";
                    emailDto.To = user.Email;
                    _mailHelper.SendEmail(emailDto);
                }
                else if(DateTime.Now >= passwordExpirationDate)
                {
                    user.IsDeleted = true;
                    await _unitOfWork.UserRepository.UpdateEntity(user);
                }
                Console.WriteLine($"Send Email: reminding change password {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            } 
        }

        public async Task<bool> VerifyConfirmForRegiser(string email)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(email) ?? throw new NotFoundException("Email not exist");
            user.IsVerified = true;
            await _unitOfWork.UserRepository.UpdateEntity(user);
            return true;            
        }

        public async Task<bool> VerifyOtpForResetPassword(OtpVerifyDto request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(request.Email) ?? throw new NotFoundException("Email not exist");
             if (user.Otp.Equals(request.Otp))
             {
                if(user.OtpCreationTime.AddMinutes(10)  < DateTime.Now)
                {
                    throw new BadRequestException("OTP has expired");
                }
                return true;
             }
            return false;
        }
    }
}
