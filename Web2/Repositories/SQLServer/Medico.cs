using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Web2.Repositories.SQLServer
{
    public class Medico
    {
        //Falta fazer o cache
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;

        public Medico(string connectionString) 
        { 
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand { Connection = conn };

        }

        public async Task <List<Models.Medico>> SelectAll()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico;";
                    using(SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Medico medico = new Models.Medico
                            {
                                Id = (int)dr["id"],
                                CRM = dr["crm"].ToString(),
                                Nome = dr["nome"].ToString()
                            };
                            medicos.Add(medico);    
                        }
                    }
                }
            }
            return medicos;
        }

        public async Task <List<Models.Medico>> SelectByNome(string nome)
        {
            List<Models.Medico> medicos = new List<Models.Medico>();
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico where nome like @nome;";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";
                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Medico medico = new Models.Medico
                            {
                                Id = (int)dr["id"],
                                CRM = dr["crm"].ToString(),
                                Nome = dr["nome"].ToString()
                            };
                            medicos.Add(medico);
                        }
                    }
                }
            }
            return medicos;
        }

        public async Task<Models.Medico> SelectById(int id)
        {
            Models.Medico medico = null;
            using (this.conn) 
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "select id, crm, nome from medico where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    using(SqlDataReader dr = await this.cmd.ExecuteReaderAsync()) 
                    { 
                        if(await dr.ReadAsync())
                        {
                            medico = new Models.Medico
                            {
                                Id = (int)dr["id"],
                                CRM = dr["crm"].ToString(),
                                Nome = dr["nome"].ToString()
                            };
                        }
                    }
                }
            }

            return medico;
        }

        public async Task<bool> Insert(Models.Medico medico)
        {
            using (this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = "SELECT COUNT(1) FROM medico WHERE crm = @crm";
                    this.cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = medico.CRM;

                    int existingCRMCount = (int)await this.cmd.ExecuteScalarAsync();
                    if (!Validations.Medico.CheckUniqueCRM(existingCRMCount))
                        return false;

                    this.cmd.CommandText = @"insert into medico (crm, nome) values (@crm, @nome); 
                    select CONVERT(int, SCOPE_IDENTITY());";
                    this.cmd.Parameters.Clear();
                    this.cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = medico.CRM;
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medico.Nome;

                    medico.Id = (int) await this.cmd.ExecuteScalarAsync();

                }
            }
            return medico.Id != 0;
        }

        public async Task<bool> Update(Models.Medico medico)
        {
            int linhasAfetadas;
            using (this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    cmd.CommandText = @"update medico set nome = @nome, crm = @crm where id=@id;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = medico.Id;
                    cmd.Parameters.Add(new SqlParameter("nome", SqlDbType.VarChar)).Value = medico.Nome;
                    cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = medico.CRM;

                    linhasAfetadas = await this.cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetadas == 1;
        }

        public async Task<bool> Delete(int id)
        {
            int linhasAfetadas;
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using (this.cmd)
                {
                    cmd.CommandText = @"delete medico where id=@id";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetadas == 1;
        }

    }
}