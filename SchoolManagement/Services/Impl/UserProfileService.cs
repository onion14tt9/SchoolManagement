using AutoMapper;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Helpers;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AuthHelper _authHelper;
        private readonly ExcelHelper _excelHelper;
        private readonly IHttpContextAccessor _httpContext;

        public UserProfileService(IUnitOfWork unitOfWork, IMapper mapper, AuthHelper authHelper, ExcelHelper excelHelper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authHelper = authHelper;
            _excelHelper = excelHelper;
            _httpContext = httpContextAccessor;
        }

        public async Task<List<UserProfileDto>> AddUserProfile(List<UserProfileDto> models)
        {
            var results = new List<UserProfileDto>();
            foreach (var model in models) 
            {
                var newProfile = _mapper.Map<User>(model);
                _authHelper.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
                newProfile.IsDeleted = false;
                newProfile.IsVerified = true;
                newProfile.PasswordHash = passwordHash;
                newProfile.PasswordSalt = passwordSalt;
                newProfile.LastModifiedPasswordDate = DateTime.Now;
                var result = _mapper.Map<UserProfileDto>(await _unitOfWork.UserRepository.AddEntity(newProfile));
                result.Password = model.Password;
                results.Add(result);
            }
            _excelHelper.GenerateUserListToExcelFile(results);
            return results;
        }

        public async Task<bool> DeleteUserProfile(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id) ?? throw new NotFoundException("User not found");
            user.IsDeleted = true;
            await _unitOfWork.UserRepository.UpdateEntity(user);
            return true;
        }

        public async Task<IEnumerable<User>> GetAllStudents()
        {
            return await _unitOfWork.UserRepository.GetAllUsersByRole(UserRole.Student);
        }

        public async Task<IEnumerable<User>> GetAllTeachers()
        {
            return await _unitOfWork.UserRepository.GetAllUsersByRole(UserRole.Teacher);
        }

        public async Task<User?> GetUserById(int id)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            } else if (user.UserId != currentUserLogin.UserId && currentUserLogin.Role != UserRole.Admin)
            {
                throw new ForbiddenException("You are not allowed to view this profile");
            }
            return user;
        }

        public async Task<UserProfileDto> UpdateUserProfile(int id, UserProfileDto model)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id) ?? throw new NotFoundException("User not found");
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            if (currentUserLogin.Role != UserRole.Admin && currentUserLogin.UserId != id)
            {
                throw new ForbiddenException("You are not allowed to access");
            }

            _authHelper.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = model.Username;
            user.Name = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Address = model.Address;

            user.Role = currentUserLogin.Role != UserRole.Admin ? user.Role : model.Role;
            // user.Role = model.Role;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var result = _mapper.Map<UserProfileDto>(await _unitOfWork.UserRepository.UpdateEntity(user));
            result.Password = model.Password;
            return result;
        }
    }
}
