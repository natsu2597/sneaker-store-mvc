using MySqlConnector;
using SneakerStore.Services.Data;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Extensions;
using SneakerStore.Services.Models;

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
                        FirstName,
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

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        Id,
                        FirstName,
                        LastName,
                        Email,
                        PasswordHash,
                        Phone,
                        Role,
                        ProfileImage,
                        CreatedAt
                    FROM Users
                    WHERE Id = @Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return reader.MapUsers();
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

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = new List<UserResponseDto>();
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
                        reader.MapUserResponseDto()
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

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        public async Task<bool> EmailExistsAsync(string email, int userId)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Email = @Email
                        AND Id <> @Id;
                ";

            using var cmd = new MySqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Email",email);
            cmd.Parameters.AddWithValue("@Id", userId);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        public async Task<bool> PhoneExistsAsync(string phone)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Phone = @Phone
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Phone", phone);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        public async Task<bool> PhoneExistsAsync(string phone, int userId)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Phone = @Phone
                        AND Id <> @Id;
                ";

            using var cmd = new MySqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Id", userId);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        public async Task UpdateAsync(UpdateProfileRequest user)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    UPDATE Users
                    SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Email = @Email,
                        Phone = @Phone,
                        ProfileImage = @ProfileImage
                    WHERE Id = @Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.Id);

            cmd.AddUserParametersUpdate(user);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ChangePasswordAsync(User user)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    UPDATE Users
                    SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Phone = @Phone,
                        PasswordHash = @PasswordHash,
                        ProfileImage = @ProfileImage
                    WHERE Id = @Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.Id);

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
