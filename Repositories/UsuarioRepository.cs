using tl2_proyecto_2024_nachoNota.Models;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data.Common;
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
            
            using (var connection = _connectionProvider.getConnection())
            {

                connection.Open();

                string commandText = @"SELECT * FROM usuario";
                var command = _commandFactory.CreateCommand(commandText, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.AsignarId(reader.GetInt32("id_usuario"));
                        usuario.AsignarRol(reader.GetInt32("id_rol"));
                        usuario.NombreUsuario = reader.GetString("nombre_usuario");
                        usuario.Password = reader.GetString("contrasenia");
                        usuarios.Add(usuario);
                    }
                }

                connection.Close();
            }
            return usuarios; 
        }
        public Usuario GetById(int id)
        {
            Usuario usuario = null;

            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "SELECT * FROM usuario WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                
                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.AsignarId(reader.GetInt32("id_usuario"));
                        usuario.AsignarRol(reader.GetInt32("id_rol"));
                        usuario.NombreUsuario = reader.GetString("nombre_usuario");
                        usuario.Password = reader.GetString("contrasenia");
                    }
                }
                connection.Close();
            }

            return usuario;
        }

        public void ChangePassword(int id, string pass)
        {
            using(var connection = _connectionProvider.getConnection())
            {
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
            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "INSERT INTO usuario(nombre_usuario, contrasenia, id_rol) VALUES (@nombre, @contra, @id_rol)";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@contra", usuario.Password);
                command.Parameters.AddWithValue("@id_rol", usuario.IdRol);
            
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(int id, Usuario usuario)
        {
            using (var connection = _connectionProvider.getConnection())
            {
                string commandText = "UPDATE usuario SET nombre_usuario = @nombre, " +
                                        "contrasenia = @pass," +
                                        "id_rol = @id_rol " +
                                        "WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@pass", usuario.Password);
                command.Parameters.AddWithValue("@id", usuario.Id);
                command.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                command.Parameters.AddWithValue("@id_rol", usuario.IdRol);
                
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using(var connection = _connectionProvider.getConnection())
            {
                string commandText = "DELETE FROM usuario WHERE id_usuario = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
