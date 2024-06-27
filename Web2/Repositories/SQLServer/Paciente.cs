using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Web2.Repositories.SQLServer
{
    public class Paciente
    {
        //Falta implementação do cache
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;
        public Paciente(string connectionString) 
        { 
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand { Connection = conn };
        }

        public async Task<List<Models.Paciente>> SelectAll()
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente> ();

            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    this.cmd.CommandText = @"select id, nome, datanascimento from Paciente;";
                    using(SqlDataReader dr =  await this.cmd.ExecuteReaderAsync())
                    { 
                        while(dr.Read())
                        {
                            Models.Paciente paciente = new Models.Paciente
                            {
                                Id = (int)dr["id"],
                                Nome = dr["nome"].ToString(),
                                DataNascimento = Convert.ToDateTime(dr["datanascimento"])
                            };
                            pacientes.Add(paciente);
                        } 
                    }

                }
            }
            return pacientes;
        }

        public async Task<Models.Paciente> SelectById(int id)
        {
            Models.Paciente paciente = null;
            using (this.conn)
            {
                await this.conn.OpenAsync ();
                using (this.cmd)
                {
                    this.cmd.CommandText = @"select id, nome, datanascimento from Paciente where id=@id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    using(SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        if(dr.Read())
                        {
                            paciente = new Models.Paciente
                            {
                                Id = (int)dr["id"],
                                Nome = dr["nome"].ToString(),
                                DataNascimento = Convert.ToDateTime(dr["datanascimento"])
                            };
                        }
                    }
                }
            }
            return paciente;
        }

        public async Task<List<Models.Paciente>> SelectByName(string nome)
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente>();
            using (this.conn)
            {
                await this.conn.OpenAsync ();
                using (this.cmd)
                {
                    this.cmd.CommandText = @"select id, nome, datanascimento from Paciente where nome like @nome;";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";
                    using(SqlDataReader dr =await this.cmd.ExecuteReaderAsync())
                    {
                        while(dr.Read())
                        {
                            Models.Paciente paciente = new Models.Paciente
                            {
                                Id = (int)dr["id"],
                                Nome = dr["nome"].ToString(),
                                DataNascimento = Convert.ToDateTime(dr["datanascimento"])
                            };
                            pacientes.Add(paciente);
                        }
                    }
                }
            }
            return pacientes;
        }

        public async Task<bool> Insert(Models.Paciente paciente)
        {
            using(this.conn)
            {
                await this.conn.OpenAsync ();
                using (this.cmd)
                {
                    this.cmd.CommandText = @"insert into Paciente (nome, datanascimento) values (@nome, @datanascimento);
                                            select convert(int, SCOPE_IDENTITY());";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = paciente.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datanascimento", SqlDbType.Date)).Value = paciente.DataNascimento;

                    paciente.Id = (int) await this.cmd.ExecuteScalarAsync();
                }
            }
            return paciente.Id != 0;
        }

        public async Task<bool> Update(Models.Paciente paciente)
        {
            int linhasAfetadas = 0;
            using(this.conn)
            {
                await this.conn.OpenAsync ();
                using (this.cmd)
                {
                    this.cmd.CommandText = @"update paciente set nome = @nome, datanascimento = @datanascimento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = paciente.Id;
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = paciente.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datanascimento", SqlDbType.Date)).Value = paciente.DataNascimento;
                    linhasAfetadas = await this.cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetadas == 1;
        }

        public async Task<bool> Delete(int id)
        {
            int linhasAfetadas = 0;
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using (this.cmd)
                {
                    this.cmd.CommandText = @"delete from paciente where id=@id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    linhasAfetadas = await this.cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetadas == 1;
        }
    }
}