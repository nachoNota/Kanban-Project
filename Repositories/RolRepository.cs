using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public RolRepository(IConnectionProvider connectionProvider, ICommandFactory commandFactory)
        {
            _connectionProvider = connectionProvider;
            _commandFactory = commandFactory;
        }

        public IEnumerable<Rol> GetAll()
        {
            var roles = new List<Rol>();
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT * FROM rol";
                var command = _commandFactory.CreateCommand(commandText, connection);

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rol = new Rol();
                        rol.Id = reader.GetInt32("id_rol");
                        rol.NombreRol = reader.GetString("rol");
                        roles.Add(rol);
                    }
                }
                connection.Close();
            }
            return roles;
        }
        public Rol GetById(int idRol)
        {
            Rol? rol = null;

            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "SELECT * FROM rol WHERE id_rol = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", idRol);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rol = new Rol();
                        rol.Id = reader.GetInt32("id_rol");
                        rol.NombreRol = reader.GetString("rol");
                    }
                }
                connection.Close();
            }

            return rol;
        }
        public void Create(string nombreRol)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "INSERT INTO rol(rol) VALUES(@nombreRol)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@nombreRol", nombreRol);
                command.ExecuteNonQuery();
                
                connection.Close();
            }
        }
        
        public void Update(int id, string nombreRol)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "UPDATE rol SET rol = @nombreRol WHERE id_rol = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@nombreRol", nombreRol);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "DELETE FROM rol WHERE id_rol = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
