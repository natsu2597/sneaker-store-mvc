using MySqlConnector;
using SneakerStore.Services.Data;
using SneakerStore.Services.Extensions;
using SneakerStore.Services.Models;

namespace SneakerStore.Services.Repository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PasswordResetRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(PasswordResetToken token)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                INSERT INTO PasswordResetTokens
                (
                    UserId,
                    TokenHash,
                    CreatedAt,
                    ExpiresAt,
                    IsUsed
                )
                VALUES
                (
                    @UserId,
                    @TokenHash,
                    @CreatedAt,
                    @ExpiresAt,
                    FALSE
                );
            ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", token.UserId);
            cmd.Parameters.AddWithValue("@TokenHash", token.TokenHash);
            cmd.Parameters.AddWithValue("@CreatedAt", token.CreatedAt);
            cmd.Parameters.AddWithValue("@ExpiresAt", token.ExpiresAt);


            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<PasswordResetToken?> GetTokenHashAsync(string tokenHash)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                SELECT
                    Id,
                    UserId,
                    TokenHash,
                    CreatedAt,
                    ExpiresAt,
                    IsUsed,
                    UsedAt
            FROM PasswordResetTokens
            WHERE TokenHash = @TokenHash
            LIMIT 1;
            ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@TokenHash", tokenHash);

            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new PasswordResetToken
            {
                Id = reader.GetInt32("Id"),
                UserId = reader.GetInt32("UserId"),
                TokenHash = reader.GetString("TokenHash"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                ExpiresAt = reader.GetDateTime("ExpiresAt"),
                IsUsed = reader.GetBoolean("IsUsed"),
                UsedAt = reader.IsDBNull(
                        reader.GetOrdinal("UsedAt"))
                        ? null
                        : reader.GetDateTime("UsedAt")
            };

        }

        public async Task MarkUsedAsync(int tokenId)
        {
            using var conn = _dbContext.CreateConnection();
            await conn.OpenAsync();

            const string sql = @"
                    UPDATE PasswordResetTokens
                    SET
                        IsUsed = TRUE,
                        UsedAt = UTC_TIMESTAMP()
                    WHERE Id = @Id;
                ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", tokenId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task InvalidatUserTokenAsync(int userId)
        {
            using var conn = _dbContext.CreateConnection();

            await conn.OpenAsync();

            const string sql = @"
                UPDATE PasswordResetTokens
                SET
                    IsUsed = TRUE,
                    UsedAt = UTC_TIMESTAMP()
                WHERE UserId = @UserId
                    AND IsUsed = FALSE;
            ";

            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            await cmd.ExecuteNonQueryAsync();
        }

        
    }
}
