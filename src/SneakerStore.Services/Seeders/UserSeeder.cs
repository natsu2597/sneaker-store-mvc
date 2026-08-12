using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using SneakerStore.Services.Data;
using SneakerStore.Services.Enums;
using SneakerStore.Services.Models;

namespace SneakerStore.Services.Seeders
{
    public class UserSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserSeeder(
            ApplicationDbContext dbContext,
            IPasswordHasher<User> passwordHasher, IConfiguration configuration)
            
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }


        public async Task SeedAsync()
        {
            var adminPassword = _configuration["SeedUsers:AdminPassword"];
            var customerPassword = _configuration["SeedUsers:CustomerPassword"];

            if(string.IsNullOrWhiteSpace(adminPassword) || string.IsNullOrWhiteSpace(customerPassword))
                throw new InvalidOperationException(
                    "Seed user passwords are not configured. Configure them using User Secrets.");

            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string checkSql = @"
                SELECT COUNT(*)
                FROM Users;
            ";

            using var checkCmd = new MySqlCommand(checkSql, conn);

            var count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

            if (count > 0)
                return;

            await SeedAdmin(conn,adminPassword);
            await SeedCustomer(conn,customerPassword);
        }

        private async Task SeedAdmin(MySqlConnection conn,string password)
        {
            var admin = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@sneakerstore.com",
                Phone = "9999999999",
                Role = UserRole.Admin
            };

            admin.PasswordHash =
                _passwordHasher.HashPassword(admin, password);

            const string sql = @"
                INSERT INTO Users
                (
                    FirstName,
                    LastName,
                    Email,
                    PasswordHash,
                    Phone,
                    Role,
                    ProfileImage
                )
                VALUES
                (
                    @FirstName,
                    @LastName,
                    @Email,
                    @PasswordHash,
                    @Phone,
                    @Role,
                    NULL
                );
            ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@FirstName", admin.FirstName);
            cmd.Parameters.AddWithValue("@LastName", admin.LastName);
            cmd.Parameters.AddWithValue("@Email", admin.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", admin.PasswordHash);
            cmd.Parameters.AddWithValue("@Phone", admin.Phone);
            cmd.Parameters.AddWithValue("@Role", (int)admin.Role);

            await cmd.ExecuteNonQueryAsync();
        }

        private async Task SeedCustomer(MySqlConnection conn,string password)
        {
            var customer = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "8888888888",
                Role = UserRole.Customer
            };

            customer.PasswordHash =
                _passwordHasher.HashPassword(customer, password);

            const string sql = @"
                INSERT INTO Users
                (
                    FirstName,
                    LastName,
                    Email,
                    PasswordHash,
                    Phone,
                    Role,
                    ProfileImage
                )
                VALUES
                (
                    @FirstName,
                    @LastName,
                    @Email,
                    @PasswordHash,
                    @Phone,
                    @Role,
                    NULL
                );
            ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
            cmd.Parameters.AddWithValue("@Email", customer.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", customer.PasswordHash);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.Parameters.AddWithValue("@Role", (int)customer.Role);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
