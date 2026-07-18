using SneakerStore.Services.Dtos;
using SneakerStore.Services.Models;

namespace SneakerStore.Services.Repository
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(User user);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<UserResponseDto>> GetAllAsync();
        Task<bool> EmailExistsAsync(string email);
        Task<bool> EmailExistsAsync(string email,int userId);
        Task<bool> PhoneExistsAsync(string phone);
        Task<bool> PhoneExistsAsync(string phone, int userId);
        Task UpdateAsync(UpdateProfileRequest request);
        Task UpdateProfileAsync(User user);
        Task DeleteAsync(int id);
    }
}