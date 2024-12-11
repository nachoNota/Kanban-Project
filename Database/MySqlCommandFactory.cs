using MySql.Data.MySqlClient;

namespace tl2_proyecto_2024_nachoNota.Database
{
    public class MySqlCommandFactory : ICommandFactory
    {
        public MySqlCommand CreateCommand(string commandText, MySqlConnection connection)
        {
            return new MySqlCommand(commandText, connection);
        }
    }
}
