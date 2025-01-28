using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class TablaRepository : ITablaRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public TablaRepository(IConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        public IEnumerable<Tabla> GetByTablero(int idTablero)
        {
            var tablas = new List<Tabla>();

            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT * FROM tabla WHERE idTablero = @idTablero";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@idTablero", idTablero);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTabla = reader.GetInt32("id_tabla");
                        int idUsuario = reader.GetInt32("id_usuario");
                        int id = reader.GetInt32("id_tablero");
                        string titulo = reader.GetString("titulo");

                        var tabla = new Tabla(id, idUsuario, idTabla, titulo);
                        tablas.Add(tabla);
                    }
                }
                connection.Close();
            }
            return tablas;
        }
    }
}
