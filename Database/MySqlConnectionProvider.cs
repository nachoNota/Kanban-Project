using MySql.Data.MySqlClient;

namespace tl2_proyecto_2024_nachoNota.Database
{
    public class MySqlConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public MySqlConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection getConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
