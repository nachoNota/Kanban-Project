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
                        tarea.AsignarId(reader.GetInt32("id_tarea"));
                        tarea.AsignarUsuario(reader.GetInt32("id_usuario"));
                        tarea.AsignarTabla(reader.GetInt32("id_tabla"));
                        tarea.Titulo = reader.GetString("titulo");
                        tarea.Descripcion = reader.GetString("descripcion");
                        if (!reader.IsDBNull(reader.GetOrdinal("imagen")))
                        {
                            tarea.Imagen = (byte[])reader["imagen"];
                        }
                        tareas.Add(tarea);
                    }
                }

                connection.Close();
            }
            return tareas;
        }
        public Tarea GetById(int id)
        {
            Tarea tarea = null;

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
                        tarea.AsignarId(reader.GetInt32("id_tarea"));
                        tarea.AsignarUsuario(reader.GetInt32("id_usuario"));
                        tarea.AsignarTabla(reader.GetInt32("id_tabla"));
                        tarea.Titulo = reader.GetString("titulo");
                        tarea.Descripcion = reader.GetString("descripcion");
                        if (!reader.IsDBNull(reader.GetOrdinal("imagen")))
                        {
                            tarea.Imagen = (byte[])reader["imagen"];
                        }
                    }
                }

                connection.Close();
            }
            return tarea;

        }
        public IEnumerable<Tarea> GetByUser(int idUsuario)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Tarea> GetByTablero(int idTablero)
        {
            throw new NotImplementedException();
        }

        public void AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            throw new NotImplementedException();
        }

        public void Create(int idTablero, Tarea tablero)
        {
            throw new NotImplementedException();
        }
        public void Update(int id, Tarea tablero)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
