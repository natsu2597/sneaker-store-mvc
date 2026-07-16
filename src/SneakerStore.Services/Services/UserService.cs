using Microsoft.AspNetCore.Identity;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Models;
using SneakerStore.Services.Repository;

namespace SneakerStore.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ICloudinaryService _cloudinaryService;

        

        public UserService(IUserRepository userRepository, PasswordHasher<User> passwordHasher,ICloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new Exception("Email already exists");

            if (await _userRepository.PhoneExistsAsync(request.Phone))
                throw new Exception("Phone number already exists");

            string? imageUrl = null;

            if(request.ProfileImage != null)
            {
                var (uploadImageUrl, _) = await _cloudinaryService.UploadImageAsync(request.ProfileImage);
                imageUrl = uploadImageUrl;
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email  = request.Email,
                Phone = request.Phone,
                ProfileImage = imageUrl,
                Role = Enums.UserRole.Customer,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            return await _userRepository.RegisterAsync(user);
        }

        public async Task<User?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ?? throw new Exception("User not found");

            var result = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password
                );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Password verification failed");

            return user;
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task UpdateAsync(UpdateUserRequest request)
        {
            await _userRepository.UpdateAsync(request);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
        
    }
}
