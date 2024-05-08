using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Web2.Repositories.SQLServer
{
    public class Medicamento
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;
        public Medicamento(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand();
            this.cmd.Connection = this.conn;
        }

        public async Task <List<Models.Medicamento>> ObterTodos()
        {
            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento;";
                    using(SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while(await dr.ReadAsync())
                        {
                            Models.Medicamento medicamento = new Models.Medicamento();
                            medicamento.Id = (int) dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (!(dr["datavencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                            medicamentos.Add(medicamento);
                        }
                    }
                }
            }
            return medicamentos;
        }

        public async Task<Models.Medicamento> ObterporID(int id)
        {
            Models.Medicamento medicamento = null;
            using (this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    using(SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if(await dr.ReadAsync())
                        {
                            medicamento = new Models.Medicamento();
                            medicamento.Id = (int) dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (!(dr["datavencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                        }
                    }
                }
            }
            return medicamento;
        }

        public async Task<List<Models.Medicamento>> ObterporNome (string nome) 
        { 
            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();
            using(this.conn)
            {
                await this.conn.OpenAsync();
                using(this.cmd)
                {
                    cmd.CommandText = "select id, nome, datafabricacao, datavencimento from medicamento where nome like @nome;";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";
                    using(SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    { 
                        while (await dr.ReadAsync())
                        {
                            Models.Medicamento medicamento = new Models.Medicamento();
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = (DateTime) dr["dataFabricacao"];
                            if (!(dr["datavencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["datavencimento"]);
                            medicamentos.Add(medicamento);
                        }
                    }
                }
            }

            return medicamentos;
        }

        public async Task<bool> Inserir(Models.Medicamento medicamento) 
        {
            using(conn)
            {
                await this.conn.OpenAsync();
                using(cmd)
                {
                    cmd.CommandText = "insert into medicamento (nome, datafabricacao, datavencimento) values (@nome,@datafabricacao,@datavencimento); select CONVERT(int, @@identity);";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    if (medicamento.DataVencimento != null)
                        cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;
                    else
                        cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = DBNull.Value;
                    medicamento.Id = (int) await cmd.ExecuteScalarAsync();
                }
            }
            return medicamento.Id != 0;
        }

        public async Task<bool> Update(Models.Medicamento medicamento)
        {
            int linhasAfetadas;
            using(conn)
            {
                await this.conn.OpenAsync();
                using(cmd) 
                {
                    cmd.CommandText = "update medicamento set nome = @nome, datafabricacao = @df, datavencimento = @dv where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = medicamento.Id;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    cmd.Parameters.Add(new SqlParameter("@df", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    cmd.Parameters.Add(new SqlParameter("dv", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    linhasAfetadas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetadas == 1;
        }

        public async Task<bool> Delete(int id) 
        {
            int linhasAfetas; 
            using(conn)
            {
                await this.conn.OpenAsync();
                using(cmd)
                {
                    cmd.CommandText = "delete medicamento where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    linhasAfetas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return linhasAfetas == 1;
        }
    }   
}