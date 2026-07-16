using SneakerStore.Services.Dtos;
using SneakerStore.Services.Models;

namespace SneakerStore.Services.Repository
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(User user);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phone);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}