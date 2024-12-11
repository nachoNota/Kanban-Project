using MySql.Data.MySqlClient;

namespace tl2_proyecto_2024_nachoNota.Database
{
    public interface ICommandFactory
    {
        MySqlCommand CreateCommand(string commandText, MySqlConnection connection);
    }
}
