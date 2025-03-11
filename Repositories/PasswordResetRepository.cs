using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Models;

namespace tl2_proyecto_2024_nachoNota.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ICommandFactory _commandFactory;

        public PasswordResetRepository(IConnectionProvider connectionProv, ICommandFactory commandFact)
        {
            _connectionProvider = connectionProv;
            _commandFactory = commandFact;
        }

        public void Create(PasswordReset passwordReset)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "INSERT INTO passwordreset(email, token, expiration) VALUES (@email, @token, @exp)";

                var command =  _commandFactory.CreateCommand(commandText, connection);
                command.Parameters.AddWithValue("@email", passwordReset.Email);
                command.Parameters.AddWithValue("@token", passwordReset.Token);
                command.Parameters.AddWithValue("@exp", passwordReset.Expiration);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public PasswordReset GetByToken(string token)
        {
            PasswordReset? passwordReset = null;
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "SELECT id, email, expiration FROM passwordreset WHERE token = @token";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@token", token);
                using(var reader =  command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        passwordReset = new PasswordReset
                        {
                            Id = reader.GetInt32("id"),
                            Email = reader.GetString("email"),
                            Token = token,
                            Expiration = reader.GetDateTime("expiration")
                        };
                    }
                }
                connection.Close();
            }
            return passwordReset;
        }

        public void Delete(int id)
        {
            using(var connection = _connectionProvider.GetConnection())
            {
                connection.Open();
                string commandText = "DELETE FROM passwordreset WHERE id = @id";
                var command = _commandFactory.CreateCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
                
                connection.Close();
            }
        }
    }
}
