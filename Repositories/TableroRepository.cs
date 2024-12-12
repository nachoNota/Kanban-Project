using MySqlX.XDevAPI.Relational;
using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class TableroRepository : ITableroRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public TableroRepository(IConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        public IEnumerable<Tablero> GetAll()
        {
            var tableros = new List<Tablero>();

            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "SELECT * FROM tablero";
                var command = _commandFactory.CreateCommand(commandText, connection);
                
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero();
                        tablero.AsignarId(reader.GetInt32("id_tablero"));
                        tablero.AsignarUsuario(reader.GetInt32("id_usuario"));
                        tablero.Titulo = reader.GetString("titulo");
                        tablero.Descripcion = reader.GetString("descripcion");
                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }

            return tableros;
        }

        public Tablero GetById(int id)
        {
            Tablero tablero = null;

            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "SELECT * FROM tablero WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tablero = new Tablero();
                        tablero.AsignarId(reader.GetInt32("id_tablero"));
                        tablero.AsignarUsuario(reader.GetInt32("id_usuario"));
                        tablero.Titulo = reader.GetString("titulo");
                        tablero.Descripcion = reader.GetString("descripcion");
                    }
                }
                connection.Close();
            }

            return tablero;
        }

        public IEnumerable<Tablero> GetByUser(int idUsuario)
        {
            var tableros = new List<Tablero>();
            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "SELECT * FROM tablero WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", idUsuario);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero();
                        tablero.AsignarId(reader.GetInt32("id_tablero"));
                        tablero.AsignarUsuario(reader.GetInt32("id_usuario"));
                        tablero.Titulo = reader.GetString("titulo");
                        tablero.Descripcion = reader.GetString("descripcion");
                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }

            return tableros;
        }
        public void Create(Tablero tablero)
        {
            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "INSERT INTO tablero(id_usuario, titulo, descripcion) VALUES (@id_usuario, @titulo, @desc)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id_usuario", tablero.IdUsuario);
                command.Parameters.AddWithValue("@titulo", tablero.Titulo);
                command.Parameters.AddWithValue("@desc", tablero.Descripcion);
                
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = _connectionProvider.getConnection())
            {
                string commandText = "DELETE FROM tablero WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(int id, Tablero tablero)
        {
            using (var connection = _connectionProvider.getConnection())
            {
                string commandText = "UPDATE tablero SET id_usuario = @id_usuario, titulo = @titulo, descripcion = @desc" +
                                    "WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id_usuario", tablero.IdUsuario);
                command.Parameters.AddWithValue("@titulo", tablero.Titulo);
                command.Parameters.AddWithValue("@desc", tablero.Descripcion);
                command.Parameters.AddWithValue("@id", tablero.Id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
