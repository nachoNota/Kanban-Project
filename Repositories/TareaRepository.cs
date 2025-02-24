using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public TareaRepository(IConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        public IEnumerable<Tarea> GetAll()
        {
            var tareas = new List<Tarea>();

            using (var connection = _connectionProvider.GetConnection())
            {

                connection.Open();

                string commandText = @"SELECT * FROM tarea";
                var command = _commandFactory.CreateCommand(commandText, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea();
                        tarea.Id = reader.GetInt32("id_tarea");
                        tarea.IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario"))
                                        ? 0 : reader.GetInt32(reader.GetOrdinal("id_usuario"));
                        tarea.Estado = (EstadoTarea)reader.GetInt32(reader.GetOrdinal("estado"));
                        tarea.IdTablero = reader.GetInt32("id_tablero");
                        tarea.Titulo = reader.GetString("titulo");
                        tarea.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        tarea.Color = reader.GetString("color");
                        tarea.FechaModificacion = reader.GetDateTime("fecha_modificacion");
                        tareas.Add(tarea);
                    }
                }

                connection.Close();
            }
            return tareas;
        }
        public Tarea GetById(int id)
        {
            Tarea? tarea = null;

            using (var connection = _connectionProvider.GetConnection())
            {

                connection.Open();

                string commandText = @"SELECT * FROM tarea WHERE id_tarea = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tarea = new Tarea();
                        tarea.Id = reader.GetInt32("id_tarea");
                        tarea.IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) 
                                        ? 0 : reader.GetInt32(reader.GetOrdinal("id_usuario"));
                        tarea.Estado = (EstadoTarea)reader.GetInt32(reader.GetOrdinal("estado"));
                        tarea.IdTablero = reader.GetInt32("id_tablero");
                        tarea.Titulo = reader.GetString("titulo");
                        tarea.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("descripcion"));
                        tarea.Color = reader.GetString("color");
                        tarea.FechaModificacion = reader.GetDateTime("fecha_modificacion");
                    }
                }

                connection.Close();
            }

            if (tarea is null) throw new KeyNotFoundException($"No se encontró tarea con id {id}");

            return tarea;

        }

        public IEnumerable<Tarea> GetByTablero(int idTablero)
        {
            var tareas = new List<Tarea>();

            using (var connection = _connectionProvider.GetConnection())
            {

                connection.Open();

                string commandText = @"SELECT * FROM tarea WHERE id_tablero = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", idTablero);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea();
                        tarea.Id = reader.GetInt32("id_tarea");
                        tarea.IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario"))
                                        ? 0 : reader.GetInt32(reader.GetOrdinal("id_usuario"));
                        tarea.Estado = (EstadoTarea)reader.GetInt32(reader.GetOrdinal("estado"));
                        tarea.IdTablero = reader.GetInt32("id_tablero");
                        tarea.Titulo = reader.GetString("titulo");
                        tarea.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"));
                        tarea.Color = reader.GetString("color");
                        tarea.FechaModificacion = reader.GetDateTime("fecha_modificacion");

                        tareas.Add(tarea);
                    }
                }

                connection.Close();
            }
            return tareas;
        }

        public void AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "UPDATE tarea SET id_usuario = @idUsuario WHERE id_tarea = @idTarea";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                command.Parameters.AddWithValue("@idTarea", idTarea);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Create(Tarea tarea)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "INSERT INTO tarea(id_usuario, titulo, descripcion, color, id_tablero, estado) VALUES " +
                                    "(@idUsuario, @titulo, @desc, @color, @idTablero, @estado)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@idUsuario", tarea.IdUsuario);
                command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@desc", tarea.Descripcion);
                command.Parameters.AddWithValue("@idTablero", tarea.IdTablero);
                command.Parameters.AddWithValue("@color", tarea.Color);
                command.Parameters.AddWithValue("@estado", ((int)tarea.Estado));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        public void Update(Tarea tarea)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "UPDATE tarea SET titulo = @titulo, descripcion = @desc, " +
                    "color = @color WHERE id_tarea = @idTarea";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@desc", tarea.Descripcion);
                command.Parameters.AddWithValue("@color", tarea.Color);
                command.Parameters.AddWithValue("@idTarea", tarea.Id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "DELETE FROM tarea WHERE id_tarea = @idTarea";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@idTarea", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public IEnumerable<Tarea> GetByTablaYTablero(int idTabla, int idTablero)
        {
            var tareas = new List<Tarea>();

            using(var connection =  _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT * FROM tarea WHERE id_tabla = @idTabla AND id_tablero = @idTablero";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@idTabla", idTabla);
                command.Parameters.AddWithValue("@idTablero", idTablero);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea();
                        tarea.Id = reader.GetInt32("id_tarea");
                        tarea.IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario"))
                                        ? 0 : reader.GetInt32(reader.GetOrdinal("id_usuario"));
                        tarea.IdTablero = reader.GetInt32("id_tablero");
                        tarea.Estado = (EstadoTarea)reader.GetInt32(reader.GetOrdinal("estado"));
                        tarea.Titulo = reader.GetString("titulo");
                        tarea.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"));
                        tarea.Color = reader.GetString("color");
                        tarea.FechaModificacion = reader.GetDateTime("fecha_modificacion");

                        tareas.Add(tarea);
                    }
                }
                connection.Close();
            }
            return tareas;
        }

        public void CambiarEstado(int idTarea, EstadoTarea estado)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "UPDATE tarea SET estado = @estado WHERE id_tarea = @idTarea";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@idTarea", idTarea);
                command.Parameters.AddWithValue("@estado", (int)estado);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
