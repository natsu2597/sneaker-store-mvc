using MySqlConnector;

namespace SneakerStore.Services.Data
{
    public class ApplicationDbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MySqlConnection CreateConnection(string connectionName = "DefaultConnection")
        {
            var connString = _configuration.GetConnectionString(connectionName);
            return new MySqlConnection(connString);
        }
    }
}
