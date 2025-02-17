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
                        string color = reader.GetString("color");
                        string? desc = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        var tablero = new Tablero(idTablero, idUsuario, titulo, color, desc);
                        
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
                        string color = reader.GetString("color");
                        string? desc = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        var tablero = new Tablero(idTablero, idUsu, titulo, color, desc);

                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }

            return tableros;

        }

        public IEnumerable<Tablero> GetTablerosConTareasAsignadas(int idUsuario)
        {
            var tableros = new List<Tablero>();

            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = @"
                             SELECT distinct tab.id_tablero, tab.id_usuario, tab.titulo, tab.descripcion, tab.color
                            FROM tablero tab
                            JOIN tarea tar USING(id_tablero)
                            WHERE tar.id_usuario = @idUsuario AND tab.id_usuario <> @idUsuario";
                
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTablero = reader.GetInt32("id_tablero");
                        int idUsu = reader.GetInt32("id_usuario");
                        string titulo = reader.GetString("titulo");
                        string color = reader.GetString("color");
                        string? desc = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        var tablero = new Tablero(idTablero, idUsu, titulo, color, desc);

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
                        string color = reader.GetString("color");
                        string? desc = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        tablero = new Tablero(idTablero, idUsuario, titulo, color, desc);
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
                        string color = reader.GetString("color");
                        string? desc = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        var tablero = new Tablero(idTablero, idUsuario, titulo, color, desc);
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

                string commandText = "INSERT INTO tablero(id_usuario, titulo, color, descripcion) " +
                    "               VALUES (@id_usuario, @titulo, @color, @desc)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id_usuario", tablero.IdUsuario);
                command.Parameters.AddWithValue("@titulo", tablero.Titulo);
                command.Parameters.AddWithValue("@color", tablero.Color);
                command.Parameters.AddWithValue("@desc", tablero.Descripcion);

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

        public void Update(Tablero tablero)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "UPDATE tablero SET titulo = @titulo, color = @color," +
                                    "descripcion = @desc WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@titulo", tablero.Titulo);
                command.Parameters.AddWithValue("@desc", tablero.Descripcion);
                command.Parameters.AddWithValue("@color", tablero.Color);
                command.Parameters.AddWithValue("@id", tablero.Id);


                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public int GetPropietario(int idTablero)
        {
            int idUsuario = 0;
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT id_usuario FROM tablero WHERE id_tablero = @idTablero";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@idTablero", idTablero);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idUsuario = reader.GetInt32("id_usuario");
                    }
                }
                connection.Close();
            }
            return idUsuario;
        }
    }
}
