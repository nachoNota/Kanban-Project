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

            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "SELECT * FROM tablero";
                var command = _commandFactory.CreateCommand(commandText, connection);
                
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTablero = reader.GetInt32("id_tablero");
                        int idUsuario = reader.GetInt32("id_usuario");
                        string titulo = reader.GetString("titulo");
                        var tablero = new Tablero(idTablero, idUsuario, titulo);
                        
                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }

            return tableros;
        }

        public IEnumerable<Tablero> GetAllByUser(int idUsuario)
        {
            var tableros = new List<Tablero>();

            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                
                string commandText = "SELECT * FROM tablero WHERE id_usuario = @idUser";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@idUser", idUsuario);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTablero = reader.GetInt32("id_tablero");
                        int idUsu = reader.GetInt32("id_usuario");
                        string titulo = reader.GetString("titulo");
                        var tablero = new Tablero(idTablero, idUsu, titulo);

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

            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "SELECT * FROM tablero WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int idTablero = reader.GetInt32("id_tablero");
                        int idUsuario = reader.GetInt32("id_usuario");
                        string titulo = reader.GetString("titulo");
                        tablero = new Tablero(idTablero, idUsuario, titulo);
                    }
                }
                connection.Close();
            }

            return tablero;
        }

        public IEnumerable<Tablero> GetByUser(int idUsuario)
        {
            var tableros = new List<Tablero>();
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "SELECT * FROM tablero WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", idUsuario);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTablero = reader.GetInt32("id_tablero");
                        int idUsu = reader.GetInt32("id_usuario");
                        string titulo = reader.GetString("titulo");

                        var tablero = new Tablero(idTablero, idUsuario, titulo);
                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }

            return tableros;
        }
        public void Create(Tablero tablero)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "INSERT INTO tablero(id_usuario, titulo) VALUES (@id_usuario, @titulo)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id_usuario", tablero.IdUsuario);
                command.Parameters.AddWithValue("@titulo", tablero.Titulo);
                
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "DELETE FROM tablero WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(int id, Tablero tablero)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "UPDATE tablero SET id_usuario = @id_usuario, titulo = @titulo" +
                                    "WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id_usuario", tablero.IdUsuario);
                command.Parameters.AddWithValue("@titulo", tablero.Titulo);
                command.Parameters.AddWithValue("@id", tablero.Id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

    }
}
