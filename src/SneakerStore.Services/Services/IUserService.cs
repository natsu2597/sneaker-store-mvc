using SneakerStore.Services.Dtos;
using SneakerStore.Services.Models;

namespace SneakerStore.Services.Services
{
    public interface IUserService
    {
        Task<int> RegisterAsync(RegisterRequest request);
        Task<User?> LoginAsync(LoginRequest request);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<List<UserResponseDto>> GetAllAsync();
        Task UpdateAsync(UpdateUserRequest request);
        Task DeleteAsync(int id);
    }
}