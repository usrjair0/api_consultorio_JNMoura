using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Web2.Repositories.SQLServer
{
    public class Autenticar
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;

        public Autenticar(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand() { Connection = conn };
        }


        public async Task<bool> Select(Models.Login login)
        {
            bool validacao = false;
            using (conn) 
            {
                await conn.OpenAsync();
                using (cmd)
                {
                    cmd.CommandText = "SELECT email, senha FROM usuario WHERE email = @email and senha=@senha";
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = login.Email;
                    cmd.Parameters.Add(new SqlParameter("@senha", SqlDbType.VarChar)).Value = login.Senha;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync()) {
                        if (await dr.ReadAsync())
                        {
                            validacao = true;
                        }
                    }
                }
            }
            return validacao;
        }


    }
}