using MySqlConnector;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Enums;
using SneakerStore.Services.Models;
using System.Data;

namespace SneakerStore.Services.Extensions
{
    public static class MapperExtensions
    {
        public static User MapUsers(this MySqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                Phone = reader.GetString("Phone"),
                Role = (UserRole)reader.GetInt32("Role"),
                ProfileImage = reader.GetString("ProfileImage"),
                CreatedAt = reader.GetDateTime("CreatedAt")
            };
        }

        public static UserResponseDto MapUserResponseDto(this MySqlDataReader reader)
        {
            return new UserResponseDto
            {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                Phone = reader.GetString("Phone"),
                Role = (UserRole)reader.GetInt32("Role"),
                ImageUrl = reader.IsDBNull("ProfileImage")
                    ? null
                    : reader.GetString("ProfileImage"),
                CreatedAt = reader.GetDateTime("CreatedAt")

            };
        }
    }
}
