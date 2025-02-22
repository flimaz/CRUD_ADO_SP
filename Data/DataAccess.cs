using Microsoft.Data.SqlClient;
using CrudSP.Models;
using System.Data;

namespace CrudSP.Data  
{
    public class DataAccess
    {

        SqlConnection _connection = null ; 
        SqlCommand _command = null ; 
        public static IConfiguration Configuration {get ; set;}

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            
            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefaultConnection");
        }

        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (_connection = new SqlConnection(GetConnectionString()))
            {
                _command = _connection.CreateCommand();
                _command.CommandType = CommandType.StoredProcedure;
                _command.CommandText = "[DBO].[listar_usuarios]";

                _connection.Open();

                SqlDataReader reader = _command.ExecuteReader();

                // Enquanto houver registros para serem lidos, executa.
                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nome = reader["Nome"] != DBNull.Value ? reader["Nome"].ToString() : string.Empty,
                        Sobrenome = reader["Sobrenome"] != DBNull.Value ? reader["Sobrenome"].ToString() : string.Empty,
                        Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                        Cargo = reader["Cargo"] != DBNull.Value ? reader["Cargo"].ToString() : string.Empty
                    };

                    // Adiciono na lista de usuário o que acabei de criar
                    usuarios.Add(usuario);
                }


                _connection.Close();
            }

            return usuarios;

        }
    
public bool Cadastrar(Usuario usuario)
{
    int id = 0;

    try
    {
        using (_connection = new SqlConnection(GetConnectionString()))
        {
            _command = _connection.CreateCommand();
            _command.CommandType = CommandType.StoredProcedure;
            _command.CommandText = "[DBO].[inserir_usuario]";
            
            _command.Parameters.AddWithValue("@Nome", usuario.Nome);
            _command.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
            _command.Parameters.AddWithValue("@Email", usuario.Email);
            _command.Parameters.AddWithValue("@Cargo", usuario.Cargo);

            _connection.Open();
            id = _command.ExecuteNonQuery();
        }
    }
    catch (Exception ex)
    {
        // Logar o erro ou tratar de alguma forma
        Console.WriteLine(ex.Message);
        return false;
    }

    return id > 0; 
}

 public Usuario BuscarUsuarioPorId(int id)
{
    Usuario usuario = null; // Inicializa como nulo

    using (SqlConnection connection = new SqlConnection(GetConnectionString()))
    {
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[DBO].[listar_usuario_id]";
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()) // Verifica se há resultados
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nome = reader["Nome"].ToString(),
                        Sobrenome = reader["SobreNome"].ToString(),
                        Email = reader["Email"].ToString(),
                        Cargo = reader["Cargo"].ToString()
                    };
                }
            }
        }
    }

    return usuario; // Retorna nulo se não encontrar
}

public bool Editar(Usuario usuario)
{
    var id = 0 ; 

    using (_connection = new SqlConnection(GetConnectionString()))
    {
        _command = _connection.CreateCommand();
        _command.CommandType = CommandType.StoredProcedure;
        _command.CommandText = "[DBO].[editar_usuario]";

        _command.Parameters.AddWithValue("@Id", usuario.Id);

        _command.Parameters.AddWithValue("@Nome", usuario.Nome);
        _command.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
        _command.Parameters.AddWithValue("@Email", usuario.Email);
        _command.Parameters.AddWithValue("@Cargo", usuario.Cargo);

        _connection.Open();

        id = _command.ExecuteNonQuery();

        _connection.Close();

    }
    return id > 0 ? true : false;  
}

public bool Remover(int id)
{
    var result = 0;

    using (_connection = new SqlConnection(GetConnectionString()))

    {
        _command = _connection.CreateCommand();
        _command.CommandType = CommandType.StoredProcedure;
        _command.CommandText = "[DBO].[remover_usuario]";
        _command.Parameters.AddWithValue("@Id", id);

        _connection.Open();

        result = _command.ExecuteNonQuery();

        _connection.Close();
    }

    return result > 0 ? true : false ; 
}


    }
}