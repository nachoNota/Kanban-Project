using MySql.Data.MySqlClient;

namespace tl2_proyecto_2024_nachoNota.Database
{
    public interface IConnectionProvider
    {
        MySqlConnection GetConnection();
    }
}
