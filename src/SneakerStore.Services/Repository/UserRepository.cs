using MySqlConnector;
using SneakerStore.Services.Data;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Enums;
using SneakerStore.Services.Extensions;
using SneakerStore.Services.Models;
using System.Data;
using System.Runtime.InteropServices;

namespace SneakerStore.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> RegisterAsync(User user)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    INSERT INTO Users
                    (FirstName,LastName,Email,PasswordHash,Phone,Role,ProfileImage)
                    VALUES
                    (@FirstName,@LastName,@Email,@PasswordHash,@Phone,@Role,@ProfileImage);
                ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.AddUserParameters(user);
            await cmd.ExecuteNonQueryAsync();

            return (int)cmd.LastInsertedId;


        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT 
                        Id,
                        FistName,
                        LastName,
                        Email,
                        Phone,
                        Role,
                        ProfileImage,
                        CreatedAt
                    FROM Users
                    WHERE @Id=Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return reader.MapUserResponseDto();
            }

            return null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT * FROM Users
                    WHERE Email = @Email;
                ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return reader.MapUsers();
            }

            return null;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT * FROM Users
                    OrderBy CreatedAt DESC;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(
                        reader.MapUsers()
                    );
            }


            return users;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Email = @Email;
                ";

            using var cmd = new MySqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Email",email);

            using var reader = await cmd.ExecuteReaderAsync();

            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        public async Task<bool> PhoneExistsAsync(string phone)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Phone = @Phone;
                ";

            using var cmd = new MySqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Phone", phone);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        

        public async Task UpdateAsync(User user)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    UPDATE Users
                    SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Phone = @Phone,
                        ProfileImage = @ProfileImage,
                    WHERE Id = @Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.AddUserParametersUpdate(user);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    DELETE FROM Users
                    WHERE Id = @Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await cmd.ExecuteNonQueryAsync();
        }

    }
}
