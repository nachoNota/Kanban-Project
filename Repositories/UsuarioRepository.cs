using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Database;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public UsuarioRepository(IConnectionProvider connectionProv, ICommandFactory commandFact)
        {
            _connectionProvider = connectionProv;
            _commandFactory = commandFact;
        }

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();
            
            using (var connection = _connectionProvider.GetConnection())
            {

                connection.Open();

                string commandText = @"SELECT * FROM usuario";
                var command = _commandFactory.CreateCommand(commandText, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idUsuario = reader.GetInt32("id_usuario");
                        string nombreUsuario = reader.GetString("nombre_usuario");
                        string pass = reader.GetString("contrasenia");
                        string email = reader.GetString("email");
                        RolUsuario rol = (RolUsuario)reader.GetInt32(reader.GetOrdinal("rol"));

                        var usuario = new Usuario(idUsuario, rol, nombreUsuario, pass, email);
                        
                        usuarios.Add(usuario);
                    }
                }

                connection.Close();
            }
            return usuarios; 
        }

		public Usuario GetByName(string nombreUsuario)
        {
			Usuario? usuario = null;

			using (var connection = _connectionProvider.GetConnection())
			{
				connection.Open();
				string commandText = "SELECT * FROM usuario WHERE nombre_usuario = @nombre";
				var command = _commandFactory.CreateCommand(commandText, connection);
				command.Parameters.AddWithValue("@nombre", nombreUsuario);

				using (var reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						int idUsuario = reader.GetInt32("id_usuario");
						string pass = reader.GetString("contrasenia");
						string email = reader.GetString("email");
						RolUsuario rol = (RolUsuario)reader.GetInt32(reader.GetOrdinal("rol"));

						usuario = new Usuario(idUsuario, rol, nombreUsuario, pass, email);
					}
				}
				connection.Close();
			}

			return usuario;
		}

		public Usuario GetById(int id)
        {
            Usuario? usuario = null;

            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT * FROM usuario WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                
                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int idUsuario = reader.GetInt32("id_usuario");
                        string nombreUsuario = reader.GetString("nombre_usuario");
                        string pass = reader.GetString("contrasenia");
                        string email = reader.GetString("email");
                        RolUsuario rol = (RolUsuario)reader.GetInt32(reader.GetOrdinal("rol"));

                        usuario = new Usuario(idUsuario, rol, nombreUsuario, pass, email);
                    }
                }
                connection.Close();
            }

            return usuario;
        }

        public Usuario GetUser(string nombreUsuario, string contrasenia)
        {
            Usuario usuario = null;

            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT * FROM usuario WHERE nombre_usuario = @usu AND contrasenia = @contra";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@usu", nombreUsuario);
                command.Parameters.AddWithValue("@contra", contrasenia);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int idUsuario = reader.GetInt32("id_usuario");
                        string email = reader.GetString("email");
                        RolUsuario rol = (RolUsuario)reader.GetInt32(reader.GetOrdinal("rol"));

                        usuario = new Usuario(idUsuario, rol, nombreUsuario, contrasenia, email);
                    }
                }
                connection.Close();
            }
            return usuario;
        }

        public IEnumerable<Usuario> SearchByName(string nombreUsuario)
        {
            var usuarios = new List<Usuario>();
            
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT * FROM usuario WHERE nombre_usuario LIKE CONCAT('%', @nombre, '%')";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@nombre", nombreUsuario);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idUsuario = reader.GetInt32("id_usuario");
                        string pass = reader.GetString("contrasenia");
                        string email = reader.GetString("email");
                        string nombre = reader.GetString("nombre_usuario");
                        RolUsuario rol = (RolUsuario)reader.GetInt32(reader.GetOrdinal("rol"));

                        var usuario = new Usuario(idUsuario, rol, nombre, pass, email);
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }

            return usuarios;
        }

        public string GetPasswordById(int id)
        {
            string password = string.Empty;
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT contrasenia FROM usuario WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        password = reader.GetString("contrasenia");
                    }
                }
                connection.Close();
            }
            return password;
        }

        public void ChangeRol(int idUsuario, RolUsuario rol)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "UPDATE usuario SET rol = @idRol WHERE id_usuario = @idUsuario";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@idRol", ((int)rol));
                command.Parameters.AddWithValue("@idUsuario", idUsuario);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void ChangePassword(int id, string pass)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "UPDATE usuario SET contrasenia = @pass WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@pass", pass);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Create(Usuario usuario)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "INSERT INTO usuario(nombre_usuario, contrasenia, rol, email)" +
                                        " VALUES (@nombre, @contra, @rol, @email)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@contra", usuario.Password);
                command.Parameters.AddWithValue("@rol", ((int)usuario.Rol));
                command.Parameters.AddWithValue("@email", usuario.Email);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "UPDATE usuario SET nombre_usuario = @nombre, " +
                                        "email = @email " +
                                        "WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", usuario.Id);
                command.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@email", usuario.Email);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();

                string commandText = "DELETE FROM usuario WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public string GetNameById(int id)
        {
            var nombre = string.Empty;
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT nombre_usuario FROM usuario WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);

                using(var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        nombre = reader.GetString("nombre_usuario");
                    }
                }
                connection.Close();
            }
            return nombre;
        }
    }
}
