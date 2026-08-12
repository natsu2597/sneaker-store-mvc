using MySqlConnector;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Models;

namespace SneakerStore.Services.Extensions
{
    public static class MySqlCommandExtensions
    {
        public static void AddUserParameters(this MySqlCommand cmd,User user)
        {

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue(@"LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.Parameters.AddWithValue("@Role", (int)user.Role);
            cmd.Parameters.AddWithValue("@ProfileImage", user.ProfileImage ?? (object)DBNull.Value);
        }

        public static void AddUserParametersUpdate(this MySqlCommand cmd, UpdateProfileRequest user)
        {
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@ProfileImage", user.CurrentImageUrl ?? (object)DBNull.Value);
        }

        public static void AddUserParametersUpdate(this MySqlCommand cmd, User user)
        {
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@ProfileImage", user.ProfileImage ?? (object)DBNull.Value);
        }
    }
}
